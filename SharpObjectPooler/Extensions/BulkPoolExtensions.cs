using System;
using System.Runtime.CompilerServices;

namespace LambdaTheDev.SharpObjectPooler.Extensions
{
    // Extension methods for IBulkPool
    public static class BulkPoolExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int RentBulk<T>(this IBulkPool<T> pool, T[] items)
            => pool.RentBulk(items, 0, items.Length);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int RentBulk<T>(this IBulkPool<T> pool, ArraySegment<T> items)
            => pool.RentBulk(items.Array, items.Offset, items.Count);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ReturnBulk<T>(this IBulkPool<T> pool, T[] items)
            => pool.ReturnBulk(items, 0, items.Length);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ReturnBulk<T>(this IBulkPool<T> pool, ArraySegment<T> items)
            => pool.ReturnBulk(items.Array, items.Offset, items.Count);
    }
}