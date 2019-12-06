using UnityEngine;

namespace MS
{
	public class BattleCamera : MonoBehaviour
	{
		public CharHandler	m_CharHandler;

		private Transform	_transform;
		private Transform	_target;
		private Vector3		_offset;

		private static BattleCamera _inst;
		public static BattleCamera GetInst()
		{
			return _inst;
		}

		private void OnDestroy()
		{
			_inst = null;
		}

		void Awake()
		{
			_inst = this;
			_transform = transform;
			_offset = new Vector3(-9.58f, 11.54f, -9.04f);
			enabled = false;
		}

		//private void Update()
		//{
		//	_transform.position = Vector3.Lerp(_transform.position, _target.position + _offset, Time.deltaTime * 5);
		//}

		public void SetTarget(CharHandler charHandler)
		{
			m_CharHandler = charHandler;
			_target = charHandler.m_ParentTrans;
			enabled = true;
		}

		public void SetPos(Vector3 pos)
		{
			_transform.position = pos + _offset;
		}
	}
}
