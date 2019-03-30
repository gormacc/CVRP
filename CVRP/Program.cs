using System;
using System.IO;

namespace CVRP
{
    class Program
    {
        static void Main(string[] args)
        {

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "dataset", "A-n32-k5.vrp");

            var data = CvrpParser.ParseFile(filePath, out bool fileExist);

            if(fileExist == false) return;

            var solver = new AntSolver(data);
//            
            solver.Solve();
        }
    }
}
