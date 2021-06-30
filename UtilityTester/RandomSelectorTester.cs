using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utility;

namespace UtilityTester
{
    [TestClass]
    public class RandomSelectorTester
    {
        private const double ProbabilitySensitivity = 0.005D;

        [TestMethod]
        public void AddTest()
        {
            int addCount = 100;

            var rsb = new RandomSelectorBuilder<int>(addCount);
            for (int i = 0; i < addCount; ++i)
            {
                rsb.Add(i, 1);
            }

            var rs = rsb.Build();
            Assert.AreEqual(rs.Count, addCount);
        }

        [TestMethod]
        public void PickThrowTest()
        {
            var rsb = new RandomSelectorBuilder<int>(0);
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
            var rsb = new RandomSelectorBuilder<int>(addCount);
            for (int i = 0; i < addCount; ++i)
            {
                rsb.Add(i, 1);
            }

            var rs1 = rsb.Build();
            var rs2 = new RandomSelector<int>(rs1);
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
            var rsb = new RandomSelectorBuilder<GameUnit>((int)GameUnit.End);
            rsb.Add(GameUnit.Rock, 50);
            rsb.Add(GameUnit.Paper, 25);
            rsb.Add(GameUnit.Scissors, 25);

            var rs = rsb.Build();
            int getCount = 100000;
            var dic = new Dictionary<GameUnit, int>();
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

            for (var key = GameUnit.Rock; key < GameUnit.End; ++key)
            {
                if (!dic.ContainsKey(key))
                {
                    continue;
                }

                double srcProb = 0;
                switch (key)
                {
                    case GameUnit.Rock: srcProb = 0.5; break;
                    case GameUnit.Paper: srcProb = 0.25; break;
                    case GameUnit.Scissors: srcProb = 0.25; break;
                    default: Assert.Fail(); break;
                }

                double currProb = dic[key] / (double)getCount;
                double diff = Math.Abs(srcProb - currProb);
                Assert.IsTrue(diff < ProbabilitySensitivity);
            }
        }

        [TestMethod]
        public void PickProbabilityTest()
        {
            for (int l = 0; l < 10; ++l)
            {
                var rsb = new RandomSelectorBuilder<GameUnit>((int)GameUnit.End);
                rsb.Add(GameUnit.Rock, 50);
                rsb.Add(GameUnit.Paper, 25);
                rsb.Add(GameUnit.Scissors, 25);

                var rs = rsb.Build();
                var pickedItem = rs.Pick();

                int getCount = 100000;
                var dic = new Dictionary<GameUnit, int>();
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

                for (var key = GameUnit.Rock; key < GameUnit.End; ++key)
                {
                    if (!dic.ContainsKey(key))
                    {
                        continue;
                    }

                    double srcProb = 0;
                    switch (pickedItem)
                    {
                        case GameUnit.Rock:
                            {
                                switch (key)
                                {
                                    case GameUnit.Rock: Assert.Fail(); return;
                                    case GameUnit.Paper: srcProb = 0.5; break;
                                    case GameUnit.Scissors: srcProb = 0.5; break;
                                    default: break;
                                }
                            }

                            break;

                        case GameUnit.Paper:
                            {
                                switch (key)
                                {
                                    case GameUnit.Rock: srcProb = 2 / 3D; break;
                                    case GameUnit.Paper: Assert.Fail(); return;
                                    case GameUnit.Scissors: srcProb = 1 / 3D; break;
                                    default: break;
                                }
                            }

                            break;

                        case GameUnit.Scissors:
                            {
                                switch (key)
                                {
                                    case GameUnit.Rock: srcProb = 2 / 3D; break;
                                    case GameUnit.Paper: srcProb = 1 / 3D; break;
                                    case GameUnit.Scissors: Assert.Fail(); return;
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
