using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utility.RandomSelect;

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
            var rsBuilder = new RandomSelectorBuilder<Human>(addCount);
            for (int i = 0; i < addCount; ++i)
            {
                string name = $"{Guid.NewGuid().ToString().Substring(0, 3)}_{i}";
                int age = ageRand.Next(0, 100);
                rsBuilder.Add(new Human { Name = name, Age = age }, 1);
            }

            var rs = rsBuilder.Create();
            Assert.AreEqual(rs.Count, addCount);
        }

        [TestMethod]
        public void PickThrowTest()
        {
            var rsBuilder = new RandomSelectorBuilder<Human>(0);
            var rPicker = rsBuilder.CreatePicker();
            bool exception = false;
            try
            {
                rPicker.Pick();
            }
            catch
            {
                exception = true;
            }

            Assert.IsTrue(exception);
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

            var rsBuilder = new RandomSelectorBuilder<Human>(3);
            rsBuilder.Add(list[0], 50);
            rsBuilder.Add(list[1], 25);
            rsBuilder.Add(list[2], 25);

            var rSelector = rsBuilder.Create();
            int getCount = 100000;
            var dic = new Dictionary<Human, int>();
            for (int i = 0; i < getCount; ++i)
            {
                var item = rSelector.Get();
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
                var rsBuilder = new RandomSelectorBuilder<Human>(3);
                rsBuilder.Add(list[0], 50);
                rsBuilder.Add(list[1], 25);
                rsBuilder.Add(list[2], 25);

                int getCount = 100000;
                var dic = new Dictionary<Human, int>();
                for (int i = 0; i < getCount; ++i)
                {
                    var rPicker = rsBuilder.CreatePicker();
                    var item = rPicker.Pick();
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
