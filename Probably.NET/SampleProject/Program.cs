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
            var hll = new HyperLogLog(0.01);
            hll.Insert(123);
            hll.Insert(234);
            hll.Insert(2);
            hll.Insert(5);
            hll.Insert(5);
            //hll.Insert((sbyte)5);
            hll.Insert(0);
            Console.WriteLine($"hll size: {hll.Count()}");

            hll.Insert("hello");
            Console.WriteLine($"hll size: {hll.Count()}");
            hll.Insert("world!");
            Console.WriteLine($"hll size: {hll.Count()}");
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
        }
    }
}
