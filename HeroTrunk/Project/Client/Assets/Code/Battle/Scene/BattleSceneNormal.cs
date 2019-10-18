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
			Invoke("OnBattleStart", 1);
		}

		public override void OnBattleStart()
		{
			base.OnBattleStart();
			SpawnMgr.GetInst().SetSpawnMonster();
			SpawnMgr.GetInst().ReleaseNextWave();
			//SpawnMgr.GetInst().EnableHerosM();
			//SpawnMgr.GetInst().EnableHerosE();
		}
	}
}
