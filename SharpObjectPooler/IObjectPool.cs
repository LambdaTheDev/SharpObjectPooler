namespace LambdaTheDev.SharpObjectPooler
{
    // Base interface for object pools. Just Rent & Return methods + some info
    public interface IObjectPool<T>
    {
        // Returns how much items are currently stored
        int Count { get; }
        
        // Returns how much items can be stored in a pool, or -1 if pool is infinite
        int MaxCapacity { get; }

        // Rents an item from a pool
        T Rent();

        // Returns item to a pool
        void Return(T item);
    }
}