using UnityEngine;
using UnityEngine.UI;

namespace MS
{
	public class GuidePanel : MonoBehaviour
	{
		public Text Description;
		public Button ContinueBtn;

		private GameObject _gameObject;
		private RectTransform _continueRect;
		private string[] _arrCurText;
		private int _iCurIndex;

		private static GuidePanel _inst;
		public static GuidePanel GetInst()
		{
			if(_inst == null)
				ResourceLoader.LoadAssetAndInstantiate("PrefabUI/Common/GuidePanel", ApplicationEntry.GetInst().uiRoot);
			return _inst;
		}

		private void Awake()
		{
			_inst = this;
			_gameObject = gameObject;
			_continueRect = ContinueBtn.GetComponent<RectTransform>();
			transform.SetAsFirstSibling();
			ContinueBtn.onClick.AddListener(Continue);
		}

		private void OnDestroy()
		{
			_inst = null;
		}

		private void Continue()
		{
			++_iCurIndex;
			SetInfo();
		}

		public void ShowPanel()
		{
			_iCurIndex = 0;
			_arrCurText = ConfigData.GetValue("Guide_Client", PlayerInfo.GuideStep.ToString(), "Description").Split('|');
			SetInfo();
			_gameObject.SetActive(true);
		}

		public void ClosePanel()
		{
			_gameObject.SetActive(false);
		}

		private void SaveGuideStep(int step)
		{
			PlayerInfo.GuideStep = step;
			Database.GetInst().SetGuideStep(PlayerInfo.PlayerId, PlayerInfo.GuideStep);
		}

		private void SetInfo()
		{
			if(_iCurIndex < _arrCurText.Length)
			{
				string t = ConfigData.GetValue("Lan_Guide_Client", _arrCurText[_iCurIndex], "Text");
				Description.text = t;

				switch(PlayerInfo.GuideStep)
				{
					case 1:
						BattleHeroListPanel.GetInst().SetActive(true);
						_continueRect.sizeDelta = new Vector2(100, 100);
						_continueRect.position = BattleHeroListPanel.GetInst().GetShowBtnPos();
						break;
				}
			}
			else
			{
				_gameObject.SetActive(false);
				switch(PlayerInfo.GuideStep)
				{
					case 0:
						BattleManager.GetInst().m_BattleScene.OnBattleInit();
						break;
					case 1:
						BattleHeroListPanel.GetInst().ShowOrHide();
						break;
				}
				SaveGuideStep(PlayerInfo.GuideStep + 1);
			}
				
		}
	}
}
