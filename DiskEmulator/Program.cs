using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiskEmulator
{
    class Program
    {
        private static Dictionary<string, Dictionary<Seek, long>> turnAroundTimes;

        static void Main(string[] args)
        {
            turnAroundTimes = new Dictionary<string, Dictionary<Seek, long>>();

            List<Seek> seeks = SeekFactory.SetData();

            DiskManager manager = new DiskManager(seeks);

            Console.WriteLine("---=========== FCFS ===========---\n");

            while (!manager.FCFS()) { }

            Console.WriteLine("\nTotal: {0} ms\n", manager.Time);

            turnAroundTimes.Add("FCFS", manager.TurnAroundTimes);

            manager.Init();

            Console.WriteLine("---=========== SSTF ===========---\n");

            while (!manager.SSTF()) { }

            Console.WriteLine("\nTotal: {0} ms\n", manager.Time);

            turnAroundTimes.Add("SSTF", manager.TurnAroundTimes);

            manager.Init();

            Console.WriteLine("---=========== LOOK ===========---\n");

            while (!manager.LOOK()) { }

            Console.WriteLine("\nTotal: {0} ms\n", manager.Time);

            turnAroundTimes.Add("LOOK", manager.TurnAroundTimes);

            manager.Init();

            Console.WriteLine("---=========== CLOOK ===========---\n");

            while (!manager.CLOOK()) { }

            Console.WriteLine("\nTotal: {0} ms\n", manager.Time);

            turnAroundTimes.Add("CLOOK", manager.TurnAroundTimes);

        }
    }
}
