using UnityEngine;

namespace MS
{
	public class BattleCam : MonoBehaviour
	{
		private Transform _transform;
		private Transform _target;
		private Vector3 _offset;

		private static BattleCam _inst;
		public static BattleCam GetInst()
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

		private void Update()
		{
			_transform.position = Vector3.Lerp(_transform.position, _target.position + _offset, Time.deltaTime * 5);
		}

		public void SetTarget(Transform target)
		{
			_target = target;
			enabled = true;
		}

		public void SetPos(Vector3 pos)
		{
			_transform.position = pos + _offset;
		}
	}
}
