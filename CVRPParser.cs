using System.Collections.Generic;
using System.IO;
using CVRP.Model;

namespace CVRP
{
    public static class CvrpParser
    {
        public static TestData ParseFile(string filePath)
        {
            int truckNumber = 1;
            int bestValue = 1;
            int capacity;

            var vertexes = new List<TestDataVertex>();

            string[] lines = File.ReadAllLines(filePath);

            var commentLine = lines[1].Split(' ', ',', ')');

            for (int i = 0; i < commentLine.Length; i++)
            {
                if (commentLine[i] == "trucks:")
                {
                    truckNumber = int.Parse(commentLine[i + 1]);
                }

                if (commentLine[i] == "value:")
                {
                    bestValue = int.Parse(commentLine[i + 1]);
                }
            }

            var capac = lines[5].Trim().Split(' ');

            capacity = int.Parse(capac[2]);

            int j = 7;

            int id, x, y, demand;
            while (lines[j].Trim() != "DEMAND_SECTION")
            {
                var coord = lines[j].Trim().Split(' ');

                id = int.Parse(coord[0]);
                x = int.Parse(coord[1]);
                y = int.Parse(coord[2]);

                vertexes.Add(new TestDataVertex(id, x, y));

                j++;
            }

            j++;
            while (lines[j].Trim() != "DEPOT_SECTION")
            {
                var dem = lines[j].Trim().Split(' ');

                id = int.Parse(dem[0]);
                demand = int.Parse(dem[1]);

                vertexes[id-1].SetDemand(demand);

                j++;
            }


            return new TestData(capacity, truckNumber, bestValue, vertexes);
        }


    }
}
