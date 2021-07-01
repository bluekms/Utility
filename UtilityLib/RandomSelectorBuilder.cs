using System;
using System.Collections.Generic;

namespace Utility
{
    public class RandomSelectorBuilder<T>
    {
        private readonly List<T> itemList;
        private readonly List<double> ratioList;

        public RandomSelectorBuilder()
        {
            itemList = new List<T>();
            ratioList = new List<double>();
        }

        public RandomSelectorBuilder(int capacity)
        {
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

        public RandomSelector<T> Create()
        {
            return new(itemList, ratioList);
        }
    }
}
