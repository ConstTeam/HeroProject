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
			_transform = handler.m_ParentTrans;
		}

		void Update()
		{
			switch(_charHandler.m_CharData.m_eState)
			{
				case BattleEnum.Enum_CharState.Idle:
					IdleState();
					break;
				case BattleEnum.Enum_CharState.Run:
					CheckNearestCharAndExcute();
					break;
				default:
					break;
			}
		}

		private void IdleState()
		{
			if(!CheckNearestCharAndExcute())
			{
				if(BattleEnum.Enum_CharType.General == _charHandler.m_CharData.m_eType)
				{
					if(_charHandler == BattleManager.GetInst().m_CharInScene.GetHeroByIndexM(0))
					{
						SpawnNormal spawn = SpawnHandler.GetInst().GetCurSpawn();
						if(null != spawn)
							_charHandler.ToRun(spawn.transform, 1);
					}
					else
					{
						CharHandler targetChar = BattleManager.GetInst().m_CharInScene.GetMainHeroBySide(_charHandler.m_CharData.m_eSide);
						if(1 == _charHandler.m_iIndex || 2 == _charHandler.m_iIndex)
						{
							int sign = _charHandler.m_iIndex * 2 - 3;//取正负
							Vector3 toPos = targetChar.m_ParentTrans.position + targetChar.m_ParentTrans.right * 4 * sign + targetChar.m_ParentTrans.forward * 5f;
							if(Vector3.Distance(toPos, targetChar.m_ParentTrans.position) > 5f)
							{
								if(IsInNavMash(toPos))
									_charHandler.ToRun(toPos, 1f);
								else
									_charHandler.ToRun(targetChar.m_ParentTrans.position, 2f);
							}
						}
					}
				}
			}
		}

		private bool CheckNearestCharAndExcute()
		{
			float dis = 0xffff;
			CharHandler ch = BattleManager.GetInst().m_CharInScene.GetConcentrate(_charHandler.m_CharData.GetOppositeSide()); //检查是否有集火目标
			if(null != ch)
			{
				dis = Vector3.Distance(ch.m_ParentTrans.position, _transform.position);
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
				_charHandler.ToSlide(ch.m_ParentTrans, touchDis);
			else
			{
				if(dis > touchDis)
					_charHandler.ToRun(ch.m_ParentTrans, touchDis);
				else
					_charHandler.ToAttack(ch);
			}
		}

		private bool CheckSlide(float dis, float touchDis)
		{
			if(_charHandler.m_CharData.m_eType == BattleEnum.Enum_CharType.General && _charHandler.m_CharData.m_eAtkType == BattleEnum.Enum_AttackType.Close)
				return dis > touchDis + ApplicationConst.fFightSlideMin && dis < touchDis + ApplicationConst.fFightSlideMax;
			return false;
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
	}
}
