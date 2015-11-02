using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiskEmulator
{
    class Program
    {
        private static Dictionary<string, Dictionary<Seek, double>> turnAroundTimes;

        static void Main(string[] args)
        {
            SetDataRun();
            //RandomDataRun();

        }

        private static void SetDataRun()
        {
            turnAroundTimes = new Dictionary<string, Dictionary<Seek, double>>();

            List<Seek> seeks = SeekFactory.SetData();

            DiskManager manager = new DiskManager(seeks);

            Console.WriteLine("---=========== FCFS ===========---\n");

            while (!manager.FCFS()) { }

            Console.WriteLine("\nTotal: {0} ms\n", manager.Time);

            turnAroundTimes.Add("FCFS", manager.TurnAroundTimes);

            double sum = turnAroundTimes["FCFS"].Sum(x => x.Value);

            double avg = sum / turnAroundTimes["FCFS"].Count;

            double variance = Variance(turnAroundTimes["FCFS"], avg);

            double stdDev = Math.Sqrt(variance);

            Console.WriteLine("Avg: {0,-4}  Variance: {1, -4}  StdDev: {2, -4}\n\n", avg, variance, stdDev);

            manager.Init();

            Console.WriteLine("---=========== SSTF ===========---\n");

            while (!manager.SSTF()) { }

            Console.WriteLine("\nTotal: {0} ms\n", manager.Time);

            turnAroundTimes.Add("SSTF", manager.TurnAroundTimes);

            avg = sum / turnAroundTimes["SSTF"].Count;

            variance = Variance(turnAroundTimes["SSTF"], avg);

            stdDev = Math.Sqrt(variance);

            Console.WriteLine("Avg: {0,-4}  Variance: {1, -4}  StdDev: {2, -4}\n\n", avg, variance, stdDev);

            manager.Init();

            Console.WriteLine("---=========== LOOK ===========---\n");

            while (!manager.LOOK()) { }

            Console.WriteLine("\nTotal: {0} ms\n", manager.Time);

            turnAroundTimes.Add("LOOK", manager.TurnAroundTimes);

            avg = sum / turnAroundTimes["LOOK"].Count;

            variance = Variance(turnAroundTimes["LOOK"], avg);

            stdDev = Math.Sqrt(variance);

            Console.WriteLine("Avg: {0,-4}  Variance: {1, -4}  StdDev: {2, -4}\n\n", avg, variance, stdDev);

            manager.Init();

            Console.WriteLine("---=========== CLOOK ===========---\n");

            while (!manager.CLOOK()) { }

            Console.WriteLine("\nTotal: {0} ms\n", manager.Time);

            turnAroundTimes.Add("CLOOK", manager.TurnAroundTimes);

            avg = sum / turnAroundTimes["CLOOK"].Count;

            variance = Variance(turnAroundTimes["CLOOK"], avg);

            stdDev = Math.Sqrt(variance);

            Console.WriteLine("Avg: {0,-4}  Variance: {1, -4}  StdDev: {2, -4}\n\n", avg, variance, stdDev);
        }

        private static void RandomDataRun()
        {
            turnAroundTimes = new Dictionary<string, Dictionary<Seek, double>>();

            List<Seek> seeks = SeekFactory.RandomData(50);

            DiskManager manager = new DiskManager(seeks);

            Console.WriteLine("---=========== FCFS ===========---\n");

            while (!manager.FCFS()) { }

            Console.WriteLine("\nTotal: {0} ms\n", manager.Time);

            turnAroundTimes.Add("FCFS", manager.TurnAroundTimes);

            double sum = turnAroundTimes["FCFS"].Sum(x => x.Value);

            double avg = sum / turnAroundTimes["FCFS"].Count;

            double variance = Variance(turnAroundTimes["FCFS"], avg);

            double stdDev = Math.Sqrt(variance);

            Console.WriteLine("Avg: {0,-4}  Variance: {1, -4}  StdDev: {2, -4}\n\n", avg, variance, stdDev);

            manager.Init();

            Console.WriteLine("---=========== SSTF ===========---\n");

            while (!manager.SSTF()) { }

            Console.WriteLine("\nTotal: {0} ms\n", manager.Time);

            turnAroundTimes.Add("SSTF", manager.TurnAroundTimes);

            avg = sum / turnAroundTimes["SSTF"].Count;

            variance = Variance(turnAroundTimes["SSTF"], avg);

            stdDev = Math.Sqrt(variance);

            Console.WriteLine("Avg: {0,-4}  Variance: {1, -4}  StdDev: {2, -4}\n\n", avg, variance, stdDev);

            manager.Init();

            Console.WriteLine("---=========== LOOK ===========---\n");

            while (!manager.LOOK()) { }

            Console.WriteLine("\nTotal: {0} ms\n", manager.Time);

            turnAroundTimes.Add("LOOK", manager.TurnAroundTimes);

            avg = sum / turnAroundTimes["LOOK"].Count;

            variance = Variance(turnAroundTimes["LOOK"], avg);

            stdDev = Math.Sqrt(variance);

            Console.WriteLine("Avg: {0,-4}  Variance: {1, -4}  StdDev: {2, -4}\n\n", avg, variance, stdDev);

            manager.Init();

            Console.WriteLine("---=========== CLOOK ===========---\n");

            while (!manager.CLOOK()) { }

            Console.WriteLine("\nTotal: {0} ms\n", manager.Time);

            turnAroundTimes.Add("CLOOK", manager.TurnAroundTimes);

            avg = sum / turnAroundTimes["CLOOK"].Count;

            variance = Variance(turnAroundTimes["CLOOK"], avg);

            stdDev = Math.Sqrt(variance);

            Console.WriteLine("Avg: {0,-4}  Variance: {1, -4}  StdDev: {2, -4}\n\n", avg, variance, stdDev);
        }

        private static double Variance(Dictionary<Seek, double> dict, double avg)
        {
            List<double> diff = new List<double>();

            foreach (KeyValuePair<Seek, double> entry in dict)
            {
                diff.Add(Math.Pow(Math.Abs(entry.Value - avg), 2));
            }
            return diff.Average();
        }
    }
}
