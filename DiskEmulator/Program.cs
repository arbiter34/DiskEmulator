using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiskEmulator
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Seek> seeks = SeekFactory.SetData();
            DiskManager manager = new DiskManager(seeks);

            while (!manager.FCFS()) { }

            Console.WriteLine("FCFS: {0}", manager.Time);

            manager.Init();

            while (!manager.SSTF()) { }

            Console.WriteLine("SSTF: {0}", manager.Time);

        }
    }
}
