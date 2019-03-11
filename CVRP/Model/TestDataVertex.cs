namespace CVRP.Model
{
    public class TestDataVertex
    {
        public int Id { get; }

        public double X { get; }

        public double Y { get; }

        public double Demand { get; }

        public TestDataVertex(int id, double x, double y, double demand)
        {
            Id = id;
            X = x;
            Y = y;
            Demand = demand;
        }
    }
}
