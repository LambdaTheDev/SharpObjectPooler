using System.Collections.Generic;

namespace LambdaTheDev.SharpObjectPooler
{
    // Interface for pools that support renting/returning multiple objects at once
    public interface IBulkPool<T> : IPool<T>
    {
        // Rents objects from a pool into output array, from offset to count. Returns how much objects were successfully returned
        int RentBulk(T[] outputArray, int offset, int count);
        
        // Puts objects from inputArray, from offset too count into a pool. Returns count of objects that were successfully returned
        int ReturnBulk(T[] inputArray, int offset, int count);
    }
}