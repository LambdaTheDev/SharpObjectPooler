using System;
using System.Collections;
using System.Collections.Generic;

namespace LambdaTheDev.SharpObjectPooler.Collections
{
    // Ring (circular) buffer implementation
    [Obsolete("Currently this is not implemented")]
    public class RingBuffer<T> : ICollection<T>
    {
        private readonly T[] _buffer;
        // private readonly bool _allowOverride;
        
        private int _count;
        private int _tail;
        private int _head;

        public int Count => _count;
        public bool IsReadOnly => false;
        public bool IsEmpty { get; }
        public bool IsFull { get; }
        
        
        public RingBuffer(int length, bool allowOverride = false)
        {
            throw new NotImplementedException();
            // Validate arguments
            if (length < 1)
                throw new ArgumentOutOfRangeException(nameof(length), "Ring buffer length must be at least 1!");
            
            // Initialize pool
            _buffer = new T[length];
            // _allowOverride = allowOverride;
        }


        #region Public API

        public void Push(T item)
        {
            if (!TryPush(item))
                throw new InvalidOperationException("Attempted to push to a full RingBuffer!");
        }

        public T Pop()
        {
            if(!TryPop(out T output))
                throw new InvalidOperationException("Attempted to pop from an empty RingBuffer!");

            return output;
        }

        public T Peek()
        {
            if(!TryPeek(out T output))
                throw new InvalidOperationException("Attempted to peek from an empty RingBuffer!");

            return output;
        }

        public bool TryPush(T item)
        {
            if (IsFull) return false;
            InternalPush(item);
            return true;
        }

        public bool TryPop(out T item)
        {
            if (IsEmpty)
            {
                item = default;
                return false;
            }

            item = InternalPull(true);
            return true;
        }

        public bool TryPeek(out T item)
        {
            if (IsEmpty)
            {
                item = default;
                return false;
            }

            item = InternalPull(false);
            return true;
        }

        #endregion


        #region Internal API

        // Pushes item to RingBuffer without any checks
        internal void InternalPush(T item)
        {
            throw new NotImplementedException();
        }

        // Pulls next item from RingBuffer, optionally removes item from buffer (Pop & Peek implementation)
        internal T InternalPull(bool removeItem)
        {
            throw new NotImplementedException();
        }
        
        #endregion
        

        #region Collection implementation methods

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T item) => Push(item);
        
        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(T item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        #endregion


        #region Enumerator

        public class Enumerator
        {
            
        }

        #endregion
    }
}