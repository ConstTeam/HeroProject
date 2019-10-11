using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MS
{
	public class HeroAll
	{
		private List<int> _lstHero = new List<int>();
		private Dictionary<int, HeroInfo> _dicHero = new Dictionary<int, HeroInfo>();

		public void SetHeroInfo(int heroId, HeroInfo heroInfo)
		{
			if(_lstHero.Contains(heroId))
				_dicHero[heroId] = heroInfo;
			else
			{
				_lstHero.Add(heroId);
				_dicHero.Add(heroId, heroInfo);
			}	
		}
	}
}
