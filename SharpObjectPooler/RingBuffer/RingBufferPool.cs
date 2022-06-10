// using System;
//
// namespace LambdaTheDev.SharpObjectPooler.RingBuffer
// {
//     public class RingBufferPool<T> : IBulkObjectPool<T>
//     {
//         public int Count { get; }
//         public int MaxCapacity { get; }
//         public T Rent()
//         {
//             throw new NotImplementedException();
//         }
//
//         public void Return(T item)
//         {
//             throw new NotImplementedException();
//         }
//
//         public int RentBulk(ArraySegment<T> outputArray)
//         {
//             throw new NotImplementedException();
//         }
//
//         public void ReturnBulk(ArraySegment<T> inputArray)
//         {
//             throw new NotImplementedException();
//         }
//     }
// }