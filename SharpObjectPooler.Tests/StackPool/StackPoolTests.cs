using LambdaTheDev.SharpObjectPooler.Pools;
using NUnit.Framework;

namespace LambdaTheDev.SharpObjectPooler.Tests.StackPool
{
    [TestFixture]
    public class StackPoolTests
    {
        private readonly IBulkPool<int> _notThreadSafePool = new StackPool<int>(10);
        private readonly IBulkPool<int> _threadSafePool = new StackPool<int>(10, -1, true);


        [SetUp]
        public void SuperFirstTest()
        {
        }

        [Test]
        public void PushTestA()
        {
            int countA = _notThreadSafePool.Count;
            int countB = _threadSafePool.Count;
            
            _notThreadSafePool.Return(101);
            _threadSafePool.Return(101);
            
            Assert.True(_notThreadSafePool.Count == ++countA && _threadSafePool.Count == ++countB);
        }

        [Test]
        public void PushTestB()
        {
            int countA = _notThreadSafePool.Count;
            int countB = _threadSafePool.Count;
            int[] array = { 23, 43, 54};

            _notThreadSafePool.ReturnBulk(array, 0, array.Length);
            _threadSafePool.ReturnBulk(array, 0, array.Length);
            
            Assert.True(_notThreadSafePool.Count == (countA + array.Length) && _threadSafePool.Count == (countB + array.Length));
        }

        [Test]
        public void RentTestA()
        {
            int countA = _notThreadSafePool.Count;
            int countB = _threadSafePool.Count;

            int x = _notThreadSafePool.Rent();
            int y = _threadSafePool.Rent();
            
            Assert.True(_notThreadSafePool.Count == (countA - 1) && _threadSafePool.Count == (countB - 1));
        }

        [Test]
        public void RentTestB()
        {
            int countA = _notThreadSafePool.Count;
            int countB = _threadSafePool.Count;
            
            int[] arrayA = new int[25];
            arrayA[24] = 32; // Just to ensure renting works
            arrayA[12] = 32;
            arrayA[23] = 32;

            int[] arrayB = new int[25];
            arrayB[24] = 32;
            arrayB[12] = 32;
            arrayB[23] = 32; 
            
            int rentedA = _notThreadSafePool.RentBulk(arrayA, 0, arrayA.Length);
            int rentedB = _threadSafePool.RentBulk(arrayB, 0, arrayB.Length);


            Assert.True((rentedA == rentedB) && rentedA == 25);
            Assert.True(arrayA[24] == 0 && arrayB[24] == 0); // new int() == 0
            Assert.True(_notThreadSafePool.Count == 0 && _threadSafePool.Count == 0);
        }
    }
}