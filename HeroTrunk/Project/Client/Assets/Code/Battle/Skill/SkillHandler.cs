using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MS
{
	public class SkillHandler
	{
		private NormalHit _normalHit;
		private NormalShoot _normalShoot;
		private NormalSkill _normalSkill;

		private Dictionary<int, SkillDataWhole> _dicSkill = new Dictionary<int, SkillDataWhole>();

		private static SkillHandler _inst;
		public static SkillHandler GetInst()
		{
			return _inst;
		}

		public SkillHandler()
		{
			_dicSkill.Clear();
			_inst = this;
			FormatSkillCommon();
			_normalHit = new NormalHit();
			_normalShoot = new NormalShoot();
			_normalSkill = new NormalSkill();
		}

		//通过技能ID取到技能信息
		public SkillDataWhole GetSkillDataByID(int skillId)
		{
			return _dicSkill[skillId];
		}

		//结构化配置表数据
		private void FormatSkillCommon()
		{
			ConfigTable _skillCommonCfg = ConfigData.GetValue("Skill_Common");
			foreach(KeyValuePair<string, ConfigRow> pair in _skillCommonCfg.m_Data)
			{
				if(pair.Key.Trim().Length == 0)
					continue;

				ConfigRow rowData = pair.Value;
				int iSkillID = int.Parse(rowData.GetValue("SkillID"));
				int iSonID = int.Parse(rowData.GetValue("SonSkillID"));

				if(!_dicSkill.ContainsKey(iSkillID))
					_dicSkill.Add(iSkillID, new SkillDataWhole(iSkillID, rowData));

				_dicSkill[iSkillID].AddSkillData(iSonID, rowData);
			}
		}

		public void NormalHit(CharHandler charHandler)
		{
			_normalHit.OnProcess(charHandler);
		}

		public void NormalShoot(CharHandler charHandler)
		{
			_normalShoot.OnProcess(charHandler);
		}

		public void NormalSkill(CharHandler charHandler, int index)
		{
			_normalSkill.OnProcess(charHandler, index);
		}
	}
}
