using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MS
{
	public class TickData : MonoBehaviour
	{
		public delegate void TickFunc();
		public delegate void TickEndFunc(bool bCancel);
		public SkillDataSon m_SkillDataSon;
		public TickFunc funcTick;
		public TickEndFunc funcEnd;
		public float m_fTotalSec;   //Tick总秒数
		public float m_fUnitSec;        //执行间隔秒数
		public int m_iInstanceID;

		public SkillEnum.SkillSonType m_eBuffType;

		private CharHandler _CharHandler;
		private CharHandler _AimCharHandler;
		private int _iAimCharIndex;

		public static int iTickDataInstanceID = 0;

		public TickData(CharHandler charHandler, CharHandler aimCharHandler, SkillDataSon skillDataSon, int aimCharIndex, int skillInstId)
		{
			m_iInstanceID = ++iTickDataInstanceID;
			_CharHandler = charHandler;
			_AimCharHandler = aimCharHandler;
			m_SkillDataSon = skillDataSon;
			m_eBuffType = skillDataSon.GetSkillDataList()[0].m_eSkillSonType;
			_iAimCharIndex = aimCharIndex;

			float fTotalSecOri;
			if(float.TryParse(m_SkillDataSon.m_sTimeLong, out fTotalSecOri))
				m_fTotalSec = fTotalSecOri;
			else
			{
				fTotalSecOri = Mathf.Ceil(BattleCalculate.ExcuteFormula(m_SkillDataSon.m_sTimeLong, m_SkillDataSon.m_sTimeMaxLong, charHandler, aimCharHandler));
				m_fTotalSec = ChangeProperty.RatioCoefficient(fTotalSecOri, skillDataSon.m_Parent.m_eSkillKind, charHandler, aimCharHandler);
			}

			if(skillDataSon.m_bDeliver)
				fTotalSecOri *= aimCharIndex + 1;

			switch(m_eBuffType)
			{
				case SkillEnum.SkillSonType.Stun:
					m_fTotalSec = fTotalSecOri * (1 - aimCharHandler.m_CharData.StunDefRatio);  //1 - 抗晕
					break;
				case SkillEnum.SkillSonType.SkillForbid:
					m_fTotalSec = fTotalSecOri * (1 - aimCharHandler.m_CharData.ForbidDefRatio);    //1 - 抗封
					break;
				default:
					m_fTotalSec = fTotalSecOri;
					break;
			}

			if(m_fTotalSec > 0f)
			{
				m_fUnitSec = m_SkillDataSon.m_fTimeUnit;

				if(m_fUnitSec > 0f)
					m_fTotalSec = m_fTotalSec - m_fTotalSec % m_fUnitSec;
				else
					m_fUnitSec = m_fTotalSec;

				SetFunction(skillInstId);
			}
		}

		private void SetFunction(int skillInstId)
		{
			SetEffect();
			List<SkillData> lstSkillDatas = m_SkillDataSon.GetSkillDataList(); ;
			for(int i = 0, len = lstSkillDatas.Count; i < len; ++i)
			{
				ChangeProperty.Change(lstSkillDatas[i], _CharHandler, _AimCharHandler, _iAimCharIndex, this, skillInstId);
			}
		}

		private void SetEffect()
		{
			string recepEffectPath = m_SkillDataSon.m_ReceptorEffect[0];
			if(!recepEffectPath.Equals(string.Empty))
			{
				switch(m_SkillDataSon.m_sReceptorPos)
				{
					case "Hand":
						_SetEffect(_AimCharHandler.m_CharBody.LeftHand);
						_SetEffect(_AimCharHandler.m_CharBody.RightHand);
						break;
					default:
						_SetEffect(_AimCharHandler.m_Transform);
						break;
				}
			}
		}

		private void _SetEffect(Transform parent)
		{
			string path = m_SkillDataSon.m_ReceptorEffect[0];
			Transform effectTrans = BattleScenePool.GetInst().PopEffect(path);
			effectTrans.SetParent(parent);
			effectTrans.localPosition = Vector3.zero;
			funcEnd += (bool bCancel) =>
			{
				BattleScenePool.GetInst().PushEffect(path, effectTrans);

				if(m_SkillDataSon.m_ReceptorEffect.Length > 1)
				{
					path = m_SkillDataSon.m_ReceptorEffect[1];
					effectTrans = BattleScenePool.GetInst().PopEffect(path);
					effectTrans.SetParent(parent);
					effectTrans.localPosition = Vector3.zero;

					BattleScenePool.GetInst().PushEffect(path, effectTrans, 5);
				}
			};
		}
	}
}
