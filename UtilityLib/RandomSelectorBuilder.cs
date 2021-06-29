using System;
using System.Collections.Generic;

namespace Utility
{
    public class RandomSelectorBuilder<T>
    {
        private readonly Random rand;
        private readonly List<T> itemList;
        private readonly List<double> ratioList;

        public RandomSelectorBuilder(Random r = null)
        {
            rand = r;
            itemList = new List<T>();
            ratioList = new List<double>();
        }

        public RandomSelectorBuilder(int capacity, Random r = null)
        {
            rand = r;
            itemList = new List<T>(capacity);
            ratioList = new List<double>(capacity);
        }

        public void Add(T item, double ratio)
        {
            if (ratio < 0)
            {
                return;
            }

            int index = itemList.IndexOf(item);
            if (index < 0)
            {
                itemList.Add(item);
                ratioList.Add(ratio);
            }
            else
            {
                ratioList[index] += ratio;
            }
        }

        public RandomSelector<T> Build()
        {
            var ratioSumList = new List<double>();
            for (int i = 0; i < ratioList.Count; ++i)
            {
                double sum = ratioList[i];
                for (int j = 0; j < i; ++j)
                {
                    sum += ratioList[j];
                }

                ratioSumList.Add(sum);
            }

            return new RandomSelector<T>(rand ?? new Random(), new List<T>(itemList), ratioSumList);
        }
    }
}
