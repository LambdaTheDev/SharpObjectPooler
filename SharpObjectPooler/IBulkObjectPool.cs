using System;

namespace LambdaTheDev.SharpObjectPooler
{
    // Base interface for Object pools that support bulk renting & returning
    public interface IBulkObjectPool<T> : IObjectPool<T>
    {
        // Rents items from pool to wrapped array & returns how much items successfully rented
        int RentBulk(ArraySegment<T> outputArray);

        // Returns objects in provided array back to pool
        void ReturnBulk(ArraySegment<T> inputArray);
    }
}