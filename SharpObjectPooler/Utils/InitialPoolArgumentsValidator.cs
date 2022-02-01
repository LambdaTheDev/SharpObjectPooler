using System;

namespace LambdaTheDev.SharpObjectPooler.Utils
{
    // Helper class used to validate (mostly constructor) arguments
    public static class InitialPoolArgumentsValidator
    {
        // Validates initial capacity & max capacity values, throws proper exception if data are incorrect
        public static void ValidatePoolArgs(int initialCapacity, int maxCapacity)
        {
            if(initialCapacity < 0)
                throw new ArgumentOutOfRangeException(nameof(initialCapacity), "Initial capacity must be greater or equal to zero!");
            
            if(maxCapacity == 0 || maxCapacity < -1)
                throw new ArgumentOutOfRangeException(nameof(maxCapacity), "Max capacity must be -1, for infinite pools, or greater than zero!");
            
            if(maxCapacity != -1 && maxCapacity < initialCapacity)
                throw new ArgumentOutOfRangeException(nameof(maxCapacity), "Max capacity must be greater or equal to initial capacity!");
        }
        
        // Validates generator method & throws exception if something is wrong
        public static void ValidateGenerator<T>(Func<T> generator)
        {
            if(generator == null)
                throw new ArgumentNullException(nameof(generator), "Generator method cannot be null!");
        }
    }
}