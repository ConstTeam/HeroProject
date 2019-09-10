using System.Collections;
using UnityEngine;

namespace MS
{
	public class CharAnim : MonoBehaviour
	{
		private CharHandler	_charHandler;
		private Animator	_animator;
		private int			_iAttackIndex;

		public int AttackIndex
		{
			get { return _iAttackIndex; }
			set { _iAttackIndex = value % _charHandler.m_CharData.m_iAtkCount; }
		}

		public void SetCharHandler(CharHandler handler)
		{
			_charHandler = handler;
			_animator = _charHandler.m_Transform.GetComponent<Animator>();
		}

		public void SetApplyRootMotion(bool bFlag)
		{
			_animator.applyRootMotion = bFlag;
		}

		public void ToIdle()
		{
			_charHandler.m_CharData.m_eState = BattleEnum.Enum_CharState.Idle;
			_animator.SetInteger("State", 0);
			_animator.SetInteger("SubState", 1);
		}

		public void ToRun()
		{
			_animator.SetInteger("State", 2);
			_animator.SetInteger("SubState", 0);
		}

		public void ToAttack()
		{
			_charHandler.m_CharData.m_eState = BattleEnum.Enum_CharState.Attack;
			_animator.SetInteger("State", 3);
			_animator.SetInteger("SubState", AttackIndex);
		}

		public void ToDead()
		{
			_charHandler.m_CharData.m_eState = BattleEnum.Enum_CharState.Dead;
			_animator.SetInteger("State", 5);
			_animator.SetInteger("SubState", 0);
		}

		public void ToSkill(int skillIndex)
		{
			_charHandler.m_CharData.m_eState = BattleEnum.Enum_CharState.DoSkill;
			StartCoroutine(_ToSkill(skillIndex));
		}

		IEnumerator _ToSkill(int skillIndex)
		{
			yield return new WaitForEndOfFrame();
			if(_charHandler.m_CharData.m_eState != BattleEnum.Enum_CharState.Dead)
			{
				_animator.SetInteger("State", 6);
				_animator.SetInteger("SubState", skillIndex);
			}
		}

		public void ToDizzy()
		{
			_charHandler.m_CharData.m_eState = BattleEnum.Enum_CharState.Dizzy;
			_animator.SetInteger("State", 13);
			_animator.SetInteger("SubState", 0);
		}

		public void ToBorn()
		{
			_charHandler.m_CharData.m_eState = BattleEnum.Enum_CharState.Born;
			_animator.SetInteger("State", 20);
			_animator.SetInteger("SubState", 0);
		}
	}
}
