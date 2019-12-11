using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MS
{
	public class BattleMainPanel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
	{
		public Button HeroBtn;
		public TextMeshProUGUI CurBigLevelText;
		public TextMeshProUGUI CurSmallLevelText;

		private Transform _heroBtnTrans;

		private static BattleMainPanel _inst;
		public static BattleMainPanel GetInst()
		{
			if(_inst == null)
				ResourceLoader.LoadAssetAndInstantiate("PrefabUI/Battle/BattleMainPanel", SceneLoaderMain.GetInst().battleUIRoot);

			return _inst;
		}

		private void Awake()
		{
			_inst = this;
			_heroBtnTrans = HeroBtn.transform;
			HeroBtn.onClick.AddListener(OpenHeroPanel);
		}

		private void OnDestroy()
		{
			_inst = null;
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			BattleManager.GetInst().TapLightning();
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			//throw new System.NotImplementedException();
		}

		private void OpenHeroPanel()
		{
			bool bShow = BattleHeroListPanel.GetInst().BeShowPanel();
			_heroBtnTrans.localRotation = bShow ? Quaternion.Euler(0, 0, 270) : Quaternion.Euler(0, 0, 90);
		}
	}
}
