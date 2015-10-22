using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiskEmulator
{
    class PositionVector
    {
        public PositionVector(int track, int sector)
        {
            this.track = track;
            this.sector = sector;
        }

        private long track;

        public long Track
        {
            get { return track; }
            set { track = value; }
        }
        private long sector;

        public long Sector
        {
            get { return sector; }
            set { sector = value; }
        }

        public bool isValid
        {
            get
            {
                return sector >= 0 && sector < 8 && track >= 0 && track < 250;
            }
        }
    }
}
