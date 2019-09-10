using UnityEngine;

namespace MS
{
	public class BulletBase : MonoBehaviour
	{
		public delegate void ShootCallback();

		public CharHandler m_CharHandler;
		public CharHandler m_AimCharHandler;
		public float m_fSpeed = 30.0f;

		protected Transform _transform;
		protected GameObject _gameObject;
		protected float _fCurDistance;
		protected ShootCallback _funcShootCb;

		protected virtual void Hide()
		{
			_gameObject.SetActive(false);
		}

		protected void HitTarget()
		{
			if(null != _funcShootCb)
				_funcShootCb();
		}

		public void Shoot(CharHandler charHandler, CharHandler aimCharHandler = null, ShootCallback cb = null)
		{
			if(null == _transform)
			{
				_transform = transform;
				_gameObject = gameObject;
			}

			m_CharHandler = charHandler;
			m_AimCharHandler = aimCharHandler;
			_funcShootCb = cb;

			_transform.rotation = charHandler.m_Transform.rotation;
			_transform.position = m_CharHandler.m_Transform.position + Vector3.up;
			_gameObject.SetActive(true);
		}
	}
}
