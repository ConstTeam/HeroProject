using System.Collections.Generic;
using UnityEngine;

namespace MS
{
	public class BattleTriggerManager
	{
		public struct HpTriggerInfo
		{
			public int m_iPercent;
			public CharHandler m_CharHandler;

			public HpTriggerInfo(int iPercent, CharHandler handler)
			{
				m_iPercent = iPercent;
				m_CharHandler = handler;
			}
		}

		private Dictionary<int, List<CharHandler>>	_dicTrigger1		= new Dictionary<int, List<CharHandler>>();	//战斗时长触发
		private Dictionary<int, List<CharHandler>>	_dicTrigger2		= new Dictionary<int, List<CharHandler>>();	//最后怪出现时长触发
		private List<HpTriggerInfo>					_lstTriggerHPSelf	= new List<HpTriggerInfo>();				//血量百分比触发
		private List<HpTriggerInfo>					_lstTriggerHPAim	= new List<HpTriggerInfo>();				//血量百分比触发
		private List<CharHandler>					_lstTriggerDead		= new List<CharHandler>();					//阵亡触发
		private List<ConnectLine>					_lstConnectMine		= new List<ConnectLine>();					//伤害锁链
		private List<ConnectLine>					_lstConnectEnemy	= new List<ConnectLine>();					//伤害锁链
		private int									_iFinalEnemyBornSec = -1;

		public void AddConnectChar(int skillInstId, CharHandler charHandler, float value)
		{
			List<ConnectLine> lst = BattleEnum.Enum_CharSide.Mine == charHandler.m_CharData.m_eSide ? _lstConnectMine : _lstConnectEnemy;
			for(int i = 0; i < lst.Count; ++i)
			{
				if(lst[i].m_iSkillInstID == skillInstId)
				{
					lst[i].AddChar(charHandler, value);
					return;
				}
			}

			Transform connectTrans = BattleScenePool.GetInst().PopEffect("Effect/PassiveSkill/ConnectLine");
			ConnectLine connectLine = connectTrans.GetComponent<ConnectLine>();
			lst.Add(connectLine);
			connectLine.m_iSkillInstID = skillInstId;
			connectLine.AddChar(charHandler, value);
		}

		public void RemoveConnectChar(int skillInstId, CharHandler charHandler)
		{
			List<ConnectLine> lst = BattleEnum.Enum_CharSide.Mine == charHandler.m_CharData.m_eSide ? _lstConnectMine : _lstConnectEnemy;
			for(int i = 0; i < lst.Count; ++i)
			{
				if(lst[i].m_iSkillInstID == skillInstId)
				{
					bool bEmpty = lst[i].RemoveChar(charHandler);
					if(bEmpty)
					{
						BattleScenePool.GetInst().PushEffect("Effect/PassiveSkill/ConnectLine", lst[i].m_Transform);
						lst.RemoveAt(i);
					}
					return;
				}
			}
		}

		public List<ConnectLine> GetConnectList(BattleEnum.Enum_CharSide side)
		{
			return BattleEnum.Enum_CharSide.Mine == side ? _lstConnectMine : _lstConnectEnemy;
		}

		//--时间Trigger-------------------------------------------------------------------------------------------
		public BattleTriggerManager(BattleSceneTimer fightSceneTimer)
		{
			fightSceneTimer.AddTickFunc(Tick);
		}

		private void Tick(int sec)
		{
			if(_dicTrigger1.ContainsKey(sec))
			{
				for(int i = 0, len = _dicTrigger1[sec].Count; i < len; ++i)
				{
					_dicTrigger1[sec][i].m_CharSkill.TimeTrigger(sec);
				}
			}

			if(_iFinalEnemyBornSec >= 0)
			{
				int t = sec - _iFinalEnemyBornSec;
				if(_dicTrigger2.ContainsKey(t))
				{
					for(int i = 0, len = _dicTrigger2[t].Count; i < len; ++i)
					{
						_dicTrigger2[t][i].m_CharSkill.FinalEnemyTrigger(t);
					}
				}
			}
		}

		public void AddBattleSecTrigger(int sec, CharHandler charHandler)
		{
			if(!_dicTrigger1.ContainsKey(sec))
				_dicTrigger1.Add(sec, new List<CharHandler>());

			if(!_dicTrigger1[sec].Contains(charHandler))
				_dicTrigger1[sec].Add(charHandler);
		}

		public void AddFinalEnemyTrigger(int sec, CharHandler charHandler)
		{
			if(!_dicTrigger2.ContainsKey(sec))
				_dicTrigger2.Add(sec, new List<CharHandler>());

			if(!_dicTrigger2[sec].Contains(charHandler))
				_dicTrigger2[sec].Add(charHandler);
		}

		public void FinalEnemyBorned()
		{
			_iFinalEnemyBornSec = BattleManager.GetInst().m_SceneTimer.m_iSec;
		}

		//--血量Trigger-------------------------------------------------------------------------------------------
		public void HPChanged(int hpPercent, CharHandler charHandler)
		{
			for(int i = _lstTriggerHPSelf.Count - 1; i >= 0; --i)
			{
				if(hpPercent < _lstTriggerHPSelf[i].m_iPercent)
				{
					CharHandler c = _lstTriggerHPSelf[i].m_CharHandler;
					if(c.m_CharData.m_eSide == charHandler.m_CharData.m_eSide)
					{
						c.m_CharSkill.HPTriggerSelf(hpPercent, charHandler);
						_lstTriggerHPSelf.RemoveAt(i);
					}
				}
			}

			for(int i = _lstTriggerHPAim.Count - 1; i >= 0; --i)
			{
				if(hpPercent < _lstTriggerHPAim[i].m_iPercent)
				{
					CharHandler c = _lstTriggerHPAim[i].m_CharHandler;
					if(c.m_CharData.m_eSide != charHandler.m_CharData.m_eSide)
					{
						c.m_CharSkill.HPTriggerAim(hpPercent, charHandler);
						_lstTriggerHPAim.RemoveAt(i);
					}
				}
			}
		}

		public void AddHPTrigger(int hpPercent, CharHandler charHandler, SkillEnum.TriggerType triggerType)
		{
			List<HpTriggerInfo> lst = SkillEnum.TriggerType.SelfCurHP == triggerType ? _lstTriggerHPSelf : _lstTriggerHPAim;
			lst.Add(new HpTriggerInfo(hpPercent, charHandler));
		}

		//--阵亡Trigger（他人阵亡）-------------------------------------------------------------------------------
		public void CharDeadEnd(CharHandler charHandler)
		{
			if(BattleEnum.Enum_CharType.General == charHandler.m_CharData.m_eType)
			{
				for(int i = _lstTriggerDead.Count - 1; i >= 0; --i)
				{
					if(_lstTriggerDead[i].m_CharData.m_eSide == charHandler.m_CharData.m_eSide)
					{
						_lstTriggerDead[i].m_CharSkill.DeadTrigger(charHandler);
						_lstTriggerDead.RemoveAt(i);
					}
				}
			}
		}

		public void AddDeadTrigger(CharHandler charHandler)
		{
			_lstTriggerDead.Add(charHandler);
		}

		public void ClearAllTrigger()
		{
			_dicTrigger1.Clear();
			_dicTrigger2.Clear();
			_lstTriggerHPSelf.Clear();
			_lstTriggerHPAim.Clear();
			_lstTriggerDead.Clear();
		}
	}
}
