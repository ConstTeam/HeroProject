using UnityEngine;

namespace MS
{
	public class SpawnHandler : MonoBehaviour
	{
		public Transform[]		Heroes;
		public SpawnNormal[]	Spawns;

		private int _iCurSpawnIndex;
		public int CurSpawnIndex
		{
			get { return _iCurSpawnIndex; }
			set { _iCurSpawnIndex = value; BattleManager.GetInst().m_BattleScene.BigLevel = value; }
		}

		private int _iCurWave;
		public int CurWave
		{
			get { return _iCurWave; }
			set { _iCurWave = value; BattleManager.GetInst().m_BattleScene.SmallLevel = value; }
		}

		public int CurSpawnId { get; set; }

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

		public CharHandler CreateHeroM(int heroId, int heroIndex)
		{
			return BattleCharCreator.CreateHero(BattleEnum.Enum_CharSide.Mine, heroId, heroIndex, Heroes[0].position, Heroes[0].rotation);
		}

		public void SetSpawnInfo(int curWave)
		{
			ConfigRow row = ConfigData.GetValue("SceneNormal_Client", CurSpawnIndex.ToString());
			if(row == null)
				CurSpawnId = -1;
			else
			{
				CurSpawnId = int.Parse(row.GetValue("SpawnId"));
				Spawns[CurSpawnId].ResetInfo(row, curWave);
			}	
		}

		public void ReleaseNextWave()
		{
			if(Spawns[CurSpawnId].CurWave < Spawns[CurSpawnId].TotalWaves)
				Spawns[CurSpawnId].ReleaseChar();
			else
			{
				++CurSpawnIndex;
				SetSpawnInfo(0);
			}
			
			CurWave = Spawns[CurSpawnId].CurWave;
		}

		public SpawnNormal GetCurSpawn()
		{
			if(CurSpawnId == -1)
				return null;
			return Spawns[CurSpawnId];		
		}
	}
}
