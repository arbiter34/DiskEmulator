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
                if (seeksDone[seek] || time < seek.ArrivalTime)
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

        public Boolean LOOK()
        {
            //Tick 
            time++;

            //Next not found
            Seek next = null;

            //Iterate over list of Seeks
            foreach (Seek seek in seeks)
            {
                //If seek has been done, skip
                if (seeksDone[seek])
                {
                    continue;
                }

                //If we're on the same track, do it now
                if (seek.Position.Track == disk.Position.Track)
                {
                    next = seek;
                    break;
                }

                //Switch based on current disk direction
                switch (disk.Direction)
                {
                    case Direction.In:
                        //If seek is the wrong direction or hasn't arrived, skip
                        if (seek.Position.Track < disk.Position.Track || time < seek.ArrivalTime)
                        {
                            continue;
                        }
                        
                        //If no other next is set, use first found in correct direction
                        if (next == null)
                        {
                            next = seek;
                        }

                        //If seek position is closer than next position, replace next with seek
                        if (Math.Abs(seek.Position.Track - disk.Position.Track) < Math.Abs(next.Position.Track - disk.Position.Track))
                        {
                            next = seek;
                        }

                        break;

                    case Direction.Out:
                        //If seek is the wrong direction or hasn't arrived, skip
                        if (seek.Position.Track > disk.Position.Track || time < seek.ArrivalTime)
                        {
                            continue;
                        }

                        //If no other next is set, use first found in correct direction
                        if (next == null)
                        {
                            next = seek;
                        }

                        //If seek position is closer than next position, replace next with seek
                        if (Math.Abs(seek.Position.Track - disk.Position.Track) < Math.Abs(next.Position.Track - disk.Position.Track))
                        {
                            next = seek;
                        }

                        break;
                }

            }

            //If there is a next seek in this direction perform it
            if (next != null)
            {
                time += disk.Seek(next.Position);
                seeksDone[next] = true;
            }
            else
            {
                //If no seek, switch direction of head
                disk.Direction = disk.Direction == Direction.In ? Direction.Out : Direction.In;
            }

            return Finished();
        }

        public Boolean CLOOK()
        {
            //Tick 
            time++;

            //Next not found
            Seek next = null;

            //Iterate over list of Seeks
            foreach (Seek seek in seeks)
            {
                //If seek has been done, skip
                if (seeksDone[seek])
                {
                    continue;
                }

                //If we're on the same track, do it now
                if (seek.Position.Track == disk.Position.Track)
                {
                    next = seek;
                    break;
                }

                //If seek is the wrong direction or hasn't arrived, skip
                if (seek.Position.Track < disk.Position.Track || time < seek.ArrivalTime)
                {
                    continue;
                }

                //If no other next is set, use first found in correct direction
                if (next == null)
                {
                    next = seek;
                }

                //If seek position is closer than next position, replace next with seek
                if (Math.Abs(seek.Position.Track - disk.Position.Track) < Math.Abs(next.Position.Track - disk.Position.Track))
                {
                    next = seek;
                }                
            }

            //Not found in same direction - circle back around
            if (next == null)
            {
                foreach (Seek seek in seeks)
                {
                    //If seek has been done or hasn't arrived - skip
                    if (seeksDone[seek] || time < seek.ArrivalTime)
                    {
                        continue;
                    }

                    //If next hasn't been set, use first possible seek
                    if (next == null)
                    {
                        next = seek;
                    }

                    //If seek is closer to start (track 0) then replace next
                    if (seek.Position.Track < next.Position.Track)
                    {
                        next = seek;
                    }
                }
            }

            //If there is a next seek in this direction perform it
            if (next != null)
            {
                time += disk.Seek(next.Position);
                seeksDone[next] = true;
            }

            return Finished();
        }

        //Check if all seeks have been completed
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
