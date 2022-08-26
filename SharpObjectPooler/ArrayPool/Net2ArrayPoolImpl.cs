using System;

namespace LambdaTheDev.SharpObjectPooler.ArrayPool
{
#if NETSTANDARD2_0
    
    // My simple ArrayPool implementation
    internal class Net2ArrayPoolImpl<T>
    {
        public T[] Rent(int size)
        {
            return Array.Empty<T>();
        }

        public void Return(T[] array)
        {
            
        }
    }
    
#endif
}