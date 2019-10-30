using System.Collections.Generic;
using UnityEngine;

namespace MS
{
	public class SpawnMgr : MonoBehaviour
	{
		public SpawnHeroBase m_SpawnHerosMine;
		public SpawnHeroBase m_SpawnHerosEnemy;

		public void EnableHerosM()
		{
			m_SpawnHerosMine.EnableCharacters();
		}

		public void CreateHerosE()
		{
			if(null != m_SpawnHerosEnemy)
				m_SpawnHerosEnemy.CreateCharacters();
		}

		public void EnableHerosE()
		{
			if(null != m_SpawnHerosEnemy)
				m_SpawnHerosEnemy.EnableCharacters();
		}

		public void ResetHeroPositionM()
		{
			List<CharHandler> lst = BattleManager.GetInst().m_CharInScene.m_listGeneralMine;
			for(int i = 0; i < lst.Count; ++i)
			{
				m_SpawnHerosMine.ResetPosition(lst[i], i);
				ResetHeroPosition(lst[i]);
			}
		}

		public void ResetHeroPositionE()
		{
			List<CharHandler> lst = BattleManager.GetInst().m_CharInScene.m_listGeneralEnemy;
			for(int i = 0; i < lst.Count; ++i)
			{
				m_SpawnHerosEnemy.ResetPosition(lst[i], i);
				ResetHeroPosition(lst[i]);
			}
		}

		private void ResetHeroPosition(CharHandler charHandler)
		{
			charHandler.ToIdle();
			charHandler.m_CharState.enabled = true;
			charHandler.m_CharMove.SetAgentEnable(true);
			charHandler.m_CharSkill.RunCD(true);
			charHandler.m_CharSkill.InitTriggerSkill();
		}
	}
}
