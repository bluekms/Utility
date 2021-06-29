using System;
using System.Collections.Generic;

namespace Utility
{
    public class RandomSelectorBuilder<T>
    {
        private readonly Random rand;
        private readonly List<T> itemList;
        private readonly List<double> ratioList;
        private double ratioSum;

        public RandomSelectorBuilder(Random r = null)
        {
            rand = r;
            itemList = new List<T>();
            ratioList = new List<double>();
            ratioSum = 0;
        }

        public RandomSelectorBuilder(int capacity, Random r = null)
        {
            rand = r;
            itemList = new List<T>(capacity);
            ratioList = new List<double>(capacity);
            ratioSum = 0;
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

            ratioSum += ratio;
        }

        public RandomSelector<T> Build()
        {
            return new RandomSelector<T>(rand ?? new Random(), new List<T>(itemList), new List<double>(ratioList), ratioSum);
        }
    }
}
