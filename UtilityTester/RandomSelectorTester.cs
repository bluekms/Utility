using System;
using System.Collections.Generic;
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
                Assert.IsTrue(diff < 0.005);
            }
        }
    }
}
