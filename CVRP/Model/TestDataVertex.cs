namespace CVRP.Model
{
    public class TestDataVertex
    {
        public int Id { get; }

        public int X { get; }

        public int Y { get; }

        public int Demand { get; }

        public TestDataVertex(int id, int x, int y, int demand)
        {
            Id = id;
            X = x;
            Y = y;
            Demand = demand;
        }
    }
}
