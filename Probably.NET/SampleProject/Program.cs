using Probably.NET;
using System;

namespace SampleProject
{
    class Program
    {
        static void Main(string[] args)
        {
            SampleHLL();
            SampleQuantileGK();
        }

        static void SampleHLL()
        {
            HyperLogLog hll1 = new HyperLogLog(0.01, 1234, 5678),
                hll2 = new HyperLogLog(0.01, 1234, 5678);
            hll1.Insert(123);
            hll1.Insert(234);
            hll1.Insert(2);
            hll1.Insert(5);

            hll1.Insert(0);
            Console.WriteLine($"hll size: {hll1.Count()}");

            hll2.Insert(5);
            hll2.Insert("hello");
            hll2.Insert("world!");
            Console.WriteLine($"hll1 size: {hll1.Count()}");
            Console.WriteLine($"hll2 size: {hll2.Count()}");

            hll1.MergeWith(hll2);


            hll1.Insert("another string");
            Console.WriteLine($"hll1 size: {hll1.Count()}");

            byte[] serialized = hll1.GetBytes();

            HyperLogLog deserialized = new HyperLogLog(serialized);
            Console.WriteLine($"deserialized size: {deserialized.Count()}");

        }

        static void SampleQuantileGK()
        {
            var gk = new QuantileGK(0.01);
            for (int i = 0; i < 1000; i++)
            {
                gk.Insert((double)i);
            }

            double p50 = gk.Quantile(0.5),
                p75 = gk.Quantile(0.75),
                p90 = gk.Quantile(0.9),
                p95 = gk.Quantile(0.95),
                p99 = gk.Quantile(0.99);

            Console.WriteLine($"P50 = {p50}");
            Console.WriteLine($"P75 = {p75}");
            Console.WriteLine($"P90 = {p90}");
            Console.WriteLine($"P95 = {p95}");
            Console.WriteLine($"P99 = {p99}");


            byte[] serialized = gk.GetBytes();


            QuantileGK gk2 = new QuantileGK(serialized);
            Console.WriteLine($"P80 = {gk.Quantile(0.8)}");
            gk2.Insert(100000);
            Console.WriteLine($"P80 = {gk.Quantile(0.8)}");

        }
    }
}
