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

		public int HeroIndex	{ get; set; }
		public int HeroId		{ get; set; }

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

			AddHeroBtn.gameObject.SetActive(false);
			HeroBgGo.SetActive(true);
			Icon.texture = ResourceLoader.LoadAsset<Texture>(string.Format("Texture/HeroIcon/Hero{0}", heroId));
		}

		private void AddHero()
		{
			HeroId = BattleManager.GetInst().AddHero(HeroIndex);
			BattleHeroListPanel.GetInst().Refresh();
		}

		private void Upgrade()
		{
			CharHandler h = BattleManager.GetInst().m_CharInScene.GetHeroByIndexM(HeroIndex);
			h.m_CharData.CurAttack *= 1.1f;
			h.m_CharData.CurDefence *= 1.1f;
			h.m_CharData.MaxHP *= 1.1f;
		}
	}
}
