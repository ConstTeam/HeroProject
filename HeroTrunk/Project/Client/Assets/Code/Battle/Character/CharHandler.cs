using UnityEngine;

namespace MS
{
	public class CharHandler : MonoBehaviour
	{
		public GameObject	m_Go;
		public Transform	m_Transform;
		public int			m_iIndex;

		public CharData		m_CharData;
		public CharState	m_CharState;
		public CharMove		m_CharMove;
		public CharAnim		m_CharAnim;
		public CharSkill	m_CharSkill;
		public CharDefence	m_CharDefence;
		//public CharTick		m_CharTick;
		//public CharEffect	m_CharEffect;
		//public CharBody		m_CharBody;
		//public CharSlide	m_CharSlide;

		public void Init(string charId, BattleEnum.Enum_CharSide side, int index = -1)
		{
			m_Go			= gameObject;
			m_Transform		= transform.parent;
			m_iIndex		= index;
			m_CharData		= m_Go.AddComponent<CharData>();
			m_CharMove		= m_Go.AddComponent<CharMove>();
			m_CharAnim		= m_Go.AddComponent<CharAnim>();
			m_CharSkill		= m_Go.AddComponent<CharSkill>();
			m_CharDefence	= new CharDefence(this);
		}

		public void ToBorn()
		{
			m_CharSkill.RunCD(true);
			m_CharMove.SetAgentEnable(true);
			ToIdle();
		}

		public void ToIdle()
		{
			if(this == BattleManager.GetInst().GetMainHero())
				m_CharMove.SetObstacleAvoidanceType(UnityEngine.AI.ObstacleAvoidanceType.NoObstacleAvoidance);

			m_CharMove.StopAutoMove();
			m_CharAnim.ToIdle();
		}

		private Vector3 _rotY = new Vector3(0, 1, 0);
		public void ToDead()
		{
			m_CharSkill.RunCD(false);
			m_CharTick.CancelAll();
			m_CharMove.StopAutoMove();
			m_CharAnim.ToDead();
			m_CharMove.SetAgentEnable(false);
			m_CharEffect.HideSkillEffect(m_CharData.m_iCurSkillID);

			Vector3 dir = Quaternion.AngleAxis(180f, _rotY) * m_Transform.forward;
			m_CharSlide.SetValues(dir, 20);
			m_CharSlide.Slide = true;
			Invoke("StopSlide", 0.1f);

			FightSceneMgr.GetInst().m_CharInScene.RemoveChar(this);

			if(Enum_CharSide.Enemy == m_CharData.m_eSide)
				FightSceneMgr.GetInst().CheckFightWin();
			else
				FightSceneMgr.GetInst().CheckFightLose();
		}

		//-------------------------------------------------------
		public void BeHit(float hurt, CharHandler srcHandler, SkillDataSon srcSkillDataSon = null, bool bDirect = true) //bDirect�Ƿ���ֱ���˺����Ƿ���������������֮�ࣩ
		{
			m_CharDefence.BeHit(hurt, srcHandler, srcSkillDataSon, bDirect);
		}
	}
}
