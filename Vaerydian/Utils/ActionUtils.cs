using System;

namespace Vaerydian
{
	public static class ActionUtils
	{

		public static float getStatProbability(int skillValue, int attributeValue, float knowledgeValue, int speedValue){
			return skillValue / 4f + attributeValue / 4f + knowledgeValue + speedValue;
		}

		public static float getHitProbability(float defender, float attacker, float max, float min){
			return (attacker/(defender+attacker))*max + (defender/(defender+attacker))*min;
		}

		public static float getDamage(float overhit, float attackSkill, float attackAttribute,
			                              float lethality, float mitigation, float absorbValue){
				return (overhit + 1f) * (attackSkill/5 + attackAttribute/4) * (lethality/mitigation) - absorbValue/10;
		}
		
	}
}

