using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MS
{
	public class CharSkill : MonoBehaviour
	{
		public Vector3 m_vecDistancePos;

		private CharHandler _charHandler;
		private int _iDeadTriggerSelf;                      //武将阵亡触发己方
		private List<int> _lstSecTrigger = new List<int>(); //战斗时长触发
		private List<int> _lstFinalTrigger = new List<int>();   //最后怪出现时长触发
		private List<int> _lstHpTriggerSelf = new List<int>();  //血量百分比触发己方
		private List<int> _lstHpTriggerOppo = new List<int>();  //血量百分比触发对方
		private Queue<int> _queMagicSkill = new Queue<int>();

		private int _iSelfDieSkillID;
		public int SelfDieSkillID
		{
			set { _iSelfDieSkillID = value; }
			get { return _iSelfDieSkillID; }
		}

		private int _iMainTriggerSkillID;
		public int MainTriggerSkillID
		{
			set { _iMainTriggerSkillID = value; }
			get { return _iMainTriggerSkillID; }
		}

		public void SetCharHandler(CharHandler handler)
		{
			_charHandler = handler;
			SelfDieSkillID = 0;
			MainTriggerSkillID = 0;
		}

		public void NormalHit()
		{
			SkillHandler.GetInst().NormalHit(_charHandler);
		}

		public void NormalShoot()
		{
			SkillHandler.GetInst().NormalShoot(_charHandler);
		}

		public void DoSkill(int index)
		{
			SkillHandler.GetInst().NormalSkill(_charHandler, index);
		}

		private void ClearTriggerDic()
		{
			_lstSecTrigger.Clear();
			_lstFinalTrigger.Clear();
			_lstHpTriggerSelf.Clear();
			_lstHpTriggerOppo.Clear();
		}

		public void InitTriggerSkill()
		{
			ClearTriggerDic();
			SkillDataWhole skillDataWhole;
			int[] skillIDs = _charHandler.m_CharData.SkillIDs;
			for(int i = 1; i < skillIDs.Length; ++i)
			{
				if(0 == skillIDs[i])
					break;

				skillDataWhole = SkillHandler.GetInst().GetSkillDataByID(skillIDs[i]);
				switch(skillDataWhole.m_eTriggerType)
				{
					case SkillEnum.TriggerType.BattleStart:
						_lstSecTrigger.Add(skillIDs[i]);
						BattleManager.GetInst().AddBattleSecTrigger(skillDataWhole.m_iTriggerParam, _charHandler);
						break;
					case SkillEnum.TriggerType.AimHeroBorn:
						_lstFinalTrigger.Add(skillIDs[i]);
						BattleManager.GetInst().AddFinalEnemyTrigger(skillDataWhole.m_iTriggerParam, _charHandler);
						break;
					case SkillEnum.TriggerType.SelfCurHP:
						_lstHpTriggerSelf.Add(skillIDs[i]);
						BattleManager.GetInst().AddHPTrigger(skillDataWhole.m_iTriggerParam, _charHandler, SkillEnum.TriggerType.SelfCurHP);
						break;
					case SkillEnum.TriggerType.AimCurHP:
						_lstHpTriggerOppo.Add(skillIDs[i]);
						BattleManager.GetInst().AddHPTrigger(skillDataWhole.m_iTriggerParam, _charHandler, SkillEnum.TriggerType.AimCurHP);
						break;
					case SkillEnum.TriggerType.SelfOtherDie:
						_iDeadTriggerSelf = skillIDs[i];
						BattleManager.GetInst().AddDeadTrigger(_charHandler);
						break;
					case SkillEnum.TriggerType.SelfDie:
						SelfDieSkillID = skillIDs[i];
						break;
					case SkillEnum.TriggerType.MainSkill:
						MainTriggerSkillID = skillIDs[i];
						break;
					default:
						break;
				}
			}
		}

		public void TimeTrigger(int sec)
		{
			int skillId;
			for(int i = _lstSecTrigger.Count - 1; i >= 0; --i)
			{
				skillId = _lstSecTrigger[i];
				if(sec == SkillHandler.GetInst().GetSkillDataByID(skillId).m_iTriggerParam)
				{
					_lstSecTrigger.RemoveAt(i);
					_charHandler.ToSkill(skillId);
				}
			}
		}

		public void FinalEnemyTrigger(int sec)
		{
			int skillId;
			for(int i = _lstFinalTrigger.Count - 1; i >= 0; --i)
			{
				skillId = _lstFinalTrigger[i];
				if(sec == SkillHandler.GetInst().GetSkillDataByID(skillId).m_iTriggerParam)
				{
					_lstFinalTrigger.RemoveAt(i);
					_charHandler.ToSkill(skillId);
				}
			}
		}

		public void HPTriggerSelf(int hpPercent, CharHandler charHandler)
		{
			int skillId;
			for(int i = _lstHpTriggerSelf.Count - 1; i >= 0; --i)
			{
				skillId = _lstHpTriggerSelf[i];
				if(hpPercent < SkillHandler.GetInst().GetSkillDataByID(skillId).m_iTriggerParam)
				{
					_lstHpTriggerSelf.RemoveAt(i);
					_charHandler.ToSkill(skillId);
				}
			}
		}

		public void HPTriggerAim(int hpPercent, CharHandler charHandler)
		{
			int skillId;
			for(int i = _lstHpTriggerOppo.Count - 1; i >= 0; --i)
			{
				skillId = _lstHpTriggerOppo[i];
				if(hpPercent < SkillHandler.GetInst().GetSkillDataByID(skillId).m_iTriggerParam)
				{
					_lstHpTriggerOppo.RemoveAt(i);
					_charHandler.ToSkill(skillId);
				}
			}
		}

		public void DeadTrigger(CharHandler charHandler)
		{
			_charHandler.ToSkill(_iDeadTriggerSelf);
		}

		//--CD控制-----------------------------------------------------------------------------------
		private List<int> _lstCD = new List<int>();
		private List<int> _lstCDRemain = new List<int>();
		private List<int> _lstWaitSkill = new List<int>();

		//获取技能CD时间
		public List<int> _LstCD
		{
			get { return _lstCD; }
		}

		public void RunCD(bool bRun)
		{
			EndInvoke(0);
			EndInvoke(1);
			if(bRun)
			{
				SkillDataWhole skillDataWhole;

				_lstCD.Clear();
				_lstCDRemain.Clear();
				_lstWaitSkill.Clear();

				for(int i = 0; i < 2; ++i)
				{
					int skillId = _charHandler.m_CharData.SkillIDs[i + 1];
					if(0 != skillId)
					{
						skillDataWhole = SkillHandler.GetInst().GetSkillDataByID(skillId);
						_lstCD.Add(skillDataWhole.m_iCDTime);
						_lstCDRemain.Add(0);
						_lstWaitSkill.Add(i);
					}
				}
			}

			//if(_charHandler == BattleCam.GetInst().m_CharHandler)    //被动复活刷新UI冷却显示
			//	Fightbar.GetInst().SetCDShow();
		}

		private void EndInvoke(int index)
		{
			if(0 == index)
				CancelInvoke("CDTick0");
			else
				CancelInvoke("CDTick1");
		}

		private void BeginInvoke(int index)
		{
			if(0 == index)
				InvokeRepeating("CDTick0", 0f, 1f);
			else
				InvokeRepeating("CDTick1", 0f, 1f);
		}

		private void CDTick0()
		{
			_CDTick(0);
		}

		private void CDTick1()
		{
			_CDTick(1);
		}

		private void _CDTick(int index)
		{
			if(0 != _lstCDRemain[index])
			{
				if(0 == --_lstCDRemain[index])
				{
					EndInvoke(index);
					_lstWaitSkill.Add(index);
				}
			}

			//if(_charHandler == BattleCam.GetInst().m_CharHandler)
			//	Fightbar.GetInst().SetCDTime(index + 1, _lstCDRemain[index]);
		}

		public bool DequeueHoldSkill()
		{
			//检查辅助武将的魂技
			if(_charHandler.IsMainHero())
			{
				List<CharHandler> lst = BattleManager.GetInst().m_CharInScene.GetOfficial(_charHandler.m_CharData.m_eSide);
				for(int i = 0; i < lst.Count; ++i)
				{
					if(lst[i].m_CharData.CurMP >= lst[i].m_CharData.MaxMP)
						lst[i].ToSkill(lst[i].m_CharData.SkillIDs[0]);
				}
			}

			if(_charHandler.m_CharData.CurMP >= _charHandler.m_CharData.MaxMP)
				return _charHandler.ToSkill(_charHandler.m_CharData.SkillIDs[0]);
			else if(_lstWaitSkill.Count > 0)
				return _DoWaitSkill(0, false);
			else if(_queMagicSkill.Count > 0)
				return _charHandler.ToSkill(_queMagicSkill.Dequeue());

			return false;
		}

		//cdIndex 比 skillIndex 小 1
		public bool DoWaitSkill(int skillIndex)
		{
			int cdIndex = skillIndex - 1;
			for(int i = 0; i < _lstWaitSkill.Count; ++i)
			{
				if(_lstWaitSkill[i] == cdIndex)
				{
					return _DoWaitSkill(i, true);
				}
			}

			return false;
		}

		private bool _DoWaitSkill(int index, bool bManual)
		{
			int cdIndex = _lstWaitSkill[index];
			if(_charHandler.ToSkill(_charHandler.m_CharData.SkillIDs[cdIndex + 1], bManual))
			{
				_lstWaitSkill.RemoveAt(index);
				_lstCDRemain[cdIndex] = _lstCD[cdIndex] + 1; //加1的原因是cd首次就进行-1操作
				BeginInvoke(cdIndex);
				return true;
			}
			return false;
		}

		public void EnqueueMagicSkill(int skillId)
		{
			if(!_queMagicSkill.Contains(skillId))
				_queMagicSkill.Enqueue(skillId);
		}
	}
}
