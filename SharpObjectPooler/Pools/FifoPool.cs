using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace LambdaTheDev.SharpObjectPooler.Pools
{
    // A FIFO pool, based on Stack collection, with new() generator only
    public class FifoPool<T> : IBulkPool<T> where T : new()
    {
        // Collection used to store items
        private readonly Stack<T> _pool;

        // Object count stored in a pool
        public int Count => _pool.Count;
        
        // Max pool capacity
        public int MaxCapacity { get; set; }
        
        
        public FifoPool(int initialCapacity, int maxCapacity = -1)
        {
            // Validate arguments
            if(initialCapacity < 0)
                throw new ArgumentOutOfRangeException(nameof(initialCapacity), "Initial capacity must be greater or equal to zero!");

            if(maxCapacity == 0 || maxCapacity < -1)
                throw new ArgumentOutOfRangeException(nameof(maxCapacity), "Max capacity must be greater than zero, or -1 if pool should be infinite!");
        
            if(maxCapacity != -1 && maxCapacity < initialCapacity)
                throw new ArgumentOutOfRangeException(nameof(initialCapacity), "Max capacity can not be lesser than initial capacity!");
            
            // Initialize pool & fill out content
            _pool = new Stack<T>(initialCapacity);
            for(int i = 0; i < initialCapacity; i++)
                _pool.Push(new T());
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Rent()
        {
            if (_pool.TryPop(out T item))
                return item;

            return new T();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Return(T item)
        {
            if (MaxCapacity != -1 && _pool.Count >= MaxCapacity)
                return;
            
            _pool.Push(item);
        }

        public int RentBulk(T[] outputArray, int offset, int count)
        {
            // ArraySegment throws an exception if offset/count is invalid
            ArraySegment<T> segment = new ArraySegment<T>(outputArray, offset, count);

            for (int i = 0; i < count; i++)
                outputArray[offset + i] = Rent();

            return count;
        }

        public int ReturnBulk(T[] inputArray, int offset, int count)
        {
            // ArraySegment throws an exception if offset/count is invalid
            ArraySegment<T> segment = new ArraySegment<T>(inputArray, offset, count);

            // Define how much items to push
            int itemsToCopy = GetItemsToCopy(count);
            
            // Push items
            for(int i = 0; i < itemsToCopy; i++)
                _pool.Push(inputArray[offset + i]);

            // Return how much objects are pushed
            return itemsToCopy;
        }

        // Least performant, recommendation - do not use
        public int ReturnBulk(IEnumerable<T> items)
        {
            // How much items were pushed
            int pushedItems = 0;
            
            foreach (T item in items)
            {
                if (MaxCapacity != -1 && _pool.Count >= MaxCapacity)
                    break;

                pushedItems++;
                _pool.Push(item);
            }

            return pushedItems;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetItemsToCopy(int count)
        {
            int itemsToCopy;
            
            if (MaxCapacity == -1)
            {
                itemsToCopy = count;
            }
            else
            {
                int difference = MaxCapacity - _pool.Count - count;
                if (difference < 0)
                    itemsToCopy = count - difference;
                else
                    itemsToCopy = count;
            }

            return itemsToCopy;
        }
    }
}