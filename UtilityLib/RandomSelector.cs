using System;
using System.Collections.Generic;

namespace Utility.RandomSelect
{
    public class RandomSelector<T>
    {
        private readonly Random random;
        private readonly List<T> items;
        private readonly List<double> weightCumulations;

        public int Count => items.Count;

        internal RandomSelector(IReadOnlyDictionary<T, double> itemWeights)
        {
            random = new Random();
            items = new List<T>();
            weightCumulations = new List<double>();

            double cumulating = 0D;
            foreach (var (item, weight) in itemWeights)
            {
                items.Add(item);

                cumulating += weight;
                weightCumulations.Add(cumulating);
            }
        }

        public T Get()
        {
            if (items.Count == 0)
            {
                throw new InvalidOperationException("Item Empty");
            }

            return items[GetRandomIndex()];
        }

        private int GetRandomIndex()
        {
            double value = random.NextDouble() * weightCumulations[^1];

            // ref: https://docs.microsoft.com/ko-kr/dotnet/api/system.collections.generic.list-1.binarysearch?view=net-5.0
            int index = weightCumulations.BinarySearch(value);
            if (index < 0)
            {
                return ~index;
            }
            else
            {
                return index;
            }
        }
    }
}
