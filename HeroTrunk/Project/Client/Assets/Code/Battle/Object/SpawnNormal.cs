using System.Collections.Generic;
using UnityEngine;

namespace MS
{
	public class SpawnNormal : MonoBehaviour
	{
		public enum SpawnShape
		{
			None,
			Rectangle,
			Circle
		}

		public SpawnShape m_Shape;
		public float m_fSizeX;
		public float m_fSizeY;
		public float m_fRadius;
		public Transform[] MonsterSpawn;
		public Transform[] BossSpawn;

		private Collider m_Collider;

		public int TotalWaves	{ get; set; }

		public int m_iIndex;
		
		public List<string[]> m_lstCharID = new List<string[]>();
		public List<string[]> m_lstCharCount = new List<string[]>();
		

		public void ShowCharacters(bool bShow) { }

		public void ResetInfo(ConfigRow row)
		{
			m_lstCharID.Clear();
			m_lstCharCount.Clear();
			string[] ids = row.GetValue("CharID").Split('|');
			string[] count = row.GetValue("CharCount").Split('|');
			for(int i = 0; i < ids.Length; ++i)
			{
				string[] v1 = ids[i].Split(',');
				m_lstCharID.Add(v1);
				string[] v2 = count[i].Split(',');
				m_lstCharCount.Add(v2);
			}
			TotalWaves = ids.Length;
		}

		public void ReleaseChar(int wave)
		{
			CreateCharacters(wave);
		}

		public void CreateCharacters(int wave)
		{
			string[] ids = m_lstCharID[wave];
			string[] counts = m_lstCharCount[wave];
			int count = 0;
			int indexMonster = 0, indexBoss = 0;
			for(int i = 0; i < ids.Length; ++i)
			{
				count = int.Parse(counts[i]);
				if(count > 0)
				{
					for(int j = 0; j < count; ++j)
					{
						BattleCharCreator.CreateMonster(BattleEnum.Enum_CharSide.Enemy, int.Parse(ids[i]), i, MonsterSpawn[indexMonster].position, MonsterSpawn[indexMonster].rotation, j);
						++indexMonster;
					}
				}
				else
				{
					for(int j = 0; j < -count; ++j)
					{
						BattleCharCreator.CreateBoss(BattleEnum.Enum_CharSide.Enemy, int.Parse(ids[i]), i, BossSpawn[indexBoss].position, BossSpawn[indexBoss].rotation);
						++indexBoss;
					}
				}
			}
		}

		#region --编辑器中绘制辅助线----------------------------------------------
		private float _fTheta = 0.1f;
		private void OnDrawGizmos()
		{
			Color defaultColor = Gizmos.color;
			Gizmos.color = Color.red;
			_OnDrawCollider();
			_OnDrawSpawns(MonsterSpawn);
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
