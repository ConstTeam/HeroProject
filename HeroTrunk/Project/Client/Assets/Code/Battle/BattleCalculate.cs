using System.Collections.Generic;
using UnityEngine;

namespace MS
{
	public class BattleCalculate
	{
		private static Dictionary<string, string> _dicMath = new Dictionary<string, string>();
		private static List<int> levelCoefficient = new List<int>();

		public static void Init()
		{
			_dicMath.Clear();
			levelCoefficient.Clear();
			ConfigTable mathCfg = ConfigData.GetValue("HeroMathSet_Common");

			foreach(KeyValuePair<string, ConfigRow> pair in mathCfg.m_Data)
			{
				_dicMath.Add(pair.Value.GetValue("MathName"), pair.Value.GetValue("Math"));
			}

			string[] arr = _dicMath["NPCLevelParam"].Split(',');
			for(int i = 0; i < arr.Length; i++)
			{
				if(arr[i].Length > 0)
					levelCoefficient.Add(int.Parse(arr[i]));
			}

			ConfigData.DeleteConfig("HeroMathSet_Common");
		}

		public static float ExcuteFormula(string formula, string formulaMax, CharHandler charHandler, CharHandler aimCharHandler)
		{
			if(null == formula || string.Empty == formula)
				return 0.0f;

			string f;
			float hurt = CheckNumber(formula);
			if(-1.0f == hurt)
			{
				f = _ExcuteFormula(formula, charHandler, aimCharHandler);
				hurt = FormulaManager.Arithmetic(f);
			}

			if(null == formulaMax || string.Empty == formulaMax)
				return hurt;

			f = _ExcuteFormula(formulaMax, charHandler, aimCharHandler);
			return Mathf.Min(hurt, FormulaManager.Arithmetic(f));
		}

		//武将普攻公式
		public static float HeroNormalHurt(CharData charData, CharData aimCharData)
		{
			string f = _dicMath["HeroNormalHurt"];
			f = f.Replace("Atk", charData.CurAttack.ToString());
			f = f.Replace("Aim_Def", aimCharData.CurDefence.ToString());
			f = f.Replace("Force", (charData.Force * charData.ForceRatio).ToString());
			f = f.Replace("X", GetLevelCoefficient(charData.m_eSide).ToString());
			float ret = FormulaManager.Arithmetic(f);
			ret *= charData.m_fAtkX;
			return ret;
		}

		public static float MonsterNormalHurt(CharData charData, CharData aimCharData)
		{
			string f = _dicMath["MonsterNormalHurt"];
			f = f.Replace("Atk", charData.CurAttack.ToString());
			f = f.Replace("Aim_Def", aimCharData.CurDefence.ToString());
			float ret = FormulaManager.Arithmetic(f);
			ret *= charData.m_fAtkX;
			return ret;
		}

		//处理公式
		private static string _ExcuteFormula(string sFormula, CharHandler charHandler, CharHandler aimCharHandler)
		{
			if(null != aimCharHandler)
			{
				sFormula = ReplaceBaseProperty(sFormula, true, aimCharHandler.m_CharData);
				sFormula = ReplaceHeroProperty(sFormula, true, aimCharHandler.m_CharData);
			}

			if(null != charHandler)
			{
				sFormula = ReplaceBaseProperty(sFormula, false, charHandler.m_CharData);
				sFormula = ReplaceHeroProperty(sFormula, false, charHandler.m_CharData);
			}

			return sFormula;
		}

		private static float CheckNumber(string formula)
		{
			float ret = -1.0f;
			try
			{
				ret = float.Parse(formula);
			}
			catch
			{
				ret = -1.0f;
			}

			return ret;
		}

		public static int GetLevelCoefficient()
		{
			return levelCoefficient[PlayerInfo.Level - 1];
		}

		private static string ReplaceBaseProperty(string formula, bool bAim, CharData charData)
		{
			if(bAim)
			{
				formula = formula.Replace("Aim_HeroLv", charData.CurLevel.ToString());
				formula = formula.Replace("Aim_MaxHP", charData.MaxHP.ToString());
				formula = formula.Replace("Aim_HP", charData.CurHP.ToString());
				formula = formula.Replace("Aim_DEF", charData.CurDefence.ToString());
				formula = formula.Replace("Aim_ATK", charData.CurAttack.ToString());
			}
			else
			{
				formula = formula.Replace("HeroLv", charData.CurLevel.ToString());
				formula = formula.Replace("MaxHP", charData.MaxHP.ToString());
				formula = formula.Replace("HP", charData.CurHP.ToString());
				formula = formula.Replace("DEF", charData.CurDefence.ToString());
				formula = formula.Replace("ATK", charData.CurAttack.ToString());
			}

			return formula;
		}

		private static string ReplaceHeroProperty(string formula, bool bAim, CharData charData)
		{
			if(bAim)
			{
				formula = formula.Replace("Aim_Force", (charData.Force * charData.ForceRatio).ToString());
				formula = formula.Replace("Aim_Magic", (charData.Magic * charData.MagicRatio).ToString());
				formula = formula.Replace("Aim_Soul", (charData.Soul * charData.SoulRatio).ToString());
				formula = formula.Replace("Aim_MP", Mathf.Max(0, charData.CurMP).ToString());
				formula = formula.Replace("Aim_X", string.Format("{0:F1}", GetLevelCoefficient(charData.m_eSide)));
			}
			else
			{
				formula = formula.Replace("Force", (charData.Force * charData.ForceRatio).ToString());
				formula = formula.Replace("Magic", (charData.Magic * charData.MagicRatio).ToString());
				formula = formula.Replace("Soul", (charData.Soul * charData.SoulRatio).ToString());
				formula = formula.Replace("MP", Mathf.Max(0, charData.CurMP).ToString());
				formula = formula.Replace("X", string.Format("{0:F1}", GetLevelCoefficient(charData.m_eSide)));
				formula = formula.Replace("SkillLv", charData.GetSkillLv(charData.m_iCurSkillID).ToString());
			}

			return formula;
		}

		private static string ReplaceProerty(string sFormula, CharHandler charHandler, bool bAim)
		{
			CharData charData = charHandler.m_CharData;
			sFormula = ReplaceBaseProperty(sFormula, bAim, charData);
			sFormula = ReplaceHeroProperty(sFormula, bAim, charData);
			return sFormula;
		}

		public static float GetSkillUpExpressValue(string formula, int heroLevel, int skillLevel, float force, float magic, float soul, float X)
		{
			formula = formula.Replace("Magic", magic.ToString());
			formula = formula.Replace("SkillLv", skillLevel.ToString());
			formula = formula.Replace("Soul", soul.ToString());
			formula = formula.Replace("HeroLv", heroLevel.ToString());
			formula = formula.Replace("Force", force.ToString());
			formula = formula.Replace("X", X.ToString());
			return FormulaManager.Arithmetic(formula);
		}

		public static string GetExpress(string name)
		{
			return _dicMath[name];
		}

		public static int GetLevelCoefficient(BattleEnum.Enum_CharSide side)
		{
			//int lev = side == BattleEnum.Enum_CharSide.Mine ? FightSceneMgr.GetInst().MineRoleLevel : FightSceneMgr.GetInst().m_iEnemyRoleLevel;
			//return levelCoefficient[lev - 1];
			return 1;
		}
	}
}
