using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MS
{
	public class BattleMainPanel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
	{
		public TextMeshProUGUI CurBigLevelText;
		public TextMeshProUGUI CurSmallLevelText;
		public TextMeshProUGUI CoinText;

		private static BattleMainPanel _inst;
		public static BattleMainPanel GetInst()
		{
			if(_inst == null)
			{
				ResourceLoader.LoadAssetAndInstantiate("PrefabUI/Battle/BattleMainPanel", SceneLoaderMain.GetInst().battleUIRoot);
				ResourceLoader.LoadAssetAndInstantiate("PrefabUI/Battle/HeroList/BattleHeroListPanel", SceneLoaderMain.GetInst().battleUIRoot);
			}
			return _inst;
		}

		private void Awake()
		{
			_inst = this;
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
	}
}
