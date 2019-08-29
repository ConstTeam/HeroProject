using UnityEngine;

namespace MS
{
	public class CharHandler : MonoBehaviour
	{
		public Transform	m_Transform;
		public int			m_iIndex;

		private void Awake()
		{
			m_Transform = transform.parent;
		}

		public void Init(int charId, BattleEnum.Enum_CharSide side, int index = -1)
		{
			m_iIndex = index;
		}
	}
}
