using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utility;

namespace UtilityTester
{
    [TestClass]
    public class RandomSelectorTester
    {
        [TestMethod]
        public void AddTest()
        {
            int addCount = 100;
            var rs = InitInt(addCount);
            Assert.AreEqual(rs.Count, addCount);

            for (int i = 0; i < addCount; ++i)
            {
                if (i % 2 == 0)
                {
                    rs.Add(i, 0.1);
                }
                else
                {
                    rs.Add(i * 100, 0.1);
                }
            }

            Assert.AreEqual(rs.Count, addCount * 1.5);
        }

        [TestMethod]
        public void DeepCopyPickTest()
        {
            int addCount = 100;
            var rs1 = InitInt(addCount);
            var rs2 = new RandomSelector<int>(rs1);
            Assert.AreEqual(rs1.Count, rs2.Count);

            for (int i = 0; i < addCount * 0.5; ++i)
            {
                rs2.Pick();
            }

            Assert.AreNotEqual(rs1.Count, rs2.Count);
            Assert.AreEqual(rs2.Count, addCount * 0.5);
        }

        [TestMethod]
        public void ObjectAddTest()
        {
            int addCount = 100;
            var rs1 = InitObject(addCount);
            Assert.AreEqual(rs1.Count, addCount);

            for (int i = 0; i < addCount * 0.5; ++i)
            {

            }
        }

        private RandomSelector<int> InitInt(int count)
        {
            var rand = new Random(Guid.NewGuid().GetHashCode());
            var rs = new RandomSelector<int>(rand);
            for (int i = 0; i < count; ++i)
            {
                rs.Add(i, 0.1);
            }

            return rs;
        }

        private RandomSelector<Human> InitObject(int count)
        {
            var rand = new Random(Guid.NewGuid().GetHashCode());
            var rs = new RandomSelector<Human>(rand);
            for (int i = 0; i < count; ++i)
            {
                string name = Guid.NewGuid().ToString().Substring(0, 3);
                rs.Add(new Human { Name = name, Age = i }, 0.1);
            }

            return rs;
        }
    }
}
