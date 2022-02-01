using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace LambdaTheDev.SharpObjectPooler.Extensions
{
    // Workaround for .NET Standard 2.0's Stack.TryPop
    public static class StackExtensions
    {
#if NETSTANDARD2_0

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryPop<T>(this Stack<T> stack, out T item)
        {
            if (stack.Count > 0)
            {
                item = stack.Pop();
                return true;
            }

            item = default;
            return false;
        }
        
#endif
    }
}