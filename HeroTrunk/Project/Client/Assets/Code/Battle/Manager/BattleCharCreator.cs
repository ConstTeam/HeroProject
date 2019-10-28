using UnityEngine;

namespace MS
{
	public class BattleCharCreator
	{
		private static void SetRadius(CharHandler handler)
		{
			handler.m_CharMove.SetRadius(handler.m_CharData.m_fBodyRange / handler.m_ParentTrans.localScale.z);
		}

		private static void ResetPosition(CharHandler h, Vector3 pos, Quaternion rot)
		{
			h.m_ParentTrans.position = pos;
			h.m_ParentTrans.rotation = rot;
		}

		#region --Hero-------------------------------------------
		public static void CreateHero(BattleEnum.Enum_CharSide side, int charId, int charIndex, Vector3 pos, Quaternion rot)
		{
			CharHandler h = BattleScenePool.GetInst().LoadHero(side, charId, charIndex);
			Rigidbody rb = h.m_Go.AddComponent<Rigidbody>();
			BoxCollider cl = h.m_Go.AddComponent<BoxCollider>();
			rb.useGravity = false;
			cl.isTrigger = true;
			h.m_CharData.SetCharType(BattleEnum.Enum_CharType.General);
			SetObstacleAvoidanceHero(h);
			SetRadius(h);
			h.m_CharSkill.InitTriggerSkill();
			//SetApplyRootMotion(h);
			SetRingLightHero(h);
			ResetPosition(h, pos, rot);
			BattleManager.GetInst().m_CharInScene.AddChar(h);
		}

		private static void SetObstacleAvoidanceHero(CharHandler handler)
		{
			if(handler.m_iIndex.Equals(0))
			{
				handler.m_CharMove.SetObstacleAvoidanceType(UnityEngine.AI.ObstacleAvoidanceType.NoObstacleAvoidance);
				handler.m_CharMove.SetObstacleAvoidancePriority(39);
			}
			else
			{
				handler.m_CharMove.SetObstacleAvoidanceType(UnityEngine.AI.ObstacleAvoidanceType.LowQualityObstacleAvoidance);
				handler.m_CharMove.SetObstacleAvoidancePriority(38);
			}
		}

		private static void SetRingLightHero(CharHandler charHandler)
		{
			string ring = BattleEnum.Enum_CharSide.Mine == charHandler.m_CharData.m_eSide ? BattleManager.GetInst().GetMainHero() == charHandler ? "Effect/PassiveSkill/RingLight_Mine" : "Effect/PassiveSkill/RingLight_MineAI" : "Effect/PassiveSkill/RingLight_Boss";
			charHandler.m_CharEffect.SetRingLight(ring);
		}
		#endregion

		#region --Boss-------------------------------------------
		public static CharHandler CreateBoss(BattleEnum.Enum_CharSide side, int charId, int charIndex, Vector3 pos, Quaternion rot)
		{
			CharHandler h = BattleScenePool.GetInst().LoadHero(side, charId, charIndex);
			Rigidbody rb = h.m_Go.AddComponent<Rigidbody>();
			BoxCollider cl = h.m_Go.AddComponent<BoxCollider>();
			rb.useGravity = false;
			cl.isTrigger = true;
			h.m_CharData.SetCharType(BattleEnum.Enum_CharType.General);
			SetObstacleAvoidanceBoss(h);
			SetRadius(h);
			h.m_CharSkill.InitTriggerSkill();
			SetApplyRootMotionBoss(h);
			SetRingLightBoss(h);
			ResetPosition(h, pos, rot);
			BattleManager.GetInst().m_CharInScene.AddChar(h);
			return h;
		}

		private static void SetApplyRootMotionBoss(CharHandler charHandler)
		{
			charHandler.m_CharAnim.SetApplyRootMotion(false);
		}

		private static void SetRingLightBoss(CharHandler charHandler)
		{
			charHandler.m_CharEffect.SetRingLight("Effect/PassiveSkill/RingLight_Boss");
		}

		private static void SetObstacleAvoidanceBoss(CharHandler handler)
		{
			handler.m_CharMove.SetObstacleAvoidanceType(UnityEngine.AI.ObstacleAvoidanceType.LowQualityObstacleAvoidance);
			handler.m_CharMove.SetObstacleAvoidancePriority(37);
		}
		#endregion

		#region --Monster----------------------------------------
		public static CharHandler CreateMonster(BattleEnum.Enum_CharSide side, int charId, int charIndex, Vector3 pos, Quaternion rot)
		{
			CharHandler h = BattleScenePool.GetInst().PopMonsterHandler(charId);
			SetMonsterData(h);
			SetObstacleAvoidanceMonster(h);
			SetRadius(h);
			h.ToBornEx();
			ResetPosition(h, pos, rot);
			BattleManager.GetInst().m_CharInScene.AddChar(h);
			return h;
		}

		private static void SetMonsterData(CharHandler charHandler)
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
			charData.CurStar = BattleManager.GetInst().m_iEnemyPlayerLevel;

			charHandler.m_CharDefence.m_fBackwardClock = 0;
		}

		private static void SetObstacleAvoidanceMonster(CharHandler handler)
		{
			handler.m_CharMove.SetObstacleAvoidanceType(UnityEngine.AI.ObstacleAvoidanceType.LowQualityObstacleAvoidance);
		}
		#endregion
	}
}
