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

		public virtual void CreateCharacters() { }
		public virtual void EnableCharacters() { }
		public virtual void ShowCharacters(bool bShow) { }
		public virtual CharHandler CreateChar(BattleEnum.Enum_CharSide side, int charId, int charIndex) { return null; }
		protected virtual void SetObstacleAvoidance(CharHandler handler) { }
		protected virtual void SetRingLight(CharHandler charHandler) { }
		protected virtual void SetApplyRootMotion(CharHandler charHandler) { }

		protected virtual void SetRadius(CharHandler handler)
		{
			handler.m_CharMove.SetRadius(handler.m_CharData.m_fBodyRange / handler.m_ParentTrans.localScale.z);
		}

		public void ReleaseChar()
		{
			CreateCharacters();
			if(BattleSceneTimer.GetInst().m_iSec >= 0)
				EnableCharacters();
		}

		public void ResetPosition(CharHandler h, int spawnId)
		{
			if(spawnId >= spawnPoints.Length)
				spawnId = 0;
			Transform trans = spawnPoints[spawnId];
			h.m_ParentTrans.position = trans.position;
			h.m_ParentTrans.rotation = trans.rotation;
		}
	}
}
