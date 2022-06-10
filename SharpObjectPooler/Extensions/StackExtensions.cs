using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace LambdaTheDev.SharpObjectPooler.Extensions
{
    // Stack extension methods
    internal static class StackExtensions
    {
        // TryPop implementation that works across .NET versions
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool FrameworkSafeTryPop<T>(this Stack<T> stack, out T item)
        {
#pragma warning disable 162
#if NETSTANDARD2_1_OR_GREATER
            return stack.TryPop(out item);
#endif
            if (stack.Count == 0)
            {
                item = default;
                return false;
            }

            item = stack.Pop();
            return true;
#pragma warning restore
        }
    }
}