using System.Collections.Generic;
using UnityEngine;

namespace MS
{
	public class CharHandler : MonoBehaviour
	{
		public GameObject	m_Go;
		public Transform	m_ParentTrans;
		public int			m_iIndex;

		public CharData		m_CharData;
		public CharState	m_CharState;
		public CharMove		m_CharMove;
		public CharAnim		m_CharAnim;
		public CharSkill	m_CharSkill;
		public CharDefence	m_CharDefence;
		public CharTick		m_CharTick;
		public CharEffect	m_CharEffect;
		public CharBody		m_CharBody;
		public CharSlide	m_CharSlide;

		public GameObject	UIObj;		//血条容器
		public GameObject	UIFollowGo; //血条跟随

		private CharAnimCb	_animCb;

		public void Init(int charId, BattleEnum.Enum_CharSide side, int index = -1)
		{
			m_Go			= gameObject;
			m_ParentTrans	= transform.parent;
			m_iIndex		= index;
			_animCb			= m_ParentTrans.GetComponent<CharAnimCb>();
			m_CharBody		= m_ParentTrans.GetComponent<CharBody>();
			m_CharData		= m_Go.AddComponent<CharData>();
			m_CharState		= m_Go.AddComponent<CharState>();
			m_CharMove		= m_Go.AddComponent<CharMove>();
			m_CharAnim		= m_Go.AddComponent<CharAnim>();
			m_CharTick		= m_Go.AddComponent<CharTick>();
			m_CharEffect	= m_Go.AddComponent<CharEffect>();
			m_CharSlide		= m_Go.AddComponent<CharSlide>();
			m_CharSkill		= m_Go.AddComponent<CharSkill>();
			m_CharDefence	= new CharDefence(this);

			m_CharData.m_iCharID = charId;
			m_CharData.m_eSide = side;
			_animCb.SetCharHandler(this);
			m_CharData.Init(index);
			m_CharState.SetCharHandler(this);
			m_CharMove.SetCharHandler(this);
			m_CharAnim.SetCharHandler(this);
			m_CharTick.SetCharHandler(this);
			m_CharEffect.SetCharHandler(this);
			m_CharSlide.SetCharHandler(this);
			m_CharSkill.SetCharHandler(this);
			m_CharState.enabled = false;

			SetHeadUI();
		}

		public void ToBorn()
		{
			m_CharSkill.RunCD(true);
			m_CharMove.SetAgentEnable(true);
			ToIdle();
		}

		public void ToBornEx()
		{
			m_CharAnim.ToBorn();
		}

		public void ToIdle()
		{
			if(this == BattleManager.GetInst().GetMainHero())
				m_CharMove.SetObstacleAvoidanceType(UnityEngine.AI.ObstacleAvoidanceType.NoObstacleAvoidance);

			m_CharMove.StopAutoMove();
			m_CharAnim.ToIdle();
		}

		public void ToRun(Transform target, float offset = 0.5f)
		{
			ToRun(target.position, offset);
		}

		public void ToRun(Vector3 pos, float offset = 0.5f)
		{
			if((pos - m_ParentTrans.position).magnitude < offset)
				return;

			m_CharData.m_eState = BattleEnum.Enum_CharState.Run;
			m_CharMove.StartAutoMove(pos, offset);
			m_CharAnim.ToRun();
		}

		public void ToSlide(Transform target = null, float offset = 0.5f)
		{
			m_CharData.m_eState = BattleEnum.Enum_CharState.Slide;
			m_CharMove.StartSlide(target, offset);
		}

		public void StopSlide()
		{
			m_CharSlide.Slide = false;
		}

		//眩晕
		public void ToDizzy()
		{
			m_CharMove.StopAutoMove();
			m_CharAnim.ToDizzy();
			m_CharEffect.HideSkillEffect(m_CharData.m_iCurSkillID);
			m_CharEffect.BeShowDizzyEffect(true);
		}

		public void DizzyEnd()
		{
			m_CharEffect.BeShowDizzyEffect(false);
			if(!IsInexistence())
			{
				ToIdle();
				OfficialHide();
			}
		}

		public void ToAttack(CharHandler charHandler = null)
		{
			if(BattleEnum.Enum_CharState.Attack != m_CharData.m_eState)
			{
				if(null != charHandler)
				{
					m_ParentTrans.LookAt(charHandler.m_ParentTrans);
					if(charHandler.m_CharData.m_eType == BattleEnum.Enum_CharType.General && charHandler.m_CharData.m_eSide == BattleEnum.Enum_CharSide.Enemy)
						m_CharMove.SetObstacleAvoidanceType(UnityEngine.AI.ObstacleAvoidanceType.LowQualityObstacleAvoidance);
				}

				if(BattleEnum.Enum_CharType.Monster == m_CharData.m_eType || !m_CharSkill.DequeueHoldSkill())
					m_CharAnim.ToAttack();
			}
		}

		private Vector3 _rotY = new Vector3(0, 1, 0);
		public void ToDead()
		{
			m_CharSkill.RunCD(false);
			m_CharTick.CancelAll();
			m_CharMove.StopAutoMove();
			m_CharAnim.ToDead();
			m_CharMove.SetAgentEnable(false);
			m_CharEffect.HideSkillEffect(m_CharData.m_iCurSkillID);

			Vector3 dir = Quaternion.AngleAxis(180f, _rotY) * m_ParentTrans.forward;
			m_CharSlide.SetValues(dir, 20);
			m_CharSlide.Slide = true;
			Invoke("StopSlide", 0.1f);

			BattleManager.GetInst().m_CharInScene.RemoveChar(this);
		}

		public void BornEnd()
		{
			m_CharState.enabled = true;
			m_CharMove.SetAgentEnable(true);
			ToIdle();
		}

		public void ReleaseSkill(int skillId)
		{
			if(!CheckCanDo(skillId, false))
				return;

			SkillDataWhole skillDataWhole = SkillHandler.GetInst().GetSkillDataByID(skillId);
			_ToSkill(skillDataWhole);
		}

		public bool ToSkill(int skillId, bool bManual = false)
		{
			if(!CheckCanDo(skillId, bManual))
				return false;

			SkillDataWhole skillDataWhole = SkillHandler.GetInst().GetSkillDataByID(skillId);
			if(!CheckMP(skillDataWhole, bManual))
				return false;

			_ToSkill(skillDataWhole);

			return true;
		}

		public void ToSkillWithoutCheck(int skillId)
		{
			SkillDataWhole skillDataWhole = SkillHandler.GetInst().GetSkillDataByID(skillId);
			if(CheckMP(skillDataWhole, false))
				_ToSkill(skillDataWhole);
		}

		private void _ToSkill(SkillDataWhole skillDataWhole)
		{
			if(skillDataWhole.m_eSkillKind == SkillEnum.SkillKind.Soul && m_CharData.SoulSkillInvalid.Value)
				HUDTextMgr.GetInst().NewText(ConfigData.GetHUDText("16"), this, HUDTextMgr.HUDTextType.BUFF);  //"技能失效"

			m_CharData.m_iCurSkillID = skillDataWhole.m_iSkillID;
			OfficialShow();

			int actionId = skillDataWhole.m_iActionID;
			if(-1 == actionId)
			{
				ExcuteSkill(0);
				Invoke("OfficialHide", 1f);
			}
			else
			{
				m_CharAnim.ToSkill(actionId);
				m_CharMove.StopAutoMove();
			}

			if(skillDataWhole.m_eSkillKind == SkillEnum.SkillKind.Soul)
				HUDTextMgr.GetInst().NewText(skillDataWhole.m_sDisplayName, this, HUDTextMgr.HUDTextType.MainSkillName);
			else
				HUDTextMgr.GetInst().NewText(skillDataWhole.m_sDisplayName, this, HUDTextMgr.HUDTextType.NormalSkillName);
		}

		private bool CheckMP(SkillDataWhole skillDataWhole, bool bManual)
		{
			if(ApplicationConst.bFreeSkill && bManual)
				return true;

			if(SkillEnum.SkillKind.Soul == skillDataWhole.m_eSkillKind)
			{
				if(m_CharData.CurMP >= m_CharData.MaxMP)
				{
					m_CharData.CurMP -= skillDataWhole.m_iNeedMP;
					return true;
				}
				else
					return false;
			}

			return true;
		}

		private void OfficialShow()
		{
			if(m_CharData.m_eType == BattleEnum.Enum_CharType.Official)
			{
				CharHandler handler = BattleManager.GetInst().GetMainHeroBySide(m_CharData.m_eSide);
				if(3 == m_iIndex)
					m_ParentTrans.position = handler.m_ParentTrans.position + handler.m_ParentTrans.right * 2 - handler.m_ParentTrans.forward * 2;
				else if(4 == m_iIndex)
					m_ParentTrans.position = handler.m_ParentTrans.position - handler.m_ParentTrans.right * 2 - handler.m_ParentTrans.forward * 2;

				m_ParentTrans.rotation = handler.m_ParentTrans.rotation;

				List<CharHandler> lst = m_CharData.m_eSide == BattleEnum.Enum_CharSide.Mine ? BattleManager.GetInst().m_CharInScene.m_listOfficialPresenceMine : BattleManager.GetInst().m_CharInScene.m_listOfficialPresenceEnemy;
				lst.Add(this);
			}
		}

		private void OfficialHide()
		{
			if(m_CharData.m_eType == BattleEnum.Enum_CharType.Official)
			{
				m_ParentTrans.position = PositionMgr.vecHidePos;
				List<CharHandler> lst = m_CharData.m_eSide == BattleEnum.Enum_CharSide.Mine ? BattleManager.GetInst().m_CharInScene.m_listOfficialPresenceMine : BattleManager.GetInst().m_CharInScene.m_listOfficialPresenceEnemy;
				lst.Remove(this);
				m_CharSkill.DequeueHoldSkill();
			}
		}

		private bool CheckCanDo(int skillId, bool bManual)
		{
			switch(m_CharData.m_eState)
			{
				case BattleEnum.Enum_CharState.Dead:
				case BattleEnum.Enum_CharState.Dizzy:
					return false;
				case BattleEnum.Enum_CharState.DoSkill:
					if(!bManual && SkillHandler.GetInst().GetSkillDataByID(skillId).m_eSkillKind == SkillEnum.SkillKind.Magic)
						m_CharSkill.EnqueueMagicSkill(skillId);
					return false;
				default:
					if(m_CharData.SkillForbid.Value)
						return false;
					if(null == BattleManager.GetInst().GetMainHeroBySide(m_CharData.m_eSide))
						return false;
					return true;
			}
		}

		//-------------------------------------------------------
		public void BeHit(float hurt, CharHandler srcHandler, SkillDataSon srcSkillDataSon = null, bool bDirect = true) //bDirect是否是直接伤害（非反弹、连锁、传递之类）
		{
			m_CharDefence.BeHit(hurt, srcHandler, srcSkillDataSon, bDirect);
		}

		public void ExcuteSkill(int index)
		{
			m_CharSkill.DoSkill(index);
		}

		public bool IsMainHero()
		{
			return this == BattleManager.GetInst().GetMainHeroBySide(m_CharData.m_eSide);
		}

		public bool IsInexistence()
		{
			return BattleEnum.Enum_CharState.Dead == m_CharData.m_eState || BattleEnum.Enum_CharState.Born == m_CharData.m_eState;
		}

		#region 添加头顶血条跟随位置----------------------------------------
		private void SetHeadUI()
		{
			UIObj = new GameObject("HeadUI");
			UIObj.transform.parent = BattleManager.GetInst().HeadUIParentTran;
			UIObj.transform.localScale = Vector3.one;
			UIObj.transform.localRotation = Quaternion.identity;

			bl_Follow3DObject UIFollow = UIObj.AddComponent<bl_Follow3DObject>();
			UIFollow.target = SetUIFollowTarget();

			if(m_iIndex < 0)  //小怪生成头顶血条
			{
				GameObject obj = ResourceLoader.LoadAssetAndInstantiate("PrefabUI/Battle/HUDMonster", UIObj.transform);
				HUDMonster item = obj.GetComponent<HUDMonster>();
				item.GetHandler(this);
			}
		}

		private Transform SetUIFollowTarget()
		{
			UIFollowGo = new GameObject("UIFollow");
			Transform objTran = UIFollowGo.transform;
			float y = m_CharBody.Head.position.y + 1f;
			objTran.position = new Vector3(m_ParentTrans.position.x, y, m_ParentTrans.position.z);
			objTran.parent = m_ParentTrans;
			objTran.localScale = Vector3.one;
			objTran.localRotation = Quaternion.identity;
			return objTran;
		}
		#endregion
	}
}
