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

            var rs = rsb.Create();
            Assert.AreEqual(rs.Count, addCount);
        }

        [TestMethod]
        public void PickThrowTest()
        {
            var rsb = new RandomSelectorBuilder<Human>(0);
            var rs = rsb.Create();
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

            var rs1 = rsb.Create();
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

            var rs = rsb.Create();
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
                Assert.IsTrue(diff < ProbabilitySensitivity);
            }
        }

        [TestMethod]
        public void PickProbabilityTest()
        {
            var list = new List<Human>(3)
            {
                new Human { Name = "AAA", Age = 30 },
                new Human { Name = "BBB", Age = 20 },
                new Human { Name = "CCC", Age = 40 },
            };

            for (int l = 0; l < 10; ++l)
            {
                var rsb = new RandomSelectorBuilder<Human>(3);
                rsb.Add(list[0], 50);
                rsb.Add(list[1], 25);
                rsb.Add(list[2], 25);

                var rs = rsb.Create();
                var pickedItem = rs.Pick();

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
                    switch (pickedItem.Name)
                    {
                        case "AAA":
                            {
                                switch (key.Name)
                                {
                                    case "AAA": Assert.Fail(); return;
                                    case "BBB": srcProb = 0.5; break;
                                    case "CCC": srcProb = 0.5; break;
                                    default: break;
                                }
                            }

                            break;

                        case "BBB":
                            {
                                switch (key.Name)
                                {
                                    case "AAA": srcProb = 2 / 3D; break;
                                    case "BBB": Assert.Fail(); return;
                                    case "CCC": srcProb = 1 / 3D; break;
                                    default: break;
                                }
                            }

                            break;

                        case "CCC":
                            {
                                switch (key.Name)
                                {
                                    case "AAA": srcProb = 2 / 3D; break;
                                    case "BBB": srcProb = 1 / 3D; break;
                                    case "CCC": Assert.Fail(); return;
                                    default: break;
                                }
                            }

                            break;

                        default:
                            break;
                    }

                    double currProb = dic[key] / (double)getCount;
                    double diff = Math.Abs(srcProb - currProb);
                    Assert.IsTrue(diff < ProbabilitySensitivity);
                }
            }
        }
    }
}
