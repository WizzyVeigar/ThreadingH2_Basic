using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadingPractice
{
    class Program
    {
        static char ch = '*';
        static int number = 0;

        static void Main(string[] args)
        {
            Program pg = new Program();

            Thread thread = new Thread(new ThreadStart(pg.WorkThreadFunction));
            Thread thread2 = new Thread(new ThreadStart(pg.AnotherThreadFunction));
            thread.Start();
            thread2.Start();

            //!Temperature Thread
            Thread thread3 = new Thread(new ThreadStart(pg.TemperatureThread));
            thread3.Start();
            while (thread3.IsAlive)
            {
                Thread.Sleep(10000);
            }
            Console.WriteLine("Thread Is dead.");

            //! IO Threads
            //Thread thread4 = new Thread(new ThreadStart(pg.InputThread));
            //Thread thread5 = new Thread(new ThreadStart(pg.OutputThread));
            //thread4.Start();
            //thread5.Start();

            for (int i = 0; i < 2; i++)
            {
                ThreadPool.QueueUserWorkItem(pg.task1);
                ThreadPool.QueueUserWorkItem(pg.task2);
            }

            #region PoolVsObjects
            //!ThreadPool VS objects
            Stopwatch stopwatch = new Stopwatch();
            Console.WriteLine("With threadpool");
            stopwatch.Start();
            ProcessWithThreadPoolMethod();
            stopwatch.Stop();
            Console.WriteLine("The pool took: " + stopwatch.ElapsedTicks.ToString());
            stopwatch.Reset();

            Console.WriteLine("Thread objects");
            stopwatch.Start();
            ProcessWithThreadMethod();
            stopwatch.Stop();
            Console.WriteLine("The objects took: " + stopwatch.ElapsedTicks.ToString());
            Console.ReadKey();
            #endregion


            Console.ReadKey();
        }

        #region StartingThreads
        public void WorkThreadFunction()
        {
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine("C# Tråde er mega ez ");
            }
        }

        public void AnotherThreadFunction()
        {
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine("yappadabbadoo");
                Thread.Sleep(1000);
            }
        }

        //! Temperature Thread
        public void TemperatureThread()
        {
            Random rnd = new Random();
            int temp;
            int alarmCount = 0;

            while (alarmCount < 3)
            {
                temp = rnd.Next(-20, 120);
                if (temp < 0 || temp > 100)
                {
                    Console.WriteLine(temp + " Temperature limit exceeded!");
                    alarmCount++;
                }
                else
                {
                    Console.WriteLine(temp);
                }
                Thread.Sleep(1000);
            }
        }

        //!IO Threads
        public void InputThread()
        {
            char temp;

            while (true)
            {
                try
                {
                    temp = Console.ReadLine().First();
                }
                catch (Exception)
                {

                    throw;
                }
                if (char.IsLetterOrDigit(temp))
                {
                    ch = temp;
                }
            }
        }

        //!IO Threads
        public void OutputThread()
        {
            while (true)
            {
                Console.Write(ch);
            }
        }
        #endregion
        #region PoolVsObjects
        public void task1(object obj)
        {
            for (int i = 0; i <= 2; i++)
            {
                Console.WriteLine("Task 1 is being executed");
            }
        }
        public void task2(object obj)
        {
            for (int i = 0; i <= 2; i++)
            {
                Console.WriteLine("Task 2 is being executed");
            }
        }

        //! Threadpool vs Objects
        static void ProcessWithThreadPoolMethod()
        {
            for (int i = 0; i < 100000; i++)
            {
                ThreadPool.QueueUserWorkItem(Process);
            }
        }

        static void ProcessWithThreadMethod()
        {
            try
            {
                for (int i = 0; i < 100000; i++)
                {
                    Thread thr = new Thread(Process);
                    thr.Start();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Something went wrong");
                throw;
            }
        }

        static void Process(object callback)
        {

        }
        #endregion

    }
}