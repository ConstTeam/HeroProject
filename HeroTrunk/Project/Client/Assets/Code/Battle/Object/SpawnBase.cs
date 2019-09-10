using UnityEngine;

namespace MS
{
	public class SpawnBase : MonoBehaviour
	{
		public delegate void ReleaseCallback(int index);

		public enum SpawnShape
		{
			None,
			Rectangle,
			Circle
		}

		public BattleEnum.Enum_CharSide m_CharSide;
		public SpawnShape m_Shape;
		public float m_fSizeX;
		public float m_fSizeY;
		public float m_fRadius;
		public Transform[] spawnPoints;

		private Collider m_Collider;

		private void Awake()
		{
			if(SpawnShape.Rectangle == m_Shape)
			{
				BoxCollider collider = gameObject.AddComponent<BoxCollider>();
				collider.size = new Vector3(m_fSizeX, 1f, m_fSizeY);
				collider.isTrigger = true;
				m_Collider = collider;
				ColliderEnable = false;
			}
			else if(SpawnShape.Circle == m_Shape)
			{
				CapsuleCollider collider = gameObject.AddComponent<CapsuleCollider>();
				collider.radius = m_fRadius;
				collider.height = 1f;
				collider.isTrigger = true;
				m_Collider = collider;
				ColliderEnable = false;
			}
		}

		public virtual void CreateCharacters() { }
		public virtual void EnableCharacters() { }
		public virtual void ShowCharacters(bool bShow) { }
		protected virtual CharHandler CreateChar(int spawnId, int charId) { return null; }
		protected virtual void SetObstacleAvoidance(CharHandler handler) { }
		protected virtual void SetRingLight(CharHandler charHandler) { }
		protected virtual void SetApplyRootMotion(CharHandler charHandler) { }

		protected virtual void SetRadius(CharHandler handler)
		{
			handler.m_CharMove.SetRadius(handler.m_CharData.m_fBodyRange / handler.m_Transform.localScale.z);
		}

		private void OnTriggerEnter(Collider other)
		{
			ReleaseChar();
		}

		public void ReleaseChar()
		{
			CreateCharacters();
			if(BattleManager.GetInst().m_SceneTimer.m_iSec >= 0)
				EnableCharacters();
		}

		public void ResetPosition(CharHandler h, int spawnId)
		{
			Transform trans = spawnPoints[spawnId];
			h.m_Transform.position = trans.position;
			h.m_Transform.rotation = trans.rotation;
		}

		public bool ColliderEnable
		{
			get
			{
				if(null == m_Collider)
					return false;

				return m_Collider.enabled;
			}
			set
			{
				if(null == m_Collider)
					return;

				m_Collider.enabled = value;
			}
		}

		#region --编辑器中绘制辅助线----------------------------------------------
		private float _fTheta = 0.1f;
		private void OnDrawGizmos()
		{
			Color defaultColor = Gizmos.color;
			Gizmos.color = Color.red;
			_OnDrawCollider();
			_OnDrawSpawns(spawnPoints);
			Gizmos.color = defaultColor;
		}

		private void _OnDrawCollider()
		{
			if(SpawnShape.Rectangle == m_Shape)
			{
				float r = Mathf.Sqrt(Mathf.Pow(m_fSizeX, 2) + Mathf.Pow(m_fSizeY, 2)) / 2;

				float angle = Mathf.Atan2(m_fSizeY, m_fSizeX);
				float x1 = r * Mathf.Cos(transform.eulerAngles.y * Mathf.Deg2Rad - angle);
				float y1 = r * Mathf.Sin(transform.eulerAngles.y * Mathf.Deg2Rad - angle);
				float x2 = r * Mathf.Cos(transform.eulerAngles.y * Mathf.Deg2Rad + angle);
				float y2 = r * Mathf.Sin(transform.eulerAngles.y * Mathf.Deg2Rad + angle);
				Gizmos.DrawLine(transform.position + new Vector3(-x1, 0, y1) + Vector3.up * 0.1f, transform.position + new Vector3(-x2, 0, y2) + Vector3.up * 0.1f);
				Gizmos.DrawLine(transform.position + new Vector3(-x2, 0, y2) + Vector3.up * 0.1f, transform.position + new Vector3(x1, 0, -y1) + Vector3.up * 0.1f);
				Gizmos.DrawLine(transform.position + new Vector3(x1, 0, -y1) + Vector3.up * 0.1f, transform.position + new Vector3(x2, 0, -y2) + Vector3.up * 0.1f);
				Gizmos.DrawLine(transform.position + new Vector3(x2, 0, -y2) + Vector3.up * 0.1f, transform.position + new Vector3(-x1, 0, y1) + Vector3.up * 0.1f);
			}
			else if(SpawnShape.Circle == m_Shape)
			{
				// 绘制圆环
				Vector3 beginPoint = Vector3.zero;
				Vector3 firstPoint = Vector3.zero;
				for(float theta = 0; theta < 2 * Mathf.PI; theta += _fTheta)
				{
					float x = m_fRadius * Mathf.Cos(theta) + transform.position.x;
					float z = m_fRadius * Mathf.Sin(theta) + transform.position.z;
					Vector3 endPoint = new Vector3(x, transform.position.y + 0.1f, z);

					if(theta == 0)
						firstPoint = endPoint;
					else
						Gizmos.DrawLine(beginPoint, endPoint);

					beginPoint = endPoint;
				}
				Gizmos.DrawLine(firstPoint, beginPoint);
			}
		}

		private float _fRadius = 0.5f;
		private void _OnDrawSpawns(Transform[] trans)
		{
			if(null == trans)
				return;

			for(int i = 0; i < trans.Length; ++i)
			{
				Transform _trans = trans[i];

				// 绘制圆环
				Vector3 beginPoint = Vector3.zero;
				Vector3 firstPoint = Vector3.zero;
				for(float theta = 0; theta < 2 * Mathf.PI; theta += _fTheta)
				{
					float x = _fRadius * Mathf.Cos(theta) + _trans.position.x;
					float z = _fRadius * Mathf.Sin(theta) + _trans.position.z;
					Vector3 endPoint = new Vector3(x, _trans.position.y + 0.1f, z);

					if(theta == 0)
						firstPoint = endPoint;
					else
						Gizmos.DrawLine(beginPoint, endPoint);

					beginPoint = endPoint;
				}

				// 绘制最后一条线段
				Gizmos.DrawLine(firstPoint, beginPoint);

				Gizmos.DrawLine(_trans.position + Vector3.up * 0.1f, _trans.position + Vector3.up * 0.1f + _trans.forward * _fRadius);
			}
		}
		#endregion
	}
}
