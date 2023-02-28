using System;


namespace cv1
{
    // 1
    /*
    delegate int Operation(int x, int y);
    delegate void SetXYHandler();
    class Calculator
    {
        int x;
        int y;

        public void SetXY(int x, int y)
        {
            this.x = x;
            this.y = y;

            //OnSetXY();
            OnSetXY?.Invoke(this, new EventArgs());
        }

        public event EventHandler OnSetXY;
        public event EventHandler<ComputeEventArgs> OnCumpute;


        public void Execute(Operation op)
        {
            int res = op.Invoke(x, y);
            Console.Write(res);
            OnCumpute?.Invoke(this, new ComputeEventArgs() { result = res });

        }

        
    }

    class ComputeEventArgs : EventArgs
    {
        public int result { get; set; }
    }
    */

    static class MyExt
    {
        public static IEnumerable<int> EvenIndexes(this IEnumerable<int> arr)
        {

            int i = 0;
            foreach(int x in arr)
            {
                if( i % 2 == 0)
                {
                    yield return x;
                }
                i++;
            }
                
        }
        public static IEnumerable<int> Transform(this IEnumerable<int> arr, Transformer tran)
        {
            foreach(int x in arr)
            {
                yield return tran(x);
            }

        }

        public static IEnumerable<int> Filter(this IEnumerable<int> arr, FilterFn filter)
        {
            foreach (int x in arr)
            {
                if(filter(x))
                {
                    yield return x;
                }
                
            }
        }

    }

    delegate int Transformer(int x);
    delegate bool FilterFn(int x);
    class Program
    {
        static void Main(string[] args)
        {
            // delegate, events
            /*
            Calculator calculator = new Calculator();

            calculator.OnCumpute += (object sender, ComputeEventArgs args) =>
            {
                Console.WriteLine("Vysledek: " + args.result);
            };

            calculator.OnSetXY += (object obj, EventArgs args) => Console.WriteLine("Nastaveno");

            calculator.SetXY(5, 10);
            calculator.Execute((x, y) => x + y);
            */

            // 2 Extension 

            int[] arr = new int[] {10, 1, 25, 38, 26, 100, 415, 15, 17};

            foreach(int x in arr.EvenIndexes().Transform(x => x * 2).Filter(x => x > 5))
            {
                Console.WriteLine(x);
                
            }

            Console.WriteLine(arr.DefaultIfEmpty(0).Average());
            Console.WriteLine(arr.Sum());
            Console.WriteLine(arr.Max());
            
            foreach(var group in arr.GroupBy(x => x % 2))
            {
                Console.WriteLine(group.Key == 0 ? "Sude" : "Liche");
                Console.WriteLine(string.Join(", ", group));
            }


            var tmp = arr.Select(x => x * 2).Where(x => x < 20).Select(x => x / 2);

            /*
            List<Product> producsts = new List<Product>();
            producsts.Add(new Product() { Name = "Samsung", isAvailable = true, Price = 1000 });
            */

        }
    }
}