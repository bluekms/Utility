using System;
using Utility;
using UtilityTester;

namespace UtilityPlayground
{
    internal class Program
    {
        private static void Main()
        {
            // var rand = new Random(Guid.NewGuid().GetHashCode());
            // var rs = new RandomSelector<Human>(rand);
            // rs.Add(new Human { Name = "aaa", Age = 10 }, 100);
            // rs.Add(new Human { Name = "aaa", Age = 10 }, 0.1);
            // rs.Add(new Human { Name = "bbb", Age = 10 }, 0.1);
            // rs.Add(new Human { Name = "ccc", Age = 10 }, 0.1);
            // Console.WriteLine(rs);
            // Console.WriteLine();
            // var rs2 = new RandomSelector<Human>(rs);
            // Console.WriteLine(rs2.Pick());
            // Console.WriteLine();
            // Console.WriteLine(nameof(rs));
            // Console.WriteLine(rs);
            // Console.WriteLine();
            // Console.WriteLine(nameof(rs2));
            // Console.WriteLine(rs2);

            int addCount = 100;

            var rand = new Random(Guid.NewGuid().GetHashCode());
            var rs = new RandomSelector<int>(rand);
            for (int i = 0; i < addCount; ++i)
            {
                rs.Add(i, 0.1);
            }

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

            Console.WriteLine(rs.Count);
            Console.WriteLine(addCount * 1.5);
            Console.WriteLine(rs.Count == addCount * 1.5);
            Console.WriteLine(rs.Count == (int)(addCount * 1.5));
        }
    }
}
