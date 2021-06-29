using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utility;

namespace UtilityTester
{
    [TestClass]
    public class RandomSelectorTester
    {
        internal enum GameUnit
        {
            Rock,
            Paper,
            Scissors,
            End,
        }

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

                double srcProb = rs.GetProbability(key);
                double currProb = dic[key] / (double)getCount;
                double diff = Math.Abs(srcProb - currProb);
                Assert.IsTrue(diff < 0.005);
            }
        }

        [TestMethod]
        public void RatioAndProbabilityTest()
        {
            var rsb = new RandomSelectorBuilder<GameUnit>((int)GameUnit.End);
            rsb.Add(GameUnit.Rock, 1 / 3D);
            rsb.Add(GameUnit.Paper, 100);
            rsb.Add(GameUnit.Scissors, 10000);

            double ratioSum = (1 / 3D) + 100 + 10000;
            double targetProb = (1 / 3D) / ratioSum;

            var rs = rsb.Build();
            double getProb = rs.GetProbability(GameUnit.Rock);
            double diff = Math.Abs(targetProb - getProb);
            Assert.IsTrue(diff < double.Epsilon);

            GameUnit item;
            RandomSelector<GameUnit> rs2;
            while (true)
            {
                rs2 = new RandomSelector<GameUnit>(rs);
                item = rs2.Pick();
                if (item != GameUnit.Rock)
                {
                    break;
                }
            }

            double srcRatio = rs.GetRatio(item);
            ratioSum -= srcRatio;
            targetProb = (1 / 3D) / ratioSum;

            getProb = rs2.GetProbability(GameUnit.Rock);
            diff = Math.Abs(targetProb - getProb);
            Assert.IsTrue(diff < double.Epsilon);
        }
    }
}
