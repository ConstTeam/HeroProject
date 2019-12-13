using System.Collections.Generic;
using UnityEngine;

namespace MS
{
	public class BattleScene : MonoBehaviour
	{
		public static int CurCoin		{ get; set; }
		public static int CurBigLv		{ get; set; }
		public static int CurSmallLv	{ get; set; }
		public static List<int> m_lstHeroID = new List<int>();
		public static List<int> m_lstHeroLv = new List<int>();

		private int _iBigLevel;
		public int BigLevel
		{
			get { return _iBigLevel; }
			set { _iBigLevel = value; BattleMainPanel.GetInst().CurBigLevelText.text = (value + 1).ToString(); Database.GetInst().NormalBattleSaveBigLevel(PlayerInfo.PlayerId, value); }
		}

		private int _iSmallLevel;
		public int SmallLevel
		{
			get { return _iSmallLevel; }
			set { _iSmallLevel = value; BattleMainPanel.GetInst().CurSmallLevelText.text = (value + 1).ToString(); Database.GetInst().NormalBattleSaveSmallLevel(PlayerInfo.PlayerId, value); }
		}

		private int _iCoin;
		public int Coin
		{
			get { return _iCoin; }
			set { _iCoin = value; BattleMainPanel.GetInst().CoinText.text = value.ToString(); if(_iCoin == 500f && PlayerInfo.GuideStep == 1) GuidePanel.GetInst().ShowPanel(); }
		}

		public virtual void OnBattleInit()
		{
			//BattleCamera.GetInst().SetPos(SpawnHandler.GetInst().Heroes[0].position);
			Invoke("OnBattleStart", 1);
		}

		public virtual void OnBattleStart()
		{
			BattleManager.GetInst().BattleCam.enabled = true;
			BattleSceneTimer.GetInst().BeginTimer();
			BattleManager.GetInst().m_BattleScene.Coin = CurCoin;
			SpawnHandler.GetInst().CurSpawnIndex = CurBigLv;
			SpawnHandler.GetInst().SetSpawnInfo(CurSmallLv);
			SpawnHandler.GetInst().ReleaseNextWave();

			for(int i = 0; i < m_lstHeroID.Count; ++i)
				BattleManager.GetInst().AddHero(i);
		}
	}
}
