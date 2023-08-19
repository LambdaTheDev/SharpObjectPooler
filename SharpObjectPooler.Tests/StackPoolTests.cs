using System;
using LambdaTheDev.SharpObjectPooler.Fifo;
using NUnit.Framework;

namespace LambdaTheDev.SharpObjectPooler.Tests
{
    [TestFixture]
    public class StackPoolTests
    {
        private readonly IBulkObjectPool<int> _notThreadSafePool = new StackPool<int>(Generator, 100);
        private readonly IBulkObjectPool<int> _threadSafePool = new StackPool<int>(Generator, 100, -1, new AdvancedPoolOptions<int>(true));
        
        private static int Generator() => 0;


        #region Generic test implementations

        private static void InternalRentTest(IBulkObjectPool<int> pool)
        {
            int currentCount = pool.Count;
            pool.Rent();
            Assert.True(pool.Count == --currentCount);
        }

        private static void InternalReturnTest(IBulkObjectPool<int> pool)
        {
            int currentCount = pool.Count;
            pool.Return(0);
            Assert.True(pool.Count == ++currentCount);
        }

        private static void InternalRentBulkTest(IBulkObjectPool<int> pool)
        {
            int currentCount = pool.Count;
            int[] rentBuffer = new int[5];
            pool.RentBulk(new ArraySegment<int>(rentBuffer));
            Assert.That(pool.Count == (currentCount - rentBuffer.Length));
        }

        private static void InternalReturnBulkTest(IBulkObjectPool<int> pool)
        {
            int currentCount = pool.Count;
            int[] returnBuffer = new int[5];
            pool.ReturnBulk(new ArraySegment<int>(returnBuffer));
            Assert.That(pool.Count == (currentCount + returnBuffer.Length));
        }
        
        #endregion


        #region NUnit test implementations

        [Test] public void RentTest() => InternalRentTest(_notThreadSafePool);
        [Test] public void RentThreadSafeTest() => InternalRentTest(_threadSafePool);

        [Test] public void ReturnTest() => InternalReturnTest(_notThreadSafePool);
        [Test] public void ReturnThreadSafeTest() => InternalReturnTest(_threadSafePool);

        [Test] public void RentBulkTest() => InternalRentBulkTest(_notThreadSafePool);
        [Test] public void RentBulkThreadSafeTest() => InternalRentBulkTest(_threadSafePool);

        [Test] public void ReturnBulkTest() => InternalRentBulkTest(_notThreadSafePool);
        [Test] public void ReturnBulkThreadSafeTest() => InternalReturnBulkTest(_threadSafePool);

        #endregion
    }
}