using ManagedBass;
using System;

namespace Taipu
{
    public class BadMetronomeTest
    {
        public double bpm, offset, beat;
        public int s;
        public bool init;

        public BadMetronomeTest(double b, double o)
        {
            bpm = b; offset = o;
            s = Bass.SampleLoad(SkinLoader.getRelPath("tick.wav"), 0, 0, 128, BassFlags.Default);
        }

        public void Update(double t)
        {
            double count = Math.Floor((t * 1000.0 - offset) / (60000.0 / bpm));

            if (!init || count < beat || count - beat > 10)
            {
                beat = count;
                init = true;
            }

            while (beat < count)
            {
                beat++;
                int ch = Bass.SampleGetChannel(s);
                Bass.ChannelSetAttribute(ch, ChannelAttribute.Volume, 1f);
                Bass.ChannelPlay(ch);
            }
        }
    }
}