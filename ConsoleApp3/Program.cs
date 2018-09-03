using ConsoleApp3.Models;
using ConsoleApp3.Threading;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp3
{
    class Program
    {
        static void Main(string[] args)
        {
            var x = new ParallelThreading();
            long elapsedMs = 0;
            Dictionary<string, long> results = new Dictionary<string, long>();            

            var dataResults = x.InitializeDataForParallelData(6500);
            var something = new PersonModel[6500];
            var something1 = new PersonModel[6500];
            var something2 = new PersonModel[6500];
            var something3 = new PersonModel[6500];
            dataResults.CopyTo(something);
            dataResults.CopyTo(something1);
            dataResults.CopyTo(something2);
            dataResults.CopyTo(something3);

            var watch2 = System.Diagnostics.Stopwatch.StartNew();
            x.AsParallelAddingParallelization(something1.ToList());
            watch2.Stop();
            elapsedMs = watch2.ElapsedMilliseconds;
            results.Add(nameof(x.AsParallelAddingParallelization), elapsedMs);

            var watch = System.Diagnostics.Stopwatch.StartNew();
            x.AsParallel(something2.ToList());
            watch.Stop();
            elapsedMs = watch.ElapsedMilliseconds;
            results.Add(nameof(x.AsParallel), elapsedMs);

            var watch1 = System.Diagnostics.Stopwatch.StartNew();
            x.WithoutAsParallel(something3.ToList());
            watch1.Stop();
            elapsedMs = watch1.ElapsedMilliseconds;
            results.Add(nameof(x.WithoutAsParallel), elapsedMs);

            var watch3 = System.Diagnostics.Stopwatch.StartNew();
            x.AsParallelAsOrdered(something.ToList());
            watch3.Stop();
            elapsedMs = watch3.ElapsedMilliseconds;
            results.Add(nameof(x.AsParallelAsOrdered), elapsedMs);


            foreach (var result in results)
            {
                WriteTime(result.Key, result.Value);
            }

            Console.ReadLine();
        }

        private static void WriteTime(string methodName, long elapsedMiliseconds)
        {
            Console.WriteLine($" {methodName}: Milisecond passed: { elapsedMiliseconds}");
        }
    }
}
