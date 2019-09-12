namespace MS
{
	public class BattleSceneNormal : BattleSceneBase
	{
		public override BattleEnum.Enum_BattleType BattleType
		{
			get { return BattleEnum.Enum_BattleType.Normal; }
		}

		public override void OnBattleInit()
		{
			SpawnMgr.GetInst().CreateHerosM();
			base.OnBattleInit();
		}

		public override void OnBattleStart()
		{
			SpawnMgr.GetInst().EnableHerosM();
			//SpawnMgr.GetInst().EnableHerosE();
		}
	}
}
