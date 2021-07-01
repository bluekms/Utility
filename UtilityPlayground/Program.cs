using System;
using System.Collections.Generic;
using System.Linq;

namespace UtilityPlayground
{
    internal class Program
    {
        public sealed class Picker<T>
        {
            private const int MaxRetries = 2;
            private const int CleanupTriggerDenominator = 4;

            public int Samples { get; set; }
            public int N { get; set; }

            private readonly Random random = new ();
            private List<T> items = new ();
            private List<double> weights = new ();
            private List<double> weightCumulations = new ();
            private int pickedCount;
            private List<bool> picked = new ();

            public Picker(IEnumerable<(T Item, double Weight)> pairs)
            {
                var cumulation = 0.0;
                foreach (var (item, weight) in pairs)
                {
                    cumulation += weight;
                    items.Add(item);
                    weights.Add(weight);
                    weightCumulations.Add(cumulation);
                    picked.Add(false);
                }
            }

            public T PickOn()
            {
                var i = GetRandomIndex();
                var count = 0;
                while (picked[i])
                {
                    if (++count == MaxRetries || pickedCount > items.Count / CleanupTriggerDenominator)
                    {
                        Cleanup();
                    }

                    i = GetRandomIndex();
                }

                picked[i] = true;
                pickedCount++;
                return items[i];
            }

            public T PickOn2()
            {
                var i = GetRandomIndex();
                Cleanup();
                return items[i];
            }

            private int GetRandomIndex()
            {
                Samples++;
                var r = random.NextDouble() * weightCumulations[^1];
                var i = weightCumulations.BinarySearch(r);
                if (i < 0)
                {
                    return ~i;
                }
                else
                {
                    return i;
                }
            }

            public T PickOn3()
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

                for (int i = index; i < items.Count; ++i)
                {
                    weightCumulations[i] -= ratio;
                }

                items.RemoveAt(index);
                weightCumulations.RemoveAt(index);

                return item;
            }

            private void Cleanup()
            {
                var newItems = new List<T>();
                var newWeights = new List<double>();
                var newWeightCumulations = new List<double>();
                var newPicked = new List<bool>();

                var cumulation = 0.0;
                for (var i = 0; i < items.Count; i++)
                {
                    N++;
                    var item = items[i];
                    var weight = weights[i];
                    cumulation += weight;
                    newItems.Add(item);
                    newWeights.Add(weight);
                    newWeightCumulations.Add(cumulation);
                    newPicked.Add(false);
                }

                items = newItems;
                weights = newWeights;
                weightCumulations = newWeightCumulations;
                picked = newPicked;
                pickedCount = 0;
            }
        }

        public static void Main()
        {
            var dic = new Dictionary<string, List<double>>();
            dic.Add("Pick1", new List<double>());
            dic.Add("Pick2", new List<double>());
            dic.Add("Pick3", new List<double>());

            for (int j = 0; j < 10; ++j)
            {
                var picker = new Picker<int>(Enumerable.Range(0, 100).Select(x => (x, 1.0)));

                var startDt = DateTime.Now;
                for (var i = 0; i < 100; i++)
                {
                    picker.PickOn();
                }

                var elapsed = (DateTime.Now - startDt).TotalMilliseconds;
                dic["Pick1"].Add(elapsed);

                Console.WriteLine($"pick1: Elapsed: {elapsed} ms");
                Console.WriteLine("GetRandomIndex: {0}", picker.Samples);
                Console.WriteLine("N: {0}", picker.N);

                var picker2 = new Picker<int>(Enumerable.Range(0, 100).Select(x => (x, 1.0)));

                startDt = DateTime.Now;
                for (var i = 0; i < 100; i++)
                {
                    picker2.PickOn2();
                }

                elapsed = (DateTime.Now - startDt).TotalMilliseconds;
                dic["Pick2"].Add(elapsed);

                Console.WriteLine($"pick1: Elapsed: {elapsed} ms");
                Console.WriteLine("GetRandomIndex: {0}", picker2.Samples);
                Console.WriteLine("N: {0}", picker2.N);

                var picker3 = new Picker<int>(Enumerable.Range(0, 100).Select(x => (x, 1.0)));

                startDt = DateTime.Now;
                for (var i = 0; i < 100; i++)
                {
                    picker3.PickOn3();
                }

                elapsed = (DateTime.Now - startDt).TotalMilliseconds;
                dic["Pick3"].Add(elapsed);

                Console.WriteLine($"pick1: Elapsed: {elapsed} ms");
                Console.WriteLine("GetRandomIndex: {0}", picker3.Samples);
                Console.WriteLine("N: {0}", picker3.N);
            }

            Console.WriteLine("=== fin ===");
            foreach (var pair in dic)
            {
                Console.WriteLine($"{pair.Key}: {Math.Round(pair.Value.Average(), 6)} ms");
            }
        }
    }
}
