using ManagedBass;
using ManagedBass.Fx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Taipu
{
    public class JukeboxSynced
    {
        public int musicStream;
        public int tempoStream;
        public double streamPosition => Bass.ChannelBytes2Seconds(tempoStream, Bass.ChannelGetPosition(tempoStream));
        public long streamPositionBytes => Bass.ChannelGetPosition(tempoStream);
        public void LoadStream(String path)
        {
            musicStream = Bass.CreateStream(path,0,0,BassFlags.Prescan | BassFlags.Decode);
            if (musicStream == 0)
            {
                Debug.WriteLine("jukebox failed to load stream "+path);
            }
            tempoStream = BassFx.TempoCreate(musicStream, BassFlags.Default);
            if (tempoStream == 0)
            {
                Debug.WriteLine("tempo stream creation failure " + Bass.LastError);
            }
            Bass.ChannelSetAttribute(tempoStream, ChannelAttribute.TempoUseAAFilter, 1f);


        }
        public void Play() => Bass.ChannelPlay(tempoStream, false);
        public void Stop() => Bass.ChannelStop(tempoStream);
    }
}
