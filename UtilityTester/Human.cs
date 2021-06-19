using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utility;

namespace UtilityTester
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

        public override bool Equals(object obj)
        {
            return Equals(obj as Human);
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
}
