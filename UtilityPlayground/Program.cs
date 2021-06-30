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
            for (int l = 0; l < 10; ++l)
            {
                var rsb = new RandomSelectorBuilder<GameUnit>((int)GameUnit.End);
                rsb.Add(GameUnit.Rock, 50);
                rsb.Add(GameUnit.Paper, 25);
                rsb.Add(GameUnit.Scissors, 25);

                var rs = rsb.Create();
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
                                    case GameUnit.Rock: Console.WriteLine("ERROR"); return;
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
                                    case GameUnit.Paper: Console.WriteLine("ERROR"); return;
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
                                    case GameUnit.Scissors: Console.WriteLine("ERROR"); return;
                                    default: break;
                                }
                            }

                            break;

                        default:
                            break;
                    }

                    double currProb = dic[key] / (double)getCount;
                    double diff = Math.Abs(srcProb - currProb);
                    Console.WriteLine($"{l}Â÷½Ã. Pick: {pickedItem.ToString().Substring(0, 1)}, Key: {key.ToString().Substring(0, 1)}, Count: {dic[key]}, Result: {diff < 0.005}");
                }
            }
        }
    }
}
