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

