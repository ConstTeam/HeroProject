using UnityEngine;

namespace MS
{
	public class BattleScene : MonoBehaviour
	{
		public int Level { get; set; }

		public virtual void ToBattle()		{ }
		public virtual void CreateMine()	{ }
		public virtual void EnableMine()	{ }
		public virtual void CreateEnemy()	{ }
		public virtual void EnableEnemy()	{ }

		public virtual void OnBattleInit()
		{
			BattleCamera.GetInst().SetPos(SpawnHandler.GetInst().Heroes[0].position);
			Invoke("OnBattleStart", 1);
		}

		public virtual void OnBattleStart()
		{
			BattleManager.GetInst().BattleCam.enabled = true;
			BattleSceneTimer.GetInst().BeginTimer();
			SpawnHandler.GetInst().CurSpawnIndex = 0;
			SpawnHandler.GetInst().SetSpawnInfo();
			SpawnHandler.GetInst().ReleaseNextWave();
			//SpawnMgr.GetInst().EnableHerosM();
			//SpawnMgr.GetInst().EnableHerosE();
		}
	}
}
