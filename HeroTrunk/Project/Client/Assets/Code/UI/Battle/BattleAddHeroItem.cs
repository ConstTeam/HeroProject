using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MS
{
	public class BattleAddHeroItem : MonoBehaviour
	{
		public Button AddHeroBtn;
		public Text Description;
		public TextMeshProUGUI NeedCoinText;

		private int _curIndex;
		private int _iNeedCoin;

		private void Awake()
		{
			AddHeroBtn.onClick.AddListener(AddHero);
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

		private void AddHero()
		{
			BattleManager.GetInst().m_BattleScene.Coin -= _iNeedCoin;
			BattleManager.GetInst().AddHero(_curIndex);
		}

		private int NeedCoin()
		{
			string f = ConfigData.GetValue("HeroLevelUp_Client", _curIndex.ToString(), "CoinFormula").Replace("Lv", "0");
			return Mathf.FloorToInt(FormulaManager.Arithmetic(f));
		}
	}
}
