using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taipu.Audio
{
    public static class BeatSnap
    {
        public static double toDivisor(double seconds, double bpm, int divisor)
        {
            double secondsPerBeat = 60.0 / bpm;
            double grid = secondsPerBeat / divisor;
            return Math.Round(seconds / grid) * grid;
        }

    }
}
