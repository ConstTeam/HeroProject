using UnityEngine;
using UnityEngine.UI;

namespace MS
{
	public class MonsterHpItem : MonoBehaviour
	{
		public Image hpImg;

		private CharHandler handler;

		public void GetHandler(CharHandler handler)
		{
			this.handler = handler;
			handler.m_CharData.FuncHPChangeCb += UpDataHp;
		}

		private void UpDataHp(float curHp, float maxHp)
		{
			if(curHp <= 0)
			{
				transform.parent.gameObject.SetActive(false);
				hpImg.fillAmount = 0;
			}
			else
			{
				transform.parent.gameObject.SetActive(true);
				hpImg.fillAmount = curHp / maxHp;
			}
		}

		private void OnDestroy()
		{
			handler.m_CharData.FuncHPChangeCb -= UpDataHp;
		}
	}
}
