using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorldGeneration.World
{
    [Serializable]
    public class World
    {
        /// <summary>
        /// world constructor
        /// </summary>
        public World() { }

        /// <summary>
        /// world constructor
        /// </summary>
        /// <param name="name">name of world</param>
        /// <param name="xSize">size of world in x dimension</param>
        /// <param name="ySize">size of world in y dimension</param>
        /// <param name="xSegments"># of world segments in x dimension</param>
        /// <param name="ySegments"># of world segments in y dimension</param>
        /// <param name="xSegmentLength">length of a segment in x dimension</param>
        /// <param name="ySegmentLength">length of a segment in y dimension</param>
        public World(String name, int xSize, int ySize)
        {
            w_Name = name;
            w_xSize = xSize;
            w_ySize = ySize;
        }

        #region Variables

        /// <summary>
        /// name of the world
        /// </summary>
        private String w_Name = "The World";
        /// <summary>
        /// name of the world
        /// </summary>
        public String Name
        {
            get { return w_Name; }
            set { w_Name = value; }
        }

        /// <summary>
        /// x-dimension of world
        /// </summary>
        private static int w_xSize = 1024;
        /// <summary>
        /// y-dimension of world
        /// </summary>
        private static int w_ySize = 1024;

        /// <summary>
        /// files associated with each segment
        /// </summary>
        private String[] w_SegmentFiles = new String[64];
        /// <summary>
        /// files associated with each segment
        /// </summary>
        public String[] SegmentFiles
        {
            get { return w_SegmentFiles; }
            set { w_SegmentFiles = value; }
        }





        #endregion


    }
}
