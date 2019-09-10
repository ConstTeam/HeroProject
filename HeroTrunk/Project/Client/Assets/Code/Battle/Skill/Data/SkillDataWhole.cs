using System;
using System.Collections.Generic;
using UnityEngine;

namespace MS
{
	public class SkillDataWhole : MonoBehaviour
	{
		public int		m_iSkillID;
		public int		m_iSkillIndex;
		public int		m_iNeedMP;
		public int		m_iCDTime;
		public float	m_fUpExpressMax;
		public string	m_sUpExpress;
		public string	m_sDisplayName;
		public string	m_sExplain;
		public string	m_sExplain2;
		public string	m_sNote;
		public string	m_sIcon;
		public string	m_sType;					//技能类型
		public int		m_iTriggerParam;
		public int		m_iActionID;
		public string	m_sEffectRate;				//发动成功率公式
		public string	m_sBulletPath;				//子弹预设路径
		public bool		m_bHitBackward;				//是否击退

		public SkillEnum.SkillKind m_eSkillKind;	//技能大类
		public SkillEnum.TriggerType m_eTriggerType;

		private Dictionary<int, SkillDataSon> _dicSkillDataSon = new Dictionary<int, SkillDataSon>();

		public SkillDataWhole(int skillId, ConfigRow row)
		{
			SetLanData(skillId, row);
			SetData(skillId, row);
		}

		private void SetLanData(int skillId, ConfigRow row)
		{
			ConfigRow LanguageRow = ConfigData.GetValue("Lan_Skill_Common", skillId.ToString());
			string sDisplayName = LanguageRow.GetValue("DisplayName");
			m_sDisplayName = sDisplayName;
			m_sExplain = LanguageRow.GetValue("Explain");
			m_sExplain2 = LanguageRow.GetValue("Explain2");
			m_sNote = LanguageRow.GetValue("Note");
			m_sType = LanguageRow.GetValue("SkillTypeName");
		}

		private void SetData(int skillId, ConfigRow rowData)
		{
			m_iSkillID = skillId;
			m_iSkillIndex = skillId % 100;
			m_sIcon = string.Format("SkillIcon{0}", m_iSkillIndex < 4 ? skillId : m_iSkillIndex);
			m_iActionID = rowData.GetValue("ActionID").Equals(String.Empty) ? -1 : int.Parse(rowData.GetValue("ActionID"));
			m_sEffectRate = rowData.GetValue("RateExpress");
			m_sEffectRate = m_sEffectRate.Equals(string.Empty) ? "1" : m_sEffectRate;

			string[] arrUpExpress = rowData.GetValue("UpExpress").Split('|');
			m_sUpExpress = arrUpExpress[0];
			m_fUpExpressMax = arrUpExpress.Length > 1 ? float.Parse(arrUpExpress[1]) : 0.0f;

			m_bHitBackward = "1" == rowData.GetValue("Backward");
			m_sBulletPath = ConfigData.GetValue("SkillEffect_Client", skillId.ToString(), "BulletPath");

			SetSkillKind(rowData.GetValue("Condition"));
		}

		private void SetSkillKind(string condition)
		{
			switch(m_iSkillIndex)
			{
				case 1:
					m_eSkillKind = SkillEnum.SkillKind.Soul;
					m_iNeedMP = int.Parse(condition);
					break;
				case 2:
				case 3:
					m_eSkillKind = SkillEnum.SkillKind.Force;
					m_iCDTime = int.Parse(condition);
					break;
				case 4:
				case 5:
					m_eSkillKind = SkillEnum.SkillKind.Magic;
					string[] triggerTime = condition.Split('|');
					m_eTriggerType = (SkillEnum.TriggerType)Enum.Parse(typeof(SkillEnum.TriggerType), triggerTime[0]);
					m_iTriggerParam = triggerTime.Length > 1 ? int.Parse(triggerTime[1]) : 0;
					break;
			}
		}

		public int GetSonCount()
		{
			return _dicSkillDataSon.Count;
		}

		public SkillDataSon GetSkillDataBySonID(int sonId)
		{
			return _dicSkillDataSon[sonId];
		}

		public void AddSkillData(int sonId, ConfigRow rowData)
		{
			if(!_dicSkillDataSon.ContainsKey(sonId))
				_dicSkillDataSon.Add(sonId, new SkillDataSon(rowData, this));

			_dicSkillDataSon[sonId].AddSkillData(sonId, rowData);
		}
	}
}
