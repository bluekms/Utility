using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utility
{
    public class RandomSelector<T>
    {
        private Random rand;
        private List<T> itemList;
        private List<double> ratioList;

        public override string ToString()
        {
            var sb = new StringBuilder();
            for (var i = 0; i < itemList.Count; ++i)
            {
                sb.AppendLine($"{itemList[i]} : {ratioList[i]}");
            }

            return sb.ToString();
        }

        public RandomSelector(Random r)
        {
            rand = r;
            itemList = new List<T>();
            ratioList = new List<double>();
        }

        public RandomSelector(Random r, int capacity)
        {
            rand = r;
            itemList = new List<T>(capacity);
            ratioList = new List<double>(capacity);
        }

        public RandomSelector(RandomSelector<T> src)
        {
            if (src == null)
            {
                throw new ArgumentNullException(nameof(src));
            }

            rand = src.rand;
            itemList = new(src.itemList);
            ratioList = new(src.ratioList);
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

        public T Get()
        {
            if (itemList.Count == 0)
            {
                return default;
            }

            return itemList[GetRandomIndex()];
        }

        public T Pick()
        {
            if (itemList.Count == 0)
            {
                return default;
            }

            int index = GetRandomIndex();
            T item = itemList[index];

            itemList.RemoveAt(index);
            ratioList.RemoveAt(index);

            return item;
        }

        private int GetRandomIndex()
        {
            int index = 0;
            double value = rand.NextDouble() * ratioList.Sum();
            while (value <= 0)
            {
                value -= ratioList[index++];
                index %= ratioList.Count;
            }

            return index;
        }
    }
}
