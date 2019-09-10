using UnityEngine;

namespace MS
{
	public class BattleSceneBase : MonoBehaviour
	{
		public virtual void OnBattleInit()
		{
			BattleCam.GetInst().SetTarget(BattleManager.GetInst().GetMainHero());
		}

		public virtual void OnBattleStart() { }
	}
}
