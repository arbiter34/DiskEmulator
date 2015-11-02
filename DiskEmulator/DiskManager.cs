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

        private double time;

        private List<Seek> seeks;

        private Dictionary<Seek, Boolean> seeksDone;

        private Dictionary<Seek, double> turnAroundTimes;

        public DiskManager(List<Seek> seeks)
        {
            this.seeks = seeks;
            Init();
        }

        public void Init()
        {
            disk = new Disk();
            seeksDone = new Dictionary<Seek, Boolean>();
            turnAroundTimes = new Dictionary<Seek, double>();
            foreach (Seek seek in seeks)
            {
                seeksDone.Add(seek, false);
                turnAroundTimes.Add(seek, 0);
            }
            time = 0;
        }

        //First come first serv
        public Boolean FCFS()
        {
            Boolean didSeek = false;

            //Iterate over list of seeks
            foreach (Seek seek in seeks)
            {
                //If done - skip
                if (seeksDone[seek])
                {
                    continue;
                }

                //If time is greater than or equal to arrival - start
                if (time >= seek.ArrivalTime)
                {
                    //Seek and add time to seek to time total
                    time += disk.Seek(seek.Position);

                    //Calculate turn around time (finish time - arrival time)
                    double turnAroundTime = time - seek.ArrivalTime;

                    //Log
                    Console.WriteLine("Arrival Time: {0,-5} Track: {1, -4} Sector: {2, -2} Turnaround Time: {3, -4}", 
                                        seek.ArrivalTime, 
                                        seek.Position.Track, 
                                        seek.Position.Sector, 
                                        turnAroundTime);

                    //Set turn around time in data log
                    turnAroundTimes[seek] = turnAroundTime;

                    //Mark done
                    seeksDone[seek] = true;
                    didSeek = true;
                    break;
                }
            }

            if (!didSeek)
            {
                if (time % 1 != 0)
                {
                    time = Math.Floor(time);
                }
                time++;
            }

            //Check and return whether finished
            return Finished();
        }

        //Shortest seek time first
        public Boolean SSTF()
        {

            //Init shortest to not existing
            Seek shortest = null;

            //Iterate over Seeks
            foreach (Seek seek in seeks)
            {
                //If seek is done or not arrived - skip
                if (seeksDone[seek] || time < seek.ArrivalTime)
                {
                    continue;
                }

                //Take first valid seek as shortest
                if (shortest == null)
                {
                    shortest = seek;
                }

                //If seek is closer than shortest - replace shortest with seek
                if (disk.calculateSeek(seek.Position) < disk.calculateSeek(shortest.Position))
                {
                    shortest = seek;
                }
            }

            //If a seek was found - do it
            if (shortest != null)
            {
                //Seek and add time to seek to time total
                time += disk.Seek(shortest.Position);

                //Calculate turn around time (finish time - arrival time)
                double turnAroundTime = time - shortest.ArrivalTime;

                //Log
                Console.WriteLine("Arrival Time: {0,-5} Track: {1, -4} Sector: {2, -2} Turnaround Time: {3, -4}", 
                                    shortest.ArrivalTime, 
                                    shortest.Position.Track, 
                                    shortest.Position.Sector, 
                                    turnAroundTime);

                //Set turn around time in data log
                turnAroundTimes[shortest] = turnAroundTime;

                //Mark done
                seeksDone[shortest] = true;
            }
            else
            {
                if (time % 1 != 0)
                {
                    time = Math.Floor(time);
                }
                time++;
            }

            //Check and return whether finished
            return Finished();
        }

        public Boolean LOOK()
        {
            Boolean advanceTime = true;

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

                if (time < seek.ArrivalTime)
                {
                    continue;
                }
                advanceTime = false;

                //Switch based on current disk direction
                switch (disk.Direction)
                {
                    case Direction.In:
                        //If seek is the wrong direction, skip
                        if (seek.Position.Track < disk.Position.Track)
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
                        //If seek is the wrong direction, skip
                        if (seek.Position.Track > disk.Position.Track)
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
                //Seek and add time to seek to time total
                time += disk.Seek(next.Position);

                //Calculate turn around time (finish time - arrival time)
                double turnAroundTime = time - next.ArrivalTime;

                //Log
                Console.WriteLine("Arrival Time: {0,-5} Track: {1, -4} Sector: {2, -2} Turnaround Time: {3, -4}",
                                    next.ArrivalTime,
                                    next.Position.Track,
                                    next.Position.Sector,
                                    turnAroundTime);

                //Set turn around time in data log
                turnAroundTimes[next] = turnAroundTime;

                //Mark done
                seeksDone[next] = true;
            }
            else
            {
                //If no seek, switch direction of head
                disk.Direction = disk.Direction == Direction.In ? Direction.Out : Direction.In;

            }

            if (advanceTime)
            {
                if (time % 1 != 0)
                {
                    time = Math.Floor(time);
                }
                time++;
            }

            return Finished();
        }

        public Boolean CLOOK()
        {
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
                //Seek and add time to seek to time total
                time += disk.Seek(next.Position);

                //Calculate turn around time (finish time - arrival time)
                double turnAroundTime = time - next.ArrivalTime;

                //Log
                Console.WriteLine("Arrival Time: {0,-5} Track: {1, -4} Sector: {2, -2} Turnaround Time: {3, -4}",
                                    next.ArrivalTime,
                                    next.Position.Track,
                                    next.Position.Sector,
                                    turnAroundTime);

                //Set turn around time in data log
                turnAroundTimes[next] = turnAroundTime;

                //Mark done
                seeksDone[next] = true;
            }
            else
            {
                if (time % 1 != 0)
                {
                    time = Math.Floor(time);
                }
                time++;
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

        public double Time
        {
            get { return time; }
            set { time = value; }
        }

        public Dictionary<Seek, double> TurnAroundTimes
        {
            get { return turnAroundTimes; }
            set { turnAroundTimes = value; }
        }
    }
}
