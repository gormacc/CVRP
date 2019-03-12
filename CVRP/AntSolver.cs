using System;
using CVRP.Model;

namespace CVRP
{
    public class AntSolver
    {
        private TestData _data;

        public AntSolver(TestData data)
        {
            _data = data;
        }

        public void Solve()
        {
            InitializeMatrixes();


        }

        private double alfa = 2;
        private double beta = 5;

        private double[,] _heuristics;
        private double[,] _pheromones;
        private double[,] _probabilities;
        private bool[] _visited;

        private void InitializeMatrixes()
        {
            int count = _data.Vertexes.Count;

            _heuristics = new double[count,count];
            _pheromones = new double[count,count];
            _probabilities = new double[count,count];
            _visited = new bool[count];

            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < count; j++)
                {
                    if (i != j)
                    {
                        var distance = CalculateDistance(_data.Vertexes[i], _data.Vertexes[j]);
                        _heuristics[i, j] = 1 / distance;
                    }
                    else
                    {
                        _heuristics[i, j] = 0;
                    }

                    _pheromones[i, j] = 0;
                    _probabilities[i, j] = 0.5;
                }
            }
        }

        private double CalculateDistance(TestDataVertex vertexA, TestDataVertex vertexB)
        {

            return Math.Sqrt(Math.Pow(vertexA.X - vertexB.X, 2) + Math.Pow(vertexA.Y - vertexB.Y, 2));
        }
    }
}
