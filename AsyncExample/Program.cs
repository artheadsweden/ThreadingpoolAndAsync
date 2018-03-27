using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncExample
{
    class Program
    {
        static void Main(string[] args)
        {
            CallWithAsync();
            CallWithAsync2();
            Console.WriteLine("Waiting for greeting");
            Console.WriteLine("Taking a nap");
            Thread.Sleep(2100);
            Console.WriteLine("Back again.... Boring taking a nap again");
            Thread.Sleep(2000);
            Console.WriteLine("Ok I'm done");


            Console.WriteLine("\n\nNext task, call async with combinators");
            MultipleAsyncMethodsWithCombinators();
            Console.WriteLine("Again, I will sleep");
            Thread.Sleep(2000);
            Console.WriteLine("And some more...");
            Thread.Sleep(2000);
            Console.WriteLine("Now I'm done");
           // var result = Greeting("Anna", 2000);
           // Console.WriteLine(result);
        }

        private async static void MultipleAsyncMethodsWithCombinators()
        {
            Task<string> t1 = GreetingAsync("Peter", 2000);
            Task<string> t2 = GreetingAsync("Mary", 3000);

            await Task.WhenAll(t1, t2);
            Console.WriteLine("Both tasks done...");
            Console.WriteLine("Result 1: {0}\nResult 2: {1}", t1.Result, t2.Result);
        }

        private async static void CallWithAsync()
        {
            string result1 = await GreetingAsync("Anna", 2000);
            Console.WriteLine(result1);

        }

        private async static void CallWithAsync2()
        {
            string result2 = await GreetingAsync("Simon", 1500);
            Console.WriteLine(result2);
        }

        private static Task<string> GreetingAsync(string name, int sleepTimeMs)
        {
            return Task.Run<string>(() =>
            {
                return Greeting(name, sleepTimeMs);
            });
        }

        static string Greeting(string name, int sleepTimeMs)
        {
            Thread.Sleep(sleepTimeMs);
            return string.Format("Hello, {0}", name);
        }
    }
}
