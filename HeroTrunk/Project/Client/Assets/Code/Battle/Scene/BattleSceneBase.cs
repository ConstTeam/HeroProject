using UnityEngine;

namespace MS
{
	public class BattleSceneBase : MonoBehaviour
	{
		public virtual void OnBattleInit()
		{
			BattleCam.GetInst().SetTarget(BattleManager.GetInst().GetMainHero().m_Transform);
		}

		public virtual void OnBattleStart() { }
	}
}
