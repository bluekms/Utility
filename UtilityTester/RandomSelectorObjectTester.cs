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

        [TestMethod]
        public void PickThrowTest()
        {
            var rs = CreateRandomSelector<Human>(0);
            bool exception = false;
            try
            {
                rs.Pick();
            }
            catch
            {
                exception = true;
            }

            Assert.IsTrue(exception);
        }

        [TestMethod]
        public void DeepCopyPickTest()
        {
            int addCount = 100;
            var rs1 = InitObject(addCount);
            var rs2 = new RandomSelector<Human>(rs1);
            Assert.AreEqual(rs1.Count, rs2.Count);

            for (int i = 0; i < addCount / 2; ++i)
            {
                rs2.Pick();
            }

            Assert.AreNotEqual(rs1.Count, rs2.Count);
            Assert.AreEqual(rs2.Count, addCount / 2);
        }

        [TestMethod]
        public void ProbabilityTest()
        {
            var list = new List<Human>(3)
            {
                new Human { Name = "AAA", Age = 30 },
                new Human { Name = "BBB", Age = 20 },
                new Human { Name = "CCC", Age = 40 },
            };

            var rs = CreateRandomSelector<Human>(3);
            rs.Add(list[0], 50);
            rs.Add(list[1], 25);
            rs.Add(list[2], 25);

            int getCount = 100000;
            var dic = new Dictionary<Human, int>();
            for (int i = 0; i < getCount; ++i)
            {
                var item = rs.Get();
                if (dic.ContainsKey(item))
                {
                    ++dic[item];
                }
                else
                {
                    dic.Add(item, 1);
                }
            }

            foreach (var key in list)
            {
                if (!dic.ContainsKey(key))
                {
                    continue;
                }

                double srcProb = rs.GetProbability(key);
                double currProb = dic[key] / (double)getCount;
                double diff = Math.Abs(srcProb - currProb);
                Assert.IsTrue(diff < 0.005);
            }
        }

        [TestMethod]
        public void RatioAndProbabilityTest()
        {
            var list = new List<Human>(3)
            {
                new Human { Name = "AAA", Age = 30 },
                new Human { Name = "BBB", Age = 20 },
                new Human { Name = "CCC", Age = 40 },
            };

            var rs = CreateRandomSelector<Human>(3);
            rs.Add(list[0], 1 / 3D);
            rs.Add(list[1], 100);
            rs.Add(list[2], 10000);

            double ratioSum = (1 / 3D) + 100 + 10000;
            double targetProb = (1 / 3D) / ratioSum;

            double getProb = rs.GetProbability(list[0]);
            double diff = Math.Abs(targetProb - getProb);
            Assert.IsTrue(diff < double.Epsilon);

            Human item;
            RandomSelector<Human> rs2;
            while (true)
            {
                rs2 = new RandomSelector<Human>(rs);
                item = rs2.Pick();
                if (item != list[0])
                {
                    break;
                }
            }

            double srcRatio = rs.GetRatio(item);
            ratioSum -= srcRatio;
            targetProb = (1 / 3D) / ratioSum;

            getProb = rs2.GetProbability(list[0]);
            diff = Math.Abs(targetProb - getProb);
            Assert.IsTrue(diff < double.Epsilon);
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
