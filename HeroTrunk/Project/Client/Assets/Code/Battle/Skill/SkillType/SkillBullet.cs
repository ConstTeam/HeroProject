using System.Collections.Generic;

namespace MS
{
	public class SkillBullet : SkillTypeBase
	{
		public override void DoSkill(SkillDataSon skillDataSon, CharHandler charHandler, List<CharHandler> targets, int skillInstId)
		{
			List<SkillData> skillDatas = skillDataSon.GetSkillDataList();
			for(int i = 0; i < targets.Count; ++i)
			{
				CharHandler aimHandler = targets[i];
				BulletBase bullet = charHandler.m_CharEffect.GetSkillBulletObj(charHandler.m_CharData.m_iCurSkillID).GetComponent<BulletBase>();
				bullet.Shoot(charHandler, targets[i], () => { ChangeProperty.Change(skillDatas[0], charHandler, aimHandler, 0); });
			}
		}
	}
}
