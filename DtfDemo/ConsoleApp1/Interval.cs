using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtTest
{
    public class Interval
    {
        public Interval(int start, int end, int step)
        {
            Start = start;
            End = end;
            Step = step;
        }

        public int Start { get; set; }
        public int End { get; set; }
        public int Step { get; set; }
    }
}
