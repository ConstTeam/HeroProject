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
				case BattleEnum.Enum_CharState.Run:
					CheckNearestCharAndExcute();
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
