using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiskEmulator
{
    class Disk
    {
        private PositionVector position;

        private Track[] tracks;

        private Direction direction;

        public Disk()
        {
            tracks = new Track[250];
            position = new PositionVector(0, 0);
            direction = Direction.In;
        }


        public double Seek(PositionVector newPosition)
        {
            if (!newPosition.isValid)
            {
                return 0;
            }

            direction = newPosition.Track > position.Track ? Direction.In : Direction.Out;

            double time = calculateSeek(newPosition);

            position.Track = newPosition.Track;
            position.Sector = newPosition.Sector;

            return time;
        }

        public double calculateSeek(PositionVector newPosition) {
            double time = (10 + 0.1 * Math.Abs(position.Track - newPosition.Track));

            if (newPosition.Sector >= position.Sector)
            {
                time += newPosition.Sector - position.Sector;
            }
            else
            {
                time += (8 - position.Sector) + newPosition.Sector;
            }


            //Include transfer time of 1 ms
            time++;

            return time;
        }


        public PositionVector Position
        {
            get { return position; }
            set { position = value; }
        }

        public Direction Direction
        {
            get { return direction; }
            set { direction = value; }
        }
    }
}
