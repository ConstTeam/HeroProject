using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MS
{
	public class BattleHeroListInfoItem : MonoBehaviour
	{
		public RawImage Icon;

		public void Init(int heroId, ToggleGroup group)
		{
			GetComponent<Toggle>().group = group;
			Icon.texture = ResourceLoader.LoadAsset<Texture>(string.Format("Texture/HeroIcon/Hero{0}", heroId));
		}
	}
}
