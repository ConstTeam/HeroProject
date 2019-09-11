using UnityEngine;

namespace MS
{
	public class BattleSceneBase
	{
		public virtual void ToBattle()		{ }
		public virtual void CreateMine()	{ }
		public virtual void EnableMine()	{ }
		public virtual void CreateEnemy()	{ }
		public virtual void EnableEnemy()	{ }
		public virtual void OnBattleStart()	{ }

		public virtual BattleEnum.Enum_BattleType BattleType
		{
			get { return BattleEnum.Enum_BattleType.Normal; }
		}

		public virtual void OnBattleInit()
		{
			BattleCamera.GetInst().SetTarget(BattleManager.GetInst().GetMainHero());
		}

	}
}
