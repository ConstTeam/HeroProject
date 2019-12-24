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
				_iNeedCoin = NeedCoin();
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
			BattleHeroListPanel.GetInst().ShowAddPanel(true);
		}

		private int NeedCoin()
		{
			string f = ConfigData.GetValue("HeroLevelUp_Client", _curIndex.ToString(), "CoinFormula").Replace("Lv", "0");
			return Mathf.FloorToInt(FormulaManager.Arithmetic(f));
		}
	}
}
