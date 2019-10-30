using UnityEngine;

namespace MS
{
	public class SpawnHandler : MonoBehaviour
	{
		public Transform[]		Heroes;
		public SpawnNormal[]	Spawns;

		public int CurSpawnIndex	{ get; set; }
		public int CurSpawnId		{ get; set; }

		private static SpawnHandler _inst;
		public static SpawnHandler GetInst()
		{
			return _inst;
		}

		private void Awake()
		{
			_inst = this;
		}

		private void OnDestroy()
		{
			_inst = null;
		}

		public void CreateHeroM(int heroId, int heroIndex)
		{
			BattleCharCreator.CreateHero(BattleEnum.Enum_CharSide.Mine, heroId, heroIndex, Heroes[0].position, Heroes[0].rotation);
		}

		public void SetSpawnInfo()
		{
			ConfigRow row = ConfigData.GetValue("SceneNormal_Client", CurSpawnIndex.ToString());
			if(row == null)
				CurSpawnId = -1;
			else
			{
				CurSpawnId = int.Parse(row.GetValue("SpawnId"));
				Spawns[CurSpawnId].ResetInfo(row);
			}	
		}

		public void ReleaseNextWave()
		{
			if(Spawns[CurSpawnId].m_iRemainingWaves <= 0)
			{
				++CurSpawnIndex;
				SetSpawnInfo();
			}
			else
				Spawns[CurSpawnId].ReleaseChar();
		}

		public SpawnNormal GetCurSpawn()
		{
			if(CurSpawnId == -1)
				return null;
			return Spawns[CurSpawnId];		
		}
	}
}
