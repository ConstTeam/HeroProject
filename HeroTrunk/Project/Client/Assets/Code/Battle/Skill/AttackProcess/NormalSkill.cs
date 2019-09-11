using System.Collections.Generic;
using UnityEngine;

namespace MS
{
	public class NormalSkill : AttackProcess
	{
		private List<List<CharHandler>> _lstTargetLists = new List<List<CharHandler>>();
		private List<CharHandler> _lstTargets = new List<CharHandler>();

		protected override List<CharHandler> GetTargets(CharHandler charHandler, SkillDataSon skillDataSon = null, int index = -1)
		{
			List<CharHandler> ret = null;
			List<List<CharHandler>> lists = GetTargetsByAimSideType(charHandler, skillDataSon);
			ret = GetTargetsByCatchType(charHandler, skillDataSon, lists, index);
			CheckRate(skillDataSon.m_Parent, charHandler, ref ret);
			return ret;
		}

		private List<List<CharHandler>> GetTargetsByAimSideType(CharHandler charHandler, SkillDataSon skillDataSon)
		{
			_lstTargetLists.Clear();
			BattleEnum.Enum_CharSide side = SkillEnum.AimSide.Self == skillDataSon.m_eAimSide ? charHandler.m_CharData.m_eSide : charHandler.m_CharData.GetOppositeSide();
			switch(skillDataSon.m_eAimSideType)
			{
				case SkillEnum.AimSideType.All:
					BattleManager.GetInst().m_CharInScene.GetAllChar(side, _lstTargetLists);
					break;
				case SkillEnum.AimSideType.General:
					BattleManager.GetInst().m_CharInScene.GetGeneral(side, _lstTargetLists);
					break;
				case SkillEnum.AimSideType.Official:
					BattleManager.GetInst().m_CharInScene.GetOfficial(side, _lstTargetLists);
					break;
				case SkillEnum.AimSideType.Hero:
					BattleManager.GetInst().m_CharInScene.GetAllHero(side, _lstTargetLists);
					break;
				case SkillEnum.AimSideType.Dead:
					BattleManager.GetInst().m_CharInScene.GetDeadGeneral(side, _lstTargetLists);
					break;
				case SkillEnum.AimSideType.Monster:
					BattleManager.GetInst().m_CharInScene.GetAllMonster(_lstTargetLists);
					break;
				case SkillEnum.AimSideType.Presence:
					BattleManager.GetInst().m_CharInScene.GetPresence(side, _lstTargetLists);
					break;
				case SkillEnum.AimSideType.Self:
					_lstTargetLists.Add(new List<CharHandler>() { charHandler });
					break;
				default:
					break;
			}
			return _lstTargetLists;
		}

		private List<CharHandler> GetTargetsByScope(CharHandler charHandler, SkillDataSon skillDataSon, List<List<CharHandler>> lists)
		{
			return _lstTargets;
		}

		private List<CharHandler> GetTargetsByCatchType(CharHandler charHandler, SkillDataSon skillDataSon, List<List<CharHandler>> lists, int index = -1)
		{
			_lstTargets.Clear();
			switch(skillDataSon.m_eCatchType)
			{
				case SkillEnum.CatchType.Randoms:
					TypeRandom(charHandler, skillDataSon, lists, _lstTargets);
					break;
				case SkillEnum.CatchType.Friend:
					TypeFriend(charHandler, skillDataSon, lists, _lstTargets);
					break;
				case SkillEnum.CatchType.HPLeast:
					TypeHPLeast(charHandler, skillDataSon, lists, _lstTargets);
					break;
				case SkillEnum.CatchType.Scope:
					TypeScope(charHandler, skillDataSon, lists, _lstTargets);
					break;
				case SkillEnum.CatchType.Rect:
					TypeRect(charHandler, skillDataSon, lists, _lstTargets);
					break;
				case SkillEnum.CatchType.Nearest:
					TypeNearest(charHandler, skillDataSon, lists, _lstTargets);
					break;
				case SkillEnum.CatchType.DistanceNear:
					if(0 == index)
					{
						TypeDistanceNear(charHandler, skillDataSon, lists, _lstTargets);
						SetEffect(skillDataSon, charHandler.m_CharSkill.m_vecDistancePos);
					}
					TypeDistance(charHandler, charHandler.m_CharSkill.m_vecDistancePos, skillDataSon, lists, _lstTargets);
					break;
				case SkillEnum.CatchType.DistanceFixed:
					if(0 == index)
					{
						TypeDistanceFixed(charHandler, skillDataSon, lists, _lstTargets);
						SetEffect(skillDataSon, charHandler.m_CharSkill.m_vecDistancePos);
					}
					TypeDistance(charHandler, charHandler.m_CharSkill.m_vecDistancePos, skillDataSon, lists, _lstTargets);
					break;
				default:
					AddAllChar(lists, _lstTargets);
					break;
			}
			return _lstTargets;
		}

		protected override void Excute(CharHandler charHandler, int index = -1, int skillInstId = -1)
		{
			SkillDataWhole skillDataWhole = SkillHandler.GetInst().GetSkillDataByID(charHandler.m_CharData.m_iCurSkillID);
			if(skillDataWhole.m_eSkillKind == SkillEnum.SkillKind.Soul && charHandler.m_CharData.SoulSkillInvalid.Value)
				return;

			SkillDataSon skillDataSon;
			List<CharHandler> targets;
			for(int i = 1; i <= skillDataWhole.GetSonCount(); ++i)
			{
				skillDataSon = skillDataWhole.GetSkillDataBySonID(i);
				targets = GetTargets(charHandler, skillDataSon, index);
				OnDoSkill(skillDataSon, charHandler, targets, skillInstId);
			}
		}

		private void OnDoSkill(SkillDataSon skillDataSon, CharHandler charHandler, List<CharHandler> targets, int skillInstId)
		{
			switch(skillDataSon.m_eSkillType)
			{
				case SkillEnum.SkillType.Buff:
				case SkillEnum.SkillType.Debuff:
				case SkillEnum.SkillType.SuperBuff:
				case SkillEnum.SkillType.SuperDebuff:
					_skillBuff.DoSkill(skillDataSon, charHandler, targets, skillInstId);
					break;
				case SkillEnum.SkillType.Property:
					_skillProperty.DoSkill(skillDataSon, charHandler, targets, skillInstId);
					break;
				case SkillEnum.SkillType.Bullet:
					_skillBullet.DoSkill(skillDataSon, charHandler, targets, skillInstId);
					break;
				case SkillEnum.SkillType.Special:
					_skillSpecial.DoSkill(skillDataSon, charHandler, targets, skillInstId);
					break;
				default:
					break;
			}
		}

		private void SetEffect(SkillDataSon skillDataSon, Vector3 pos)
		{
			string recepEffectPath = skillDataSon.m_Parent.m_sBulletPath;
			if(!recepEffectPath.Equals(string.Empty))
			{
				Transform effectTrans = BattleScenePool.GetInst().PopEffect(recepEffectPath);
				effectTrans.position = pos;
				BattleScenePool.GetInst().PushEffect(recepEffectPath, effectTrans, 5);
			}
		}

		#region --筛选对方的方法------------
		private void AddAllChar(List<List<CharHandler>> lists, List<CharHandler> listRet)
		{
			for(int i = 0; i < lists.Count; ++i)
			{
				for(int j = 0; j < lists[i].Count; ++j)
				{
					listRet.Add(lists[i][j]);
				}
			}
		}

		private void TypeRandom(CharHandler charHandler, SkillDataSon skillDataSon, List<List<CharHandler>> lists, List<CharHandler> listRet)
		{
			int totalNum = 0;
			for(int i = 0; i < lists.Count; ++i)
			{
				totalNum += lists[i].Count;
			}
			if(totalNum > skillDataSon.m_iCatchNum)
			{
				List<CharHandler> tmpList = new List<CharHandler>();
				AddAllChar(lists, tmpList);
				for(int i = 0; i < skillDataSon.m_iCatchNum; ++i)
				{
					int index = Random.Range(0, tmpList.Count);
					listRet.Add(tmpList[index]);
					tmpList.RemoveAt(index);
				}
			}
			else
				AddAllChar(lists, listRet);

			CharHandler con = GetConcentrate(charHandler, skillDataSon.m_eAimSide);
			if(null != con && !listRet.Contains(con))
				listRet[0] = con;
		}

		private void TypeFriend(CharHandler charHandler, SkillDataSon skillDataSon, List<List<CharHandler>> lists, List<CharHandler> listRet)
		{
			for(int i = 0; i < lists.Count; ++i)
			{
				for(int j = 0; j < lists[i].Count; ++j)
				{
					CharHandler tmpChar = lists[i][j];
					if(tmpChar != charHandler && !tmpChar.IsInexistence())
						listRet.Add(lists[i][j]);
				}
			}
		}

		private void TypeHPLeast(CharHandler charHandler, SkillDataSon skillDataSon, List<List<CharHandler>> lists, List<CharHandler> listRet)
		{
			CharHandler con = GetConcentrate(charHandler, skillDataSon.m_eAimSide);
			if(null == con)
			{
				for(int i = 0; i < lists.Count; ++i)
				{
					for(int j = 0; j < lists[i].Count; ++j)
					{
						CharHandler tmpChar = lists[i][j];
						if(!tmpChar.IsInexistence())
						{
							if(0 == listRet.Count)
								listRet.Add(lists[i][j]);
							else if(tmpChar.m_CharData.CurHP < listRet[0].m_CharData.CurHP)
								listRet[0] = tmpChar;
						}
					}
				}
			}
			else
				listRet.Add(con);
		}

		private void TypeScope(CharHandler charHandler, SkillDataSon skillDataSon, List<List<CharHandler>> lists, List<CharHandler> listRet)
		{
			CharHandler con = GetConcentrate(charHandler, skillDataSon.m_eAimSide);
			if(null != con)
				charHandler.m_ParentTrans.LookAt(con.m_ParentTrans);

			for(int i = 0; i < lists.Count; ++i)
			{
				GetCharInSector(charHandler, skillDataSon.m_iA, skillDataSon.m_iB, lists[i], listRet);
			}
		}

		private void TypeNearest(CharHandler charHandler, SkillDataSon skillDataSon, List<List<CharHandler>> lists, List<CharHandler> listRet)
		{
			CharHandler con = GetConcentrate(charHandler, skillDataSon.m_eAimSide);
			if(null == con)
			{
				for(int i = 0; i < lists.Count; ++i)
				{
					GetCharNearest(charHandler, skillDataSon.m_iA, skillDataSon.m_iB, lists[i], listRet);
				}
			}
			else
				listRet.Add(con);
		}

		private void TypeDistanceNear(CharHandler charHandler, SkillDataSon skillDataSon, List<List<CharHandler>> lists, List<CharHandler> listRet)
		{
			CharHandler con = GetConcentrate(charHandler, skillDataSon.m_eAimSide);
			if(null == con)
			{
				for(int i = 0; i < lists.Count; ++i)
				{
					GetCharNearest(charHandler, 360f, skillDataSon.m_iDistance, lists[i], listRet);
				}
			}
			else
				listRet.Add(con);

			if(listRet.Count > 0)
				charHandler.m_CharSkill.m_vecDistancePos = listRet[0].m_ParentTrans.position;
			else
				charHandler.m_CharSkill.m_vecDistancePos = charHandler.m_ParentTrans.position;
		}

		private void TypeDistanceFixed(CharHandler charHandler, SkillDataSon skillDataSon, List<List<CharHandler>> lists, List<CharHandler> listRet)
		{
			CharHandler con = GetConcentrate(charHandler, skillDataSon.m_eAimSide);
			if(null != con)
				charHandler.m_ParentTrans.LookAt(con.m_ParentTrans);

			charHandler.m_CharSkill.m_vecDistancePos = charHandler.m_ParentTrans.position + charHandler.m_ParentTrans.forward * skillDataSon.m_iDistance;
		}

		private void TypeDistance(CharHandler charHandler, Vector3 centerPos, SkillDataSon skillDataSon, List<List<CharHandler>> lists, List<CharHandler> listRet)
		{
			CharHandler con = GetConcentrate(charHandler, skillDataSon.m_eAimSide);
			if(null != con)
				charHandler.m_ParentTrans.LookAt(con.m_ParentTrans);

			for(int i = 0; i < lists.Count; ++i)
			{
				GetCharDistance(charHandler, centerPos, skillDataSon.m_iA, skillDataSon.m_iB, skillDataSon.m_iDistance, lists[i], listRet);
			}
		}

		private void TypeRect(CharHandler charHandler, SkillDataSon skillDataSon, List<List<CharHandler>> lists, List<CharHandler> listRet)
		{
			CharHandler con = GetConcentrate(charHandler, skillDataSon.m_eAimSide);
			if(null != con)
				charHandler.m_ParentTrans.LookAt(con.m_ParentTrans);

			for(int i = 0; i < lists.Count; ++i)
			{
				GetCharInRect(charHandler, skillDataSon.m_iA, skillDataSon.m_iB, lists[i], listRet);
			}
		}
		#endregion

		private void CheckRate(SkillDataWhole skillDataWhole, CharHandler charHandler, ref List<CharHandler> targets)
		{
			for(int i = targets.Count - 1; i >= 0; --i)
			{
				float rate = BattleCalculate.ExcuteFormula(skillDataWhole.m_sEffectRate, null, charHandler, targets[i]);
				if(Random.Range(0f, 1f) > rate)
				{
					HUDTextMgr.GetInst().NewText(ConfigData.GetValue("Lan_Battle_Client", "15", "Text"), targets[i], HUDTextMgr.HUDTextType.BUFF);   //"释放不成功"
					targets.RemoveAt(i);
				}
			}
		}
	}
}
