using ManagedBass;
using ManagedBass.Fx;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Taipu
{
    public class JukeboxSynced
    {
        public int musicStream;
        public int tempoStream;
        public Metronome metronome = null;
        public double streamPosition => Bass.ChannelBytes2Seconds(tempoStream, Bass.ChannelGetPosition(tempoStream));
        public long streamPositionBytes => Bass.ChannelGetPosition(tempoStream);
        public ManagedBass.PlaybackState state => Bass.ChannelIsActive(tempoStream);
        public void LoadStream(String path)
        {
            musicStream = Bass.CreateStream(path,0,0,BassFlags.Prescan | BassFlags.Decode);
            if (musicStream == 0)
            {
                Debug.WriteLine("jukebox failed to load stream "+path);
            }
            tempoStream = BassFx.TempoCreate(musicStream, BassFlags.Default);
            Bass.ChannelSetAttribute(tempoStream, ChannelAttribute.TempoUseAAFilter, 1f);
            if (tempoStream == 0)
            {
                Debug.WriteLine("tempo stream creation failure " + Bass.LastError);
                tempoStream = musicStream;
            }


        }
        public void Start(bool restart) {
            Bass.ChannelPlay(tempoStream, restart);
            if (metronome != null)
            {
                metronome.CalculateBeat(streamPositionBytes);
            }
        }

        public void Stop() => Bass.ChannelStop(tempoStream);
        public void Seek(double secondsPosition)
        {
            Bass.ChannelSetPosition(tempoStream, Bass.ChannelSeconds2Bytes(tempoStream,secondsPosition));
            if (metronome != null){
               metronome.CalculateBeat(streamPositionBytes);
            }
        }
    }
}
