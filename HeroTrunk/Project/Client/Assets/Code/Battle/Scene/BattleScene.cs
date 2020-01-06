using UnityEngine;

namespace MS
{
	public class BattleScene : MonoBehaviour
	{
		private int _iCoin;
		public int Coin
		{
			get { return _iCoin; }
			set
			{
				_iCoin = value;
				BattleMainPanel.GetInst().CoinText.text = value.ToString();
				BattleHeroListPanel.GetInst().SyncCoin();
				if(_iCoin >= 500f && PlayerInfo.GuideStep == 1)
					GuidePanel.GetInst().ShowPanel();
			}
		}

		public virtual void OnBattleInit()
		{
			Database.GetInst().SyncBattleInfo(PlayerInfo.PlayerId);
		}

		public virtual void OnBattleStart()
		{
			Invoke("_OnBattleStart", 1);
		}

		private void _OnBattleStart()
		{
			BattleManager.GetInst().BattleCam.enabled = true;
			BattleSceneTimer.GetInst().BeginTimer();
		}
	}
}
