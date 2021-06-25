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
            for (int i = 0; i < addCount; ++i)
            {
                if (i % 2 == 0)
                {
                    var item = rs2.Pick();
                    rs.Add(item, 0.1);
                }
                else
                {
                    string name = Guid.NewGuid().ToString().Substring(0, 3);
                    rs.Add(new Human { Name = name, Age = i }, 0.1);
                }
            }

            Assert.AreEqual(rs.Count, addCount * 1.5);
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
