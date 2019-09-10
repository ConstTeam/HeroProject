using System;

namespace MS
{
	public class SkillData
	{
		public SkillDataSon m_Parent;
		public int			m_iSonSkillID;
		public int			m_iHurtTypeSign		= 1;		//技能伤害公式正负号
		public string		m_sEffectExpress;				//技能公式
		public string		m_sEffectExpressMax = "";		//技能最大值公式
		public SkillEnum.SkillSonType m_eSkillSonType;		//子技能类型


		public SkillData(int sonSkillId, ConfigRow rowData, SkillDataSon parent)
		{
			m_Parent = parent;
			m_iSonSkillID = sonSkillId;
			HandleSkillType(rowData);
			SetEffectExpress(rowData.GetValue("EffectExpress"));
		}

		//处理技能类型
		private void HandleSkillType(ConfigRow rowData)
		{
			string hurtType = rowData.GetValue("HurtType");
			if(hurtType.IndexOf("Add") > -1)
			{
				hurtType = hurtType.Replace("Add", "");
				m_iHurtTypeSign = 1;
			}
			else if(hurtType.IndexOf("Reduce") > -1)
			{
				hurtType = hurtType.Replace("Reduce", "");
				m_iHurtTypeSign = -1;
			}

			m_eSkillSonType = (SkillEnum.SkillSonType)Enum.Parse(typeof(SkillEnum.SkillSonType), hurtType);
		}

		private void SetEffectExpress(string sEffectExpress)
		{
			if(string.Empty == sEffectExpress)
				return;

			string[] arrExp = sEffectExpress.Split('|');
			m_sEffectExpress = arrExp[0];
			m_sEffectExpressMax = arrExp.Length > 1 ? arrExp[1] : string.Empty;
		}
	}
}
