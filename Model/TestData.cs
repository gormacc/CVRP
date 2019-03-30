using System.Collections.Generic;

namespace CVRP.Model
{
    public class TestData
    {
        public int TruckCapacity { get; }

        public int OptimalSolution { get; }

        public int TrucksNumber { get; }

        public List<TestDataVertex> Vertexes { get; }

        public TestData(int truckCapacity,int trucksNumber, int optimalSolution, List<TestDataVertex> vertexes)
        {
            TruckCapacity = truckCapacity;
            TrucksNumber = trucksNumber;
            OptimalSolution = optimalSolution;
            Vertexes = vertexes;
        }

        public TestData()
        {
            
        }
    }
}
