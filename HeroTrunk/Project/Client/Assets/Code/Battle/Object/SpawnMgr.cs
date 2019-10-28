using System.Collections.Generic;
using UnityEngine;

namespace MS
{
	public class SpawnMgr : MonoBehaviour
	{
		public enum SpawnType
		{
			Normal,
			PVP
		}

		public SpawnType m_eSpawnType;
		public SpawnHeroBase m_SpawnHerosMine;
		public SpawnHeroBase m_SpawnHerosEnemy;

		private int _iCurSpawnMonsterIndex;

		private static SpawnMgr _inst;
		public static SpawnMgr GetInst()
		{
			return _inst;
		}

		private void OnDestroy()
		{
			_inst = null;
		}

		private void Awake()
		{
			_inst = this;
			_iCurSpawnMonsterIndex = 0;
		}

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

		public SpawnBase GetNextSpawn()
		{
			//if(_iCurSpawnMonsterIndex < m_SpawnMonsters.Length)
			//	return m_SpawnMonsters[_iCurSpawnMonsterIndex];

			if(null != m_SpawnHerosEnemy && m_SpawnHerosEnemy.ColliderEnable)
				return m_SpawnHerosEnemy;

			return null;
		}
	}
}
