using UnityEngine;

namespace MS
{
	public class CharSlide : MonoBehaviour
	{
		private Transform _transform;
		private float _speed = 0f;
		private Vector3 _dir = Vector3.zero;

		private bool _bSlide = false;
		public bool Slide
		{
			get { return _bSlide; }
			set { _bSlide = value; }
		}

		public void SetCharHandler(CharHandler handler)
		{
			_transform = handler.m_ParentTrans;
		}

		public void SetValues(Vector3 dir, float speed)
		{
			_dir = dir;
			_speed = speed;
		}

		private void Update()
		{
			if(Slide)
				Excute(_dir);
		}

		private void Excute(Vector3 dir)
		{
			_transform.Translate(dir * Time.deltaTime * _speed, Space.World);
		}
	}
}
