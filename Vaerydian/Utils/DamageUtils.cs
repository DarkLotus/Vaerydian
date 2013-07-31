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

using Vaerydian.Characters;
using Microsoft.Xna.Framework;

namespace Vaerydian.Utils
{
    public enum DamageClass
    {
        DIRECT,
        OVERTIME,
        AREA
    }

	public enum DamageBasis{
		NONE = 0,
		STATIC = 1,
		ATTRIBUTE = 2,
		SKILL = 3,
		ITEM = 4,
		WEAPON = 5
	}
	
	public struct DamageDef{
		public string Name;
		public DamageType DamageType;
		public DamageBasis DamageBasis;
		public int Min;
		public int Max;
		public SkillName SkillName;
		public StatType StatType;
	}

    public enum DamageType
    {
        NONE = 0,
		SLASHING = 1,
		CRUSHING = 2,
		PIERCING = 3,
		ICE = 4,
		FIRE = 5,
		EARTH = 6,
		WIND = 7,
		WATER = 8,
		LIGHT = 9,
		DARK = 10,
		CHAOS = 11,
		ORDER = 12,
		POISON = 13,
		DISEASE = 14,
		ARCANE  = 15,
		MENTAL = 16,
		SONIC = 17,
		ACID = 18
    }


	static class DamageUtils{

		public static Color getDamageColor(DamageType dType){
			switch(dType){
			case DamageType.NONE:
				return Color.White;
			case DamageType.ACID:
				return Color.YellowGreen;
			case DamageType.ARCANE:
				return Color.MediumPurple;
			case DamageType.CHAOS:
				return Color.DarkMagenta;
			case DamageType.CRUSHING:
				return Color.Wheat;
			case DamageType.DARK:
				return Color.DarkGray;
			case DamageType.DISEASE:
				return Color.SandyBrown;
			case DamageType.EARTH:
				return Color.Brown;
			case DamageType.FIRE:
				return Color.OrangeRed;
			case DamageType.ICE:
				return Color.LightBlue;
			case DamageType.LIGHT:
				return Color.LightYellow;
			case DamageType.MENTAL:
				return Color.Azure;
			case DamageType.PIERCING:
				return Color.Red;
			case DamageType.POISON:
				return Color.Green;
			case DamageType.SLASHING:
				return Color.Silver;
			case DamageType.SONIC:
				return Color.Silver;
			case DamageType.WATER:
				return Color.Blue;
			case DamageType.WIND:
				return Color.LightGray;
			default:
				return Color.White;
			}
		}

		public static int calculateDamage(){
			return -1;
		}

	}
}
