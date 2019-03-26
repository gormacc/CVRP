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

        private double alfa = 0.5;
        private double beta = 2;
        private double evaporation = 0.2;
        private double Qvalue = 1000;

        private readonly int _ants;
        private readonly Random _rand;
        private int _theBest = 5000;

        private double[,] _heuristics;
        private int[,] _distances;
        private double[,] _pheromones;

        private void InitializeMatrixes()
        {
            int count = _data.Vertexes.Count;

            _heuristics = new double[count, count];
            _distances = new int[count, count];
            _pheromones = new double[count, count];

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

                    _pheromones[i, j] = 1;
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
            List<AntInfo> allAnts = new List<AntInfo>();
            while (true)
            {
                allAnts.Clear();
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
                    allAnts.Add(ant);
                }
                AverageRouteInfo(allAnts);
                ActualizePheromones(allAnts);
            }
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

        private bool MyRandom(double probability)
        {
            return _rand.NextDouble() < probability;
        }

        private int FindNewVertex(AntInfo ant)
        {
            bool notFound = true;
            int nextVertex = ant.CurrentVertex;
            double[] probabilities = CalculateProbabilities(ant);
            int tries = 1;
            int maxTries = 10;
            int count = ant.Visited.Length;
            int i = _rand.Next(0, count-1);
            while (notFound)
            {
                i++;

                if (i >= count)
                {
                    i = 1;
                    tries++;
                }

                if (ant.CurrentVertex == i || ant.Visited[i]) continue;

                if (MyRandom(probabilities[i]) || tries > maxTries )
                {
                    nextVertex = i;
                    notFound = false;
                    break;
                }
            }

            return nextVertex;
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

//        private void WriteAntInfo(AntInfo ant, int antNumber)
//        {
//            Console.WriteLine("--------------------------------------------------------------------------------------------------------------");
//            Console.WriteLine("Ant number: {0}", antNumber);
//            Console.Write("Route : ");
//            for (int i = 0; i < ant.CurrentRoute.Count; i++)
//            {
//                Console.Write("{0},",ant.CurrentRoute[i]);
//            }
//            Console.WriteLine();
//            Console.WriteLine("Distance covered : {0}", ant.CurrentDistance);
//            Console.WriteLine("Depot visitings: {0}", ant.DepotVisiting);
//            Console.WriteLine("--------------------------------------------------------------------------------------------------------------");
//        }

        private double[] CalculateProbabilities(AntInfo ant)
        {
            double sum = 0.0;
            int count = _data.Vertexes.Count;
            double [] probabilities = new double[count];
            bool [] available = new bool[count];
            int currentVertex = ant.CurrentVertex;

            for (int i = 0; i < count; i++)
            {
                if (i == 0 || i == currentVertex || ant.Visited[i] ||
                    _data.TruckCapacity < ant.CurrentCapacity + _data.Vertexes[i].Demand)
                {
                    available[i] = false;
                }
                else
                {
                    available[i] = true;
                    sum += Math.Pow(_pheromones[currentVertex, i], alfa) * Math.Pow(_heuristics[currentVertex, i], beta);
                }
            }

            for (int i = 0; i < count; i++)
            {
                if (available[i])
                {
                    probabilities[i] = (Math.Pow(_pheromones[currentVertex, i], alfa) * Math.Pow(_heuristics[currentVertex, i], beta)) / sum;
                }
                else
                {
                    probabilities[i] = 0;
                }
            }

            return probabilities;
        }

        private void ActualizePheromones(List<AntInfo> allAnts)
        {
            var count = _data.Vertexes.Count;
            var newPheromones = new double[count, count];

            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < count; j++)
                {
                    newPheromones[i, j] = 0;
                }
            }

            for (int i = 0; i < allAnts.Count; i++)
            {
                for (int j = 0; j < allAnts[i].CurrentRoute.Count - 1; j++)
                {
                    var prevCity = allAnts[i].CurrentRoute[j];
                    var nextCity = allAnts[i].CurrentRoute[j+1];
                    var pheromone = Qvalue / _distances[prevCity, nextCity];
                    newPheromones[prevCity, nextCity] += pheromone;
                }
            }

            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < count; j++)
                {
                    _pheromones[i, j] = (1 - evaporation) * _pheromones[i, j] + newPheromones[i, j];
                }
            }
        }

        private void AverageRouteInfo(List<AntInfo> allAnts)
        {
            var sum = 0;
            var min = 10000000;
            for (int i = 0; i < allAnts.Count; i++)
            {
                sum += allAnts[i].CurrentDistance;
                min = Math.Min(min, allAnts[i].CurrentDistance);
            }

            _theBest = Math.Min(_theBest, min);

            Console.WriteLine("Sredni dystans wszystkich mrówek : {0}, najmniejszy dystans : {1}, najlepszy do tej pory : {2}, najlepsze możliwe : {3}", 
                sum/allAnts.Count, min, _theBest, _data.OptimalSolution);

        }
    }
}
