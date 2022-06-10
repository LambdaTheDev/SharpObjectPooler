using System;
using System.Runtime.CompilerServices;

namespace LambdaTheDev.SharpObjectPooler.Utils
{
    // Helper methods used to validate pool constructor arguments
    public static class PoolArgsValidators
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ValidateGenerator<T>(Func<T> generator)
        {
            if(generator == null) ThrowArgumentNull(nameof(generator), "Generator method cannot be null!");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ValidateCapacity(int initial, int max)
        {
            if(initial < 0)
                ThrowArgumentOutOfRange(nameof(initial), "Initial capacity must be a positive number!");
            
            if(max < 0 && max != -1)
                ThrowArgumentOutOfRange(nameof(max), "Max capacity must be a positive number, or -1 if pool is meant to be infinite!");
            
            if(max != -1 && initial > max)
                ThrowArgumentOutOfRange(nameof(initial), "Initial capacity cannot be larger than max capacity!");
        }

        private static void ThrowArgumentNull(string argName, string message)
        {
            throw new ArgumentNullException(argName, message);
        }

        private static void ThrowArgumentOutOfRange(string argName, string message)
        {
            throw new ArgumentOutOfRangeException(argName, message);
        }
    }
}