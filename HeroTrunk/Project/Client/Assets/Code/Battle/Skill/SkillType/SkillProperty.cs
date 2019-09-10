using System.Collections.Generic;

namespace MS
{
	public class SkillProperty : SkillTypeBase
	{
		public override void DoSkill(SkillDataSon skillDataSon, CharHandler charHandler, List<CharHandler> targets, int skillInstId)
		{
			List<SkillData> skillDatas = skillDataSon.GetSkillDataList();
			for(int i = 0; i < targets.Count; ++i)
			{
				for(int j = 0; j < skillDatas.Count; ++j)
					ChangeProperty.Change(skillDatas[j], charHandler, targets[i], i);
			}
		}
	}
}
