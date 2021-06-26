using System;
using System.Collections.Generic;
using System.Linq;
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

            var rs = CreateRandomSelector<Human>(3);
            rs.Add(list[0], 1);
            rs.Add(list[1], 1);
            rs.Add(list[2], 1);

            var getProb = rs.GetProbability(item => item.Name == "AAA");
            Console.WriteLine(getProb);
        }

        private static RandomSelector<T> CreateRandomSelector<T>(int count)
        {
            var rand = new Random(Guid.NewGuid().GetHashCode());
            return new RandomSelector<T>(rand, count);
        }
    }
}
