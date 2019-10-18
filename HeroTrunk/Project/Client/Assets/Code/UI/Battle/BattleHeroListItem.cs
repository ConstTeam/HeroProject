using UnityEngine;
using UnityEngine.UI;

namespace MS
{
	public class BattleHeroListItem : MonoBehaviour
	{
		public GameObject HeroBgGo;
		public Button AddHeroBtn;
		public Button UpgradBtn;

		public int Index { get; set; }

		private GameObject _gameObject;

		private void Awake()
		{
			_gameObject = gameObject;
			AddHeroBtn.onClick.AddListener(AddHero);
			HeroBgGo.SetActive(false);
		}

		public void Show(bool bShow)
		{
			_gameObject.SetActive(bShow);
		}

		public void ShowHero()
		{
			AddHeroBtn.gameObject.SetActive(false);
			HeroBgGo.SetActive(true);
		}

		private void AddHero()
		{
			BattleManager.GetInst().AddHero(Index);
			BattleHeroListPanel.GetInst().Refresh();
		}
	}
}
