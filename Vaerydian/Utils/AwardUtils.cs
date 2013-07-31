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
using ECSFramework;
using Vaerydian.Characters;
using Vaerydian.Components.Actions;
using Vaerydian.Factories;

namespace Vaerydian.Utils
{
	public static class AwardUtils
	{
		private static Random rand = new Random();


		public static void attemptSkillAward(Entity receiver, Entity awarder, float rSkill, float aSkill, SkillName skillname, int amount){
			Interactable interactor = ComponentMapper.get<Interactable> (receiver);
			Interactable interactee = ComponentMapper.get<Interactable> (awarder);

			//only do if interaction supported
			if (interactor != null && interactor != null) {
				//only skill-up if you can
				if (interactor.SupportedInteractions.MAY_ADVANCE && 
					interactee.SupportedInteractions.CAUSES_ADVANCEMENT) {

					//if still possible to skill-up
					if (rSkill < aSkill)
					{
						if (rand.NextDouble() <= ((double)(aSkill - rSkill) / (double)aSkill) * 0.5)
							UtilFactory.createSkillupAward(awarder,receiver, skillname,1);
					}

				}
			}
		}

		public static void attemptStatAward(Entity receiver, Entity awarder, float rStat, float aStat, StatType stattype, int amount){

			Interactable interactor = ComponentMapper.get<Interactable> (receiver);
			Interactable interactee = ComponentMapper.get<Interactable> (awarder);
			
			//only do if interaction supported
			if (interactor != null && interactor != null) {
				//only skill-up if you can
				if (interactor.SupportedInteractions.MAY_ADVANCE && 
				    interactee.SupportedInteractions.CAUSES_ADVANCEMENT) {
					
					//if still possible to skill-up
					if (rStat < aStat)
					{
						if (rand.NextDouble() <= ((double)(aStat - rStat) / (double)aStat) * 0.5)
							UtilFactory.createAttributeAward(awarder,receiver, stattype,1);
					}
					
				}
			}
		}


	}
}

