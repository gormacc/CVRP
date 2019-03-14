using System;
using System.Collections.Generic;
using CVRP.Model;

namespace CVRP
{
    public class AntSolver
    {
        private TestData _data;

        public AntSolver(TestData data)
        {
            _data = data;
            _ants = _data.Vertexes.Count;
            _rand = new Random(DateTime.Now.Millisecond);
        }

        private double alfa = 2;
        private double beta = 5;

        private readonly int _ants;
        private Random _rand;

        private double[,] _heuristics;
        private int[,] _distances;
        private double[,] _pheromones;
        private double[,] _probabilities;

        private void InitializeMatrixes()
        {
            int count = _data.Vertexes.Count;

            _heuristics = new double[count, count];
            _distances = new int[count, count];
            _pheromones = new double[count, count];
            _probabilities = new double[count, count];

            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < count; j++)
                {
                    if (i != j)
                    {
                        var distance = CalculateDistance(_data.Vertexes[i], _data.Vertexes[j]);
                        _distances[i, j] = (int)distance;
                        _heuristics[i, j] = 1 / distance;
                    }
                    else
                    {
                        _distances[i, j] = 0;
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

        public void Solve()
        {
            InitializeMatrixes();

            while (Console.ReadKey().Key != ConsoleKey.Escape)
            {
                for (int i = 0; i < _ants; i++)
                {
                    bool findingRoute = true;
                    AntInfo ant = new AntInfo(_data.Vertexes.Count);

                    while (findingRoute)
                    {
                        bool isTruckFull = CheckCurrentCapacity(ant);
                        ant.NextVertex = isTruckFull == false ? FindNewVertex(ant) : 0;

                        ActualizeCurrentRoute(ant);
                        if (ant.DepotVisiting == _data.TrucksNumber || CheckEndOfRoute(ant)) findingRoute = false;
                    }
                    WriteAntInfo(ant, i+1);
                }
            }
        }

        private bool MyRandom(double probability)
        {
            return _rand.NextDouble() < probability;
        }

        private int FindNewVertex(AntInfo ant)
        {
            bool notFound = true;
            int nextVertex = ant.CurrentVertex;
            while (notFound)
            {
                for (int i = 1; i < ant.Visited.Length; i++)
                {
                    if(ant.CurrentVertex == i || ant.Visited[i]) continue;

                    if (MyRandom(_probabilities[ant.CurrentVertex, i]) && _data.TruckCapacity >= ant.CurrentCapacity + _data.Vertexes[i].Demand)
                    {
                        nextVertex = i;
                        notFound = false;
                        break;
                    }           
                }
            }

            return nextVertex;
        }

        private bool CheckCurrentCapacity(AntInfo ant)
        {
            for (int i = 1; i < ant.Visited.Length; i++)
            {
                if (ant.Visited[i] == false && _data.TruckCapacity >= ant.CurrentCapacity + _data.Vertexes[i].Demand)
                {
                    return false;
                }
            }

            return true;
        }

        private void ActualizeCurrentRoute(AntInfo ant)
        {
            if (ant.NextVertex == 0)
            {
                ant.CurrentCapacity = 0;
                ant.DepotVisiting += 1;
            }
            else
            {
                ant.Visited[ant.NextVertex] = true;
            }

            ant.CurrentRoute.Add(ant.NextVertex);
            ant.CurrentDistance += _distances[ant.CurrentVertex, ant.NextVertex];
            ant.CurrentCapacity += _data.Vertexes[ant.NextVertex].Demand;
            ant.CurrentVertex = ant.NextVertex;
        }

        private bool CheckEndOfRoute(AntInfo ant)
        {
            for (int i = 1; i < ant.Visited.Length; i++)
            {
                if (ant.Visited[i] == false)
                {
                    return false;
                }
            }

            return ant.CurrentVertex == 0;
        }

        private void WriteAntInfo(AntInfo ant, int antNumber)
        {
            Console.WriteLine("--------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("Ant number: {0}", antNumber);
            Console.Write("Route : ");
            for (int i = 0; i < ant.CurrentRoute.Count; i++)
            {
                Console.Write("{0},",ant.CurrentRoute[i]);
            }
            Console.WriteLine();
            Console.WriteLine("Distance covered : {0}", ant.CurrentDistance);
            Console.WriteLine("Depot visitings: {0}", ant.DepotVisiting);
            Console.WriteLine("--------------------------------------------------------------------------------------------------------------");
        }
    }
}
