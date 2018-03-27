using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadPoolExample
{
    public class Fibonacci
    {
        public Fibonacci(long n, ManualResetEvent doneEvent)
        {
            _n = n;
            _doneEvent = doneEvent;
        }

        public void Run()
        {
            Console.WriteLine("Calculating fib of {0}...", _n);
            _fibOfN = Calculate(_n);
            Console.WriteLine("Result is {0}", _fibOfN);
        }

        public void ThreadPoolCallback(Object threadContext)
        {
            int threadIndex = (int)threadContext;
            Console.WriteLine("Thread {0} started...", threadIndex);
            _fibOfN = Calculate(_n);
            Console.WriteLine("Thread {0} result calculated", threadIndex);
            _doneEvent.Set();
        }

        public long Calculate(long n)
        {
            if( n <= 1)
            {
                return n;
            }
            return Calculate(n - 1) + Calculate(n - 2);
        }

        private long _n;
        public long N { get { return _n; } }

        private long _fibOfN;
        public long FibOfN { get { return _fibOfN; } }

        private ManualResetEvent _doneEvent;
    }

    class Program
    {
        static void Main(string[] args)
        {
            const int FibonacciCalculations = 10;
            ManualResetEvent[] doneEvents = new ManualResetEvent[FibonacciCalculations];
            Fibonacci[] fibArray = new Fibonacci[FibonacciCalculations];
            Random r = new Random();

            Console.WriteLine("Launching {0} tasks...", FibonacciCalculations);
            for(int i = 0; i < FibonacciCalculations; i++)
            {
                doneEvents[i] = new ManualResetEvent(false);
                Fibonacci f = new Fibonacci(r.Next(20, 45), doneEvents[i]);
                fibArray[i] = f;
                ThreadPool.QueueUserWorkItem(f.ThreadPoolCallback, i);
            }
            WaitHandle.WaitAll(doneEvents);
            Console.WriteLine("All calculations are complete.");
            for(int i = 0; i < FibonacciCalculations; i++)
            {
                Fibonacci f = fibArray[i];
                Console.WriteLine("Fiboancci({0}) = {1}", f.N, f.FibOfN);
            }
        }
    }
}
