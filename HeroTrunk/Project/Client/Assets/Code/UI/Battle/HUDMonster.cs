using UnityEngine;
using UnityEngine.UI;

namespace MS
{
	public class HUDMonster : MonoBehaviour
	{
		public Image hpImg;

		private CharHandler handler;
		private GameObject _parentGo;

		private void Start()
		{
			_parentGo = transform.parent.gameObject;
		}

		public void GetHandler(CharHandler handler)
		{
			this.handler = handler;
			handler.m_CharData.FuncHPChangeCb += UpDataHp;
		}

		private void UpDataHp(float curHp, float maxHp)
		{
			if(_parentGo != null)
			{
				if(curHp <= 0)
				{
					_parentGo.SetActive(false);
					hpImg.fillAmount = 0;
				}
				else
				{
					_parentGo.SetActive(true);
					hpImg.fillAmount = curHp / maxHp;
				}
			}
		}

		private void OnDestroy()
		{
			handler.m_CharData.FuncHPChangeCb -= UpDataHp;
		}
	}
}
