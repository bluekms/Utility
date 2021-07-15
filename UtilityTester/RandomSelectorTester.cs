using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utility.RandomSelect;

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

            var rsBuilder = new RandomSelectorBuilder<int>(addCount);
            for (int i = 0; i < addCount; ++i)
            {
                rsBuilder.Add(i, 1);
            }

            var rs = rsBuilder.Create();
            Assert.AreEqual(rs.Count, addCount);
        }

        [TestMethod]
        public void PickThrowTest()
        {
            var rsBuilder = new RandomSelectorBuilder<int>(0);
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
            var rsBuilder = new RandomSelectorBuilder<GameUnit>((int)GameUnit.End);
            rsBuilder.Add(GameUnit.Rock, 50);
            rsBuilder.Add(GameUnit.Paper, 25);
            rsBuilder.Add(GameUnit.Scissors, 25);

            var rSelector = rsBuilder.Create();
            int getCount = 100000;
            var dic = new Dictionary<GameUnit, int>();
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
            for (int i = 0; i < 10; ++i)
            {
                var rsBuilder = new RandomSelectorBuilder<GameUnit>((int)GameUnit.End);
                rsBuilder.Add(GameUnit.Rock, 50);
                rsBuilder.Add(GameUnit.Paper, 25);
                rsBuilder.Add(GameUnit.Scissors, 25);

                int getCount = 100000;
                var dic = new Dictionary<GameUnit, int>();
                for (int j = 0; j < getCount; ++j)
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
