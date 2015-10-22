using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiskEmulator
{
    static class SeekFactory
    {
        public static List<Seek> SetData()
        {
            List<Seek> seeks = new List<Seek> {
                new Seek(0, new PositionVector(54, 0)),
                new Seek(23, new PositionVector(132, 6)),
                new Seek(26, new PositionVector(29, 2)),
                new Seek(29, new PositionVector(23, 1)),
                new Seek(35, new PositionVector(198, 7)),
                new Seek(45, new PositionVector(170, 5)),
                new Seek(57, new PositionVector(180, 3)),
                new Seek(83, new PositionVector(78, 4)),
                new Seek(88, new PositionVector(73, 5)),
                new Seek(95, new PositionVector(249, 7))
            };
            return seeks;
        }

        public static List<Seek> RandomData(int num)
        {
            Random random = new Random();

            List<Seek> seeks = new List<Seek>();
            for (int i = 0; i < num; i++)
            {
                seeks.Add(new Seek(i * 2, new PositionVector(random.Next(249), random.Next(7))));
            }
            return seeks;
        }
    }
}
