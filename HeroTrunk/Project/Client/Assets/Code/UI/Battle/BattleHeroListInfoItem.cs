using UnityEngine;
using UnityEngine.UI;

namespace MS
{
	public class BattleHeroListInfoItem : MonoBehaviour
	{
		public RawImage Icon;
		public GameObject Cover;

		private Toggle _toggle;

		public int HeroId { get; set; }

		private void Awake()
		{
			_toggle = GetComponent<Toggle>();
			Cover.SetActive(false);
		}

		public void Init(int heroId, ToggleGroup group)
		{
			HeroId = heroId;
			_toggle.group = group;
			Icon.texture = ResourceLoader.LoadAsset<Texture>(string.Format("Texture/HeroIcon/Hero{0}", HeroId));
		}

		public bool IsToggleOn()
		{
			return _toggle.isOn && _toggle.interactable;
		}

		public void DisableToggle()
		{
			Cover.SetActive(true);
			_toggle.interactable = false;
		}
	}
}
