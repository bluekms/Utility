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
            var rs = InitInt(addCount);
            Assert.AreEqual(rs.Count, addCount);

            for (int i = 0; i < addCount; ++i)
            {
                if (i % 2 == 0)
                {
                    rs.Add(i, 0.1);
                }
                else
                {
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
            // star  | percent | count | probability
            //     5 |     0.9 |    10 |    0.09
            //     4 |     4.1 |    40 |    0.1025
            //     3 |      35 |   100 |    0.35
            //     2 |      35 |   150 |    0.23333
            //     1 |      25 |   200 |    0.125
            // total |     100 |   500 |    0.2
            double[] percent = { 25, 35, 35, 4.1, 0.9 };
            double[] count = { 200, 150, 100, 40, 10 };
            double[] probability =
            {
                percent[(int)CharacterCard.StarGrade.Star1] / count[(int)CharacterCard.StarGrade.Star1],
                percent[(int)CharacterCard.StarGrade.Star2] / count[(int)CharacterCard.StarGrade.Star2],
                percent[(int)CharacterCard.StarGrade.Star3] / count[(int)CharacterCard.StarGrade.Star3],
                percent[(int)CharacterCard.StarGrade.Star4] / count[(int)CharacterCard.StarGrade.Star4],
                percent[(int)CharacterCard.StarGrade.Star5] / count[(int)CharacterCard.StarGrade.Star5],
            };

            var rand = new Random(Guid.NewGuid().GetHashCode());
            var rs = new RandomSelector<CharacterCard>(rand);

            int id = 0;
            for (var star = CharacterCard.StarGrade.Star1; star < CharacterCard.StarGrade.End; ++star)
            {
                for (int i = 0; i < count[(int)star]; ++i)
                {
                    rs.Add(new CharacterCard { Id = id++, Star = star }, probability[(int)star]);
                }
            }

            int getCount = 100000;
            var dic = new Dictionary<CharacterCard.StarGrade, int>(getCount);
            for (int i = 0; i < getCount; ++i)
            {
                var card = rs.Get();
                if (dic.ContainsKey(card.Star))
                {
                    ++dic[card.Star];
                }
                else
                {
                    dic.Add(card.Star, 1);
                }
            }
        }

        private static RandomSelector<int> InitInt(int count)
        {
            var rand = new Random(Guid.NewGuid().GetHashCode());
            var rs = new RandomSelector<int>(rand);
            for (int i = 0; i < count; ++i)
            {
                rs.Add(i, 0.1);
            }

            return rs;
        }

        private static RandomSelector<Human> InitObject(int count)
        {
            var rand = new Random(Guid.NewGuid().GetHashCode());
            var rs = new RandomSelector<Human>(rand);
            for (int i = 0; i < count; ++i)
            {
                string name = Guid.NewGuid().ToString().Substring(0, 3);
                rs.Add(new Human { Name = name, Age = i }, 0.1);
            }

            return rs;
        }
    }
}
