using System;

namespace cv3
{
    internal class Program
    {
        static SimpleStack<int> stack = new SimpleStack<int>();
        static Random random = new Random();
        static void Main(string[] args)
        {

            object lockObject = new object();

            Thread t1 = new Thread(() => {
                while (true)
                {
                    stack.Push(random.Next());

                    lock (lockObject)
                    {
                        Monitor.Pulse(lockObject);
                    }

                    Thread.Sleep(100);
                }
            });
            t1.Start();


            for(int i = 0; i < 5; i++)
            {
                Thread t = new Thread(() =>
                { 
                    int tid = Thread.CurrentThread.ManagedThreadId;
                    while(true)
                    {
                        if(stack.TryPop(out int x))
                        {
                            Console.WriteLine(tid + " " + x);
                        }
                        else
                        {
                            Console.WriteLine(tid + " " + "Empty");
                            lock (lockObject)
                            {
                                Monitor.Wait(lockObject);
                            }       
                        }

                        Thread.Sleep(random.Next(40, 1000));
                    }        
                });
                t.Start();
            }
            

            /*
            List<Thread> threads = new List<Thread>();

            for(int i = 0; i < 5; i++)
            {
                threads.Add(new Thread(function));
            }

            foreach(Thread t in threads)
            {
                t.Start();
            }
            */

        }
        /*
        static void function()
        {
            while(true)
            {
                if (random.NextDouble() < 0.6)
                {
               
                        if (!stack.TryPop(out int x))
                        {
                              // int x = stack.Pop();
                              // Console.WriteLine(x);
                        }                                             
                }
                else
                {                    
                    stack.Push(random.Next());                 
                }
            }
        }
        */
    }
}