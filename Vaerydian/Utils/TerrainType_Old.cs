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

namespace Vaerydian.Utils
{
    public static class TerrainType_Old
    {
        //default
        public const short NOTHING = 0;
       
        //base types
        public const short BASE_LAND = 001;
        public const short BASE_OCEAN = 002;
        public const short BASE_MOUNTAIN = 003;
        public const short BASE_RIVER = 004;

        //ocean types
        public const short OCEAN_ICE = 100;
        public const short OCEAN_LITTORAL = 101;
        public const short OCEAN_SUBLITTORAL = 102;
        public const short OCEAN_ABYSSAL = 103;
        public const short OCEAN_COAST = 104;

        //land types
        public const short LAND_SCORCHED = 200;
        public const short LAND_DESERT = 201;
        public const short LAND_SAVANA = 202;
        public const short LAND_TEMPERATE_FOREST = 203;
        public const short LAND_TROPICAL_RAIN_FOREST = 204;
        public const short LAND_SWAMP = 205;
        public const short LAND_TAIGA = 206;
        public const short LAND_TUNDRA = 207;
        public const short LAND_TEMPERATE_GRASSLAND = 208;
        public const short LAND_SHRUBLAND = 209;
        public const short LAND_TEMPERATE_RAIN_FOREST = 210;
        public const short LAND_TROPICAL_FOREST = 211;
        public const short LAND_SNOW_PLAINS = 212;
        public const short LAND_MARSH = 213;
        public const short LAND_BOG = 214;
        public const short LAND_ARCTIC_DESERT = 215;
        public const short LAND_GLACIER = 216;
        public const short LAND_HYBOREAN_RIMELAND = 217;

        //mountain types
        public const short MOUNTAIN_FOOTHILL = 300;
        public const short MOUNTAIN_LOWLAND = 301;
        public const short MOUNTAIN_HIGHLAND = 302;
        public const short MOUNTAIN_CASCADE = 303;
        public const short MOUNTAIN_DRY_PEAK = 304;
        public const short MOUNTAIN_SNOWY_PEAK = 305;
        public const short MOUNTAIN_VOLCANO = 306;

        //cave types
        public const short CAVE_ENTRANCE = 400;
        public const short CAVE_WALL = 401;
        public const short CAVE_FLOOR = 402;
		public const short CAVE_STALAGMITE = 403;
				
		//forest types
		public const short FOREST_ENTRANCE = 500;
		public const short FOREST_FLOOR = 501;
		public const short FOREST_TREE = 502;

		//dungeon types
		public const short DUNGEON_EARTH = 600;
		public const short DUNGEON_BEDROCK = 601;
		public const short DUNGEON_WALL = 602;
		public const short DUNGEON_FLOOR = 603;
		public const short DUNGEON_CORRIDOR = 604;
		public const short DUNGEON_DOOR = 605;
    }
}
