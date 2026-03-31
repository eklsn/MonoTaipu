using System;
using System.Collections.Generic;

namespace Taipu
{
    public class TaipuMap
    {
        public String songName = "Untitled";
        public String songAuthor = "Unnamed";
        public String mapAuthor = "Unnamed";
        public String mapDiffName = "Normal";
        public double mapDifficulty = 0;
        public double appearTime = 0.3;
        public double preRingTime = 0.5;
        public double ringTime = 0.5;
        public double hitTimeframe = 0;
        public double disappearTime = 0.5;
        public double previewTime = 0;
        public double beatOffset = 0;
        public double bpm = 120;

        public String audioFile = "Audio file here...";
        public String videoFile = "Video file here...";
        public String imageBg = "BG image here...";
        public string imageCover = "Cover image here...";
        public bool videoOnly = false;

        public double minusHPIdle = 0.01;
        public double minusHPMiss = 0;
        public String editorsNote = "";

        public double schemeVersion = 0;

        public List<String[]> keys = new();
    }
}
