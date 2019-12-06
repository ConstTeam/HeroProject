using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MS
{
	public class BattleHeroListItem : MonoBehaviour
	{
		public GameObject HeroBgGo;
		public Button AddHeroBtn;
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

		private GameObject _gameObject;

		private void Awake()
		{
			_gameObject = gameObject;
			AddHeroBtn.onClick.AddListener(AddHero);
			UpgradBtn.onClick.AddListener(Upgrade);
			HeroBgGo.SetActive(false);
		}

		public void Show(bool bShow)
		{
			_gameObject.SetActive(bShow);
		}

		public void ShowHero(int heroId)
		{
			if(HeroBgGo.activeSelf)
				return;

			HeroId = heroId;
			AddHeroBtn.gameObject.SetActive(false);
			HeroBgGo.SetActive(true);
			Icon.texture = ResourceLoader.LoadAsset<Texture>(string.Format("Texture/HeroIcon/Hero{0}", heroId));
		}

		private void AddHero()
		{
			BattleManager.GetInst().AddHero(HeroIndex);
		}

		private void Upgrade()
		{
			++HeroLevel;
			CharHandler h = BattleManager.GetInst().m_CharInScene.GetHeroByIndexM(HeroIndex);
			h.m_CharData.CurAttack = h.m_CharData.OriAtk * HeroLevel;
			h.m_CharData.CurDefence = h.m_CharData.OriDef * HeroLevel;
			h.m_CharData.MaxHP = h.m_CharData.OriHP * HeroLevel;
		}
	}
}
