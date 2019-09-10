using System.Collections.Generic;

namespace MS
{
	public class SkillSpecial : SkillTypeBase
	{
		public override void DoSkill(SkillDataSon skillDataSon, CharHandler charHandler, List<CharHandler> targets, int skillInstId)
		{
			List<SkillData> skillDatas = skillDataSon.GetSkillDataList();
			switch(skillDatas[0].m_eSkillSonType)
			{
				case SkillEnum.SkillSonType.SkillRelease:
					ReleaseSkill(skillDataSon, charHandler, targets);
					break;
				case SkillEnum.SkillSonType.CleanBuff:
					CleanBuff(skillDataSon, charHandler, targets);
					break;
				case SkillEnum.SkillSonType.CleanDebuff:
					CleanDebuff(skillDataSon, charHandler, targets);
					break;
				case SkillEnum.SkillSonType.CleanBuffDebuff:
					CleanBuffDebuff(skillDataSon, charHandler, targets);
					break;
				case SkillEnum.SkillSonType.ReviveHero:
					ReviveHero(skillDatas[0], charHandler, targets);
					break;
				default:
					break;
			}
		}

		private void ReleaseSkill(SkillDataSon skillDataSon, CharHandler charHandler, List<CharHandler> targets)
		{
			for(int i = 0; i < targets.Count; ++i)
			{
				targets[i].ReleaseSkill(targets[i].m_CharData.SkillIDs[0]);
			}
		}

		private void CleanBuff(SkillDataSon skillDataSon, CharHandler charHandler, List<CharHandler> targets)
		{
			for(int i = 0; i < targets.Count; ++i)
			{
				targets[i].m_CharTick.CancelByType(SkillEnum.SkillType.Buff);
			}
		}

		private void CleanDebuff(SkillDataSon skillDataSon, CharHandler charHandler, List<CharHandler> targets)
		{
			for(int i = 0; i < targets.Count; ++i)
			{
				targets[i].m_CharTick.CancelByType(SkillEnum.SkillType.Debuff);
			}
		}

		private void CleanBuffDebuff(SkillDataSon skillDataSon, CharHandler charHandler, List<CharHandler> targets)
		{
			for(int i = 0; i < targets.Count; ++i)
			{
				targets[i].m_CharTick.CancelAll();
			}
		}

		private void ReviveHero(SkillData skillData, CharHandler charHandler, List<CharHandler> targets)
		{
			CharHandler aimCharHandler = targets[0];
			float value = BattleCalculate.ExcuteFormula(skillData.m_sEffectExpress, skillData.m_sEffectExpressMax, charHandler, aimCharHandler);
			aimCharHandler.m_CharData.CurHP = value;
			BattleManager.GetInst().m_CharInScene.ReAddChar(aimCharHandler);
			aimCharHandler.ToBorn();
		}
	}
}
