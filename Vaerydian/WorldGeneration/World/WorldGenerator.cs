using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Threading;
using LibNoise;
using LibNoise.Modifiers;

namespace WorldGeneration.World
{
    /// <summary>
    /// generates all new terrain
    /// </summary>
    public class WorldGenerator
    {
        #region Variables

        //testing value for wind
        private float wg_WindTestValue;

        //world temperature band placeholder
        private float wg_WorldTempBand = 0.0f;

        /// <summary>
        /// perlin noise generator for land
        /// </summary>
        private Perlin land = new Perlin();
        /// <summary>
        /// ridged multi-fractal generator for mountains
        /// </summary>
        private RidgedMultifractal mountains = new RidgedMultifractal();
        /// <summary>
        /// ridge multi-fractal generator for clouds
        /// </summary>
        private RidgedMultifractal clouds = new RidgedMultifractal();

        /// <summary>
        /// terrain map for the world
        /// </summary>
        private Terrain[,] wg_WorldTerrainMap = new Terrain[0,0];
        /// <summary>
        /// terrain map for the world
        /// </summary>
        public Terrain[,] WorldTerrainMap
        {
            get { return wg_WorldTerrainMap; }
            set { wg_WorldTerrainMap = value; }
        }

        /// <summary>
        /// true if a river is present in this cell
        /// </summary>
        private bool[,] wg_RiverMap;
        /// <summary>
        /// true if a river is present in this cell
        /// </summary>
        public bool[,] RiverMap
        {
            get { return wg_RiverMap; }
            set { wg_RiverMap = value; }
        }

        /// <summary>
        /// x dimension of world
        /// </summary>
        private int wg_xFinish;
        /// <summary>
        /// x dimension of world
        /// </summary>
        public int xFinish
        {
            get { return wg_xFinish; }
            set { wg_xFinish = value; }
        }
        /// <summary>
        /// y dimension of world
        /// </summary>
        private int wg_yFinish;
        /// <summary>
        /// y dimension of world
        /// </summary>
        public int yFinish
        {
            get { return wg_yFinish; }
            set { wg_yFinish = value; }
        }

        private int wg_xStart;

        private int wg_yStart;

        /// <summary>
        /// seeding value for world
        /// </summary>
        private int wg_Seed;
        /// <summary>
        /// seeding value for world
        /// </summary>
        public int Seed
        {
            get { return wg_Seed; }
            set { wg_Seed = value; }
        }

        /// <summary>
        /// z dimension of world
        /// </summary>
        private float wg_ZSlice;
        /// <summary>
        /// z dimension of world
        /// </summary>
        public float ZSlice
        {
            get { return wg_ZSlice; }
            set { wg_ZSlice = value; }
        }

        /// <summary>
        /// size of the tiles
        /// </summary>
        private int wg_TerrainSize;

        /// <summary>
        /// size of the tiles
        /// </summary>
        public int TerrainSize
        {
            get { return wg_TerrainSize; }
            set { wg_TerrainSize = value; }
        }

        /// <summary>
        /// locking variable
        /// </summary>
        private ReaderWriterLock rwl = new ReaderWriterLock();
        
        /// <summary>
        /// holds the progress message, which gets updated throughout the generation process
        /// </summary>
        private String wg_ProgressMessage = "";

        /// <summary>
        /// status message describing generation progress
        /// </summary>
        private String wg_StatusMessage = "Generating World...";

        /// <summary>
        /// status message describing generation progress
        /// </summary>
        public String StatusMessage
        {
            get {
                //get a lock on the variable
                rwl.AcquireReaderLock(Timeout.Infinite);

                //attempt to return the message
                try
                {
                    return wg_StatusMessage + wg_ProgressMessage;
                }
                finally 
                {   //ensure that the rwl is released
                    rwl.ReleaseLock();
                }
            
            
            }
            set { wg_StatusMessage = value; }
        }

        /// <summary>
        /// counter for use in status messages
        /// </summary>
        private int counter = 0;

        #endregion

        /// <summary>
        /// generates a new xDimension by yDimension world with the given seed
        /// </summary>
        /// <param name="xDimension"></param>
        /// <param name="yDimension"></param>
        /// <param name="seed"></param>
        public void generateNewWorld(int xStart, int yStart,int xFinish, int yFinish, float zSlice, int size, int seed)
        {
            wg_xStart = xStart;
            wg_yStart = yStart;
            wg_xFinish = xFinish;
            wg_yFinish = yFinish;
            wg_ZSlice = zSlice;
            wg_Seed = seed;

            //generate world  
            wg_WorldTerrainMap = new Terrain[size, size];

            //set seed
            //perlinNoise.Random = new Random(wg_Seed);
            //perlinNoise.randomSort();

            land.Seed = wg_Seed;
            land.Persistence = 0.5;
            land.OctaveCount = 16;
            land.NoiseQuality = NoiseQuality.High;
            land.Frequency = 4;

            mountains.Seed = wg_Seed;///3;
            mountains.OctaveCount = 4;
            mountains.Frequency = 4;
            mountains.Lacunarity = 4;
            mountains.NoiseQuality = NoiseQuality.High;

            //update status message
            wg_StatusMessage = "Creating Base Terrain, Height, Temperature, and Wind Maps: ";

            Terrain terrain;

            //populate the world terrain Map
            for (int x = wg_xStart; x < wg_xFinish; x++)
            {
                for (int y = wg_yStart; y < wg_yFinish; y++)
                {
                    wg_ProgressMessage =  (int)((float)((float)(counter++ ) / (float)(wg_xFinish * wg_yFinish)) * 100f) + "%";

                    //create new temp terrain
                    terrain = new Terrain();

                    //generate height
                    generateHeight(x, y, terrain);

                    //generate temperature information based on world temp band, terrain height, etc.
                    generateTemperature(x, y, terrain);

                    //generate wind bands
                    generateWind(x, y, terrain);

                }
            }

            //reset progress message
            wg_ProgressMessage = "";
            counter = 0;

            //generate rainfall map using temperature map, wind bands, and height map
            wg_StatusMessage = "Creating Rain Map: ";
            generateRainfall();

            //reset progress message
            wg_ProgressMessage = "";
            counter = 0;
  
            //generate rivers
            wg_StatusMessage = "Creating Rivers: ";
            generateRivers(100);

            //reset progress message
            wg_ProgressMessage = "";
            counter = 0;

            //generate terrain Biomes
            wg_StatusMessage = "Creating Biomes: ";
            generateBiomes();

            //reset progress message
            wg_ProgressMessage = "";
            counter = 0;

            //populate necessary data structures
            wg_StatusMessage = "Populating Data Structures: ";

            //reset progress message
            wg_ProgressMessage = "";

            //save world
            wg_StatusMessage = "Saving World: ";

            //reset progress message
            wg_ProgressMessage = "";

            //issue a garbage collector action
            GC.Collect();
        }

        /// <summary>
        /// linearly interpolate x between values a and b
        /// </summary>
        /// <param name="a">max value</param>
        /// <param name="b">min value</param>
        /// <param name="x">value to linearly interpolate</param>
        /// <returns></returns>
        private float lerp(float a, float b, float x)
        {
            return (a * (1f - x) + b * x);
        }

        /// <summary>
        /// generates height information for a given terrain cell
        /// </summary>
        /// <param name="x">x-axis coordinate</param>
        /// <param name="y">y-axis coordinate</param>
        /// <param name="terrain">terrain cell</param>
        private void generateHeight(int x, int y, Terrain terrain)
        {
            double m = mountains.GetValue((double)x / wg_xFinish, (double)y / wg_yFinish, wg_ZSlice);
            double l = land.GetValue((double)x / wg_xFinish, (double)y / wg_yFinish, wg_ZSlice);

            terrain.Height = (m + 2*l)/3f;

            if (terrain.Height > 0.0)
                terrain.Height = terrain.Height / 0.5f;

            //figure out its base terrain type
            terrain.BaseTerrainType = BaseTerrainType.Land;
            terrain.Rainfall = 0.0f;//base rainfall on land
            if (terrain.Height <= 0.1) 
            {
                //terrain.Height = 0f;
                terrain.BaseTerrainType = BaseTerrainType.Ocean; 
                //since its an ocean, also set its rainfall to 100%
                terrain.Rainfall = 1f;
            }
            if (terrain.Height >= 0.5) 
            { 
                terrain.BaseTerrainType = BaseTerrainType.Mountain;
                terrain.Rainfall = 0f; //base rainfall on mountains
            }

            //assign the terrain to the map
            wg_WorldTerrainMap[x, y] = terrain;
        }

        /// <summary>
        /// generates temperature information for a given terrain cell
        /// </summary>
        /// <param name="x">x-axis coordinate</param>
        /// <param name="y">y-axis coordinate</param>
        /// <param name="terrain">terrain cell</param>
        private void generateTemperature(int x, int y, Terrain terrain)
        {
            //currently in souther or northern hemisphere
            if (y <= wg_yFinish / 2)
            {
                //find out what the current temp in northern hemisphere
                wg_WorldTempBand = lerp(0.0f, 1.0f, (float)y / ((float)wg_yFinish / 2));
            }
            else
            {   //find out current temp in southern hemisphere
                wg_WorldTempBand = lerp(1.0f, 0.0f, ((float)y - ((float)wg_yFinish / 2)) / ((float)wg_yFinish / 2));
            }

            //determine the temperature of the terrain cell
            if (wg_WorldTerrainMap[x, y].Height < 0.1)
            {
                //if it is water, reduce it down so the water is at most a temp of 0.4
                //wg_WorldTerrainMap[x, y].Temperature = wg_WorldTempBand * 0.8f + (float)wg_WorldTerrainMap[x,y].Height * 0.2f;
                wg_WorldTerrainMap[x, y].Temperature = lerp(0f, wg_WorldTempBand, (1.6f + (float)wg_WorldTerrainMap[x, y].Height) / 2f)*0.75f;
            }
            else if (wg_WorldTerrainMap[x, y].Height > 0.1)
            {
                //if it is above 0.3 in height, start to reduce its temperature by how far away it is from max height
                wg_WorldTerrainMap[x, y].Temperature = lerp(0f, wg_WorldTempBand, (1.0f - (float)wg_WorldTerrainMap[x, y].Height) / 0.9f);
            }
            else
            {
                //otherwise, just use the temperature band
                wg_WorldTerrainMap[x, y].Temperature = wg_WorldTempBand;
            }
        }

        /// <summary>
        /// generates wind information for a given terrain cell
        /// </summary>
        /// <param name="x">x-axis coordinate</param>
        /// <param name="y">y-axis coordinate</param>
        /// <param name="terrain">terrain cell</param>
        private void generateWind(int x, int y, Terrain terrain)
        {
            wg_WindTestValue = 0.25f;


            if (wg_WindTestValue <= 1.0f && wg_WindTestValue > 0.5)
            {
                wg_WorldTerrainMap[x, y].WindDirection = WindDirection.FromNorth;
            }
            else if (wg_WindTestValue <= 0.5f && wg_WindTestValue >= 0.0f)
            {
                wg_WorldTerrainMap[x, y].WindDirection = WindDirection.FromEast;
            }
            else if (wg_WindTestValue <= 0.0f && wg_WindTestValue > -0.5f)
            {
                wg_WorldTerrainMap[x, y].WindDirection = WindDirection.FromSouth;
            }
            else
            {
                wg_WorldTerrainMap[x, y].WindDirection = WindDirection.FromWest;
            }


            /*
            if ((wg_WindTestValue >= 0f) && (wg_WindTestValue <= 0.5f))
            {
                wg_WorldTerrainMap[x, y].WindDirection = WindDirection.FromEast;
            }
            else if ((wg_WindTestValue > 0.5f) && (wg_WindTestValue <= 1f))
            {
                wg_WorldTerrainMap[x, y].WindDirection = WindDirection.FromWest;
            }*/
            
            /*
            if ((wg_WindTestValue >= 0f) && (wg_WindTestValue <= 0.1f))
            {
                wg_WorldTerrainMap[x, y].WindDirection = WindDirection.FromEast;
            }
            else if ((wg_WindTestValue > 0.1f) && (wg_WindTestValue <= 0.2f))
            {
                wg_WorldTerrainMap[x, y].WindDirection = WindDirection.FromWest;
            }
            else if ((wg_WindTestValue > 0.2f) && (wg_WindTestValue <= 0.3f))
            {
                wg_WorldTerrainMap[x, y].WindDirection = WindDirection.FromEast;
            }
            else if ((wg_WindTestValue > 0.3f) && (wg_WindTestValue <= 0.4f))
            {
                wg_WorldTerrainMap[x, y].WindDirection = WindDirection.FromWest;
            }
            else if ((wg_WindTestValue > 0.4f) && (wg_WindTestValue <= 0.5f))
            {
                wg_WorldTerrainMap[x, y].WindDirection = WindDirection.FromEast;
            }
            else if ((wg_WindTestValue > 0.5f) && (wg_WindTestValue <= 0.6f))
            {
                wg_WorldTerrainMap[x, y].WindDirection = WindDirection.FromWest;
            }
            else if ((wg_WindTestValue > 0.6f) && (wg_WindTestValue <= 0.7f))
            {
                wg_WorldTerrainMap[x, y].WindDirection = WindDirection.FromEast;
            }
            else if ((wg_WindTestValue > 0.7f) && (wg_WindTestValue <= 0.8))
            {
                wg_WorldTerrainMap[x, y].WindDirection = WindDirection.FromWest;
            }
            else if ((wg_WindTestValue > 0.8f) && (wg_WindTestValue <= 0.9f))
            {
                wg_WorldTerrainMap[x, y].WindDirection = WindDirection.FromEast;
            }
            else if ((wg_WindTestValue > 0.9f) && (wg_WindTestValue <= 1f))
            {
                wg_WorldTerrainMap[x, y].WindDirection = WindDirection.FromWest;
            }

            wg_WorldTerrainMap[x, y].WindDirection = WindDirection.FromWest;*/
        }
       
        /// <summary>
        /// generates rainfall information for entire world
        /// </summary>
        private void generateRainfall()
        {
            float c;
            float l;
            float maxRainDetected = 0f;
            
            clouds.Seed = wg_Seed;
            clouds.OctaveCount = 4;
            clouds.Frequency = 4;
            clouds.Lacunarity = 4;
            clouds.NoiseQuality = NoiseQuality.High;
            
            land.Seed = wg_Seed;

            //determine rainfall for each
            for (int x = wg_xStart; x < (wg_xFinish); x++)
            {
                for (int y = wg_yStart; y < (wg_yFinish); y++)
                {
                    wg_ProgressMessage = (int)(((float)(counter++) / (float)((wg_xFinish) * (wg_yFinish))) * 100f) + "%";

                    c = (float)((clouds.GetValue((double)x / wg_xFinish, (double)y / wg_yFinish, wg_ZSlice) + 1f) / 2f);
                    l = (float)((land.GetValue((double)x / wg_xFinish, (double)y / wg_yFinish, wg_ZSlice) + 1f) / 2f);

                    wg_WorldTerrainMap[x, y].Rainfall = (1f-l) * c;//(1.5f - (float) wg_WorldTerrainMap[x,y].Height);
                        
                    
                    if (wg_WorldTerrainMap[x, y].Rainfall > maxRainDetected)
                        maxRainDetected = wg_WorldTerrainMap[x, y].Rainfall;
                }
            }
            
            //normalize all values and set the rainfall
            for (int x = wg_xStart; x < (wg_xFinish); x++)
            {
                for (int y = wg_yStart; y < (wg_yFinish); y++)
                {
                    wg_WorldTerrainMap[x, y].Rainfall = wg_WorldTerrainMap[x, y].Rainfall / maxRainDetected;
                }
            }
        }

        /// <summary>
        /// generates the biomes for the world
        /// </summary>
        private void generateBiomes()
        {
            Terrain terrain;

            //loop through all of them
            for (int x = wg_xStart; x < (wg_xFinish); x++)
            {
                for (int y = wg_yStart; y < (wg_yFinish); y++)
                {
                    //update progress message
                    wg_ProgressMessage = (int)(((float)(counter++) / (float)((wg_xFinish - 1) * (wg_yFinish - 1))) * 100f) + "%";

                    //get a terrain copy
                    terrain = wg_WorldTerrainMap[x, y];

                    //check if base type is ocean
                    if (terrain.BaseTerrainType == BaseTerrainType.Ocean)
                    {
                        if (terrain.Temperature <= 0.10)// && terrain.Height > -0.25)// && terrain.Rainfall > 0.65)
                        {
                            terrain.OceanTerrainType = OceanTerrainType.Ice;
                            continue;
                        }else if(terrain.Height > 0.05)
                        {
                            terrain.OceanTerrainType = OceanTerrainType.Littoral;
                            continue;
                        }
                        else if (terrain.Height > -0.15)
                        {
                            terrain.OceanTerrainType = OceanTerrainType.Sublittoral;
                            continue;
                        }
                        else
                        {
                            terrain.OceanTerrainType = OceanTerrainType.Abyssal;
                            continue;
                        }

                    }//check if base type is land
                    else if (terrain.BaseTerrainType == BaseTerrainType.Land)
                    {

                        if (terrain.Temperature <= 0.15)
                        {
                            terrain.LandTerrainType = LandTerrainType.Arctic;
                            continue;
                        }
                        else if (terrain.Temperature <= 0.25f && terrain.Height < 0.5)
                        {
                            terrain.LandTerrainType = LandTerrainType.Tundra;
                            continue;
                        }
                        else if (terrain.Temperature > 0.35f && terrain.Rainfall > 0.65f && terrain.Height < 0.2)
                        {
                            terrain.LandTerrainType = LandTerrainType.Swamp;
                            continue;
                        }
                        else if (terrain.Height > 0.1 && terrain.Height <= 0.125)
                        {
                            terrain.LandTerrainType = LandTerrainType.Beach;
                            continue;
                        }
                        else if (terrain.Rainfall <= 0.10)// && terrain.Temperature >.45)
                        {
                            terrain.LandTerrainType = LandTerrainType.Desert;
                            continue;
                        }
                        else if (terrain.Rainfall > 0.10 && terrain.Rainfall <= 0.25)
                        {
                            terrain.LandTerrainType = LandTerrainType.Grassland;
                            continue;
                        }
                        else if (terrain.Temperature > 0.7f && terrain.Rainfall > 0.35f)
                        {
                            terrain.LandTerrainType = LandTerrainType.Jungle;
                            continue;
                        }
                        else if (terrain.Temperature > 0.25f && terrain.Rainfall > 0.25f)
                        {
                            terrain.LandTerrainType = LandTerrainType.Forest;
                            continue;
                        }
                        else
                        {
                            terrain.LandTerrainType = LandTerrainType.Grassland;
                            continue;
                        }


                    }//check if base type is mountain
                    else if (terrain.BaseTerrainType == BaseTerrainType.Mountain)
                    {

                        if (terrain.Height < 0.6 && terrain.Rainfall > 0.25 && terrain.Temperature > 0.25)
                        {
                            terrain.MountainTerrainType = MountainTerrainType.Foothill;
                            continue;
                        }
                        else if (terrain.Height < 0.7 && terrain.Rainfall > 0.25 && terrain.Temperature > 0.15)
                        {
                            terrain.MountainTerrainType = MountainTerrainType.Steppes;
                            continue;
                        }
                        else if (terrain.Height >= 0.7 || terrain.Temperature < 0.1)// && terrain.Rainfall > 0.15)
                        {
                            terrain.MountainTerrainType = MountainTerrainType.SnowyPeak;
                            continue;
                        }
                        else
                        {
                            terrain.MountainTerrainType = MountainTerrainType.Cascade;
                            continue;
                        }


                    }
                }
            }

        }

        /// <summary>
        /// generates rivers on the terrains
        /// </summary>
        /// <param name="riverCount">number of rivers to generate</param>
        private void generateRivers(int riverCount)
        {
            Random rand = new Random(wg_Seed);
            int xVal;
            int yVal;
            int i = 0;
            wg_RiverMap = new bool[wg_xFinish, wg_yFinish];


            //create
            while (i < riverCount)
            {
                wg_ProgressMessage = i + " / " + 100;


                yVal = wg_yFinish - 2;

                //choose x position
                xVal = rand.Next(wg_xFinish);

                //choose y position
                yVal = rand.Next(wg_yFinish);

                //ensure they are not on a map edge
                if (xVal == 0)
                    xVal = 1;
                if (xVal == wg_xFinish - 1)
                    xVal = wg_xFinish - 2;
                if (yVal == 0)
                    yVal = 1;
                if (yVal == wg_yFinish - 1)
                    yVal = wg_yFinish - 2;

                //test position as viable candidate
                if (viableRiverCandidate(xVal, yVal))
                {
                    i++;
                    buildRiver(xVal, yVal);
                }
            }
        }

        /// <summary>
        /// constructs a river until it reaches water
        /// </summary>
        /// <param name="x">x coord start</param>
        /// <param name="y">y coord start</param>
        private void buildRiver(int xStart, int yStart)
        {
            bool stillHasPaths = true;

            int x = xStart;
            int y = yStart;
            int xVal = 0;
            int yVal = 0;

            double minHeight = wg_WorldTerrainMap[x, y].Height;

            while (stillHasPaths)
            {
                //check each of this cell's neighbors
                for (int i = -1; i < 2; i++)
                {
                    for (int j = -1; j < 2; j++)
                    {

                        if (wg_WorldTerrainMap[x + i, y + j].Height < minHeight)
                        {
                            //skip this cell
                            if (i == 0 && j == 0)
                               continue;

                            //capture the lowest point seen
                            minHeight = wg_WorldTerrainMap[x + i, y + j].Height;
                            xVal = i;
                            yVal = j;
                        }
                    }
                }

                //check to see if a low point was found
                if (xVal == 0 && yVal == 0)
                {
                    //no low points were found reduce parameters
                    wg_WorldTerrainMap[x, y].Height += 0.01;
                    minHeight = wg_WorldTerrainMap[x, y].Height;
                    continue;
                }
                else
                {
                    //if the lowest tile found was an ocean or another River tile, the river is done
                    if (wg_WorldTerrainMap[x + xVal, y + yVal].BaseTerrainType == BaseTerrainType.Ocean)// || wg_WorldTerrainMap[x + xVal, y + yVal].BaseTerrainType == BaseTerrainType.River)
                    {
                        break;
                    }
                    else
                    {
                        //create the river
                        wg_WorldTerrainMap[x + xVal, y + yVal].BaseTerrainType = BaseTerrainType.River;
                        
                        //see if this one was UL corner
                        if (xVal == 1 && yVal == 1 )
                        {
                            //see which adjacent should also be filled
                            if (wg_WorldTerrainMap[x, y + 1].Height < wg_WorldTerrainMap[x + 1, y].Height)
                            {
                                wg_WorldTerrainMap[x, y + 1].BaseTerrainType = BaseTerrainType.River;
                            }
                            else
                            {
                                wg_WorldTerrainMap[x + 1, y].BaseTerrainType = BaseTerrainType.River;
                            }
                        }
                        else if (xVal == -1 && yVal == -1)
                        {
                            if (wg_WorldTerrainMap[x, y-1].Height < wg_WorldTerrainMap[x-1, y].Height)
                            {
                                wg_WorldTerrainMap[x, y-1].BaseTerrainType = BaseTerrainType.River;
                            }
                            else
                            {
                                wg_WorldTerrainMap[x - 1, y].BaseTerrainType = BaseTerrainType.River;
                            }
                        }
                        else if(xVal == 1 && yVal == -1)
                        {
                            if (wg_WorldTerrainMap[x, y-1].Height < wg_WorldTerrainMap[x+1, y].Height)
                            {
                                wg_WorldTerrainMap[x, y-1].BaseTerrainType = BaseTerrainType.River;
                            }
                            else
                            {
                                wg_WorldTerrainMap[x + 1, y].BaseTerrainType = BaseTerrainType.River;
                            }
                        }
                        else if(xVal == -1 && yVal == 1)
                        {
                            if (wg_WorldTerrainMap[x-1, y].Height < wg_WorldTerrainMap[x, y+1].Height)
                            {
                                wg_WorldTerrainMap[x - 1, y].BaseTerrainType = BaseTerrainType.River;
                            }
                            else
                            {
                                wg_WorldTerrainMap[x , y+1].BaseTerrainType = BaseTerrainType.River;
                            }
                        }


                        //setup variables for next pass
                        x += xVal;
                        y += yVal;
                        minHeight = wg_WorldTerrainMap[x, y].Height;
                        xVal = 0;
                        yVal = 0;

                        if (x == 0)
                            break;
                        if (x == wg_xFinish - 1)
                            break;
                        if (y == 0)
                            break;
                        if (y == wg_yFinish - 1)
                            break;
                    }
                }

            }


        }

        /// <summary>
        /// returns true if the given coordinate is a viable river spawning location
        /// </summary>
        /// <param name="x">x coordinate</param>
        /// <param name="y">y coordiante</param>
        /// <returns></returns>
        private bool viableRiverCandidate(int x, int y)
        {
            //grab a terrain copy
            Terrain terrain = wg_WorldTerrainMap[x,y];

            //must be within the specified parameters
            if ((terrain.Height > 0.5) && (terrain.Height < 0.7) &&
                (terrain.Rainfall > 0.15) && (terrain.Temperature > 0.15) &&
                (terrain.BaseTerrainType != BaseTerrainType.River))
            {
                return true;
            }

            return false;
            
        }

    }
}
