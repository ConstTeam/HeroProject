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
			//BattleCamera.GetInst().SetTarget(BattleManager.GetInst().GetMainHero())
			BattleCamera.GetInst().SetPos(SpawnMgr.GetInst().m_SpawnHerosMine.GetSpawnTrans().position);
		}

		public override void OnBattleStart()
		{
			SpawnMgr.GetInst().ReleaseNextWave();
			//SpawnMgr.GetInst().EnableHerosM();
			//SpawnMgr.GetInst().EnableHerosE();
		}
	}
}
