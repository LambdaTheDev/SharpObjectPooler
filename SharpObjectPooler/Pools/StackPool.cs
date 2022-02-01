using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using LambdaTheDev.SharpObjectPooler.Utils;

namespace LambdaTheDev.SharpObjectPooler.Pools
{
    // Pool implementation based on (Concurrent)Stack<T> and new() object generator
    public class StackPool<T> : IBulkPool<T> where T : new()
    {
        private readonly Stack<T> _pool; // Pool for not thread safe items
        private readonly ConcurrentStack<T> _threadSafePool; // Pool for thread safe items
        private readonly bool _isThreadSafe; // Bool value, that says if pool is thread safe or not

        // Gets how much items are in Pool
        public int Count
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _isThreadSafe ? _threadSafePool.Count : _pool.Count;
        }
        
        public int MaxCapacity { get; } // Gets max capacity


        public StackPool(int initialCapacity, int maxCapacity = -1, bool isThreadSafe = false)
        {
            // Validate arguments
            InitialPoolArgumentsValidator.ValidatePoolArgs(initialCapacity, maxCapacity);
            
            // Initialize pool collections
            if(isThreadSafe)
                _threadSafePool = new ConcurrentStack<T>();
            else
                _pool = new Stack<T>(initialCapacity);
            
            // Initialize initial capacity
            for(int i = 0; i < initialCapacity; i++)
            {
                if(isThreadSafe)
                    _threadSafePool.Push(new T());
                else
                    _pool.Push(new T());
            }

            // Set fields field
            MaxCapacity = maxCapacity;
            _isThreadSafe = isThreadSafe;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Rent()
        {
            T item;
            if (_isThreadSafe)
            {
                if(!_threadSafePool.TryPop(out item))
                    item = new T();

                return item;
            }
            
            if(!_pool.TryPop(out item))
                item = new T();

            return item;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Return(T item)
        {
            if (MaxCapacity != -1 && Count >= MaxCapacity)
                return;
            
            if(_isThreadSafe)
                _threadSafePool.Push(item);
            else
                _pool.Push(item);
        }

        public int RentBulk(T[] outputArray, int offset, int count)
        {
            // ArraySegment throws an exception, if offset & count is invalid
            new ArraySegment<T>(outputArray, offset, count);
            
            int successfulRents = 0;
            
            if (_isThreadSafe)
            {
                successfulRents = _threadSafePool.TryPopRange(outputArray, offset, count);
                if (successfulRents < count)
                {
                    for(int i = 0; i < count - successfulRents; i++)
                        outputArray[successfulRents + i] = new T();
                }

                return count;
            }

            while (_pool.TryPop(out T item) && successfulRents <= count)
            {
                outputArray[offset + successfulRents] = item;
                successfulRents++;
            }

            if (successfulRents < count)
            {
                for(int i = 0; i < count - successfulRents; i++)
                    outputArray[successfulRents + i] = new T();
            }

            return count;
        }

        public int ReturnBulk(T[] inputArray, int offset, int count)
        {
            // ArraySegment throws an exception, if offset & count is invalid
            new ArraySegment<T>(inputArray, offset, count);

            int itemsToReturn;
            if (MaxCapacity == -1)
                itemsToReturn = count;

            else
            {
                itemsToReturn = MaxCapacity - Count - count;
                if (itemsToReturn >= 0)
                    itemsToReturn = count;
                else
                    itemsToReturn -= count - itemsToReturn;
            }

            if(_isThreadSafe)
                _threadSafePool.PushRange(inputArray, offset, itemsToReturn);
            
            else
            {
                for(int i = 0; i < itemsToReturn; i++)
                    _pool.Push(inputArray[offset + i]);
            }

            return itemsToReturn;
        }
    }
}