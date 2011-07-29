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

namespace WorldGeneration.Terrain
{
    /// <summary>
    /// generates all new terrain
    /// </summary>
    public class WorldGenerator
    {
        //purely private variables
        
        //testing value for wind
        private float wg_WindTestValue;

        //world temperature band placeholder
        private float wg_WorldTempBand = 0.0f;

        //perlin noise generator
        private PerlinNoise perlinNoise = new PerlinNoise();

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
        private int wg_XDimension;
        /// <summary>
        /// x dimension of world
        /// </summary>
        public int XDimension
        {
            get { return wg_XDimension; }
            set { wg_XDimension = value; }
        }
        /// <summary>
        /// y dimension of world
        /// </summary>
        private int wg_YDimension;
        /// <summary>
        /// y dimension of world
        /// </summary>
        public int YDimension
        {
            get { return wg_YDimension; }
            set { wg_YDimension = value; }
        }

        /// <summary>
        /// seeding value for world
        /// </summary>
        private float wg_Seed;
        /// <summary>
        /// seeding value for world
        /// </summary>
        public float Seed
        {
            get { return wg_Seed; }
            set { wg_Seed = value; }
        }

        /// <summary>
        /// size of the tiles
        /// </summary>
        private int wg_TileSize;

        /// <summary>
        /// size of the tiles
        /// </summary>
        public int TileSize
        {
            get { return wg_TileSize; }
            set { wg_TileSize = value; }
        }

        private ReaderWriterLock rwl = new ReaderWriterLock();
        
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

                rwl.AcquireReaderLock(Timeout.Infinite);

                try
                {
                    return wg_StatusMessage + wg_ProgressMessage;
                }
                finally 
                {
                    rwl.ReleaseLock();
                }
            
            
            }
            set { wg_StatusMessage = value; }
        }

        private int counter = 0;


        /// <summary>
        /// generates a new xDimension by yDimension world with the given seed
        /// </summary>
        /// <param name="xDimension"></param>
        /// <param name="yDimension"></param>
        /// <param name="seed"></param>
        public void generateNewWorld(int xDimension, int yDimension, int tileSize, float seed)
        {

            wg_XDimension = xDimension;
            wg_YDimension = yDimension;
            wg_Seed = seed;

            //generate world
            wg_WorldTerrainMap = new Terrain[xDimension, yDimension];

            
            //update status message
            wg_StatusMessage = "Creating Base Terrain, Height, Temperature, and Wind Maps: ";

            Terrain terrain;

            //populate the world terrain Map
            for (int x = 0; x < wg_XDimension; x++)
            {
                for (int y = 0; y < wg_YDimension; y++)
                {
                    wg_ProgressMessage =  (int)((float)((float)(counter++ ) / (float)(wg_XDimension * wg_YDimension)) * 100f) + "%";

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
            //generateRainfall(60 , 2.3f);
            generateRainfall();


            //generateRainfall(10);

            
            //reset progress message
            wg_ProgressMessage = "";
            counter = 0;


            /*
             * [NOT CURRENTLY ACTIVE]
            
            //generate rivers
            wg_StatusMessage = "Creating Rivers: ";

            //reset progress message
            wg_ProgressMessage = "";
            */



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
            //set its height
            terrain.Height = perlinNoise.perlin((double)x / wg_XDimension, (double)y / wg_YDimension, wg_Seed, 5, 4, 0.9, 0.7);

            //figure out its base terrain type
            terrain.BaseTerrainType = BaseTerrainType.Land;
            terrain.Rainfall = 0.0f;//base rainfall on land
            if (terrain.Height <= 0.1) 
            {
                terrain.Height = 0.0;
                terrain.BaseTerrainType = BaseTerrainType.Ocean; 
                //since its an ocean, also set its rainfall to 100%
                terrain.Rainfall = 1f;
            }
            if (terrain.Height >= 0.4) 
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
            if (y <= wg_YDimension / 2)
            {
                //find out what the current temp in northern hemisphere
                wg_WorldTempBand = lerp(0.0f, 1.0f, (float)y / ((float)wg_YDimension / 2));
            }
            else
            {   //find out current temp in southern hemisphere
                wg_WorldTempBand = lerp(1.0f, 0.0f, ((float)y - ((float)wg_YDimension / 2)) / ((float)wg_YDimension / 2));
            }

            //determine the temperature of the terrain cell
            if (wg_WorldTerrainMap[x, y].Height < 0.1)
            {
                //if it is water, reduce it down so the water is at most a temp of 0.4
                wg_WorldTerrainMap[x, y].Temperature = wg_WorldTempBand * 0.4f;
            }
            else if (wg_WorldTerrainMap[x, y].Height > 0.1)
            {
                //if it is above 0.3 in height, start to reduce its temperature by how far away it is from max height
                wg_WorldTerrainMap[x, y].Temperature = lerp(0f, wg_WorldTempBand, (1.0f - (float)wg_WorldTerrainMap[x, y].Height) / 0.7f);
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
            wg_WindTestValue = (float)perlinNoise.perlin(wg_WorldTerrainMap[x, y].Height, wg_WorldTerrainMap[x, y].Height, wg_Seed, 5, 4, 0.9, 0.7);


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

        private void generateRainfall()
        {

            float terrain1;
            float terrain2;
            float terrain3;
            float terrain4;
            float terrain5;
            float avg;
            float val;
            float avg2;
            float maxRainDetected = 0f;
            float[,] generationOLD = new float[wg_XDimension, wg_YDimension];
            float[,] generationNEW = new float[wg_XDimension, wg_YDimension];
            float waterContrib = 0f ;
            int xVal;
            int yVal;

            int generations = 50;
            float rainPropigation = 3.5f;
            float rainMultiplier = 300f;

            //populate generation NEW
            for (int x = 1; x < (wg_XDimension - 1); x++)
            {
                for (int y = 1; y < (wg_YDimension - 1); y++)
                {
                    generationNEW[x,y] = wg_WorldTerrainMap[x, y].Rainfall;
                }
            }

            //run each calculation several generations to allow values to equalize
            for (int i = 0; i <= generations; i++)
            {

                //make the last gen the OLD gen
                generationOLD = generationNEW;
                maxRainDetected = 0f;//reset for next gen round.

                for (int x = 0; x < (wg_XDimension); x++)
                {
                    for (int y = 0; y < (wg_YDimension); y++)
                    {
                        //update progress message
                        wg_ProgressMessage =  (int)(((float)(counter++)/(float)(generations * (wg_XDimension-1) * (wg_YDimension-1)))*100f) + "%";

                        //copy coordinates into safe values
                        xVal = x;
                        yVal = y;

                        //adjust values if on an edge of the map
                        if (x == 0)
                            xVal = 1;
                        if (x == wg_XDimension - 1)
                            xVal = wg_XDimension - 2;
                        if (y == 0)
                            yVal = 1;
                        if (y == wg_YDimension - 1)
                            yVal = wg_YDimension - 2;

                        //set whether or not water is a contributor for this coordinate
                        if (wg_WorldTerrainMap[x, y].Height <= 0.1)
                        {   //water exists here
                            waterContrib = 1f;
                        }
                        else
                        {   //no water exists here
                            waterContrib = 0f;
                        }

                        //figure out where the wind is coming from
                        /*if (wg_WorldTerrainMap[x, y].WindDirection == WindDirection.FromWest)
                        {
                            terrain1 = generationOLD[xVal - 1, yVal + 1];
                            terrain2 = generationOLD[xVal - 1, yVal];
                            terrain3 = generationOLD[xVal - 1, yVal - 1];
                            terrain4 = generationOLD[xVal, yVal + 1];
                            terrain5 = generationOLD[xVal, yVal - 1];  
                        }
                        else if (wg_WorldTerrainMap[x, y].WindDirection == WindDirection.FromEast)
                        {
                            terrain1 = generationOLD[xVal + 1, yVal + 1];
                            terrain2 = generationOLD[xVal + 1, yVal];
                            terrain3 = generationOLD[xVal + 1, yVal - 1];
                            terrain4 = generationOLD[xVal, yVal + 1];
                            terrain5 = generationOLD[xVal, yVal - 1];
                        }
                        else if (wg_WorldTerrainMap[x, y].WindDirection == WindDirection.FromNorth)
                        {
                            terrain1 = generationOLD[xVal + 1, yVal + 1];
                            terrain2 = generationOLD[xVal, yVal + 1];
                            terrain3 = generationOLD[xVal - 1, yVal + 1];
                            terrain4 = generationOLD[xVal - 1, yVal];
                            terrain5 = generationOLD[xVal + 1, yVal];
                            
                        }
                        else//from south
                        {
                            terrain1 = generationOLD[xVal + 1, yVal - 1];
                            terrain2 = generationOLD[xVal, yVal - 1];
                            terrain3 = generationOLD[xVal - 1, yVal - 1];
                            terrain4 = generationOLD[xVal - 1, yVal];
                            terrain5 = generationOLD[xVal + 1, yVal];
                        }*/

                        terrain1 = generationOLD[xVal - 1, yVal + 1];
                        terrain2 = generationOLD[xVal - 1, yVal];
                        terrain3 = generationOLD[xVal - 1, yVal - 1];
                        terrain4 = generationOLD[xVal, yVal + 1];
                        terrain5 = generationOLD[xVal, yVal - 1];  

                        avg = (terrain2 + rainPropigation * (terrain1 + terrain3) + rainPropigation * 0.5f * (terrain4 + terrain5)) / 5f;
                        val = rainMultiplier * waterContrib * wg_WorldTerrainMap[x, y].Temperature;
                        avg2 = (avg + val) / 2f;

                        generationNEW[x, y] = avg2 * (1f - (float)wg_WorldTerrainMap[x, y].Height);

                        if (generationNEW[x, y] > maxRainDetected)
                            maxRainDetected = generationNEW[x, y];
                    }
                }
            }


            //normalize all values and set the rainfall
            for (int x = 0; x < (wg_XDimension); x++)
            {
                for (int y = 0; y < (wg_YDimension); y++)
                {
                    //values appear to be exponential, so we'll need to do a logarithmic normalization based on the max rain
                    wg_WorldTerrainMap[x, y].Rainfall = (float)(Math.Log((double)generationNEW[x, y]) / (double)Math.Log(maxRainDetected));
                    //wg_WorldTerrainMap[x, y].Rainfall = generationNEW[x, y];// / maxRainDetected;
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
            for (int x = 0; x < (wg_XDimension); x++)
            {
                for (int y = 0; y < (wg_YDimension); y++)
                {
                    //update progress message
                    wg_ProgressMessage = (int)(((float)(counter++) / (float)((wg_XDimension - 1) * (wg_YDimension - 1))) * 100f) + "%";

                    //get a terrain copy
                    terrain = wg_WorldTerrainMap[x, y];

                    //check if base type is ocean
                    if (terrain.BaseTerrainType == BaseTerrainType.Ocean)
                    {
                    }//check if base type is land
                    else if (terrain.BaseTerrainType == BaseTerrainType.Land)
                    {

                        //terrain.Temperature <= 0.15
                        //Arctic
                        //terrain.Temperature <= 0.25f && terrain.Height < 0.5
                        //Tundra
                        //terrain.Temperature >.45 && terrain.Rainfall <= 0.05
                        //Desert
                        //terrain.Height > 0.1 && terrain.Height <= 0.15
                        //beach
                        //terrain.Temperature > 0.75f && terrain.Rainfall > 0.55f
                        //jungle
                        //terrain.Temperature <= 0.75f && terrain.Rainfall > 0.75f
                        //swamp
                        //terrain.Temperature > 0.25f && terrain.Rainfall > 0.25f
                        //forest
                        //whatever
                        //grassland


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
                        else if (terrain.Rainfall <= 0.05 && terrain.Temperature >.45)
                        {
                            terrain.LandTerrainType = LandTerrainType.Desert;
                            continue;
                        }
                        else if (terrain.Height > 0.1 && terrain.Height <= 0.15)
                        {
                            terrain.LandTerrainType = LandTerrainType.Beach;
                            continue;
                        }
                        
                        else if (terrain.Temperature > 0.75f && terrain.Rainfall > 0.55f)
                        {
                            terrain.LandTerrainType = LandTerrainType.Jungle;
                            continue;
                        }
                        else if (terrain.Temperature <= 0.75f && terrain.Rainfall > 0.75f)
                        {
                            terrain.LandTerrainType = LandTerrainType.Swamp;
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
                    }
                }
            }

        }


        /// <summary>
        /// [NOT CURRENTLY ACTIVE]
        /// 
        /// generates rivers on the terrains
        /// </summary>
        /// <param name="riverCount">number of rivers to generate</param>
        private void generateRivers(int riverCount)
        {
            Random rand = new Random((int)wg_Seed);
            int xVal;
            int yVal;
            int i = 0;
            wg_RiverMap = new bool[wg_XDimension, wg_YDimension];


            //create
            while (i < riverCount)
            {
                wg_ProgressMessage = i + " / " + 100;

                //choose x position
                xVal = rand.Next(wg_XDimension);

                //choose y position
                yVal = rand.Next(wg_YDimension);

                //test position as viable candidate
                if (viableRiverCandidate(xVal, yVal))
                {
                    i++;
                    buildRiver(xVal, yVal);
                }
            }

            for (int x = 0; x < wg_XDimension; x++)
            {
                for (int y = 0; y < wg_YDimension; y++)
                {

                    




                }
            }




        }


        
        /// <summary>
        /// [NOT CURRENTLY ACTIVE]
        /// 
        /// constructs a river until it reaches water
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void buildRiver(int x, int y)
        {
            bool stillHasPaths = true;

            while (stillHasPaths)
            {

            }


        }


        


        /// <summary>
        /// [NOT CURRENTLY ACTIVE]
        /// 
        /// returns true if the given coordinate is a viable river spawning location
        /// </summary>
        /// <param name="x">x coordinate</param>
        /// <param name="y">y coordiante</param>
        /// <returns></returns>
        private bool viableRiverCandidate(int x, int y)
        {
            //grab a terrain copy
            Terrain terrain = wg_WorldTerrainMap[x,y];

            //must be within the specified hieghtrange
            if ((terrain.Height > 0.3) && (terrain.Height < 0.6))
            {
                //must receive adequate rainfall
                if (terrain.Rainfall > 0.5)
                {
                    return true;
                }

                return false;
            }

            return false;
            
        }

    }
}
