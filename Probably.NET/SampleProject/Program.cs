using Probably.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleProject
{
    class Program
    {
        static void Main(string[] args)
        {
            SampleHLL();
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

            hll.Insert("hello");
            Console.WriteLine($"hll size: {hll.Count()}");
            hll.Insert("world");
            Console.WriteLine($"hll size: {hll.Count()}");
        }
    }
}
