using System.Collections.Generic;

namespace CVRP
{
    public class AntInfo
    {
        public int CurrentVertex { get; set; } = 0;
        public int NextVertex { get; set; } = 0;
        public int CurrentCapacity { get; set; } = 0;
        public List<int> CurrentRoute { get; set; } = new List<int>() {0};
        public int CurrentDistance { get; set; } = 0;
        public int DepotVisiting { get; set; } = 0;
        public bool[] Visited { get; set; }

        public AntInfo(int cityNumber)
        {
            Visited = new bool[cityNumber];
        }
    }
}
