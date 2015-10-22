using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiskEmulator
{
    class Seek
    {
        public Seek(int arrivalTime, PositionVector position)
        {
            this.arrivalTime = arrivalTime;
            this.position = position;
        }

        private int arrivalTime;

        public int ArrivalTime
        {
            get { return arrivalTime; }
            set { arrivalTime = value; }
        }

        private PositionVector position;

        internal PositionVector Position
        {
            get { return position; }
            set { position = value; }
        }
    }
}
