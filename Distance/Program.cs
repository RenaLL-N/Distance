using System;
using System.IO;
using System.Collections;
using System.Reflection;
using System.Threading;
using Parcs;
using System.Diagnostics;

namespace Distance
{
    internal class Program : IModule
    {
        static void Main(string[] args)
        {
            var job = new Job();
            if (!File.Exists(Assembly.GetExecutingAssembly().Location))
            {
                Console.WriteLine("File doesn't exist");
                return;
            }
            job.AddFile(Assembly.GetExecutingAssembly().Location);
            (new Program()).Run(new ModuleInfo(job, null));
            Console.ReadKey();
        }

        public void Run(ModuleInfo info, CancellationToken token = default)
        {
            var sw = new Stopwatch();
            var readingPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\DataFiles";

            List<Point> dots = new List<Point>();

            ReaderWriter.ReadPointsFromFile(readingPath + @"\input.txt", ref dots);

            sw.Start();
            const int pointsNum = 2;
            var points = new IPoint[pointsNum];
            var channels = new IChannel[pointsNum];
            for (var i = 0; i < pointsNum; i++)
            {
                points[i] = info.CreatePoint();
                channels[i] = points[i].CreateChannel();
                points[i].ExecuteClass("Distance.Executor");
            }

            int step = Executor.factorial(dots.Count()) / pointsNum;
            //
            for (var i = 0; i < pointsNum; i++)
            {
                channels[i].WriteObject(dots);
                channels[i].WriteObject(i * step);
                channels[i].WriteObject((i + 1) * step);
            }
            //
            double minDistance = double.MaxValue;
            var shortOrder = new List<Point>();
            for (var i = 0; i < pointsNum; i++)
            {
                var workerShortOrder = channels[i].ReadObject<List<Point>>();
                double workerShortDistance = channels[i].ReadObject<double>();

                if (workerShortDistance < minDistance)
                {
                    minDistance = workerShortDistance;
                    shortOrder = workerShortOrder;
                }
            }
            sw.Stop();

            ReaderWriter.write(readingPath + @"\output.txt", ref shortOrder, minDistance);

            Console.WriteLine($"Total time {sw.ElapsedMilliseconds} ms");
        }
    }
}