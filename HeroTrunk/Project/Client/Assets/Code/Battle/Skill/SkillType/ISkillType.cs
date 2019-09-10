using System.Collections.Generic;
using UnityEngine;

namespace MS
{
	public class SkillTypeBase
	{
		public virtual void DoSkill(SkillDataSon skillDataSon, CharHandler charHandler, List<CharHandler> targets, int skillInstId = -1) { }

		protected bool CheckRate(SkillDataWhole skillDataWhole, CharHandler charHandler, CharHandler target)
		{
			float rate = BattleCalculate.ExcuteFormula(skillDataWhole.m_sEffectRate, null, charHandler, target);
			if(Random.Range(0f, 1f) > rate)
			{
				HUDTextMgr.GetInst().NewText(ConfigData.GetHUDText("15"), target, HUDTextMgr.HUDTextType.BUFF);   //"释放不成功"

				return false;
			}

			return true;
		}
	}
}
