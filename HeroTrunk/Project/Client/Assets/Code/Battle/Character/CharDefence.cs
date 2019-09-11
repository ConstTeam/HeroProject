using System.Collections.Generic;
using UnityEngine;

namespace MS
{
	public class CharDefence
	{
		public float m_fBackwardClock;

		private CharHandler _charHandler;
		private MPData _MPData;
		private float _iLoseHpTemp;
		private float _iLoseHpSingleTemp;


		public CharDefence(CharHandler handler)
		{
			_charHandler = handler;

			_MPData = BattleManager.GetInst().GetMPData();
			_iLoseHpTemp = 0;
			_iLoseHpSingleTemp = 0;
		}

		public void BeHit(float hurt, CharHandler srcHandler, SkillDataSon srcSkillDataSon, bool bDirect)
		{
			if(CheckCanBeHit())
			{
				hurt = Mathf.Max(1f, hurt);

				if(_charHandler.m_CharData.Rebound.Value > 0 && bDirect)
				{
					_BeHit(hurt, srcHandler, srcSkillDataSon, bDirect);
					Rebound(hurt, srcHandler);
				}
				else if(_charHandler.m_CharData.Absorb.Value > 0)
					Absorb(hurt, srcHandler);
				else
				{
					_BeHit(hurt, srcHandler, srcSkillDataSon, bDirect);
					if(srcHandler.m_CharData.AbsorbHP.Value > 0)
						AbsorbHP(hurt, srcHandler);
				}
			}
		}

		private void _BeHit(float hurt, CharHandler srcHandler, SkillDataSon srcSkillDataSon, bool bDirect)
		{
			_charHandler.m_CharData.CurHP -= hurt;

			if(bDirect)
			{
				CharHandler c;
				List<ConnectLine> connectList = BattleManager.GetInst().m_TriggerManager.GetConnectList(_charHandler.m_CharData.m_eSide);
				for(int i = 0; i < connectList.Count; ++i)
				{
					if(connectList[i].HasChar(_charHandler))
					{
						for(int j = 0; j < connectList[i].m_lstCharHandler.Count; ++j)
						{
							c = connectList[i].m_lstCharHandler[j];
							if(c == _charHandler)
								continue;

							c.BeHit(hurt * connectList[i].m_lstPercent[j], srcHandler, srcSkillDataSon, false);
						}
					}
				}
			}

			HPLosted(hurt, srcHandler, srcSkillDataSon);
			if(srcSkillDataSon == null)
				HUDTextMgr.GetInst().NewText(Mathf.FloorToInt(0 - hurt).ToString(), _charHandler, HUDTextMgr.HUDTextType.NormalHit);
			else
				HUDTextMgr.GetInst().NewText(Mathf.FloorToInt(0 - hurt).ToString(), _charHandler, HUDTextMgr.HUDTextType.SkillHit);
		}

		private bool CheckCanBeHit()
		{
			return !(BattleEnum.Enum_CharState.Dead == _charHandler.m_CharData.m_eState || _charHandler.m_CharData.UnBeatable.Value || BattleEnum.Enum_CharType.Official == _charHandler.m_CharData.m_eType);
		}

		//反弹伤害
		private void Rebound(float hurt, CharHandler srcHandler)
		{
			float rebound = _charHandler.m_CharData.Rebound.Value;
			hurt *= rebound;
			srcHandler.BeHit(hurt, _charHandler, null, false);
			HUDTextMgr.GetInst().NewText(string.Format(ConfigData.GetHUDText("17"), hurt), _charHandler, HUDTextMgr.HUDTextType.BUFF);    //"反弹伤害 {0:N0}"
		}

		//吸收伤害
		private void Absorb(float hurt, CharHandler srcHandler)
		{
			float absorb = _charHandler.m_CharData.Absorb.Value;
			if(absorb < hurt)
			{
				float tmp = hurt - absorb;
				HUDTextMgr.GetInst().NewText(string.Format(ConfigData.GetHUDText("18"), absorb), _charHandler, HUDTextMgr.HUDTextType.BUFF);  //"吸收伤害 {0:N0}"
				HUDTextMgr.GetInst().NewText((0 - tmp).ToString(), _charHandler, HUDTextMgr.HUDTextType.BUFF);

				_charHandler.m_CharTick.CancelAbsorb();
				_charHandler.BeHit(tmp, srcHandler);
			}
			else if(absorb == hurt)
			{
				HUDTextMgr.GetInst().NewText(string.Format(ConfigData.GetHUDText("18"), hurt), _charHandler, HUDTextMgr.HUDTextType.BUFF);    //"吸收伤害 {0:N0}"
				_charHandler.m_CharTick.CancelAbsorb();
			}
			else
				HUDTextMgr.GetInst().NewText(string.Format(ConfigData.GetHUDText("18"), hurt), _charHandler, HUDTextMgr.HUDTextType.BUFF);    //"吸收伤害 {0:N0}"

			_charHandler.m_CharData.Absorb.Value -= hurt;
		}

		//吸血
		private void AbsorbHP(float hurt, CharHandler srcHandler)
		{
			float addHp = Mathf.Max(1f, hurt * srcHandler.m_CharData.AbsorbHP.Value);
			srcHandler.m_CharData.CurHP += addHp;
			HUDTextMgr.GetInst().NewText(string.Format("{0:N0}", addHp), srcHandler, HUDTextMgr.HUDTextType.AbsorbHP);
		}

		private void HPLosted(float hurt, CharHandler srcHandler, SkillDataSon srcSkillDataSon = null)
		{
			//受击特效
			SetEffect(srcSkillDataSon, _charHandler.m_ParentTrans.position);

			if(_charHandler.m_CharData.CurHP <= 0)
			{
				_charHandler.ToDead();
				if(BattleEnum.Enum_CharType.Monster == _charHandler.m_CharData.m_eType)
					BattleManager.GetInst().m_CharInScene.AddMPAll(_MPData.m_fAddMPFromMonsterDead);
				else
					BattleManager.GetInst().m_CharInScene.AddMPAll(_MPData.m_fAddMPFromHeroDead);
			}

			if(BattleEnum.Enum_CharType.Monster == _charHandler.m_CharData.m_eType)
			{
				//击退
				if(null == srcSkillDataSon || srcSkillDataSon.m_Parent.m_bHitBackward)
				{
					float t = Time.time;
					if(t - m_fBackwardClock > ApplicationConst.fBackwardCD)
					{
						Vector3 dir = (_charHandler.m_ParentTrans.position - srcHandler.m_ParentTrans.position).normalized * 1f;
						_charHandler.m_ParentTrans.Translate(dir, Space.World);
						m_fBackwardClock = t;
					}
				}
			}
			else
			{
				//全局加魔
				_iLoseHpTemp += hurt;
				if(_iLoseHpTemp >= _charHandler.m_CharData.MaxHP * _MPData.m_fHPLosePercent)
				{
					int mult = Mathf.FloorToInt(_iLoseHpTemp / (_charHandler.m_CharData.MaxHP * _MPData.m_fHPLosePercent));
					_iLoseHpTemp -= _charHandler.m_CharData.MaxHP * _MPData.m_fHPLosePercent * mult;
					BattleManager.GetInst().m_CharInScene.AddMPAll(_MPData.m_fAddMPFromHPLose * mult);
				}

				//单人加魔
				_iLoseHpSingleTemp += hurt;
				if(_iLoseHpSingleTemp >= _charHandler.m_CharData.MaxHP * _MPData.m_fHPLosePercentSingle)
				{
					int mult = Mathf.FloorToInt(_iLoseHpSingleTemp / (_charHandler.m_CharData.MaxHP * _MPData.m_fHPLosePercentSingle));
					_iLoseHpSingleTemp -= _charHandler.m_CharData.MaxHP * _MPData.m_fHPLosePercentSingle * mult;
					_charHandler.m_CharData.CurMP += _MPData.m_fAddMPFromHPLoseSingle * mult;
				}

				//损血Trigger
				BattleManager.GetInst().m_TriggerManager.HPChanged(Mathf.CeilToInt(_charHandler.m_CharData.CurHP * 100 / _charHandler.m_CharData.MaxHP), _charHandler);
			}
		}

		private void SetEffect(SkillDataSon skillDataSon, Vector3 pos)
		{
			if(null == skillDataSon)
				return;

			string recepEffectPath = skillDataSon.m_ReceptorEffect[0];
			if(!recepEffectPath.Equals(string.Empty))
			{
				Transform effectTrans = BattleScenePool.GetInst().PopEffect(recepEffectPath);
				effectTrans.position = pos;
				BattleScenePool.GetInst().PushEffect(recepEffectPath, effectTrans, 1);
			}
		}
	}
}
