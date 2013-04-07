using System;
using Vaerydian.Utils;

namespace Vaerydian.Generators
{
	public static class DungeonGen
	{
		/// <summary>
        /// number of paramebers for CaveGen
        /// </summary>
        public const short DUNGEON_PARAMS_SIZE = 4;

		public const short DUNGEON_PARAMS_XSIZE = 0;

		public const short DUNGEON_PARAMS_YSIZE = 1;

		public const short DUNGEON_PARAMS_SEED = 2;

		public const short DUNGEON_PARAMS_FEATURE_COUNT = 3;

		//set direction
		private const int NORTH = 0;
		private const int EAST = 1;
		private const int SOUTH = 2;
		private const int WEST = 3;


		private static Random d_Rand = new Random();

		private static String d_StatusMessage = "Generating Dungeon...";

        public static String StatusMessage
        {
            get { return d_StatusMessage; }
            set { d_StatusMessage = value; }
        }

		/// <summary>
        /// generate a dungeon
        /// </summary>
        /// <param name="map"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static bool generate( Map map, object[] parameters)
        {
            try
            {
				generateMap(map,
				            (int)parameters[DUNGEON_PARAMS_XSIZE],
				            (int)parameters[DUNGEON_PARAMS_YSIZE],
				            (int)parameters[DUNGEON_PARAMS_SEED],
				            (int)parameters[DUNGEON_PARAMS_FEATURE_COUNT]);
            }
            catch (Exception e)
            {
				Console.Error.WriteLine(e.ToString());
                return false;
            }

            return true;
        }

		private static void generateMap(Map map, int xSize, int ySize, int seed, int features){

			d_Rand = new Random(seed);

			MapHelper.floodInitializeAll(map, TerrainType_Old.DUNGEON_BEDROCK);

			MapHelper.floodAllOp(map,setBlocking);

			MapHelper.floodFillSpecific(map,1,1,xSize-1,ySize-1, TerrainType_Old.DUNGEON_EARTH);

			//create dungeon
			createDungeon(map,xSize,ySize,features);

			MapHelper.floodAllOp(map, addVariation);
		}

		/// <summary>
        /// set terrain to blocking
        /// </summary>
        /// <param name="terrain">terrain to be set</param>
        /// <param name="args"></param>
         private static void setBlocking(Terrain terrain, params object[] args) { terrain.IsBlocking = true; }

        /// <summary>
        /// set terrain to not blocking
        /// </summary>
        /// <param name="terrain">terrain to be set</param>
        /// <param name="args"></param>
        private static void setNotBlocking(Terrain terrain, params object[] args) { terrain.IsBlocking = false; }

		private static void addVariation(Terrain terrain, params object[] args)
        {
            terrain.Variation += (float)(0.125 - (d_Rand.NextDouble() * 0.25));
        }

		private static bool createDungeon (Map map, int xSize, int ySize, int features)
		{

			makeRoom (map, xSize / 2, ySize / 2, d_Rand.Next(6,20),d_Rand.Next(6,20), d_Rand.Next (0, 4));

			int createdFeatures = 1;


			//try to build the dungeon
			for (int tries = 0; tries < 1000; tries++) {

				//if we've reached the number of desired feature, break
				if(createdFeatures == features)
					break;

				int dx = 0;
				int dy = 0;
				int xmod = 0;
				int ymod = 0;
				int direction = -1;

				//try to find a usable terrain
				for(int search = 0; search < 1000; search++){

					dx = d_Rand.Next(1,xSize-1);
					dy = d_Rand.Next(1,ySize-1);
					direction = -1;

					//if this terrain is part of the already dug dungeon...
					if(map.Terrain[dx,dy].TerrainType == TerrainType_Old.DUNGEON_WALL ||
					   map.Terrain[dx,dy].TerrainType == TerrainType_Old.DUNGEON_CORRIDOR){
						//perform tests if it can be reached
						if(map.Terrain[dx,dy+1].TerrainType == TerrainType_Old.DUNGEON_FLOOR ||
						   map.Terrain[dx,dy+1].TerrainType == TerrainType_Old.DUNGEON_CORRIDOR){
							direction = NORTH;
							xmod = 0;
							ymod = -1;
						}
						if(map.Terrain[dx-1,dy].TerrainType == TerrainType_Old.DUNGEON_FLOOR ||
						   map.Terrain[dx-1,dy].TerrainType == TerrainType_Old.DUNGEON_CORRIDOR){
							direction = EAST;
							xmod = 1;
							ymod = 0;
						}
						if(map.Terrain[dx,dy-1].TerrainType == TerrainType_Old.DUNGEON_FLOOR ||
						   map.Terrain[dx,dy-1].TerrainType == TerrainType_Old.DUNGEON_CORRIDOR){
							direction = SOUTH;
							xmod = 0;
							ymod = 1;
						}
						if(map.Terrain[dx+1,dy].TerrainType == TerrainType_Old.DUNGEON_FLOOR ||
						   map.Terrain[dx+1,dy].TerrainType == TerrainType_Old.DUNGEON_CORRIDOR){
							direction = WEST;
							xmod = -1;
							ymod = 0;
						}


						//make sure there isnt another opening nearby
						if(direction > -1){
							if(map.Terrain[dx,dy+1].TerrainType == TerrainType_Old.DUNGEON_DOOR) direction = -1;
							if(map.Terrain[dx,dy-1].TerrainType == TerrainType_Old.DUNGEON_DOOR) direction = -1;
							if(map.Terrain[dx+1,dy].TerrainType == TerrainType_Old.DUNGEON_DOOR) direction = -1;
							if(map.Terrain[dx-1,dy].TerrainType == TerrainType_Old.DUNGEON_DOOR) direction = -1;
						}

						//if we found a valid direction, break out of this look
						if (direction > -1) break;
					}

				}
				//if we fuond a valid tile, determine which feature to build
				if(direction > -1){
					int feature = d_Rand.Next(0,100);

					if(feature <= 100){//make a room
						if(makeRoom(map,dx+xmod,dy+ymod,d_Rand.Next(6,20),d_Rand.Next(6,20),direction)){
							createdFeatures++;

							//set its entrance and ensure its reachable
							map.Terrain[dx,dy].TerrainType = TerrainType_Old.DUNGEON_DOOR;
							map.Terrain[dx,dy].IsBlocking = false;
							map.Terrain[dx+xmod,dy+ymod].TerrainType = TerrainType_Old.DUNGEON_FLOOR;
							map.Terrain[dx+xmod,dy+ymod].IsBlocking = false;
						}
					}
					else if(feature >= 100){//make a corridor
						if(makeCorridor(map,dx+xmod,dy+ymod,d_Rand.Next(6,20),direction)){
							createdFeatures++;

							map.Terrain[dx,dy].TerrainType = TerrainType_Old.DUNGEON_DOOR;
							map.Terrain[dx,dy].IsBlocking = false;
						}
					}

				}
			}

			return true;
		}

		private static bool makeCorridor (Map map, int x, int y, int length, int direction)
		{
			//set corridor dimensions
			int len = d_Rand.Next (6, length);
			int dx = 0;
			int dy = 0;

			switch (direction) {
			case NORTH:
				//check if there's enough space for the corridor
				//start with checking it's not out of the boundaries
				if (x < 0 || x > map.XSize) return false;
				else dx = x;
	 
				//same thing here, to make sure it's not out of the boundaries
				for (dy = y; dy > (y-len); dy--){
					if (dy < 0 || dy > map.YSize) return false; //oh boho, it was!
					if (map.Terrain[dx,dy].TerrainType != TerrainType_Old.DUNGEON_EARTH) return false;
				}
	 
				//if we're still here, let's start building
				for (dy = y; dy > (y-len); dy--){
					map.Terrain[dx,dy].TerrainType = TerrainType_Old.DUNGEON_CORRIDOR;
					map.Terrain[dx,dy].IsBlocking = false;
				}
				break;
			case EAST:
				if (y < 0 || y > map.YSize) return false;
				else dy = y;
 
				for (dx = x; dx < (x+len); dx++){
					if (dx < 0 || dx > map.XSize) return false;
					if (map.Terrain[dx,dy].TerrainType != TerrainType_Old.DUNGEON_EARTH) return false;
				}
 
				for (dx = x; dx < (x+len); dx++){
					map.Terrain[dx,dy].TerrainType = TerrainType_Old.DUNGEON_CORRIDOR;
					map.Terrain[dx,dy].IsBlocking = false;
				}
				break;
			case SOUTH:
				if (x < 0 || x > map.XSize) return false;
				else dx = x;
	 
				for (dy = y; dy < (y+len); dy++){
					if (dy < 0 || dy > map.YSize) return false;
					if (map.Terrain[dx,dy].TerrainType != TerrainType_Old.DUNGEON_EARTH) return false;
				}
	 
				for (dy = y; dy < (y+len); dy++){
					map.Terrain[dx,dy].TerrainType = TerrainType_Old.DUNGEON_CORRIDOR;
					map.Terrain[dx,dy].IsBlocking = false;
				}
				break;
			case WEST:
				if (dy < 0 || dy > map.YSize) return false;
				else dy = y;
	 
				for (dx = x; dx > (x-len); dx--){
					if (dx < 0 || dx > map.XSize) return false;
					if (map.Terrain[dx,dy].TerrainType != TerrainType_Old.DUNGEON_EARTH) return false;
				}
	 
				for (dx = x; dx > (x-len); dx--){
					map.Terrain[dx,dy].TerrainType = TerrainType_Old.DUNGEON_CORRIDOR;
					map.Terrain[dx,dy].IsBlocking = false;
				}
				break;
			default:
				return false;
			}


			return true;
		}

		private static bool makeRoom (Map map, int x, int y, int xLength, int yLength, int direction)
		{
			//set room dimensions, should be at least 4x4
			int xlen = d_Rand.Next (6, xLength);
			int ylen = d_Rand.Next (6, yLength);

			switch (direction) {
			case NORTH:
				//space enough to build it?
				for(int dy = y; dy > (y - ylen);dy--){
					if(dy<0||dy>map.YSize) return false;
					for(int dx = (x-xlen/2);dx<(x+(xlen+1)/2);dx++){
						if(dx<0||dx>map.XSize) return false;
						if(map.Terrain[dx,dy].TerrainType != TerrainType_Old.DUNGEON_EARTH) return false;
					}
				}

				//ok to build room
				for(int dy = y; dy > (y -ylen);dy--){
					for(int dx = (x-xlen/2);dx<(x+(xlen+1)/2);dx++){
						if(dx == (x-xlen/2)) map.Terrain[dx,dy].TerrainType = TerrainType_Old.DUNGEON_WALL;
						else if(dx == (x+(xlen-1)/2))map.Terrain[dx,dy].TerrainType = TerrainType_Old.DUNGEON_WALL;
						else if(dy == y) map.Terrain[dx,dy].TerrainType = TerrainType_Old.DUNGEON_WALL;
						else if(dy == (y-ylen+1)) map.Terrain[dx,dy].TerrainType = TerrainType_Old.DUNGEON_WALL;
						else{ map.Terrain[dx,dy].TerrainType = TerrainType_Old.DUNGEON_FLOOR; map.Terrain[dx,dy].IsBlocking = false;}
					}
				}
				break;
			case EAST:
				//space enough to build it?
				for (int dy = (y-ylen/2); dy < (y+(ylen+1)/2); dy++){
					if (dy < 0 || dy > map.YSize) return false;
					for (int dx = x; dx < (x+xlen); dx++){
						if (dx < 0 || dx > map.XSize) return false;
						if (map.Terrain[dx,dy].TerrainType != TerrainType_Old.DUNGEON_EARTH) return false;
					}
				}
	 
				//ok to build room
				for (int dy = (y-ylen/2); dy < (y+(ylen+1)/2); dy++){
					for (int dx = x; dx < (x+xlen); dx++){
	 					if (dx == x) map.Terrain[dx,dy].TerrainType = TerrainType_Old.DUNGEON_WALL;
						else if (dx == (x+xlen-1)) map.Terrain[dx,dy].TerrainType = TerrainType_Old.DUNGEON_WALL;
						else if (dy == (y-ylen/2)) map.Terrain[dx,dy].TerrainType = TerrainType_Old.DUNGEON_WALL;
						else if (dy == (y+(ylen-1)/2)) map.Terrain[dx,dy].TerrainType = TerrainType_Old.DUNGEON_WALL;
	 					else{ map.Terrain[dx,dy].TerrainType = TerrainType_Old.DUNGEON_FLOOR; map.Terrain[dx,dy].IsBlocking = false;}
					}
				}

				break;
			case SOUTH:
				//space enough to build it?
				for (int dy = y; dy < (y+ylen); dy++){
					if (dy < 0 || dy > map.YSize) return false;
					for (int dx = (x-xlen/2); dx < (x+(xlen+1)/2); dx++){
						if (dx < 0 || dx > map.XSize) return false;
						if (map.Terrain[dx,dy].TerrainType != TerrainType_Old.DUNGEON_EARTH) return false;
					}
				}

				//ok to build room
				for (int dy = y; dy < (y+ylen); dy++){
					for (int dx = (x-xlen/2); dx < (x+(xlen+1)/2); dx++){
						if (dx == (x-xlen/2)) map.Terrain[dx,dy].TerrainType = TerrainType_Old.DUNGEON_WALL;
						else if (dx == (x+(xlen-1)/2)) map.Terrain[dx,dy].TerrainType = TerrainType_Old.DUNGEON_WALL;
						else if (dy == y) map.Terrain[dx,dy].TerrainType = TerrainType_Old.DUNGEON_WALL;
						else if (dy == (y+ylen-1)) map.Terrain[dx,dy].TerrainType = TerrainType_Old.DUNGEON_WALL;
						else{ map.Terrain[dx,dy].TerrainType = TerrainType_Old.DUNGEON_FLOOR; map.Terrain[dx,dy].IsBlocking = false;}
					}
				}
				break;
			case WEST:
				//space enough to build it?
				for (int dy = (y-ylen/2); dy < (y+(ylen+1)/2); dy++){
					if (dy < 0 || dy > map.YSize) return false;
					for (int dx = x; dx > (x-xlen); dx--){
						if (dx < 0 || dx > map.XSize) return false;
						if (map.Terrain[dx,dy].TerrainType != TerrainType_Old.DUNGEON_EARTH) return false;
					}
				}

				//ok to build room
				for (int dy = (y-ylen/2); dy < (y+(ylen+1)/2); dy++){
					for (int dx = x; dx > (x-xlen); dx--){
						if (dx == x) map.Terrain[dx,dy].TerrainType = TerrainType_Old.DUNGEON_WALL;
						else if (dx == (x-xlen+1)) map.Terrain[dx,dy].TerrainType = TerrainType_Old.DUNGEON_WALL;
						else if (dy == (y-ylen/2)) map.Terrain[dx,dy].TerrainType = TerrainType_Old.DUNGEON_WALL;
						else if (dy == (y+(ylen-1)/2)) map.Terrain[dx,dy].TerrainType = TerrainType_Old.DUNGEON_WALL;
						else{ map.Terrain[dx,dy].TerrainType = TerrainType_Old.DUNGEON_FLOOR; map.Terrain[dx,dy].IsBlocking = false;}
					}
				}
				break;
			default:
				return false;
			}


			return true;
		}


	}
}

