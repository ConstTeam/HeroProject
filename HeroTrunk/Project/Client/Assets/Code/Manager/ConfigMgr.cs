using UnityEngine;

namespace MS
{
	public class ConfigMgr
	{
		public static int BattleHeroLevelUpCoin(int index, int level)
		{
			string f = ConfigData.GetValue("HeroLevelUp_Client", index.ToString(), "CoinFormula").Replace("Lv", level.ToString());
			return Mathf.FloorToInt(FormulaManager.Arithmetic(f));
		}
	}
}
