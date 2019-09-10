using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MS
{
	public class SpawnMonster : SpawnBase
	{
		public int m_iIndex;
		public int m_iMonsterID;
		public int m_iWaves;

		public override void CreateCharacters()
		{
			--m_iWaves;
			ColliderEnable = false;
			StartCoroutine(_CreateCharacters());
		}

		private IEnumerator _CreateCharacters()
		{
			for(int i = 0; i < spawnPoints.Length; ++i)
			{
				CreateChar(i, m_iMonsterID);
				yield return new WaitForSeconds(0.2f);
			}
		}

		protected override CharHandler CreateChar(int spawnId, int charId)
		{
			CharHandler h = BattleScenePool.GetInst().PopMonsterHandler(charId);
			SetMonsterData(h);
			ResetPosition(h, spawnId);
			SetObstacleAvoidance(h);
			SetRadius(h);
			h.ToBornEx();
			BattleManager.GetInst().m_CharInScene.AddChar(h);
			return h;
		}

		private void SetMonsterData(CharHandler charHandler)
		{
			CharData charData = charHandler.m_CharData;
			charData.m_eSide = BattleEnum.Enum_CharSide.Enemy;
			charData.m_eType = BattleEnum.Enum_CharType.Monster;

			ConfigRow monsterInfo = ConfigData.GetValue("Monster_Client", charData.m_iCharID.ToString());
			charData.m_fAtkX = float.Parse(monsterInfo.GetValue("AttackX"));
			charData.m_fAtkRange = float.Parse(monsterInfo.GetValue("AtkRange"));
			charData.m_fBodyRange = float.Parse(monsterInfo.GetValue("BodyRange"));
			charData.m_fMoveSpeed = float.Parse(monsterInfo.GetValue("MoveSpeed"));
			charData.CurAttack = float.Parse(monsterInfo.GetValue("Attack"));
			charData.m_fOriAtk = charData.CurAttack;
			charData.CurDefence = float.Parse(monsterInfo.GetValue("Defence"));
			charData.m_fOriDef = charData.CurDefence;
			charData.CriticalRatio = float.Parse(monsterInfo.GetValue("CriticalRatio"));
			charData.BlockRatio = float.Parse(monsterInfo.GetValue("BlockRatio"));
			charData.MaxHP = float.Parse(monsterInfo.GetValue("Hp"));
			charData.CurHP = charData.MaxHP;
			charData.m_fOriHP = charData.MaxHP;
			charData.CurLevel = BattleManager.GetInst().m_iEnemyPlayerLevel;

			charHandler.m_CharDefence.m_fBackwardClock = 0;
		}

		protected override void SetObstacleAvoidance(CharHandler handler)
		{
			handler.m_CharMove.SetObstacleAvoidanceType(UnityEngine.AI.ObstacleAvoidanceType.LowQualityObstacleAvoidance);
		}
	}
}
