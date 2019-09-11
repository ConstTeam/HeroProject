using UnityEngine;

namespace MS
{
	public class BulletTrace : BulletBase
	{
		private void Update()
		{
			Vector3 bulletPos = _transform.position;
			float moveDis = m_fSpeed * Time.deltaTime;

			if(null != m_AimCharHandler)
			{
				Vector3 aimPos = m_AimCharHandler.m_ParentTrans.position + Vector3.up;
				_fCurDistance = Vector3.Distance(bulletPos, aimPos);
				if(_fCurDistance < 0.2f)
				{
					HitTarget();
					Hide();
				}
				else
				{
					moveDis = Mathf.Min(moveDis, _fCurDistance);
					_transform.LookAt(aimPos);
					_transform.position += _transform.forward * moveDis;
				}
			}
			else
			{
				if(Mathf.Abs(bulletPos.x) > 100 || Mathf.Abs(bulletPos.z) > 100)
					Hide();
				else
					_transform.position += _transform.forward * moveDis;
			}
		}
	}
}
