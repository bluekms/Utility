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
            var rs = InitInt(addCount);
            Assert.AreEqual(rs.Count, addCount);

            for (int i = 0; i < addCount; ++i)
            {
                if (i % 2 == 0)
                {
                    // add prob
                    rs.Add(i, 0.1);
                }
                else
                {
                    // new item
                    rs.Add(i * 100, 0.1);
                }
            }

            Assert.AreEqual(rs.Count, addCount * 1.5);
        }

        [TestMethod]
        public void DeepCopyPickTest()
        {
            int addCount = 100;
            var rs1 = InitInt(addCount);
            var rs2 = new RandomSelector<int>(rs1);
            Assert.AreEqual(rs1.Count, rs2.Count);

            for (int i = 0; i < addCount * 0.5; ++i)
            {
                rs2.Pick();
            }

            Assert.AreNotEqual(rs1.Count, rs2.Count);
            Assert.AreEqual(rs2.Count, addCount * 0.5);
        }

        [TestMethod]
        public void ProbabilityTest()
        {
            var rs = CreateRandomSelector<GameUnit>((int)GameUnit.End);

            rs.Add(GameUnit.Rock, 50);
            rs.Add(GameUnit.Paper, 25);
            rs.Add(GameUnit.Scissors, 25);

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
            var rs = CreateRandomSelector<GameUnit>((int)GameUnit.End);

            rs.Add(GameUnit.Rock, 1 / 3D);
            rs.Add(GameUnit.Paper, 100);
            rs.Add(GameUnit.Scissors, 10000);

            double ratioSum = (1 / 3D) + 100 + 10000;
            double targetProb = (1 / 3D) / ratioSum;

            double getProb = rs.GetProbability(GameUnit.Rock);
            double diff = Math.Abs(targetProb - getProb);
            Assert.IsTrue(diff < double.Epsilon);

            RandomSelector<GameUnit> rs2 = null;
            GameUnit item = GameUnit.End;
            while (true)
            {
                rs2 = new RandomSelector<GameUnit>(rs);
                item = rs2.Pick();
                if (item != GameUnit.Rock)
                {
                    break;
                }
            }

            var srcRatio = rs.GetRatio(item);
            ratioSum -= srcRatio;
            targetProb = (1 / 3D) / ratioSum;

            getProb = rs2.GetProbability(GameUnit.Rock);
            diff = Math.Abs(targetProb - getProb);
            Assert.IsTrue(diff < double.Epsilon);
        }

        private static RandomSelector<T> CreateRandomSelector<T>(int count)
        {
            var rand = new Random(Guid.NewGuid().GetHashCode());
            return new RandomSelector<T>(rand, count);
        }

        private static RandomSelector<int> InitInt(int count)
        {
            var rs = CreateRandomSelector<int>(count);
            for (int i = 0; i < count; ++i)
            {
                rs.Add(i, 0.1);
            }

            return rs;
        }
    }
}
