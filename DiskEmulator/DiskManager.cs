using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiskEmulator
{
    class DiskManager
    {
        private Disk disk;

        private long time;

        private List<Seek> seeks;

        private Dictionary<Seek, Boolean> seeksDone;

        public DiskManager(List<Seek> seeks)
        {
            this.seeks = seeks;
            Init();
        }

        public void Init()
        {
            disk = new Disk();
            seeksDone = new Dictionary<Seek, Boolean>();
            foreach (Seek seek in seeks)
            {
                seeksDone.Add(seek, false);
            }
            time = 0;
        }

        public Boolean FCFS()
        {
            time++;

            foreach (Seek seek in seeks)
            {
                if (time > seek.ArrivalTime)
                {
                    time += disk.Seek(seek.Position);
                    seeksDone[seek] = true;
                }
            }

            return Finished();
        }

        public Boolean SSTF()
        {
            time++;

            Seek shortest = null;

            foreach (Seek seek in seeks)
            {
                if (seeksDone[seek])
                {
                    continue;
                }

                if (shortest == null)
                {
                    shortest = seek;
                }

                if (Math.Abs(seek.Position.Track - disk.Position.Track) < Math.Abs(shortest.Position.Track - disk.Position.Track))
                {
                    shortest = seek;
                }
            }

            if (shortest != null)
            {
                time += disk.Seek(shortest.Position);
                seeksDone[shortest] = true;
            }

            return Finished();
        }

        private Boolean Finished()
        {
            foreach (KeyValuePair<Seek, Boolean> entry in seeksDone)
            {
                if (!entry.Value)
                {
                    return false;
                }
            }
            return true;
        }

        public long Time
        {
            get { return time; }
            set { time = value; }
        }
    }
}
