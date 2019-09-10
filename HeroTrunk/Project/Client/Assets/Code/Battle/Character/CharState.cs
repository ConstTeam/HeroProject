using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MS
{
	public class CharState : MonoBehaviour
	{
		private Transform _transform;
		private CharHandler _charHandler;

		public void SetCharHandler(CharHandler handler)
		{
			_charHandler = handler;
			_transform = handler.m_Transform;
		}

		void Update()
		{
			switch(_charHandler.m_CharData.m_eState)
			{
				case BattleEnum.Enum_CharState.Idle:
					IdleState();
					break;
				case BattleEnum.Enum_CharState.Run:
					RunState();
					break;
				default:
					break;
			}
		}

		private bool CheckNearestCharAndExcute()
		{
			float dis = 0xffff;
			CharHandler ch = BattleManager.GetInst().m_CharInScene.GetConcentrate(_charHandler.m_CharData.GetOppositeSide()); //检查是否有集火目标
			if(null != ch)
			{
				dis = Vector3.Distance(ch.m_Transform.position, _transform.position);
				_CheckNearestCharAndExcute(ch, dis);
				return true;
			}

			ch = BattleManager.GetInst().m_CharInScene.GetNearestChar(_charHandler, _charHandler.m_CharData.GetOppositeSide(), ref dis);
			if(null != ch)
			{
				_CheckNearestCharAndExcute(ch, dis);
				return true;
			}

			return false;
		}

		private void _CheckNearestCharAndExcute(CharHandler ch, float dis)
		{
			float touchDis = _charHandler.m_CharData.m_fAtkRange;
			if(CheckSlide(dis, touchDis))
				_charHandler.ToSlide(ch.m_Transform, touchDis);
			else
			{
				if(dis > touchDis)
					_charHandler.ToRun(ch.m_Transform, touchDis);
				else
					_charHandler.ToAttack(ch);
			}
		}

		private bool CheckSlide(float dis, float touchDis)
		{
			switch(FightSceneMgr.m_eBattleType)
			{
				case FightSceneMgr.BattleType.Normal:
				case FightSceneMgr.BattleType.Elite:
					if(FightSceneMgr.GetInst().GetMainHero() == _charHandler && Enum_AttackType.Close == _charHandler.m_CharData.m_eAtkType)
						return dis > touchDis + ApplicationConst.fFightSlideMin && dis < touchDis + ApplicationConst.fFightSlideMax;
					return false;
				default:
					return false;
			}
		}

		//--站立状态----------------------------------------------------------------------
		private void IdleState()
		{
			if(_charHandler.IsAuto)
			{
				if(!CheckNearestCharAndExcute())
				{
					if(Enum_CharType.General == _charHandler.m_CharData.m_eType)
					{
						if(_charHandler == FightSceneMgr.GetInst().GetMainHero())
						{
							Spawn spawn = SpawnMgr.GetInst().GetNextSpawn();
							if(null != spawn)
								_charHandler.ToRun(spawn.spawnPoints[0].transform, 1);
						}
						else
						{
							CharHandler charHandler = FightSceneMgr.GetInst().GetMainHeroBySide(_charHandler.m_CharData.m_eSide);
							if(1 == _charHandler.m_iIndex || 2 == _charHandler.m_iIndex)
							{
								int sign = _charHandler.m_iIndex * 2 - 3;//取正负
								Vector3 toPos = charHandler.m_Transform.position + charHandler.m_Transform.right * 2 * sign + charHandler.m_Transform.forward * 2.5f;
								if(Vector3.Distance(toPos, charHandler.m_Transform.position) > 1f)
								{
									if(IsInNavMash(toPos))
										_charHandler.ToRun(toPos, 1f);
									else
										_charHandler.ToRun(charHandler.m_Transform.position, 2f);
								}
							}
						}
					}
				}
			}
			else
			{
				if(_charHandler.m_CharAnim.ManualIndex > 0)
				{
					if(!CheckNearestCharAndExcute())    //无目标空打
						_charHandler.ToAttack();
				}
			}
		}

		private Ray ray = new Ray();
		private RaycastHit hit;
		private bool IsInNavMash(Vector3 origin)
		{
			ray.origin = origin;
			ray.direction = Vector3.down;
			if(Physics.Raycast(ray, out hit, 5, 1 << 9))
			{
				if(hit.collider.gameObject.CompareTag("Ground"))
					return true;
			}

			return false;
		}

		//--跑动状态----------------------------------------------------------------------
		public void RunState()
		{
			if(_charHandler.IsAuto)
				CheckNearestCharAndExcute();
		}
	}
}
