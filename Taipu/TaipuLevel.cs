using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taipu
{
    public class TaipuLevel
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

        public String audioFile = "";
        public String videoFile = "";
        public String imageBg = "";
        public string imageCover = "";
        public bool videoOnly = false;

        public double minusHPPerMs = 0.01;
        public double minusHPPerMiss = 0;

        public List<String[]> keys = new();
    }
}
