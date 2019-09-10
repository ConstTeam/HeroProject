using System.Collections.Generic;
namespace MS
{
	public class SkillBuff : SkillTypeBase
	{
		public override void DoSkill(SkillDataSon skillDataSon, CharHandler charHandler, List<CharHandler> targets, int skillInstId)
		{
			for(int i = 0; i < targets.Count; ++i)
			{
				TickData tickData = new TickData(charHandler, targets[i], skillDataSon, i, skillInstId);
				targets[i].m_CharTick.AddTick(tickData);
			}
		}
	}
}
