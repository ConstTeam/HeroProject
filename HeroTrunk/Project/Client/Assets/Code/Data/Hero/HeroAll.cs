using System.Collections.Generic;

namespace MS
{
	public class HeroAll
	{
		private static List<int> _lstHero = new List<int>();
		private static Dictionary<int, HeroInfo> _dicHero = new Dictionary<int, HeroInfo>();

		public static List<int> GetHeroList()
		{
			return _lstHero;
		}

		public static void SetHeroInfo(int heroId, HeroInfo heroInfo)
		{
			if(_lstHero.Contains(heroId))
				_dicHero[heroId] = heroInfo;
			else
			{
				_lstHero.Add(heroId);
				_dicHero.Add(heroId, heroInfo);
			}	
		}

		public static HeroInfo GetHeroInfo(int heroId)
		{
			return _dicHero[heroId];
		}
	}
}
