using System;

namespace CVRP
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello world");

            var solver = new AntSolver(TestDataSets.GetFirstTestDataSet());
            
            solver.Solve();
        }
    }
}
