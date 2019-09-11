using UnityEngine;

namespace MS
{
	public class CharAnimCb : MonoBehaviour
	{
		private CharHandler _charHandler;

		public void SetCharHandler(CharHandler handler)
		{
			_charHandler = handler;
		}

		#region--普攻回调---------
		//----（用于生成普通攻击的特效）---
		public void NormalHitBegin()
		{
			if(_charHandler != null)
				_charHandler.NormalHitBegin();
		}

		//---普通攻击(使攻击目标受到伤害)---
		public void NormalHit()
		{
			if(_charHandler != null)
				_charHandler.NormalHit();
		}

		public void SmiteHit()
		{
			if(_charHandler != null)
				_charHandler.NormalHit();
		}

		public void NormalHitMid()
		{
			if(_charHandler != null)
				_charHandler.NormalHitMid();
		}

		public void NormalHitEnd()
		{
			if(_charHandler != null)
				_charHandler.NormalHitEnd();
		}
		#endregion

		//----被击结束--（回到待机动作）---
		public void BeHitEnded()
		{

		}

		public void BornEnd()
		{
			if(_charHandler != null)
				_charHandler.BornEnd();
		}

		public void DeadEnded()
		{
			if(_charHandler != null)
				_charHandler.DeadEnd();
		}

		public void Shoot()
		{
			if(_charHandler != null)
				_charHandler.ShootTrace();
		}

		public void RangeShoot()
		{

		}

		public void SkillMove()
		{

		}

		#region--技能回调--------
		private int _iExcuteIndex;
		public void SkillBegin()
		{
			_iExcuteIndex = 0;
			if(_charHandler != null)
				_charHandler.SkillBegin();
		}

		public void SkillExcute()
		{
			if(_charHandler != null)
				_charHandler.ExcuteSkill(_iExcuteIndex++);
		}

		public void SkillEnded()
		{
			if(_charHandler != null)
				_charHandler.SkillEnded();
		}
		#endregion

		#region--音效------------
		public void AttackSound(int num)
		{

		}

		public void SkillSound(int num)
		{

		}

		public void DeadSound()
		{

		}

		public void BirthSound()
		{

		}

		public void BeHitSound()
		{

		}
		#endregion

		public void ShakeCamera(int i)
		{

		}
	}
}
