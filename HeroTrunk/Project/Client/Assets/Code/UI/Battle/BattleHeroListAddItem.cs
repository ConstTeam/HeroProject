using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MS
{
	public class BattleHeroListAddItem : MonoBehaviour
	{
		public Button AddHeroBtn;
		public Text Description;
		public TextMeshProUGUI NeedCoinText;

		private int _curIndex;
		private int _iNeedCoin;

		private void Awake()
		{
			AddHeroBtn.onClick.AddListener(OpenAddPanel);
			Description.text = ConfigData.GetStaticText("20001");
		}

		public void SetState(int index)
		{
			if(index < 5)
			{
				_curIndex = index;
				_iNeedCoin = ConfigMgr.BattleHeroLevelUpCoin(_curIndex, 0);
				NeedCoinText.text = _iNeedCoin.ToString();
				SetBtnState();
			}
			else
				gameObject.SetActive(false);
		}

		public void SetBtnState()
		{
			AddHeroBtn.interactable = BattleManager.GetInst().m_BattleScene.Coin >= _iNeedCoin;
		}

		private void OpenAddPanel()
		{
			BattleHeroListPanel.GetInst().ShowAddPanel(true, _curIndex, _iNeedCoin);
		}
	}
}
