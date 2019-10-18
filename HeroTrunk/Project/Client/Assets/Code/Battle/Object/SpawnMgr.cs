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
		public SpawnMonster[] m_SpawnMonsters;

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

		public void Begin()
		{
			SetSpawnMonster();
		}

		public void CreateHeroM(int heroId, int heroIndex)
		{
			m_SpawnHerosMine.CreateChar(BattleEnum.Enum_CharSide.Mine, heroId, heroIndex);
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

		private void SetSpawnMonster()
		{
			string strIds = SectionData.GetMonsterSpawns(BattleManager.m_iSectionID, BattleManager.m_eBattleType);
			if(string.Empty == strIds)
			{
				if(null != m_SpawnHerosEnemy)
					m_SpawnHerosEnemy.ColliderEnable = true;
			}
			else
			{
				string[] monsterInfos = strIds.Split('|');
				string[] ids;
				for(int i = 0; i < m_SpawnMonsters.Length; ++i)
				{
					if(i > monsterInfos.Length - 1)
					{
						Debug.LogError("配置表中小怪数量和刷怪点数量不符，请修改！");
						break;
					}
					ids = monsterInfos[i].Split(';');
					m_SpawnMonsters[i].m_iIndex = i;
					m_SpawnMonsters[i].m_iMonsterID = int.Parse(ids[0]);
					m_SpawnMonsters[i].m_iWaves = int.Parse(ids[1]);
				}
				m_SpawnMonsters[0].ColliderEnable = true;
			}
		}

		public SpawnBase GetNextSpawn()
		{
			if(_iCurSpawnMonsterIndex < m_SpawnMonsters.Length)
				return m_SpawnMonsters[_iCurSpawnMonsterIndex];

			if(null != m_SpawnHerosEnemy && m_SpawnHerosEnemy.ColliderEnable)
				return m_SpawnHerosEnemy;

			return null;
		}

		public void SetSpawnState()
		{
			if(_iCurSpawnMonsterIndex >= m_SpawnMonsters.Length)
				return;

			if(m_SpawnMonsters[_iCurSpawnMonsterIndex].m_iWaves <= 0)
			{
				++_iCurSpawnMonsterIndex;
				if(_iCurSpawnMonsterIndex < m_SpawnMonsters.Length)
					m_SpawnMonsters[_iCurSpawnMonsterIndex].ColliderEnable = true;
				else if(null != m_SpawnHerosEnemy)
					m_SpawnHerosEnemy.ColliderEnable = true;
			}
			else if(!m_SpawnMonsters[_iCurSpawnMonsterIndex].ColliderEnable)
				ReleaseNextWave();
		}

		public void ReleaseNextWave()
		{
			m_SpawnMonsters[_iCurSpawnMonsterIndex].ReleaseChar();
		}
	}
}
