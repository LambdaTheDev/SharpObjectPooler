using System.Runtime.CompilerServices;
#if !NETSTANDARD2_0
using System.Buffers;
#endif

namespace LambdaTheDev.SharpObjectPooler.ArrayPool
{
    // Simple utility that wraps C#'s ArrayPool if possible, or uses my custom implementation
    public static class FrameworkSafeArrayPool<T>
    {
#if NETSTANDARD2_0
        // If .NET Standard, use my implementation
        private static readonly Net2ArrayPoolImpl<T> Pool = new Net2ArrayPoolImpl<T>();
#endif
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] Rent(int length)
        {
#if NETSTANDARD2_0
            return Pool.Rent(length);
#else
            return ArrayPool<T>.Shared.Rent(length);
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Return(T[] array)
        {
#if NETSTANDARD2_0
            Pool.Return(array);
#else
            ArrayPool<T>.Shared.Return(array);
#endif
        }
    }
}