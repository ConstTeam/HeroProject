using UnityEngine;
using UnityEngine.UI;

namespace MS
{
	public class BattleMainPanel : MonoBehaviour
	{
		public Button HeroBtn;

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
			HeroBtn.onClick.AddListener(OpenHeroPanel);
		}

		private void OnDestroy()
		{
			_inst = null;
		}

		public void InitPanel()
		{
			
		}

		private void OpenHeroPanel()
		{
			BattleHeroListPanel.GetInst().OpenPanel();
		}
	}
}
