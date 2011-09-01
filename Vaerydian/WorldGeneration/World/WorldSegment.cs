using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorldGeneration.World
{
    [Serializable]
    public class WorldSegment
    {
        /// <summary>
        /// constructor
        /// </summary>
        public WorldSegment() { }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="xSize">size of the underlying terrain in x dimension</param>
        /// <param name="ySize">size of the underlying terrain in y dimension</param>
        public WorldSegment(int xSize, int ySize)
        {
            ws_xLength = xSize;
            ws_yLength = ySize;
            ws_Terrain = new Terrain[xSize, ySize];
        }
        
        /// <summary>
        /// filename of this segment
        /// </summary>
        private String ws_FileName = "none";
        /// <summary>
        /// filename of this segment
        /// </summary>
        public String FileName
        {
            get { return ws_FileName; }
            set { ws_FileName = value; }
        }

        

        /// <summary>
        /// length of the underlying terrain in x dimension
        /// </summary>
        private int ws_xLength = 128;
        /// <summary>
        /// length of the underlying terrain in y dimension
        /// </summary>
        private int ws_yLength = 128;

        /// <summary>
        /// terrain represented by this segment
        /// </summary>
        private Terrain[,] ws_Terrain = new Terrain[128, 128];
        /// <summary>
        /// terrain represented by this segment
        /// </summary>
        public Terrain[,] Terrain
        {
            get { return ws_Terrain; }
            set { ws_Terrain = value; }
        }


    }
}
