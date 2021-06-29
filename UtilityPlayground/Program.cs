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
                    default: break;
                }

                double currProb = dic[key] / (double)getCount;
                double diff = Math.Abs(srcProb - currProb);
                Console.WriteLine($"{key.ToString().Substring(0, 1)}: diff: {Math.Round(diff, 6)}. Success: {diff < 0.005}");
            }
        }
    }
}
