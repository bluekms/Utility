using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utility
{
    public class RandomSelector<T>
    {
        private readonly Random rand;
        private readonly List<T> itemList;
        private readonly List<double> ratioSumList;

        internal RandomSelector(Random r, List<T> itemList, List<double> ratioList)
        {
            rand = r;
            this.itemList = itemList;

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

        public RandomSelector(RandomSelector<T> src)
        {
            if (src == null)
            {
                throw new ArgumentNullException(nameof(src));
            }

            rand = src.rand;
            itemList = new List<T>(src.itemList);
            ratioSumList = new List<double>(src.ratioSumList);
        }

        public T Get()
        {
            if (itemList.Count == 0)
            {
                throw new InvalidOperationException("Item Empty");
            }

            return itemList[GetRandomIndex()];
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

        public int Count
        {
            get { return itemList.Count; }
        }

        private int GetRandomIndex()
        {
            double value = rand.NextDouble() * ratioSumList[^1];
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
