using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorldGeneration.Cave
{
    public class Cave
    {
        public Cave(int xSize, int ySize)
        {
            c_xSize = xSize;
            c_ySize = ySize;
        }
                
        private static int c_xSize = 100;

        private static int c_ySize = 100;

        private CaveTerrain[,] c_Terrain = new CaveTerrain[c_xSize, c_ySize];

        public CaveTerrain[,] Terrain
        {
            get { return c_Terrain; }
            set { c_Terrain = value; }
        }


    }
}
