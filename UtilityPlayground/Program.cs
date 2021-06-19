using System;
using Utility;

namespace UtilityPlayground
{
    public class Human : IEquatable<Human>
    {
        public string Name { get; set; }
        public int Age { get; set; }

        public override string ToString()
        {
            return $"Name: {Name}, Age: {Age}";
        }

        public bool Equals(Human other)
        {
            return Name == other.Name && Age == other.Age;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var rand = new Random(Guid.NewGuid().GetHashCode());

            var rs = new RandomSelector<Human>(rand);
            rs.Add(new Human { Name = "aaa", Age = 10 }, 100);
            rs.Add(new Human { Name = "aaa", Age = 10 }, 0.1);
            rs.Add(new Human { Name = "bbb", Age = 10 }, 0.1);
            rs.Add(new Human { Name = "ccc", Age = 10 }, 0.1);
            Console.WriteLine(rs);
            Console.WriteLine();

            var rs2 = new RandomSelector<Human>(rs);
            Console.WriteLine(rs2.Pick());
            Console.WriteLine();

            Console.WriteLine(nameof(rs));
            Console.WriteLine(rs);
            Console.WriteLine();
            Console.WriteLine(nameof(rs2));
            Console.WriteLine(rs2);
        }
    }
}
