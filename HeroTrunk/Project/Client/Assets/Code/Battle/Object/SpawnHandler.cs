using UnityEngine;

namespace MS
{
	public class SpawnHandler : MonoBehaviour
	{
		public Transform[]		Heroes;
		public SpawnNormal[]	Spawns;

		public int CurSpawnIndex { get; set; }

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
			Spawns[CurSpawnIndex].ResetInfo(CurSpawnIndex);
		}

		public void ReleaseNextWave()
		{
			if(Spawns[CurSpawnIndex].m_iRemainingWaves > 0)
				Spawns[CurSpawnIndex].ReleaseChar();
			else
			{
				++CurSpawnIndex;
				if(CurSpawnIndex < Spawns.Length)
					SetSpawnInfo();	
			}
		}
	}
}
