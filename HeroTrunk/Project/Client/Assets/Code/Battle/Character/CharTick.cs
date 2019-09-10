using System.Collections.Generic;
using UnityEngine;

namespace MS
{
	public class CharTick : MonoBehaviour
	{
		private CharHandler _charHandler;
		private List<TickData> _lstTick = new List<TickData>();
		private float _fDeltaTime = 0f;

		public void SetCharHandler(CharHandler handler)
		{
			_charHandler = handler;
		}

		public void AddTick(TickData tickData)
		{
			_lstTick.Add(tickData);
		}

		public void Update()
		{
			_fDeltaTime += Time.deltaTime;
			if(_fDeltaTime >= 1.0f)
			{
				Excute();
				_fDeltaTime = 0;
			}
		}

		private void Excute()
		{
			if(_lstTick.Count > 0)
			{
				TickData t;
				for(int i = _lstTick.Count - 1, len = 0; i >= len; --i)
				{
					t = _lstTick[i];
					if(t.m_fTotalSec <= 0)
					{
						if(null != t.funcEnd)
							t.funcEnd(false);

						_lstTick.Remove(t);
					}
					else
					{
						if(0 == --t.m_fTotalSec % t.m_fUnitSec && null != t.funcTick)
							t.funcTick();
					}
				}
			}
		}

		//停止所有Buff Debuff
		public void CancelAll()
		{
			TickData tickData;
			for(int i = _lstTick.Count - 1, len = 0; i >= len; --i)
			{
				tickData = _lstTick[i];

				if(null != tickData.funcEnd)
					tickData.funcEnd(true);

				_lstTick.Remove(tickData);
			}
		}

		//停止某类型Buff
		public void CancelByType(SkillEnum.SkillType buffType)
		{
			TickData tickData;
			for(int i = _lstTick.Count - 1, len = 0; i >= len; --i)
			{
				tickData = _lstTick[i];
				if(tickData.m_SkillDataSon.m_eSkillType == buffType)
				{
					if(null != tickData.funcEnd)
						tickData.funcEnd(true);

					_lstTick.Remove(tickData);
				}
			}
		}

		public void CancelAbsorb()
		{
			CancelByInstID(_charHandler.m_CharData.Absorb.InstanceID);
		}

		public void CancelByInstID(int instId)
		{
			TickData tickData;
			for(int i = _lstTick.Count - 1, len = 0; i >= len; --i)
			{
				tickData = _lstTick[i];
				if(instId == tickData.m_iInstanceID)
				{
					if(null != tickData.funcEnd)
						tickData.funcEnd(true);

					_lstTick.Remove(tickData);
				}
			}
		}
	}
}
