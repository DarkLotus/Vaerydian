using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorldGeneration.Cave
{
    public class CaveEngine
    {
        private Random ce_Rand = new Random();

        private static int ce_xSize, ce_ySize;

        private static int ce_Generations = 5;

        private static double ce_Probability = 0.35;

        private static int ce_FloorCountRule = 5;

        private static int ce_WallCountRule = 4;

        private Cave ce_Cave = new Cave(ce_xSize, ce_ySize);

        private String cs_StatusMessage = "Generating Cave...";

        public String StatusMessage
        {
            get { return cs_StatusMessage; }
            set { cs_StatusMessage = value; }
        }

        public void generateCave(int xSize, int ySize, int seed, int generations, double probability)
        {
            ce_xSize = xSize;
            ce_ySize = ySize;
            ce_Cave = new Cave(ce_xSize, ce_ySize);
            ce_Generations = generations;
            ce_Probability = probability;

            ce_Rand = new Random(seed);

            //setup base generations
            CaveTerrain[,] gen1 = new CaveTerrain[ce_xSize, ce_ySize];
            CaveTerrain[,] gen2 = new CaveTerrain[ce_xSize, ce_ySize];

            //fill in each generation with default walls
            for (int i = 0; i < ce_xSize; i++)
            {
                for (int j = 0; j < ce_ySize; j++)
                {
                    gen1[i, j] = new CaveTerrain();
                    gen2[i, j] = new CaveTerrain();
                    gen1[i, j].BaseCaveType = CaveType.Wall;
                    gen2[i, j].BaseCaveType = CaveType.Wall;
                }
            }

            //choose random floors based upon set probability in gen 2, but dont include the rim
            for (int i = 1; i < ce_xSize-1; i++)
            {
                for (int j = 1; j < ce_ySize-1; j++)
                {
                    if (ce_Rand.NextDouble() < ce_Probability)
                    {
                        gen2[i, j].BaseCaveType = CaveType.Floor;
                    }
                }
            }

            //generationally evolve
            for (int g = 0; g < ce_Generations; g++)
            {
                //copy over the generations
                gen1 = gen2;

                //for each generation
                for (int i = 1; i < ce_xSize - 1; i++)
                {
                    for (int j = 1; j < ce_ySize - 1; j++)
                    {
                        if (gen1[i, j].BaseCaveType == CaveType.Wall)
                        {
                            if (getCaveTypeNeighborCount(gen1, i, j, CaveType.Wall) < ce_WallCountRule)
                                gen2[i, j].BaseCaveType = CaveType.Floor;
                        }
                        else if (gen1[i, j].BaseCaveType == CaveType.Floor)
                        {
                            if (getCaveTypeNeighborCount(gen1, i, j, CaveType.Wall) >= ce_FloorCountRule)
                                gen2[i, j].BaseCaveType = CaveType.Wall;
                        }
                    }
                }
            }

            //update cave's terrain
            ce_Cave.Terrain = gen2;
        }

        /// <summary>
        /// gets the count of the matching cave type at the indicated cell in the indicated terrain
        /// </summary>
        /// <param name="terrain">terrain in which to look</param>
        /// <param name="xCell">cell's x coordinate</param>
        /// <param name="yCell">cell's y coodinate</param>
        /// <param name="caveType">cave terrain type to look for</param>
        /// <returns>integer indicating number of matching neighbors</returns>
        private int getCaveTypeNeighborCount(CaveTerrain[,] terrain, int xCell, int yCell, CaveType caveType)
        {
            int count = 0;

            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (i == 0 && j == 0)
                        continue;
                    if (terrain[xCell + i, yCell + j].BaseCaveType == caveType)
                        count++;
                }
            }

            return count;
        }


        public CaveTerrain getTerrain(int x, int y)
        {

            if (x < ce_xSize && y < ce_ySize)
                return ce_Cave.Terrain[x, y];
            else
                return new CaveTerrain();
        }
    }
}
