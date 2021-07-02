using System;
using System.Collections.Generic;

namespace Utility
{
    public class RandomSelectorBuilder<T>
    {
        private bool isCreated;
        private readonly Dictionary<T, double> itemWeights;

        public RandomSelectorBuilder()
        {
            isCreated = false;
            itemWeights = new Dictionary<T, double>();
        }

        public RandomSelectorBuilder(int capacity)
        {
            isCreated = false;
            itemWeights = new Dictionary<T, double>(capacity);
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

            if (!itemWeights.TryAdd(item, weight))
            {
                throw new ArgumentException($"An item has already been added. Item: {item}, Weight: {weight}");
            }
        }

        public RandomSelector<T> Create()
        {
            isCreated = true;
            return new RandomSelector<T>(itemWeights);
        }

        public RandomPicker<T> CreatePicker()
        {
            isCreated = true;
            return new RandomPicker<T>(itemWeights);
        }
    }
}