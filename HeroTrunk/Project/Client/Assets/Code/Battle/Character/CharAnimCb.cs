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

		public void BornEnd()
		{
			if(_charHandler != null)
				_charHandler.BornEnd();
		}

		#region--��Ч------
		private void BirthSound()
		{

		}
		#endregion
	}
}
