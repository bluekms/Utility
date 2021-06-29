using System;
using System.Collections.Generic;
using Utility;
using UtilityTester;

namespace UtilityPlayground
{
    internal enum GameUnit
    {
        Rock,
        Paper,
        Scissors,
        End,
    }

    internal class Program
    {
        private static void Main()
        {
            var list = new List<Human>(3)
            {
                new Human { Name = "AAA", Age = 30 },
                new Human { Name = "BBB", Age = 20 },
                new Human { Name = "CCC", Age = 40 },
            };

            var rsb = new RandomSelectorBuilder<Human>(new Random());
            rsb.Add(list[0], 1);
            rsb.Add(list[1], 1);
            rsb.Add(list[2], 1);

            var rs = rsb.Build();
            var getProb = rs.GetProbability(item => item.Name == "AAA");
            Console.WriteLine(getProb);
        }
    }
}
