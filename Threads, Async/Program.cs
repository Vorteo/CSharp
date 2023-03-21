namespace cv3v2
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // await Test();
            await Experiment();
        }


        private static async Task Experiment()
        {
            Console.WriteLine("Start");
            await Task.Delay(1000);
            StreamWriter sw = new StreamWriter("test.txt");

            await sw.WriteLineAsync("adavlvmafmaofmoamfoamf");
            
            await Task.Delay(1000);
            Console.WriteLine("End");
        }

        private static async Task Test()
        {
            await File.WriteAllTextAsync("test.txt", "ABADAAGCAFT");

            string x = await File.ReadAllTextAsync("test.txt");
        }
    }
}