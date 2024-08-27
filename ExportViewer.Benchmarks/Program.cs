using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace ExportViewer.Benchmarks
{
    public class Program
    {
        static void Main (string[] args)
        {
            var summary = BenchmarkRunner.Run<Program>();
        }

        private const string TestString = "Hello, World!";

        [Benchmark]
        public void UseStringBuilder ()
        {
            var sb = new System.Text.StringBuilder();
            for (int i = 0; i < 1000; i++)
            {
                sb.Append(TestString);
            }
            
            _ = sb.ToString();
        }

        [Benchmark]
        public void UseStringConcatenation ()
        {
            string result = "";
            for (int i = 0; i < 1000; i++)
            {
                result += TestString;
            }
        }
    }
}
