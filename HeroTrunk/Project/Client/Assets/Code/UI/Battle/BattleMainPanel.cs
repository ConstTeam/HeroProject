using UnityEngine;

namespace MS
{
	public class BattleMainPanel : MonoBehaviour
	{
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
		}

		private void OnDestroy()
		{
			_inst = null;
		}

		public void InitPanel()
		{
			ResourceLoader.LoadAssetAndInstantiate("PrefabUI/Battle/BattleHeroListPanel", SceneLoaderMain.GetInst().battleUIRoot);
		}
	}
}
