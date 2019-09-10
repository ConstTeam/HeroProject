using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MS
{
	public class SpawnMgr : MonoBehaviour
	{
		public enum SpawnType
		{
			Normal,
			PVP
		}

		public SpawnType m_eSpawnType;
		public SpawnHeroBase m_SpawnHerosMine;
		public SpawnHeroBase m_SpawnHerosEnemy;
		public SpawnMonster[] m_SpawnMonsters;

		private int _iCurSpawnMonsterIndex;
	}
}
