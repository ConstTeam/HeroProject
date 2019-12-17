using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MS
{
	public class BattleHeroListItem : MonoBehaviour
	{
		public Button UpgradBtn;
		public RawImage Icon;
		public TextMeshProUGUI CoinText;
		public TextMeshProUGUI LevelText;

		public int HeroIndex	{ get; set; }
		public int HeroId		{ get; set; }

		private int _iHeroLevel;
		public int HeroLevel
		{
			get { return _iHeroLevel; }
			set { _iHeroLevel = value; LevelText.text = value.ToString(); }
		}

		private int _iNeedCoin;
		public int NeedCoin
		{
			get { return _iNeedCoin; }
			set { _iNeedCoin = value; CoinText.text = value.ToString(); }
		}

		private CharHandler _charHandler;
		private GameObject _gameObject;

		private void Awake()
		{
			_gameObject = gameObject;
			UpgradBtn.onClick.AddListener(Upgrade);
			_gameObject.SetActive(false);
		}

		public void ShowHero(int heroId)
		{
			if(_gameObject.activeSelf)
				return;

			_gameObject.SetActive(true);
			HeroId = heroId;
			Icon.texture = ResourceLoader.LoadAsset<Texture>(string.Format("Texture/HeroIcon/Hero{0}", heroId));
			_charHandler = BattleManager.GetInst().m_CharInScene.GetHeroByIndexM(HeroIndex);
			SetLevel(1);
		}

		public void SetBtnState()
		{
			UpgradBtn.interactable = BattleManager.GetInst().m_BattleScene.Coin >= NeedCoin;
		}

		private void Upgrade()
		{
			BattleManager.GetInst().m_BattleScene.Coin -= NeedCoin;
			SetLevel(HeroLevel + 1);
		}

		private void SetLevel(int level)
		{
			HeroLevel = level;
			_charHandler.m_CharData.CurAttack = _charHandler.m_CharData.OriAtk * HeroLevel;
			_charHandler.m_CharData.CurDefence = _charHandler.m_CharData.OriDef * HeroLevel;
			_charHandler.m_CharData.MaxHP = _charHandler.m_CharData.OriHP * HeroLevel;

			string f = ConfigData.GetValue("HeroLevelUp_Client", HeroIndex.ToString(), "CoinFormula").Replace("Lv", level.ToString());
			NeedCoin = Mathf.FloorToInt(FormulaManager.Arithmetic(f));
			SetBtnState();
		}
	}
}
