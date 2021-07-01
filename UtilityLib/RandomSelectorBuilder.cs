using System;
using System.Collections.Generic;

namespace Utility
{
    public class RandomSelectorBuilder<T>
    {
        private readonly List<T> items;
        private readonly List<double> weights;
        private bool isCreated;

        public RandomSelectorBuilder()
        {
            isCreated = false;
            items = new List<T>();
            weights = new List<double>();
        }

        public RandomSelectorBuilder(int capacity)
        {
            items = new List<T>(capacity);
            weights = new List<double>(capacity);
        }

        public void Add(T item, double weight)
        {
            if (isCreated)
            {
                throw new InvalidOperationException("Already Created");
            }

            if (weight < 0)
            {
                throw new ArgumentException("Weight must be bigger than zero");
            }

            int index = items.IndexOf(item);
            if (index < 0)
            {
                items.Add(item);
                weights.Add(weight);
            }
            else
            {
                weights[index] += weight;
            }
        }

        public RandomSelector<T> Create()
        {
            isCreated = true;
            return new RandomSelector<T>(items, weights);
        }

        public RandomPicker<T> CreatePicker()
        {
            isCreated = true;
            return new RandomPicker<T>(items, weights);
        }
    }
}
