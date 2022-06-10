namespace LambdaTheDev.SharpObjectPooler
{
    // Base interface for buffer pools (.NET's ArrayPool equivalent)
    public interface IBufferPool<T>
    {
        // Rents a buffer with length >= minimumLength
        T[] RentBuffer(int minimumLength);

        // Returns buffer to a pool, and optionally resets array content
        void ReturnBuffer(T[] item, bool resetBuffer);
    }
}