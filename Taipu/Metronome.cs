using ManagedBass;
using System;
using System.Diagnostics;

namespace Taipu
{
    public class Metronome
    {
        public int audioStream = -1;
        public int metroStream = -1;

        public double bpm = 120;
        public int curBeat = 0;
        public int totalBeat = 0;
        public SyncProcedure beatDeleg;
        public long bytesPerBeat = 0;
        public long nextBeatPosition = 0;

        public Metronome(int stream, double tempo, double offset)
        {
            beatDeleg = new SyncProcedure(BEAT);
            audioStream = stream;
            metroStream = Bass.CreateStream("D:/Taipu/metronome.wav", 0, 0, BassFlags.Default);

            if (metroStream == 0)
            {
                Debug.WriteLine("can't find metronome snd " + Bass.LastError);
            }
            bpm = tempo;
            CalculateBeatLength(tempo);
            long currentPos = Bass.ChannelGetPosition(audioStream);
            CalculateBeat(currentPos);
        }

        public void CalculateBeatLength(double bpm)
        {
            double secsPerBeat = 60.0 / bpm;
            bytesPerBeat = Bass.ChannelSeconds2Bytes(audioStream, secsPerBeat);
        }

        public void SetBPM(double bpm)
        {
            this.bpm = bpm;
            CalculateBeatLength(bpm);
        }

        public void CalculateBeat(long currentPos)
        {
            totalBeat = (int)(currentPos / bytesPerBeat);
            curBeat = totalBeat % 4;
            nextBeatPosition = (totalBeat + 1) * bytesPerBeat;
            SchedNextBeat();
        }

        private void SchedNextBeat()
        {
            Bass.ChannelSetSync(
                audioStream,
                SyncFlags.Position | SyncFlags.Mixtime | SyncFlags.Onetime,
                nextBeatPosition,
                beatDeleg,
                IntPtr.Zero
            );
        }

        private void BEAT(int handle, int channel, int data, IntPtr user)
        {
            Bass.ChannelPlay(metroStream, true);
            totalBeat++;
            curBeat = totalBeat % 4;
            nextBeatPosition += bytesPerBeat;
            SchedNextBeat();
        }
    }
}