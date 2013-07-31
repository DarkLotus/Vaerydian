/*
 Author:
      Thomas H. Jonell <@Net_Gnome>
 
 Copyright (c) 2013 Thomas H. Jonell

 This program is free software: you can redistribute it and/or modify
 it under the terms of the GNU Lesser General Public License as published by
 the Free Software Foundation, either version 3 of the License, or
 (at your option) any later version.

 This program is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 GNU Lesser General Public License for more details.

 You should have received a copy of the GNU Lesser General Public License
 along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Vaerydian.Utils;

using LibNoise;
using LibNoise.Models;
using LibNoise.Modfiers;
using LibNoise.Modifiers;
using System.Threading;

namespace Vaerydian.Generators
{
    /// <summary>
    /// generates a world
    /// </summary>
    public static class WorldGen
    {
        /// <summary>
        /// parameter index for world params array size
        /// </summary>
        public const short WORLD_PARAMS_SIZE = 8;

        /// <summary>
        /// parameter index for world gen's starting x index
        /// </summary>
        public const short WORLD_PARAMS_X = 0;

        /// <summary>
        /// parameter index for world gen's starting y index
        /// </summary>
        public const short WORLD_PARAMS_Y = 1;
        
        /// <summary>
        /// parameter index for world gen's finishing x index
        /// </summary>
        public const short WORLD_PARAMS_DX = 2;
        
        /// <summary>
        /// parameter index for world gen's finishing y index
        /// </summary>
        public const short WORLD_PARAMS_DY = 3;
        
        /// <summary>
        /// parameter index for z value for noise
        /// </summary>
        public const short WORLD_PARAMS_Z = 4;

        /// <summary>
        /// parameter index for number of cells in x-dimension
        /// </summary>
        public const short WORLD_PARAMS_XSIZE = 5;

        /// <summary>
        /// parameter index for number of cells in y-dimension
        /// </summary>
        public const short WORLD_PARAMS_YSIZE = 6;

        /// <summary>
        /// parameter index for random's seed value
        /// </summary>
        public const short WORLD_PARAMS_SEED = 7;

        //noise generator
        private static Perlin w_Perlin = new Perlin();
        private static Cylinder w_Noise = new Cylinder(w_Perlin);
        private static RidgedMultifractal w_Multi = new RidgedMultifractal();

        //height constants
        private const float HEIGHT_OCEAN = 0.1f;
        private const float HEIGHT_MOUNTAIN = 0.6f;

        //moisture constants
        private const float MOISTURE_LAND = 0.3f;
        private const float MOISTURE_OCEAN = 1.0f;
        private const float MOISTURE_MOUNTAIN = 0.1f;

        //humidity constants
        private const float HUMIDITY_RAIN_THRESH = 0.05f;
        private const float HUMIDITY_DECREASE_OCEAN = 0.9f;
        private const float HUMIDITY_DECREASE_LAND = 0.5f;
        private const float HUMIDITY_DECREASE_MOUNTAIN = 0.25f;
        
        //rainfall constants
        private const int RAINFALL_GENERATIONS = 1;
        private const int RAINFALL_PROPIGATION_LIMIT = 100;
        private const float RAINFALL_MOISTURE_CONVERSION = 0.5f;
        private const float RAINFALL_LAND_BASE = 0.3f;
        private const float RAINFALL_MOUNTAIN_BASE = 0.1f;
        private const float RAINFALL_OCEAN_BASE = 0.75f;

        private static Random w_Rand;

        /// <summary>
        /// locking variable
        /// </summary>
        private static ReaderWriterLock rwl = new ReaderWriterLock();

        private static String w_StatusMessage = "Generating World...";
        private static string w_ProgressMessage = "";
        /// <summary>
        /// status message
        /// </summary>
        public static String StatusMessage
        {
            get
            {                 //get a lock on the variable
                rwl.AcquireReaderLock(Timeout.Infinite);

                //attempt to return the message
                try
                {
                    return w_StatusMessage + w_ProgressMessage;
                }
                finally
                {   //ensure that the rwl is released
                    rwl.ReleaseLock();
                }
            }

            set { w_StatusMessage = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="map"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static bool generate( Map map, object[] parameters)
        {
            try
            {
                generateMap( map,
                            (int)parameters[WORLD_PARAMS_X],
                            (int)parameters[WORLD_PARAMS_Y],
                            (int)parameters[WORLD_PARAMS_DX],
                            (int)parameters[WORLD_PARAMS_DY],
                            (float)parameters[WORLD_PARAMS_Z],
                            (int)parameters[WORLD_PARAMS_XSIZE],
                            (int)parameters[WORLD_PARAMS_YSIZE],
                            (int)parameters[WORLD_PARAMS_SEED]);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.ToString());
                return false;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="map"></param>
        /// <param name="xStart"></param>
        /// <param name="yStart"></param>
        /// <param name="xFinish"></param>
        /// <param name="yFinish"></param>
        /// <param name="zSlice"></param>
        /// <param name="xSize"></param>
        /// <param name="ySize"></param>
        /// <param name="seed"></param>
        private static void generateMap( Map map, int xStart, int yStart,int xFinish, int yFinish, float zSlice, int xSize, int ySize, int seed)
        {

            w_StatusMessage = "Initializing...";

            w_Rand = new Random(seed);
            map.Seed = seed;

            //initialize the map
            MapHelper.floodInitializeAll( map, TerrainType_Old.NOTHING);

            w_Perlin.Seed = seed;
            w_Perlin.Frequency = 4;
            w_Perlin.OctaveCount = 16;
            w_Perlin.Persistence = 0.5;
            w_Perlin.NoiseQuality = NoiseQuality.Standard;
            w_Noise.SourceModule = w_Perlin;

            w_StatusMessage = "Altitude... ";

            //generate heights
            MapHelper.floodSpecificOpCoords( map, xStart, yStart, xFinish, yFinish, heightBuilder, xFinish, yFinish, zSlice);

            w_StatusMessage = "Temperature... ";

            //generate temperature
            MapHelper.floodAllOpCoords( map, temperatureBuilder, xFinish, yFinish);

            w_StatusMessage = "Wind Patterns... ";

            //generate wind
            MapHelper.floodSpecificOpCoords( map, xStart, yStart, xFinish, yFinish, windBuilder, xFinish, yFinish, zSlice);

            w_Multi.Seed = seed+1;
            w_Multi.OctaveCount = 4;
            w_Multi.Frequency = 4;
            w_Multi.Lacunarity = 4;
            w_Multi.NoiseQuality = NoiseQuality.Standard;

            w_StatusMessage = "Rainfall... ";

            //generate rainfall
            MapHelper.floodSpecificOpCoords( map, xStart, yStart, xFinish, yFinish, rainfallBuilder, xFinish, yFinish, zSlice);

            //normalize rainfall
            MapHelper.floodAllOp( map, normalizeRainfall, getMaxRainfall(map), getMinRainfall(map));

            w_StatusMessage = "Biomes...";

            //generate biomes
            MapHelper.floodAllOp( map, buildBiomes);

            MapHelper.floodAllOp(map, addVariation);

            //generate cave locations
            MapHelper.floodAllOp(map, defineCaves);

            //TODO: finish world generation
        }


        private static void defineCaves(Terrain terrain, params object[] args)
        {
            if (terrain.TerrainType == TerrainType_Old.MOUNTAIN_FOOTHILL &&
               terrain.Rainfall >= 0.5f)
            {
                if (w_Rand.NextDouble() > 0.99)
                {
                    terrain.TerrainType = TerrainType_Old.CAVE_ENTRANCE;
                    terrain.ObjectType = ObjectType.TRANSITION;
                }
            }
        }


        private static void addVariation(Terrain terrain, params object[] args)
        {
            terrain.Variation += (float)(0.125 - (w_Rand.NextDouble() * 0.25));
        }

        /// <summary>
        /// generates height for a given terrain cell
        /// </summary>
        /// <param name="terrain"></param>
        /// <param name="args"></param>
        private static void heightBuilder( Terrain terrain, params object[] args) 
        {
            int x = (int)args[0];
            int y = (int)args[1];
            int dx = (int)args[2];
            int dy = (int)args[3];
            float z = (float) args[4];

            w_ProgressMessage = (int)((float)((float)(x * dx + y) / (float)(dx * dx + dy)) * 100f) + "%";

            //set height
            float height = (float)w_Perlin.GetValue((double)((double)x / dx), (double)((double)y / dy), (double) z);
            terrain.Height = height > 1f ? 1f : height;

            //set base to land
            terrain.TerrainType = TerrainType_Old.BASE_LAND;
            terrain.Moisture = MOISTURE_LAND;

            if (terrain.Height <= HEIGHT_OCEAN)
            {
                terrain.TerrainType = TerrainType_Old.BASE_OCEAN;
                terrain.Moisture = MOISTURE_OCEAN;
            }
            if (terrain.Height >= HEIGHT_MOUNTAIN)
            {
                terrain.TerrainType = TerrainType_Old.BASE_MOUNTAIN;
                terrain.Moisture = MOISTURE_MOUNTAIN;
            }

        }

        /// <summary>
        /// generates temperature information for a given terrain cell
        /// </summary>
        /// <param name="terrain"></param>
        /// <param name="args"></param>
        private static void temperatureBuilder( Terrain terrain, params object[] args)
        {
            int x = (int) args[0];
            int y = (int) args[1];
            int dx = (int) args[2];
            int dy = (int) args[3];
            float tempBand = 0f;

            w_ProgressMessage = (int)((float)((float)(x * dx + y) / (float)(dx * dx + dy)) * 100f) + "%";

            //currently in southern or northern hemisphere
            /*
            if (y <= dy / 2)
            {
                //find out what the current temp in northern hemisphere
                tempBand = MapHelper.lerp(0.0f, 1.0f, (float)y / ((float)dy / 2));
            }
            else
            {   //find out current temp in southern hemisphere
                tempBand = MapHelper.lerp(1.0f, 0.0f, ((float)y - ((float)dy / 2)) / ((float)dy / 2));
            }*/

            tempBand = MapHelper.lerp(0.0f, 1.0f, (float)y / (float)dy);

            //determine the temperature of the terrain cell
            if (terrain.Height < HEIGHT_OCEAN)
            {
                //if it is water, reduce it down so the water is at most a temp of 0.4
                terrain.Temperature = MapHelper.lerp(0f, tempBand, (1.6f + terrain.Height) / 2f)*0.75f;
            }
            else if (terrain.Height >= HEIGHT_OCEAN)
            {
                //if it is above mountain in height, start to reduce its temperature by how far away it is from max height
                terrain.Temperature = MapHelper.lerp(0f, tempBand, (1f-terrain.Height/6f));//(1.0f - terrain.Height) / 0.9f);
            }
            else
            {
                //otherwise, just use the temperature band
                terrain.Temperature = tempBand;
            }
        }
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="terrain"></param>
        /// <param name="args"></param>
        private static void windBuilder( Terrain terrain, params object[] args)
        {
            //Retrieve arguments
            int x = (int)args[0];
            int y = (int)args[1];
            int dx = (int)args[2];
            int dy = (int)args[3];
            float z = (float)args[4];

            w_ProgressMessage = (int)((float)((float)(x * dx + y) / (float)(dx * dx + dy)) * 100f) + "%";

            //get values
            float a = (float)w_Perlin.GetValue((double)((double)x / dx), (double)((double)y / dy), z - 1.0);
            float b = (float)w_Perlin.GetValue((double)((double)x / dx), (double)((double)y / dy), z + 1.0);

            //normalize
            /*
            float l = (float) System.Math.Sqrt((double)(a * a + b * b));

            if (l == 0)
            {
                a = 0;
                b = 0;
            }
            else
            {
                a = a / l;
                b = b / l;
            }*/

            //set wind values
            terrain.WindX = a;
            terrain.WindY = b;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="terrain"></param>
        /// <param name="args"></param>
        private static void rainfallBuilder( Terrain terrain, params object[] args)
        {
            int x = (int)args[0];
            int y = (int)args[1];
            int dx = (int)args[2];
            int dy = (int)args[3];
            float z = (float)args[4];

            w_ProgressMessage = (int)((float)((float)(x * dx + y) / (float)(dx * dx + dy)) * 100f) + "%";

            //float c = (float)w_Multi.GetValue((double)x / dx, (double)y / dy, z);
            //float c = (float)w_Perlin.GetValue(terrain.WindX, terrain.WindY,z);
            float c = (float)w_Perlin.GetValue(((double)x / dx) + terrain.WindX, ((double)y / dy) + terrain.WindY, z);

            terrain.Rainfall = c;// (c + 1f) / 2f;
        }


        private static float clampRain(Terrain terrain, float rainfall)
        {
            if (rainfall >= (-1f * terrain.Temperature + 1f))
                return (-1f * terrain.Temperature + 1f);
            else
                return rainfall;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="map"></param>
        /// <returns></returns>
        private static float getMaxRainfall(Map map)
        {
            float val = 0;
            for (int i = 0; i < map.XSize; i++)
            {
                for (int j = 0; j < map.YSize; j++)
                {
                    if (map.Terrain[i, j].Rainfall > val)
                        val = map.Terrain[i, j].Rainfall;
                }
            }
            return val;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="map"></param>
        /// <returns></returns>
        private static float getMinRainfall(Map map)
        {
            float val = float.MaxValue;
            for (int i = 0; i < map.XSize; i++)
            {
                for (int j = 0; j < map.YSize; j++)
                {
                    if (map.Terrain[i, j].Rainfall < val)
                        val = map.Terrain[i, j].Rainfall;
                }
            }
            return val;
        }
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="terrain"></param>
        /// <param name="args"></param>
        private static void normalizeRainfall( Terrain terrain, params object[] args)
        {
            float maxVal = (float)args[0];
            float minVal = (float)args[1];
            float range = maxVal - minVal;
            float adj = 0f - minVal;
            float newMax = maxVal + adj;

            //terrain.Rainfall = ((terrain.Rainfall + (range / 2f)) / range) / maxVal;// (range / 2);
            terrain.Rainfall = (terrain.Rainfall + adj) / newMax;
        }

        private static void buildBiomes( Terrain terrain, params object[] args)
        {
            if (terrain.TerrainType == TerrainType_Old.BASE_OCEAN)
            {
                if (terrain.Temperature <= 0.10 && terrain.Height > -0.15)
                {
                    terrain.TerrainType = TerrainType_Old.OCEAN_ICE;
                    return;
                }
                else if (terrain.Height > 0.085 && terrain.Height <= HEIGHT_OCEAN)
                {
                    terrain.TerrainType = TerrainType_Old.OCEAN_COAST;
                    return;
                }
                else if (terrain.Height > 0.05)
                {
                    terrain.TerrainType = TerrainType_Old.OCEAN_LITTORAL;
                    return;
                }
                else if (terrain.Height > -0.15)
                {
                    terrain.TerrainType = TerrainType_Old.OCEAN_SUBLITTORAL;
                    return;
                }
                else
                {
                    terrain.TerrainType = TerrainType_Old.OCEAN_ABYSSAL;
                    return;
                }
            }
            else if (terrain.TerrainType == TerrainType_Old.BASE_LAND)
            {
                if (((terrain.Temperature >= 0f) && (terrain.Temperature < 0.40f) &&
                    (terrain.Rainfall >= 0) && (terrain.Rainfall < 0.15f)))
                {
                    terrain.TerrainType = TerrainType_Old.LAND_ARCTIC_DESERT;
                }
                else if (((terrain.Temperature >= 0.3f) && (terrain.Temperature < 0.75f) &&
                   (terrain.Rainfall >= 0) && (terrain.Rainfall < 0.15f)))
                {
                    terrain.TerrainType = TerrainType_Old.LAND_DESERT;
                }
                else if (((terrain.Temperature >= 0.75f) && (terrain.Temperature <= 1f) &&
                   (terrain.Rainfall >= 0) && (terrain.Rainfall < 0.15f)))
                {
                    terrain.TerrainType = TerrainType_Old.LAND_SCORCHED;
                }
                else if (((terrain.Temperature >= 0f) && (terrain.Temperature < 0.15f) &&
                    (terrain.Rainfall >= 0.15f) && (terrain.Rainfall < 0.65f)))
                {
                    terrain.TerrainType = TerrainType_Old.LAND_SNOW_PLAINS;
                }
                else if (((terrain.Temperature >= 0.15f) && (terrain.Temperature < 0.45f) &&
                    (terrain.Rainfall >= 0.15f) && (terrain.Rainfall < 0.40f)))
                {
                    terrain.TerrainType = TerrainType_Old.LAND_TUNDRA;
                }
                else if (((terrain.Temperature >= 0.15f) && (terrain.Temperature < 0.45f) &&
                   (terrain.Rainfall >= 0.40f) && (terrain.Rainfall < 0.65f)))
                {
                    terrain.TerrainType = TerrainType_Old.LAND_TAIGA;
                }
                else if (((terrain.Temperature >= 0.45f) && (terrain.Temperature < 0.80f) &&
                   (terrain.Rainfall >= 0.15f) && (terrain.Rainfall < 0.30f)))
                {
                    terrain.TerrainType = TerrainType_Old.LAND_TEMPERATE_GRASSLAND;
                }
                else if (((terrain.Temperature >= 0.45f) && (terrain.Temperature < 0.80f) &&
                   (terrain.Rainfall >= 0.30f) && (terrain.Rainfall < 0.45f)))
                {
                    terrain.TerrainType = TerrainType_Old.LAND_SHRUBLAND;
                }
                else if (((terrain.Temperature >= 0.80f) && (terrain.Temperature <= 1f) &&
                   (terrain.Rainfall >= 0.15) && (terrain.Rainfall < 0.45f)))
                {
                    terrain.TerrainType = TerrainType_Old.LAND_SAVANA;
                }
                else if (((terrain.Temperature >= 0.45f) && (terrain.Temperature < 0.85f) &&
                   (terrain.Rainfall >= 0.45f) && (terrain.Rainfall < 0.65f)))
                {
                    terrain.TerrainType = TerrainType_Old.LAND_TEMPERATE_FOREST;
                }
                else if (((terrain.Temperature >= 0.85f) && (terrain.Temperature <= 1f) &&
                    (terrain.Rainfall >= 0.45f) && (terrain.Rainfall < 0.75f)))
                {
                    terrain.TerrainType = TerrainType_Old.LAND_TROPICAL_FOREST;
                }
                else if (((terrain.Temperature >= 0f) && (terrain.Temperature < 0.20f) &&
                    (terrain.Rainfall >= 0.65f) && (terrain.Rainfall < 0.85f)))
                {
                    terrain.TerrainType = TerrainType_Old.LAND_GLACIER;
                }
                else if (((terrain.Temperature >= 0.20f) && (terrain.Temperature < 0.50f) &&
                (terrain.Rainfall >= 0.65f) && (terrain.Rainfall < 0.85f)))
                {
                    terrain.TerrainType = TerrainType_Old.LAND_MARSH;
                }
                else if (((terrain.Temperature >= 0.50f) && (terrain.Temperature < 0.85f) &&
                   (terrain.Rainfall >= 0.65f) && (terrain.Rainfall < 0.85f)))
                {
                    terrain.TerrainType = TerrainType_Old.LAND_TEMPERATE_RAIN_FOREST;
                }
                else if (((terrain.Temperature >= 0f) && (terrain.Temperature < 0.25f) &&
                   (terrain.Rainfall >= 0.85f) && (terrain.Rainfall <= 1f)))
                {
                    terrain.TerrainType = TerrainType_Old.LAND_HYBOREAN_RIMELAND;
                }
                else if (((terrain.Temperature >= 0.25f) && (terrain.Temperature < 0.55f) &&
                    (terrain.Rainfall >= 0.85f) && (terrain.Rainfall <= 1f)))
                {
                    terrain.TerrainType = TerrainType_Old.LAND_BOG;
                }
                else if (((terrain.Temperature >= 0.55f) && (terrain.Temperature < 0.85f) &&
                   (terrain.Rainfall >= 0.85f) && (terrain.Rainfall <= 1f)))
                {
                    terrain.TerrainType = TerrainType_Old.LAND_SWAMP;
                }
                else if (((terrain.Temperature >= 0.85f) && (terrain.Temperature <= 1f) &&
                    (terrain.Rainfall >= 0.75f) && (terrain.Rainfall <= 1f)))
                {
                    terrain.TerrainType = TerrainType_Old.LAND_TROPICAL_RAIN_FOREST;
                }
            }
            else if (terrain.TerrainType == TerrainType_Old.BASE_MOUNTAIN)
            {
                if (terrain.Height >= 0.6f && terrain.Height < 0.7f)
                {
                    terrain.TerrainType = TerrainType_Old.MOUNTAIN_FOOTHILL;
                }
                else if (terrain.Height >= 0.7f && terrain.Height < 0.8f)
                {
                    terrain.TerrainType = TerrainType_Old.MOUNTAIN_LOWLAND;
                }
                else if (terrain.Height >= 0.8f && terrain.Height < 0.9f)
                {
                    terrain.TerrainType = TerrainType_Old.MOUNTAIN_HIGHLAND;
                }
                else if (terrain.Height >= 0.9f && terrain.Height < 0.99f)
                {
                    terrain.TerrainType = TerrainType_Old.MOUNTAIN_CASCADE;
                }
                else if (terrain.Height >= 0.99f && terrain.Height <= 1f && terrain.Rainfall < 0.5f)
                {
                    terrain.TerrainType = TerrainType_Old.MOUNTAIN_DRY_PEAK;
                }
                else if (terrain.Height >= 0.99f && terrain.Height <= 1f && terrain.Rainfall >= 0.5f)
                {
                    terrain.TerrainType = TerrainType_Old.MOUNTAIN_SNOWY_PEAK;
                }
            }
        }


        private static void buildBiomesOLD( Terrain terrain, params object[] args)
        {
            //check if base type is ocean
            if (terrain.TerrainType == TerrainType_Old.BASE_OCEAN)
            {
                if (terrain.Temperature <= 0.10)// && terrain.Height > -0.25)// && terrain.Rainfall > 0.65)
                {
                    terrain.TerrainType = TerrainType_Old.OCEAN_ICE;
                    return;
                }
                else if (terrain.Height > 0.05)
                {
                    terrain.TerrainType = TerrainType_Old.OCEAN_LITTORAL;
                    return;
                }
                else if (terrain.Height > -0.15)
                {
                    terrain.TerrainType = TerrainType_Old.OCEAN_SUBLITTORAL;
                    return;
                }
                else
                {
                    terrain.TerrainType = TerrainType_Old.OCEAN_ABYSSAL;
                    return;
                }

            }//check if base type is land
            else if (terrain.TerrainType == TerrainType_Old.BASE_LAND)
            {

                if (terrain.Temperature <= 0.15)
                {
                    terrain.TerrainType = TerrainType_Old.LAND_TUNDRA;
                    return;
                }
                else if (terrain.Temperature <= 0.25f && terrain.Height < 0.5)
                {
                    terrain.TerrainType = TerrainType_Old.LAND_TAIGA;
                    return;
                }
                else if (terrain.Temperature > 0.35f && terrain.Rainfall > 0.65f && terrain.Height < 0.2)
                {
                    terrain.TerrainType = TerrainType_Old.LAND_SWAMP;
                    return;
                }
                else if (terrain.Height > 0.1 && terrain.Height <= 0.125)
                {
                    terrain.TerrainType = TerrainType_Old.OCEAN_COAST;
                    return;
                }
                else if (terrain.Rainfall <= 0.10)// && terrain.Temperature >.45)
                {
                    terrain.TerrainType = TerrainType_Old.LAND_SCORCHED;
                    return;
                }
                else if (terrain.Rainfall > 0.10 && terrain.Rainfall <= 0.25)
                {
                    terrain.TerrainType = TerrainType_Old.LAND_SAVANA;
                    return;
                }
                else if (terrain.Temperature > 0.7f && terrain.Rainfall > 0.35f)
                {
                    terrain.TerrainType = TerrainType_Old.LAND_TROPICAL_RAIN_FOREST;
                    return;
                }
                else if (terrain.Temperature > 0.25f && terrain.Rainfall > 0.25f)
                {
                    terrain.TerrainType = TerrainType_Old.LAND_TEMPERATE_FOREST;
                    return;
                }
                else
                {
                    terrain.TerrainType = TerrainType_Old.LAND_SAVANA;
                    return;
                }


            }//check if base type is mountain
            else if (terrain.TerrainType == TerrainType_Old.BASE_MOUNTAIN)
            {

                if (terrain.Height < 0.6 && terrain.Rainfall > 0.25 && terrain.Temperature > 0.25)
                {
                    terrain.TerrainType = TerrainType_Old.MOUNTAIN_FOOTHILL;
                    return;
                }
                else if (terrain.Height < 0.7 && terrain.Rainfall > 0.25 && terrain.Temperature > 0.15)
                {
                    terrain.TerrainType = TerrainType_Old.MOUNTAIN_LOWLAND;
                    return;
                }
                else if (terrain.Height >= 0.7 || terrain.Temperature < 0.1)// && terrain.Rainfall > 0.15)
                {
                    terrain.TerrainType = TerrainType_Old.MOUNTAIN_CASCADE;
                    return;
                }
                else
                {
                    terrain.TerrainType = TerrainType_Old.MOUNTAIN_HIGHLAND;
                    return;
                }
            }
        }

		/*
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
                    wg_WorldTerrainMap[x, y].Height += 0.01f;
                    minHeight = wg_WorldTerrainMap[x, y].Height;
                    continue;
                }
                else
                {
                    //if the lowest tile found was an ocean or another River tile, the river is done
                    if (wg_WorldTerrainMap[x + xVal, y + yVal].TerrainType == TerrainType.BASE_OCEAN)// || wg_WorldTerrainMap[x + xVal, y + yVal].BaseTerrainType == BaseTerrainType.River)
                    {
                        break;
                    }
                    else
                    {
                        //create the river
                        wg_WorldTerrainMap[x + xVal, y + yVal].TerrainType = TerrainType.BASE_RIVER;
                        
                        //see if this one was UL corner
                        if (xVal == 1 && yVal == 1 )
                        {
                            //see which adjacent should also be filled
                            if (wg_WorldTerrainMap[x, y + 1].Height < wg_WorldTerrainMap[x + 1, y].Height)
                            {
                                wg_WorldTerrainMap[x, y + 1].TerrainType = TerrainType.BASE_RIVER;
                            }
                            else
                            {
                                wg_WorldTerrainMap[x + 1, y].TerrainType = TerrainType.BASE_RIVER;
                            }
                        }
                        else if (xVal == -1 && yVal == -1)
                        {
                            if (wg_WorldTerrainMap[x, y-1].Height < wg_WorldTerrainMap[x-1, y].Height)
                            {
                                wg_WorldTerrainMap[x, y-1].TerrainType = TerrainType.BASE_RIVER;
                            }
                            else
                            {
                                wg_WorldTerrainMap[x - 1, y].TerrainType = TerrainType.BASE_RIVER;
                            }
                        }
                        else if(xVal == 1 && yVal == -1)
                        {
                            if (wg_WorldTerrainMap[x, y-1].Height < wg_WorldTerrainMap[x+1, y].Height)
                            {
                                wg_WorldTerrainMap[x, y-1].TerrainType = TerrainType.BASE_RIVER;
                            }
                            else
                            {
                                wg_WorldTerrainMap[x + 1, y].TerrainType = TerrainType.BASE_RIVER;
                            }
                        }
                        else if(xVal == -1 && yVal == 1)
                        {
                            if (wg_WorldTerrainMap[x-1, y].Height < wg_WorldTerrainMap[x, y+1].Height)
                            {
                                wg_WorldTerrainMap[x - 1, y].TerrainType = TerrainType.BASE_RIVER;
                            }
                            else
                            {
                                wg_WorldTerrainMap[x , y+1].TerrainType = TerrainType.BASE_RIVER;
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
                (terrain.TerrainType != TerrainType.BASE_RIVER))
            {
                return true;
            }

            return false;
            
        }
        */
    }
}
