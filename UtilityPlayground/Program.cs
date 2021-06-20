using System;
using System.Collections.Generic;
using Utility;
using UtilityTester;

namespace UtilityPlayground
{
    internal class Program
    {
        private static void Main()
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

            for (int i = 0; i < 5; ++i)
            {
                Console.WriteLine($"=== {i}차시 === ({rs.Count})");

                int getCount = 1000000;
                var dic = new Dictionary<CharacterCard.StarGrade, List<CharacterCard>>(getCount);
                for (int j = 0; j < getCount; ++j)
                {
                    var card = rs.Get();
                    if (dic.ContainsKey(card.Star))
                    {
                        dic[card.Star].Add(card);
                    }
                    else
                    {
                        var list = new List<CharacterCard> { card };
                        dic.Add(card.Star, list);
                    }
                }

                for (var star = CharacterCard.StarGrade.Star1; star < CharacterCard.StarGrade.End; ++star)
                {
                    var cardList = dic[star];
                    var diff = percent[(int)star] * 0.01 - cardList.Count / (double)getCount;
                    if (diff < 0)
                    {
                        diff *= -1;
                    }

                    Console.WriteLine($"{star}: {cardList.Count}\t{cardList.Count / (double)getCount}");
                }

                Console.WriteLine();
            }
        }
    }
}
