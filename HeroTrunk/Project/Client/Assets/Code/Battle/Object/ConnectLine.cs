using System.Collections.Generic;
using UnityEngine;

namespace MS
{
	public class ConnectLine : MonoBehaviour
	{
		public LineRenderer LineRenderer;
		public List<CharHandler> m_lstCharHandler = new List<CharHandler>();
		public List<float> m_lstPercent = new List<float>();
		public int m_iSkillInstID;

		[HideInInspector]
		public Transform m_Transform;

		private void Awake()
		{
			m_Transform = transform;
		}

		private void Update()
		{
			int i = 0;
			for(; i < m_lstCharHandler.Count; ++i)
			{
				LineRenderer.SetPosition(i, m_lstCharHandler[i].m_ParentTrans.position + Vector3.up);
			}
			LineRenderer.SetPosition(i, m_lstCharHandler[0].m_ParentTrans.position + Vector3.up);
		}

		public void AddChar(CharHandler charHandler, float value)
		{
			m_lstCharHandler.Add(charHandler);
			m_lstPercent.Add(value);
			LineRenderer.positionCount = m_lstCharHandler.Count + 1;
		}

		public bool RemoveChar(CharHandler charHandler)
		{
			for(int i = 0; i < m_lstCharHandler.Count; ++i)
			{
				if(m_lstCharHandler[i] == charHandler)
				{
					m_lstCharHandler.RemoveAt(i);
					m_lstPercent.RemoveAt(i);
					break;
				}
			}

			if(m_lstCharHandler.Count > 0)
			{
				LineRenderer.positionCount = m_lstCharHandler.Count + 1;
				return false;
			}
			return true;
		}

		public bool HasChar(CharHandler charHandler)
		{
			for(int i = 0; i < m_lstCharHandler.Count; ++i)
			{
				if(m_lstCharHandler[i] == charHandler)
					return true;
			}

			return false;
		}
	}
}
