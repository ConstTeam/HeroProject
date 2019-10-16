using UnityEngine;

namespace MS
{
	public class ChangeProperty
	{
		delegate void AddValueFunc(CharHandler aimCharHandler, float fValue);
		delegate void ChangeBoolFunc(CharHandler aimCharHandler, bool bFlag);

		public static void Change(SkillData skillData, CharHandler charHandler, CharHandler aimCharHandler, int aimCharIndex, TickData tickData = null, int skillInstId = -1)
		{
			float fValue = 0.0f;
			if(string.Empty != skillData.m_sEffectExpress)
			{
				fValue = Mathf.Max(0, BattleCalculate.ExcuteFormula(skillData.m_sEffectExpress, skillData.m_sEffectExpressMax, charHandler, aimCharHandler)) * skillData.m_iHurtTypeSign;
				fValue = RatioCoefficient(fValue, skillData.m_Parent.m_Parent.m_eSkillKind, charHandler, aimCharHandler);

				if(skillData.m_Parent.m_bDeliver)
					fValue *= aimCharIndex + 1;
			}

			SkillEnum.SkillSonType hurtType = skillData.m_eSkillSonType;
			switch(hurtType)
			{
				#region--攻防血魔
				case SkillEnum.SkillSonType.Attack:
					fValue = CtrlValue(aimCharHandler.m_CharData.CurAttack, fValue);
					_AddValue(charHandler, aimCharHandler, fValue, tickData, _AddAttack, ConfigData.GetHUDText("23"), ConfigData.GetHUDText("23"));     //"攻 {1}{0:N0}"
					break;
				case SkillEnum.SkillSonType.Defence:
					fValue = CtrlValue(aimCharHandler.m_CharData.CurDefence, fValue);
					_AddValue(charHandler, aimCharHandler, fValue, tickData, _AddDefence, ConfigData.GetHUDText("24"), ConfigData.GetHUDText("24"));        //"防 {1}{0:N0}"
					break;
				case SkillEnum.SkillSonType.HP:
					_ChangeHP(aimCharHandler, fValue, charHandler, skillData);
					if(null != tickData)
					{
						if(tickData.m_fUnitSec == tickData.m_fTotalSec)
							tickData.funcTick = null;
						else
							tickData.funcTick = () => { _ChangeHP(aimCharHandler, fValue, charHandler, skillData); };
					}
					break;
				case SkillEnum.SkillSonType.MP:
					_AddValue(charHandler, aimCharHandler, fValue, tickData, _AddMP, ConfigData.GetHUDText("25"), ConfigData.GetHUDText("25"));    //"士气 {1}{0:N0}"
					break;
				#endregion
				#region --三维属性
				case SkillEnum.SkillSonType.Force:
					_AddValue(charHandler, aimCharHandler, fValue, tickData, _AddForce, ConfigData.GetHUDText("26"), ConfigData.GetHUDText("26"));  // "武 {1}{0:N0}"
					break;
				case SkillEnum.SkillSonType.Soul:
					_AddValue(charHandler, aimCharHandler, fValue, tickData, _AddSoul, ConfigData.GetHUDText("27"), ConfigData.GetHUDText("27"));   //"魂 {1}{0:N0}"
					break;
				case SkillEnum.SkillSonType.Magic:
					_AddValue(charHandler, aimCharHandler, fValue, tickData, _AddMagic, ConfigData.GetHUDText("28"), ConfigData.GetHUDText("28"));     //"法 {1}{0:N0}"
					break;
				case SkillEnum.SkillSonType.ForceRatio:
					_AddValue(charHandler, aimCharHandler, fValue, tickData, _AddForceRatio, ConfigData.GetHUDText("29"), ConfigData.GetHUDText("30"));    //"武提升 {1}{0:P1}","武提升结束"
					break;
				case SkillEnum.SkillSonType.SoulRatio:
					_AddValue(charHandler, aimCharHandler, fValue, tickData, _AddSoulRatio, ConfigData.GetHUDText("31"), ConfigData.GetHUDText("32"));    //"魂提升 {1}{0:P1}","魂提升结束"
					break;
				case SkillEnum.SkillSonType.MagicRatio:
					_AddValue(charHandler, aimCharHandler, fValue, tickData, _AddMagicRatio, ConfigData.GetHUDText("33"), ConfigData.GetHUDText("34"));    //"法提升 {1}{0:P1}","法提升结束"
					break;
				#endregion
				#region --武功魂技法术系数
				case SkillEnum.SkillSonType.ForceSkillAtkRatio:
					_AddValue(charHandler, aimCharHandler, fValue, tickData, _AddForceSkillAtkRatio, ConfigData.GetHUDText("35"), ConfigData.GetHUDText("36"));  // "武功提升 {1}{0:P1}","武功提升结束"
					break;
				case SkillEnum.SkillSonType.SoulSkillAtkRatio:
					_AddValue(charHandler, aimCharHandler, fValue, tickData, _AddSoulSkillAtkRatio, ConfigData.GetHUDText("37"), ConfigData.GetHUDText("38"));    // "魂技提升 {1}{0:P1}","魂技提升结束"
					break;
				case SkillEnum.SkillSonType.MagicSkillAtkRatio:
					_AddValue(charHandler, aimCharHandler, fValue, tickData, _AddMagicSkillAtkRatio, ConfigData.GetHUDText("39"), ConfigData.GetHUDText("40"));  //"法术提升 {1}{0:P1}", "法术提升结束"
					break;
				case SkillEnum.SkillSonType.ForceSkillDefRatio:
					_AddValue(charHandler, aimCharHandler, fValue, tickData, _AddForceSkillDefRatio, ConfigData.GetHUDText("41"), ConfigData.GetHUDText("42"));  // "武功免伤 {1}{0:P1}", "武功免伤结束"
					break;
				case SkillEnum.SkillSonType.SoulSkillDefRatio:
					_AddValue(charHandler, aimCharHandler, fValue, tickData, _AddSoulSkillDefRatio, ConfigData.GetHUDText("43"), ConfigData.GetHUDText("44"));   //"魂技免伤 {1}{0:P1}","魂技免伤结束"
					break;
				case SkillEnum.SkillSonType.MagicSkillDefRatio:
					_AddValue(charHandler, aimCharHandler, fValue, tickData, _AddMagicSkillDefRatio, ConfigData.GetHUDText("45"), ConfigData.GetHUDText("46"));  // "法术免伤 {1}{0:P1}", "法术免伤结束"
					break;
				#endregion
				#region --暴击治疗系数
				case SkillEnum.SkillSonType.CriticalRatio:
					_AddValue(charHandler, aimCharHandler, fValue, tickData, _AddCriticalRatio, ConfigData.GetHUDText("47"), ConfigData.GetHUDText("48")); // "暴击率 {1}{0:P1}","暴击率恢复"
					break;
				case SkillEnum.SkillSonType.CureUpRatio:
					_AddValue(charHandler, aimCharHandler, fValue, tickData, _AddCureUpRatio, ConfigData.GetHUDText("49"), ConfigData.GetHUDText("50")); // "治疗提升 {1}{0:P1}","治疗提升结束"
					break;
				case SkillEnum.SkillSonType.CureDownRatio:
					_AddValue(charHandler, aimCharHandler, fValue, tickData, _AddCureDownRatio, ConfigData.GetHUDText("51"), ConfigData.GetHUDText("52"));    //"治疗压制 {1}{0:P1}","治疗压制结束"
					break;
				#endregion
				#region --数值盾
				case SkillEnum.SkillSonType.Absorb:
					Absorb(fValue, aimCharHandler, tickData);
					break;
				case SkillEnum.SkillSonType.AbsorbHP:
					AbsorbHP(fValue, aimCharHandler, tickData);
					break;
				case SkillEnum.SkillSonType.Rebound:
					Rebound(fValue, aimCharHandler, tickData);
					break;
				case SkillEnum.SkillSonType.SuperAbsorb:
					SuperAbsorb(fValue, aimCharHandler, tickData);
					break;
				#endregion
				case SkillEnum.SkillSonType.SkillForbid:
					SkillForbid(aimCharHandler, tickData);
					break;
				case SkillEnum.SkillSonType.SoulSkillInvalid:
					SoulSkillInvalid(aimCharHandler, tickData);
					break;
				case SkillEnum.SkillSonType.Stun:
					Stun(aimCharHandler, tickData);
					break;
				case SkillEnum.SkillSonType.Unbeatable:
					UbBeatable(aimCharHandler, tickData);
					break;
				case SkillEnum.SkillSonType.Concentrate:
					Concentrate(aimCharHandler, tickData);
					break;
				case SkillEnum.SkillSonType.Connect:
					Connect(fValue, aimCharHandler, tickData, skillInstId);
					break;
				default:
					break;
			}
		}

		private static float CtrlValue(float curValue, float fValue)
		{
			if(fValue < 0 && -fValue > curValue)
				return -curValue;

			return fValue;
		}

		private static void _AddValue(CharHandler charHandler, CharHandler aimCharHandler, float fValue, TickData tickData, AddValueFunc func, string beginText, string endText)
		{
			HUDTextMgr.GetInst().NewText(string.Format(beginText, fValue, fValue > 0 ? "+" : string.Empty), aimCharHandler, HUDTextMgr.HUDTextType.BUFF);
			func(aimCharHandler, fValue);
			if(null != tickData)
			{
				if(tickData.m_fUnitSec == tickData.m_fTotalSec)
					tickData.funcTick = null;
				else
					tickData.funcTick = () => { func(aimCharHandler, fValue); };

				tickData.funcEnd += (bool bCancel) =>
				{
					func(aimCharHandler, -fValue);
					HUDTextMgr.GetInst().NewText(string.Format(endText, fValue, fValue > 0 ? "-" : string.Empty), aimCharHandler, HUDTextMgr.HUDTextType.BUFF);
				};
			}
		}

		private static void _ChangeBool(CharHandler charHandler, CharHandler aimCharHandler, bool bFlag, TickData tickData, ChangeBoolFunc func, string beginText, string endText)
		{
			HUDTextMgr.GetInst().NewText(beginText, aimCharHandler, HUDTextMgr.HUDTextType.BUFF);

			func(aimCharHandler, bFlag);
			if(null != tickData)
			{
				tickData.funcTick = null;
				tickData.funcEnd += (bool bCancel) =>
				{
					func(aimCharHandler, !bFlag);
					HUDTextMgr.GetInst().NewText(endText, aimCharHandler, HUDTextMgr.HUDTextType.BUFF);
				};
			}
		}

		#region --攻防魔治疗
		private static void _AddAttack(CharHandler aimCharHandler, float fValue)
		{
			aimCharHandler.m_CharData.CurAttack += fValue;
		}

		private static void _AddDefence(CharHandler aimCharHandler, float fValue)
		{
			aimCharHandler.m_CharData.CurDefence += fValue;
		}

		private static void _AddMP(CharHandler aimCharHandler, float fValue)
		{
			aimCharHandler.m_CharData.CurMP += fValue;
		}

		private static void _AddCureUpRatio(CharHandler aimCharHandler, float fValue)
		{
			aimCharHandler.m_CharData.CureUpRatio += fValue;
		}

		private static void _AddCureDownRatio(CharHandler aimCharHandler, float fValue)
		{
			aimCharHandler.m_CharData.CureDownRatio += fValue;
		}

		private static void _ChangeHP(CharHandler aimCharHandler, float fValue, CharHandler charHandler, SkillData skillData)
		{
			if(fValue <= 0)
				aimCharHandler.BeHit(-fValue, charHandler, skillData.m_Parent);
			else
			{
				fValue = Mathf.Max(1f, fValue * (1 + aimCharHandler.m_CharData.CureUpRatio - aimCharHandler.m_CharData.CureDownRatio));
				HUDTextMgr.GetInst().NewText(string.Format("{0:N0}", fValue), aimCharHandler, HUDTextMgr.HUDTextType.AbsorbHP);
				aimCharHandler.m_CharData.CurHP += fValue;
			}
		}
		#endregion

		#region --三维属性
		private static void _AddForce(CharHandler aimCharHandler, float fValue)
		{
			aimCharHandler.m_CharData.Force += fValue;
		}

		private static void _AddSoul(CharHandler aimCharHandler, float fValue)
		{
			aimCharHandler.m_CharData.Rule += fValue;
		}

		private static void _AddMagic(CharHandler aimCharHandler, float fValue)
		{
			aimCharHandler.m_CharData.Strategy += fValue;
		}

		private static void _AddForceRatio(CharHandler aimCharHandler, float fValue)
		{
			aimCharHandler.m_CharData.ForceRatio += fValue;
		}

		private static void _AddSoulRatio(CharHandler aimCharHandler, float fValue)
		{
			aimCharHandler.m_CharData.RuleRatio += fValue;
		}

		private static void _AddMagicRatio(CharHandler aimCharHandler, float fValue)
		{
			aimCharHandler.m_CharData.ResourcefulnessRatio += fValue;
		}
		#endregion

		#region --武功魂技法术
		private static void _AddForceSkillAtkRatio(CharHandler aimCharHandler, float fValue)
		{
			aimCharHandler.m_CharData.ForceSkillAtkRatio += fValue;
		}

		private static void _AddSoulSkillAtkRatio(CharHandler aimCharHandler, float fValue)
		{
			aimCharHandler.m_CharData.SoulSkillAtkRatio += fValue;
		}

		private static void _AddMagicSkillAtkRatio(CharHandler aimCharHandler, float fValue)
		{
			aimCharHandler.m_CharData.MagicSkillAtkRatio += fValue;
		}

		private static void _AddForceSkillDefRatio(CharHandler aimCharHandler, float fValue)
		{
			aimCharHandler.m_CharData.ForceSkillDefRatio += fValue;
		}

		private static void _AddSoulSkillDefRatio(CharHandler aimCharHandler, float fValue)
		{
			aimCharHandler.m_CharData.SoulSkillDefRatio += fValue;
		}

		private static void _AddMagicSkillDefRatio(CharHandler aimCharHandler, float fValue)
		{
			aimCharHandler.m_CharData.MagicSkillDefRatio += fValue;
		}
		#endregion

		private static void _AddCriticalRatio(CharHandler aimCharHandler, float fValue)
		{
			aimCharHandler.m_CharData.CriticalRatio += fValue;
		}

		private static void Absorb(float fValue, CharHandler aimCharHandler, TickData tickData)
		{
			aimCharHandler.m_CharData.Absorb.AddNew(tickData.m_iInstanceID, fValue);
			if(null != tickData)
			{
				tickData.funcTick = null;
				tickData.funcEnd += (bool bCancel) =>
				{
					aimCharHandler.m_CharData.Absorb.Value = 0;
				};
			}
		}

		private static void AbsorbHP(float fValue, CharHandler aimCharHandler, TickData tickData)
		{
			aimCharHandler.m_CharData.AbsorbHP.AddNew(tickData.m_iInstanceID, fValue);
			if(null != tickData)
			{
				tickData.funcTick = null;
				tickData.funcEnd += (bool bCancel) =>
				{
					aimCharHandler.m_CharData.AbsorbHP.Value = 0;
				};
			}
		}

		private static void SuperAbsorb(float fValue, CharHandler aimCharHandler, TickData tickData)
		{
			aimCharHandler.m_CharData.Absorb.AddNew(tickData.m_iInstanceID, fValue);
			if(null != tickData)
			{
				tickData.funcTick = null;
				tickData.funcEnd += (bool bCancel) =>
				{
					aimCharHandler.m_CharData.Absorb.Value = 0;

					if(!bCancel)
						aimCharHandler.m_CharData.CurHP = aimCharHandler.m_CharData.MaxHP;
				};
			}
		}

		private static void Rebound(float fValue, CharHandler aimCharHandler, TickData tickData)
		{
			aimCharHandler.m_CharData.Rebound.AddNew(tickData.m_iInstanceID, fValue);
			if(null != tickData)
			{
				tickData.funcTick = null;
				tickData.funcEnd += (bool bCancel) =>
				{
					aimCharHandler.m_CharData.Rebound.Value = 0;
				};
			}
		}

		private static void SkillForbid(CharHandler aimCharHandler, TickData tickData)
		{
			aimCharHandler.m_CharData.SkillForbid.AddNew(tickData.m_iInstanceID, true);
			if(null != tickData)
			{
				tickData.funcTick = null;
				tickData.funcEnd += (bool bCancel) =>
				{
					aimCharHandler.m_CharData.SkillForbid.Value = false;
				};
			}
		}

		private static void SoulSkillInvalid(CharHandler aimCharHandler, TickData tickData)
		{
			aimCharHandler.m_CharData.SoulSkillInvalid.AddNew(tickData.m_iInstanceID, true);
			if(null != tickData)
			{
				tickData.funcTick = null;
				tickData.funcEnd += (bool bCancel) =>
				{
					aimCharHandler.m_CharData.SoulSkillInvalid.Value = false;
				};
			}
		}

		private static void Stun(CharHandler aimCharHandler, TickData tickData)
		{
			aimCharHandler.m_CharData.Stun.AddNew(tickData.m_iInstanceID, true);
			aimCharHandler.ToDizzy();
			if(null != tickData)
			{
				tickData.funcTick = null;
				tickData.funcEnd += (bool bCancel) =>
				{
					aimCharHandler.m_CharData.Stun.Value = false;
					if(!aimCharHandler.m_CharData.Stun.Value)
					{
						aimCharHandler.DizzyEnd();
					}
				};
			}
		}

		private static void UbBeatable(CharHandler aimCharHandler, TickData tickData)
		{
			aimCharHandler.m_CharData.UnBeatable.AddNew(tickData.m_iInstanceID, true);
			if(null != tickData)
			{
				tickData.funcTick = null;
				tickData.funcEnd += (bool bCancel) =>
				{
					aimCharHandler.m_CharData.UnBeatable.Value = false;
				};
			}
		}

		private static void Concentrate(CharHandler aimCharHandler, TickData tickData)
		{
			HUDTextMgr.GetInst().NewText(ConfigData.GetHUDText("19"), aimCharHandler, HUDTextMgr.HUDTextType.BUFF);   //"集火目标"

			aimCharHandler.m_CharData.Concentrate.AddNew(tickData.m_iInstanceID, true);
			if(null != tickData)
			{
				tickData.funcTick = null;
				tickData.funcEnd += (bool bCancel) =>
				{
					aimCharHandler.m_CharData.Concentrate.Value = false;
					if(!aimCharHandler.m_CharData.Concentrate.Value)
						HUDTextMgr.GetInst().NewText(ConfigData.GetHUDText("20"), aimCharHandler, HUDTextMgr.HUDTextType.BUFF);   //"集火结束"

				};
			}
		}

		private static void Connect(float fValue, CharHandler aimCharHandler, TickData tickData, int skillInstId)
		{
			BattleManager.GetInst().m_TriggerManager.AddConnectChar(skillInstId, aimCharHandler, fValue);
			if(null != tickData)
			{
				tickData.funcTick = null;
				tickData.funcEnd += (bool bCancel) =>
				{
					BattleManager.GetInst().m_TriggerManager.RemoveConnectChar(skillInstId, aimCharHandler);
				};
			}
		}

		public static float RatioCoefficient(float fValue, SkillEnum.SkillKind skillKind, CharHandler srcCharHandler, CharHandler aimCharHandler)
		{
			bool bSameSide = srcCharHandler.m_CharData.m_eSide == aimCharHandler.m_CharData.m_eSide;
			switch(skillKind)
			{
				case SkillEnum.SkillKind.Soul:
					fValue *= 1 + srcCharHandler.m_CharData.SoulSkillAtkRatio - (bSameSide ? 0 : aimCharHandler.m_CharData.SoulSkillDefRatio); //（1 + 攻方魂技提升 - 防方魂技免伤）
					break;
				case SkillEnum.SkillKind.Magic:
					fValue *= 1 + srcCharHandler.m_CharData.MagicSkillAtkRatio - (bSameSide ? 0 : aimCharHandler.m_CharData.MagicSkillDefRatio); //（1 + 攻方法术提升 - 防方法术免伤）
					break;
				default:
					fValue *= 1 + srcCharHandler.m_CharData.ForceSkillAtkRatio - (bSameSide ? 0 : aimCharHandler.m_CharData.ForceSkillDefRatio); //（1 + 攻方武功提升 - 防方武功免伤）

					//计算暴击格挡
					float criticalRatio = srcCharHandler.m_CharData.CriticalRatio;
					float blockRatio = bSameSide ? 0 : aimCharHandler.m_CharData.BlockRatio;
					if(criticalRatio + blockRatio > 1)
					{
						criticalRatio = criticalRatio / (criticalRatio + blockRatio);
						blockRatio = 1 - criticalRatio;
					}

					float rand = Random.Range(0f, 1f);
					if(rand < criticalRatio)
					{
						fValue *= 2;    //暴击
						HUDTextMgr.GetInst().NewText(string.Format(ConfigData.GetHUDText("21"), fValue), aimCharHandler, HUDTextMgr.HUDTextType.NormalHit);   //"暴击{0:N0}"
					}
					else if(rand - criticalRatio < blockRatio)
					{
						fValue = 0;     //格挡
						HUDTextMgr.GetInst().NewText(ConfigData.GetHUDText("22"), aimCharHandler, HUDTextMgr.HUDTextType.BUFF);   //"格挡"
					}
					break;
			}

			return fValue;
		}
	}
}
