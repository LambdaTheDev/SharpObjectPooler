namespace LambdaTheDev.SharpObjectPooler
{
    // Base interface for all Pools
    public interface IPool<T>
    {
        // Returns how much objects are currently in a pool
        int Count { get; }
        
        // Gets or sets Pool's max capacity (or -1, if it's infinite)
        int MaxCapacity { get; set; }
        
        // Rents an object from a pool or creates a new one
        T Rent();
        
        // Returns object to a pool
        void Return(T item);
    }
}