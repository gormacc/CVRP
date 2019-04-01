using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Remoting.Contexts;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using CVRP.Model;
using Microsoft.Win32;
using Path = System.IO.Path;

namespace CVRP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private TestData _data { get; set; }
        private List<Score> _solutions = new List<Score>();

        private void InitializeConsts()
        {
            alfa = double.Parse(AlfaText.Text);
            beta = double.Parse(BetaText.Text);
            evaporation = double.Parse(EvaporateText.Text);
            Qvalue = double.Parse(QValueText.Text);
        }

        private void LoadFile(object sender, RoutedEventArgs e)
        {
//            OpenFileDialog ofd = new OpenFileDialog();
//            string initialDirectory = Path.Combine(Directory.GetCurrentDirectory(), "dataset");
//            if (Directory.Exists(initialDirectory))
//            {
//                ofd.InitialDirectory = initialDirectory;
//            }
//
//            if (ofd.ShowDialog(this) == true)
//            {
//                _data = CvrpParser.ParseFile(ofd.FileName);
//            }
//
//            DrawVertexes();
//            _solutions.Clear();
//            InitializeConsts();
//            StartButton.IsEnabled = true;

            InitializeConsts();

            var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "dataset");
            var files = Directory.GetFiles(directoryPath);
            WriteScoresToFile(files[0]);

            for (int j = 0; j < files.Length; j++)
            {
                var fileName = Path.GetFileNameWithoutExtension(files[j]);
                var outFilePath = Path.Combine(Directory.GetCurrentDirectory(), "result", fileName + ".txt");
//                File.Create(outFilePath);
                _data = CvrpParser.ParseFile(files[j]);

                for (int i = 0; i < 8; i++)
                {
                    ClearScores();
                    Solve();
                    WriteScoresToFile(outFilePath);
                }
            }

            MessageBox.Show("Zrobione");
        }

        private void ClearScores()
        {
            _solutions.Clear();
            _loopCounter = 0;
            _theBest = 10000;
            _theBestSolution.Clear();
            _ants = _data.Vertexes.Count;
            _rand = new Random(DateTime.Now.Millisecond);
        }

        private void WriteScoresToFile(string outFilePath)
        {
            var scores = string.Empty;

            foreach (var solution in _solutions)
            {
                scores += $"{solution.LoopCount} {solution.Solution} {solution.TruckNumber}";
                scores += Environment.NewLine;
            }
            scores += Environment.NewLine;
            scores += Environment.NewLine;
            scores += Environment.NewLine;
            File.AppendAllText(outFilePath, scores);
        }

        private Thread _antThread;

        private void Solve(object sender, RoutedEventArgs e)
        {
            _ants = _data.Vertexes.Count;
            _rand = new Random(DateTime.Now.Millisecond);
            BestSolutionText.Text = _data.OptimalSolution.ToString();
            StopButton.IsEnabled = true;

            Task.Run(() => Solve());
        }

        private void StopSolving(object sender, RoutedEventArgs e)
        {
            _antThread.Abort();
            ShowScore();
            Close();
        }

        private void ShowScore()
        {
            var scores = string.Empty;

            foreach (var solution in _solutions)
            {
                scores += $"{solution.LoopCount} {solution.Solution} {solution.TruckNumber} \n";
            }

            MessageBox.Show(scores, "Wyniki");
        }

        private const int _canvasMultiplier = 6;

        private void DrawVertexes()
        {
            foreach (var testDataVertex in _data.Vertexes)
            {
                var color = testDataVertex.Id == 1 ? Brushes.Red : Brushes.Black;

                var circle = new Ellipse()
                {
                    Height = 8,
                    Width = 8,
                    Fill = color
                };

                MyCanvas.Children.Add(circle);

                Canvas.SetLeft(circle, _canvasMultiplier*testDataVertex.X);
                Canvas.SetTop(circle, _canvasMultiplier*testDataVertex.Y);

            }
        }

        private void DrawEdges(List<int> route)
        {
            var edges = new List<Line>();

            foreach (var child in MyCanvas.Children)
            {
                if (child is Line line)
                {
                    edges.Add(line);
                }
            }

            foreach (var edge in edges)
            {
                MyCanvas.Children.Remove(edge);
            }

            for (int i = 0; i < route.Count-1; i++)
            {
                var vertexOne = _data.Vertexes[route[i]];
                var vertexTwo = _data.Vertexes[route[i+1]];

                var line = new Line()
                {
                    StrokeThickness = 4,
                    Stroke = Brushes.Black,
                    X1 = vertexOne.X * _canvasMultiplier,
                    Y1 = vertexOne.Y * _canvasMultiplier,
                    X2 = vertexTwo.X * _canvasMultiplier,
                    Y2 = vertexTwo.Y * _canvasMultiplier
                };

                MyCanvas.Children.Add(line);
            }
        }


        // ***************************************************************************************************************************

        private double alfa;
        private double beta;
        private double evaporation;
        private double Qvalue;

        private long _loopCounter = 0;
        private int _ants;
        private Random _rand;
        private int _theBest = 5000;
        private List<int> _theBestSolution = new List<int>();

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

        private void Solve()
        {
//            _antThread = Thread.CurrentThread;
            InitializeMatrixes();
            List<AntInfo> allAnts = new List<AntInfo>();
            while (_loopCounter < 3000)
            {
                allAnts.Clear();
                for (int i = 0; i < _ants; i++)
                {
                    bool findingRoute = true;
                    AntInfo ant = new AntInfo(_data.Vertexes.Count);

                    while (findingRoute)
                    {
                        ant.NextVertex = FindNewVertex(ant);
                        if (_data.TruckCapacity < ant.CurrentCapacity + _data.Vertexes[ant.NextVertex].Demand)
                        {
                            ant.NextVertex = 0;
                        }

                        ActualizeCurrentRoute(ant);
                        if (CheckEndOfRoute(ant)) findingRoute = false;
                    }
                    allAnts.Add(ant);
                }
                AverageRouteInfo(allAnts);
                ActualizePheromones(allAnts);
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
            double[] probabilities = CalculateProbabilities(ant);
            int tries = 1;
            int maxTries = 10;
            int count = ant.Visited.Length;
            int i = _rand.Next(0, count - 1);
            while (notFound)
            {
                i++;

                if (i >= count)
                {
                    i = 1;
                    tries++;
                }

                if (ant.CurrentVertex == i || ant.Visited[i]) continue;

                if (MyRandom(probabilities[i]) || tries > maxTries)
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

            if (ant.CurrentVertex != 0)
            {
                ant.NextVertex = 0;
                ActualizeCurrentRoute(ant);
            }

            return true;
        }

        private double[] CalculateProbabilities(AntInfo ant)
        {
            double sum = 0.0;
            int count = _data.Vertexes.Count;
            double[] probabilities = new double[count];
            bool[] available = new bool[count];
            int currentVertex = ant.CurrentVertex;

            for (int i = 0; i < count; i++)
            {
                if (i == 0 || i == currentVertex || ant.Visited[i])
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
                    var nextCity = allAnts[i].CurrentRoute[j + 1];
                    var pheromone = Qvalue / _distances[prevCity, nextCity];
                    newPheromones[prevCity, nextCity] += pheromone;
                }
            }

            for (int i = 0; i < _theBestSolution.Count-1; i++)
            {
                var prevCity = _theBestSolution[i];
                var nextCity = _theBestSolution[i+1];
                var pheromone = Qvalue / _distances[prevCity, nextCity];
                newPheromones[prevCity, nextCity] += pheromone;
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
            int minTrucks = 0;
            List<int> minRoute = new List<int>();
            for (int i = 0; i < allAnts.Count; i++)
            {
                sum += allAnts[i].CurrentDistance;
                if(min > allAnts[i].CurrentDistance)
                {
                    min = allAnts[i].CurrentDistance;
                    minRoute = allAnts[i].CurrentRoute;
                    minTrucks = allAnts[i].DepotVisiting;
                }
            }

            this.Dispatcher.Invoke(() =>
            {
                _loopCounter += 1;
                LoopCounterText.Text = _loopCounter.ToString();
            });

            if (_theBest > min)
            {
                _theBest = min;
                _theBestSolution.Clear();
                _theBestSolution.AddRange(minRoute);

                _solutions.Add(new Score()
                {
                    Solution = min,
                    LoopCount = _loopCounter,
                    TruckNumber = minTrucks
                });

                this.Dispatcher.Invoke(() =>
                {
                    DrawEdges(minRoute);
                    CurrentBestSolutionText.Text = min.ToString();
                });
                
            }

        }

        //*********************************************************************************************************

    }
}
