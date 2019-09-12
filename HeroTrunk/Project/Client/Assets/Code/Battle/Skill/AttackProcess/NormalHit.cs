using System.Collections.Generic;

namespace MS
{
	public class NormalHit : AttackProcess
	{
		private List<List<CharHandler>> _lstTargetLists = new List<List<CharHandler>>();
		private List<CharHandler> _lstTargets = new List<CharHandler>();

		protected override List<CharHandler> GetTargets(CharHandler charHandler, SkillDataSon skillDataSon = null, int index = -1)
		{
			_lstTargets.Clear();
			_lstTargetLists.Clear();
			BattleEnum.Enum_CharSide side = charHandler.m_CharData.GetOppositeSide();
			BattleManager.GetInst().m_CharInScene.GetAllChar(side, _lstTargetLists);

			for(int i = 0; i < _lstTargetLists.Count; ++i)
			{
				GetCharInSector(charHandler, charHandler.m_CharData.m_fAtkRadian, charHandler.m_CharData.m_fAtkRange + 2f, _lstTargetLists[i], _lstTargets);
			}

			return _lstTargets;
		}

		protected override void Excute(CharHandler charHandler, int index = -1, int skillInstId = -1)
		{
			CharData charData = charHandler.m_CharData;
			List<CharHandler> targets = GetTargets(charHandler);
			float hurt;

			if(BattleEnum.Enum_CharType.Monster == charHandler.m_CharData.m_eType)
			{
				for(int i = 0; i < targets.Count; ++i)
				{
					hurt = BattleCalculate.MonsterNormalHurt(charData, targets[i].m_CharData);
					hurt = ChangeProperty.RatioCoefficient(hurt, SkillEnum.SkillKind.Force, charHandler, targets[i]);
					targets[i].BeHit(hurt, charHandler);
				}
			}
			else
			{
				for(int i = 0; i < targets.Count; ++i)
				{
					hurt = BattleCalculate.HeroNormalHurt(charData, targets[i].m_CharData);
					hurt = 100000;//ChangeProperty.RatioCoefficient(hurt, SkillEnum.SkillKind.Force, charHandler, targets[i]);
					targets[i].BeHit(hurt, charHandler);
				}
			}
		}
	}
}
