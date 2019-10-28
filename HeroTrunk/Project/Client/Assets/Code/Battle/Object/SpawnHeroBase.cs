using System.Collections.Generic;
using UnityEngine;

namespace MS
{
	public class SpawnHeroBase : SpawnBase
	{
		private void CreateCharOfficial(BattleEnum.Enum_CharSide side, int charId, int charIndex)
		{
			CharHandler h = BattleScenePool.GetInst().LoadHero(side, charId, charIndex);
			h.m_CharData.SetCharType(BattleEnum.Enum_CharType.Official);
			SetObstacleAvoidance(h);
			h.m_CharSkill.InitTriggerSkill();
			SetRingLight(h);
			BattleManager.GetInst().m_CharInScene.AddChar(h);
		}

		public override void ShowCharacters(bool bShow)
		{
			List<CharHandler> lst;
			if(BattleEnum.Enum_CharSide.Mine == m_CharSide)
				lst = BattleManager.GetInst().m_CharInScene.m_listGeneralMine;
			else
				lst = BattleManager.GetInst().m_CharInScene.m_listGeneralEnemy;

			for(int i = 0; i < lst.Count; ++i)
			{
				lst[i].m_ParentTrans.gameObject.SetActive(bShow);
			}
		}

		public Transform GetSpawnTrans()
		{
			return spawnPoints[0];
		}
	}
}
