using System.Collections.Generic;

namespace CVRP.Model
{
    public class TestData
    {
        public double TruckCapacity { get; }

        public double OptimalSolution { get; }

        public int TrucksNumber { get; }

        public List<TestDataVertex> Vertexes { get; }

        public TestData(double truckCapacity,int trucksNumber, double optimalSolution, List<TestDataVertex> vertexes)
        {
            TruckCapacity = truckCapacity;
            TrucksNumber = trucksNumber;
            OptimalSolution = optimalSolution;
            Vertexes = vertexes;
        }
    }
}
