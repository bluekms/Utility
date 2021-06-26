using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utility;

namespace UtilityTester
{
    public class CharacterCard
    {
        public enum StarGrade
        {
            Star1 = 0,
            Star2 = 1,
            Star3 = 2,
            Star4 = 3,
            Star5 = 4,
            End,
        }

        public int Id { get; set; }
        public StarGrade Star { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}, Star: {Star}, Name: {Name}";
        }
    }
}
