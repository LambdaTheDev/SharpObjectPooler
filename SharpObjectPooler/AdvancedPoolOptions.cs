using System;

namespace LambdaTheDev.SharpObjectPooler
{
    // Wrapper for advanced pool options
    public readonly struct AdvancedPoolOptions<T>
    {
        public readonly bool IsThreadSafe;
        public readonly Action<T> OnRent;
        public readonly Action<T> OnReturn;


        public AdvancedPoolOptions(bool isThreadSafe, Action<T> onRent = null, Action<T> onReturn = null)
        {
            IsThreadSafe = isThreadSafe;
            OnRent = onRent;
            OnReturn = onReturn;
        }
    }
}