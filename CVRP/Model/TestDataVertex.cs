namespace CVRP.Model
{
    public class TestDataVertex
    {
        public int Id { get; }

        public int X { get; }

        public int Y { get; }

        public int Demand { get; private set; }

        public TestDataVertex(int id, int x, int y)
        {
            Id = id;
            X = x;
            Y = y;
        }

        public void SetDemand(int demand)
        {
            Demand = demand;
        }
    }
}
