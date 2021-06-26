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
        private readonly List<double> ratioList;
        private double ratioSum;

        public RandomSelector(Random r)
        {
            rand = r;
            itemList = new List<T>();
            ratioList = new List<double>();
            ratioSum = 0;
        }

        public RandomSelector(Random r, int capacity)
        {
            rand = r;
            itemList = new List<T>(capacity);
            ratioList = new List<double>(capacity);
            ratioSum = 0;
        }

        public RandomSelector(RandomSelector<T> src)
        {
            if (src == null)
            {
                throw new ArgumentNullException(nameof(src));
            }

            rand = src.rand;
            itemList = new List<T>(src.itemList);
            ratioList = new List<double>(src.ratioList);
            ratioSum = src.ratioSum;
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
            ratioSum -= ratioList[index];

            itemList.RemoveAt(index);
            ratioList.RemoveAt(index);

            return item;
        }

        public int Count
        {
            get { return itemList.Count; }
        }

        public double GetRatio(T item)
        {
            int index = itemList.IndexOf(item);
            if (index < 0)
            {
                return index;
            }

            return ratioList[index];
        }

        public double GetRatio(Predicate<T> match)
        {
            int index = itemList.FindIndex(match);
            if (index < 0)
            {
                return index;
            }

            return ratioList[index];
        }

        public double GetProbability(T item)
        {
            return GetRatio(item) / ratioSum;
        }

        public double GetProbability(Predicate<T> match)
        {
            return GetRatio(match) / ratioSum;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            for (int i = 0; i < itemList.Count; ++i)
            {
                sb.AppendLine($"{itemList[i]} : {ratioList[i]}");
            }

            return sb.ToString();
        }

        private int GetRandomIndex()
        {
            int index = 0;
            double value = rand.NextDouble() * ratioSum;
            while (value > 0)
            {
                index = (++index) % ratioList.Count;
                value -= ratioList[index];
            }

            return index;
        }
    }
}
