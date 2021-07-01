using System;
using System.Collections.Generic;

namespace Utility
{
    public class RandomPicker<T>
    {
        private readonly Random rand;
        private readonly List<T> itemList;
        private readonly List<double> ratioSumList;

        public int Count => itemList.Count;

        internal RandomPicker(IReadOnlyList<T> itemList, IReadOnlyList<double> ratioList)
        {
            rand = new Random();
            this.itemList = new(itemList);

            ratioSumList = new List<double>();
            for (int i = 0; i < ratioList.Count; ++i)
            {
                double sum = ratioList[i];
                for (int j = 0; j < i; ++j)
                {
                    sum += ratioList[j];
                }

                ratioSumList.Add(sum);
            }
        }

        public T Pick()
        {
            if (itemList.Count == 0)
            {
                throw new InvalidOperationException("Item Empty");
            }

            int index = GetRandomIndex();
            var item = itemList[index];

            double ratio;
            if (index == 0)
            {
                ratio = ratioSumList[index];
            }
            else
            {
                ratio = ratioSumList[index] - ratioSumList[index - 1];
            }

            for (int i = index; i < ratioSumList.Count; ++i)
            {
                ratioSumList[i] -= ratio;
            }

            itemList.RemoveAt(index);
            ratioSumList.RemoveAt(index);

            return item;
        }

        private int GetRandomIndex()
        {
            double value = rand.NextDouble() * ratioSumList[^1];

            // ref: https://docs.microsoft.com/ko-kr/dotnet/api/system.collections.generic.list-1.binarysearch?view=net-5.0
            int index = ratioSumList.BinarySearch(value);
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
