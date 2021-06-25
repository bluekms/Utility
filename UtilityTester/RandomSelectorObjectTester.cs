using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utility;

namespace UtilityTester
{
    [TestClass]
    public class RandomSelectorObjectTester
    {
        [TestMethod]
        public void AddTest()
        {
            int addCount = 100;
            var rs = InitObject(addCount);
            Assert.AreEqual(rs.Count, addCount);

            var rs2 = new RandomSelector<Human>(rs);
            int dupleCount = addCount / 2;
            for (int i = 0; i < dupleCount; ++i)
            {
            }
        }

        private static RandomSelector<T> CreateRandomSelector<T>(int count)
        {
            var rand = new Random(Guid.NewGuid().GetHashCode());
            return new RandomSelector<T>(rand, count);
        }

        private static RandomSelector<Human> InitObject(int count)
        {
            var rs = CreateRandomSelector<Human>(count);
            for (int i = 0; i < count; ++i)
            {
                string name = Guid.NewGuid().ToString().Substring(0, 3);
                rs.Add(new Human { Name = name, Age = i }, 0.1);
            }

            return rs;
        }
    }
}
