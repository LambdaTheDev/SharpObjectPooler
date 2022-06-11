using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using LambdaTheDev.SharpObjectPooler.Extensions;
using LambdaTheDev.SharpObjectPooler.Utils;

namespace LambdaTheDev.SharpObjectPooler.Fifo
{
    // A FIFO pool implementation backed by (Concurrent)Stack collection
    public sealed class StackPool<T> : IBulkObjectPool<T>
    {
        // Pool collections
        private readonly Stack<T> _pool;
        private readonly ConcurrentStack<T> _threadSafePool;
        
        // Method used to generate new pool items
        private readonly Func<T> _generator;
        
        // Determines if Stack or ConcurrentStack was used
        private readonly bool _isThreadSafe;

        public int Count => _isThreadSafe ? _threadSafePool.Count : _pool.Count;
        public int MaxCapacity { get; }

        
        public StackPool(Func<T> generator, int initialCapacity, int maxCapacity = -1, bool isThreadSafe = false)
        {
            // Validate arguments
            PoolArgsValidators.ValidateGenerator(generator);
            PoolArgsValidators.ValidateCapacity(initialCapacity, maxCapacity);
            
            // Initialize pool
            if (isThreadSafe) _threadSafePool = new ConcurrentStack<T>();
            else _pool = new Stack<T>(initialCapacity);
            
            // Initialize initial capacity
            for (int i = 0; i < initialCapacity; i++)
            {
                if(isThreadSafe) _threadSafePool.Push(generator.Invoke());
                else _pool.Push(generator.Invoke());
            }
            
            // Set fields
            MaxCapacity = maxCapacity;
            _generator = generator;
            _isThreadSafe = isThreadSafe;
        }
        
        public T Rent()
        {
            T item;
            if (_isThreadSafe) { if (!_threadSafePool.TryPop(out item)) item = _generator.Invoke(); }
            else { if (!_pool.FrameworkSafeTryPop(out item)) item = _generator.Invoke(); }
            return item;
        }

        public void Return(T item)
        {
            if (MaxCapacity != -1 && Count >= MaxCapacity) return;
            if(_isThreadSafe) _threadSafePool.Push(item);
            else _pool.Push(item);
        }
        
        public int RentBulk(ArraySegment<T> outputArray)
        {
            if (outputArray.Array == null) return 0;

            // Get how much items can be successfully rent & rent them
            int successfulRents;
            if(_isThreadSafe) successfulRents = _threadSafePool.TryPopRange(outputArray.Array, outputArray.Offset, outputArray.Count);
            else
            {
                int outputOffset = outputArray.Offset;
                successfulRents = Math.Max(0, Math.Min(outputArray.Count, _pool.Count));
                for (int i = 0; i < successfulRents; i++)
                    outputArray.Array[outputOffset + i] = _pool.Pop();
            }
            
            // Generate necessary items
            int itemsToGenerate = outputArray.Count - successfulRents;
            for (int i = outputArray.Offset + successfulRents; i < itemsToGenerate; i++)
                outputArray.Array[i] = _generator.Invoke();

            return outputArray.Count;
        }

        public void ReturnBulk(ArraySegment<T> inputArray)
        {
            if (inputArray.Array == null) return;

            int remainingSpace = MaxCapacity == -1 ? inputArray.Count : MaxCapacity - Count;
            int itemsToReturn = remainingSpace <= inputArray.Count ? remainingSpace : inputArray.Count;
            
            if(_isThreadSafe)
                _threadSafePool.PushRange(inputArray.Array, inputArray.Offset, itemsToReturn);
            else
            {
                int inputOffset = inputArray.Offset;
                for(int i = 0; i < itemsToReturn; i++)
                    _pool.Push(inputArray.Array[inputOffset + i]);
            }
        }
    }
}