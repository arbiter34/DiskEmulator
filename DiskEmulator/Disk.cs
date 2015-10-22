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

        public Disk()
        {
            tracks = new Track[250];
            position = new PositionVector(0, 0);
        }


        public long Seek(PositionVector newPosition)
        {
            if (!newPosition.isValid)
            {
                return 0;
            }

            long time = (long)(10 + 0.1 * Math.Abs(position.Track - newPosition.Track));

            if (newPosition.Sector >= position.Sector)
            {
                time += newPosition.Sector - position.Sector;
            }
            else
            {
                time += (8 - position.Sector) + newPosition.Sector;
            }

            return time;
        }


        public PositionVector Position
        {
            get { return position; }
            set { position = value; }
        }
    }
}
