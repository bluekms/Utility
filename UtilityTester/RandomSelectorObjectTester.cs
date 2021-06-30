using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utility;

namespace UtilityTester
{
    [TestClass]
    public class RandomSelectorObjectTester
    {
        private const double ProbabilitySensitivity = 0.005D;

        [TestMethod]
        public void AddTest()
        {
            int addCount = 100;
            var ageRand = new Random();
            var rsb = new RandomSelectorBuilder<Human>(addCount);
            for (int i = 0; i < addCount; ++i)
            {
                string name = $"{Guid.NewGuid().ToString().Substring(0, 3)}_{i}";
                int age = ageRand.Next(0, 100);
                rsb.Add(new Human { Name = name, Age = age }, 1);
            }

            var rs = rsb.Build();
            Assert.AreEqual(rs.Count, addCount);
        }

        [TestMethod]
        public void PickThrowTest()
        {
            var rsb = new RandomSelectorBuilder<Human>(0);
            var rs = rsb.Build();
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
            var ageRand = new Random();
            var rsb = new RandomSelectorBuilder<Human>(addCount);
            for (int i = 0; i < addCount; ++i)
            {
                string name = $"{Guid.NewGuid().ToString().Substring(0, 3)}_{i}";
                int age = ageRand.Next(0, 100);
                rsb.Add(new Human { Name = name, Age = age }, 1);
            }

            var rs1 = rsb.Build();
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

            var rsb = new RandomSelectorBuilder<Human>(3);
            rsb.Add(list[0], 50);
            rsb.Add(list[1], 25);
            rsb.Add(list[2], 25);

            var rs = rsb.Build();
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

                double srcProb = 0;
                switch (key.Name)
                {
                    case "AAA": srcProb = 0.5; break;
                    case "BBB": srcProb = 0.25; break;
                    case "CCC": srcProb = 0.25; break;
                    default: break;
                }

                double currProb = dic[key] / (double)getCount;
                double diff = Math.Abs(srcProb - currProb);
                Assert.IsTrue(diff < 0.005);
            }
        }
    }
}
