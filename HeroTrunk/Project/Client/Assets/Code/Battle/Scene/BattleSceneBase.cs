using UnityEngine;

namespace MS
{
	public class BattleSceneBase : MonoBehaviour
	{
		public int Level { get; set; }

		public virtual void ToBattle()		{ }
		public virtual void CreateMine()	{ }
		public virtual void EnableMine()	{ }
		public virtual void CreateEnemy()	{ }
		public virtual void EnableEnemy()	{ }

		public virtual BattleEnum.Enum_BattleType BattleType
		{
			get { return BattleEnum.Enum_BattleType.Normal; }
		}

		public virtual void OnBattleInit(){}

		public virtual void OnBattleStart()
		{
			BattleManager.GetInst().BattleCam.enabled = true;
			BattleSceneTimer.GetInst().BeginTimer();
		}
	}
}
