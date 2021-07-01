using System;
using System.Collections.Generic;

namespace Utility
{
    public class RandomSelector<T>
    {
        private readonly Random random;
        private readonly List<T> items;
        private readonly List<double> weightCumulations;

        public int Count => items.Count;

        internal RandomSelector(IReadOnlyList<T> items, IReadOnlyList<double> weights)
        {
            random = new Random();
            this.items = new(items);

            var cumulation = 0D;
            weightCumulations = new List<double>();
            for (int i = 0; i < weights.Count; ++i)
            {
                cumulation += weights[i];
                weightCumulations.Add(cumulation);
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
