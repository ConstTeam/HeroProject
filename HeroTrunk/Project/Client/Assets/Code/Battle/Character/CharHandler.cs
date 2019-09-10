using UnityEngine;

namespace MS
{
	public class CharHandler : MonoBehaviour
	{
		public GameObject	m_Go;
		public Transform	m_Transform;
		public int			m_iIndex;

		public CharData		m_CharData;
		public CharMove		m_CharMove;
		public CharAnim		m_CharAnim;

		public void Init(string charId, BattleEnum.Enum_CharSide side, int index = -1)
		{
			m_Go		= gameObject;
			m_Transform	= transform.parent;
			m_iIndex	= index;
			m_CharData	= m_Go.AddComponent<CharData>();
			m_CharMove	= m_Go.AddComponent<CharMove>();
			m_CharAnim	= m_Go.AddComponent<CharAnim>();
		}

		public void ToIdle()
		{
			if(this == BattleManager.GetInst().GetMainHero())
				m_CharMove.SetObstacleAvoidanceType(UnityEngine.AI.ObstacleAvoidanceType.NoObstacleAvoidance);

			m_CharMove.StopAutoMove();
			m_CharAnim.ToIdle();
		}
	}
}
