using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MS
{
	public class SkillDataSon
	{
		public SkillDataWhole			m_Parent;
		public SkillEnum.AimSide		m_eAimSide;
		public SkillEnum.AimSideType	m_eAimSideType;
		public SkillEnum.CatchType		m_eCatchType;
		public SkillEnum.SkillType		m_eSkillType;
		public string[]					m_ReceptorEffect;	//特效受体效果
		public string					m_sReceptorPos;		//特效受体位置
		public int						m_iA;				//扇形范围夹角/矩形范围宽度
		public int						m_iB;				//扇形范围半径/矩形范围长度
		public int						m_iDistance;		//技能中心点距释放者距离
		public int						m_iCatchNum;		//受体数量
		public bool						m_bDeliver;			//是否弹射

		public string					m_sTimeLong;
		public string					m_sTimeMaxLong;
		public float					m_fTimeUnit;

		private List<SkillData> _lstSkillData = new List<SkillData>();

		public SkillDataSon(ConfigRow rowData, SkillDataWhole parent)
		{
			m_Parent = parent;
			m_bDeliver = false;
			string sSkillType = rowData.GetValue("SkillType");
			m_eSkillType = string.Empty == sSkillType ? SkillEnum.SkillType.Property : (SkillEnum.SkillType)Enum.Parse(typeof(SkillEnum.SkillType), sSkillType);
			string[] temp = rowData.GetValue("Camp_AimType").Split('_');
			m_eAimSide = (SkillEnum.AimSide)Enum.Parse(typeof(SkillEnum.AimSide), temp[0]);
			m_eAimSideType = (SkillEnum.AimSideType)Enum.Parse(typeof(SkillEnum.AimSideType), temp[1]);
			m_eCatchType = rowData.GetValue("CatchType").Length > 0 ? (SkillEnum.CatchType)Enum.Parse(typeof(SkillEnum.CatchType), rowData.GetValue("CatchType")) : SkillEnum.CatchType.All;

			SetCatchScope(rowData.GetValue("CatchScope"));
			SetBuffTime(rowData.GetValue("EffectExpress"));
			SetReceptorEffect(rowData.GetValue("ReceptorEffect"));
		}

		public void AddSkillData(int sonId, ConfigRow rowData)
		{
			SkillData skillData = new SkillData(sonId, rowData, this);
			_lstSkillData.Add(skillData);
		}

		public List<SkillData> GetSkillDataList()
		{
			return _lstSkillData;
		}

		private void SetCatchScope(string info)
		{
			if(!info.Equals(string.Empty))
			{
				string[] arr = info.Split(';');
				if(1 == arr.Length)
					m_iCatchNum = int.Parse(arr[0]);
				else if(2 == arr.Length)
				{
					if(arr[1].Equals("Deliver"))
					{
						m_bDeliver = true;
						m_iCatchNum = int.Parse(arr[0]);
					}
					else
					{
						m_iA = int.Parse(arr[0]);
						m_iB = int.Parse(arr[1]);
					}
				}
				else
				{
					m_iA = int.Parse(arr[0]);
					m_iB = int.Parse(arr[1]);
					m_iDistance = int.Parse(arr[2]);
				}
			}
		}

		private void SetBuffTime(string info)
		{
			switch(m_eSkillType)
			{
				case SkillEnum.SkillType.Buff:
				case SkillEnum.SkillType.Debuff:
				case SkillEnum.SkillType.SuperBuff:
				case SkillEnum.SkillType.SuperDebuff:
					Hashtable dic = MiniJSON.jsonDecode(info) as Hashtable;
					if(dic.ContainsKey("long"))
						m_sTimeLong = dic["long"].ToString();
					if(dic.ContainsKey("unit"))
						m_fTimeUnit = float.Parse(dic["unit"].ToString());
					if(dic.ContainsKey("max"))
						m_sTimeMaxLong = dic["max"].ToString();
					break;
			}
		}

		private void SetReceptorEffect(string info)
		{
			string[] effect = info.Split('|');
			m_ReceptorEffect = effect[0].Split(';');

			if(effect.Length > 1)
				m_sReceptorPos = effect[1];
		}
	}
}
