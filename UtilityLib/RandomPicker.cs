using System;
using System.Collections.Generic;

namespace Utility
{
    public class RandomPicker<T>
    {
        private readonly Random random;
        private readonly List<T> items;
        private readonly List<double> weightCumulations;

        public int Count => items.Count;

        internal RandomPicker(IReadOnlyList<T> items, IReadOnlyList<double> weights)
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

        public T Pick()
        {
            if (items.Count == 0)
            {
                throw new InvalidOperationException("Item Empty");
            }

            int index = GetRandomIndex();
            var item = items[index];

            double ratio;
            if (index == 0)
            {
                ratio = weightCumulations[index];
            }
            else
            {
                ratio = weightCumulations[index] - weightCumulations[index - 1];
            }

            for (int i = index; i < weightCumulations.Count; ++i)
            {
                weightCumulations[i] -= ratio;
            }

            items.RemoveAt(index);
            weightCumulations.RemoveAt(index);

            return item;
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
