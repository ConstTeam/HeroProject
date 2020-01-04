using UnityEngine;

namespace MS
{
	public class SpawnHandler : MonoBehaviour
	{
		public Transform[] Heroes;
		public SpawnNormal[] Spawns;

		private int _iCurSpawnIndex;
		public int CurSpawnIndex
		{
			get { return _iCurSpawnIndex; }
			set { _iCurSpawnIndex = value; BattleMainPanel.GetInst().CurBigLevelText.text = value.ToString(); }
		}

		private int _iCurWave;
		public int CurWave
		{
			get { return _iCurWave; }
			set { _iCurWave = value; BattleMainPanel.GetInst().CurSmallLevelText.text = value.ToString(); }
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

		public void SetSpawnInfo(int curSpawnIndex)
		{
			CurSpawnIndex = curSpawnIndex;
			ConfigRow row = ConfigData.GetValue("NormalScene_Client", curSpawnIndex.ToString());
			if(row == null)
				CurSpawnId = -1;
			else
			{
				CurSpawnId = int.Parse(row.GetValue("SpawnId"));
				Spawns[CurSpawnId].ResetInfo(row);
			}
		}

		public void ReleaseCurWave()
		{
			Spawns[CurSpawnId].ReleaseChar(CurWave);
		}

		public void ReleaseNextWave()
		{
			if(CurWave < Spawns[CurSpawnId].TotalWaves - 1)
				Database.GetInst().NormalBattleSaveCurWave(PlayerInfo.PlayerId, CurWave + 1);
			else
				Database.GetInst().NormalBattleSaveCurSpawn(PlayerInfo.PlayerId, CurSpawnIndex + 1);
		}

		public SpawnNormal GetCurSpawn()
		{
			if(CurSpawnId == -1)
				return null;
			return Spawns[CurSpawnId];		
		}
	}
}
