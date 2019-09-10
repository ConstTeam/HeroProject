using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MS
{
	public class SpawnHeroBase : SpawnBase
	{
		public override void CreateCharacters()
		{
			List<int> lst = BattleEnum.Enum_CharSide.Mine == m_CharSide ? BattleScenePool.GetInst().m_lstCharMineID : BattleScenePool.GetInst().m_lstCharEnemyID;
			for(int i = 0; i < lst.Count; ++i)
			{
				if(i < 3)
					CreateChar(i, lst[i]);
				else
					CreateCharOfficial(lst[i]);
			}

			ColliderEnable = false;
		}

		public bool CreateCharacterSingle(int index)
		{
			List<int> lst = BattleEnum.Enum_CharSide.Mine == m_CharSide ? BattleScenePool.GetInst().m_lstCharMineID : BattleScenePool.GetInst().m_lstCharEnemyID;
			if(lst.Count > index)
			{
				CreateChar(0, lst[index]);
				return true;
			}

			return false;
		}

		protected override CharHandler CreateChar(int spawnId, int charId)
		{
			CharHandler h = BattleScenePool.GetInst().GetCharHandler(m_CharSide, charId);
			Rigidbody rb = h.m_Go.AddComponent<Rigidbody>();
			BoxCollider cl = h.m_Go.AddComponent<BoxCollider>();
			rb.useGravity = false;
			cl.isTrigger = true;
			h.m_CharData.SetCharData(BattleEnum.Enum_CharType.General);
			SetObstacleAvoidance(h);
			SetRadius(h);
			h.m_CharSkill.InitTriggerSkill();
			SetApplyRootMotion(h);
			BattleManager.GetInst().m_CharInScene.AddChar(h);
			SetRingLight(h);
			ResetPosition(h, spawnId);

			return h;
		}

		private void CreateCharOfficial(int charId)
		{
			CharHandler h = BattleScenePool.GetInst().GetCharHandler(m_CharSide, charId);
			h.m_CharData.SetCharData(BattleEnum.Enum_CharType.Official);
			SetObstacleAvoidance(h);
			h.m_CharSkill.InitTriggerSkill();
			SetRingLight(h);
			BattleManager.GetInst().m_CharInScene.AddChar(h);
		}

		public override void EnableCharacters()
		{
			bool bMine = BattleEnum.Enum_CharSide.Mine == m_CharSide;
			List<CharHandler> lst = bMine ? BattleManager.GetInst().m_CharInScene.m_listGeneralMine : BattleManager.GetInst().m_CharInScene.m_listGeneralEnemy;
			if(lst.Count <= 0)
				return;

			for(int i = 0; i < lst.Count; ++i)
			{
				lst[i].m_CharState.enabled = true;
				lst[i].m_CharMove.SetAgentEnable(true);
				lst[i].m_CharSkill.RunCD(true);
			}

			if(!bMine)
				BattleManager.GetInst().FinalEnemyBorned(lst);
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
				lst[i].m_Transform.gameObject.SetActive(bShow);
			}
		}
	}
}
