using System.Collections.Generic;
using UnityEngine;

namespace MS
{
	public class BattleHeroListPanel : MonoBehaviour
	{
		private List<BattleHeroListItem> _lstHeroItem = new List<BattleHeroListItem>();
		private GameObject _gameObject;

		private static BattleHeroListPanel _inst;
		public static BattleHeroListPanel GetInst()
		{
			if(_inst == null)
				ResourceLoader.LoadAssetAndInstantiate("PrefabUI/Battle/BattleHeroListPanel", SceneLoaderMain.GetInst().battleUIRoot);
			return _inst;
		}

		private void Awake()
		{
			_inst = this;
			_gameObject = gameObject;
		}

		private void OnDestroy()
		{
			_inst = null;
		}

		public void OpenPanel()
		{
			_gameObject.SetActive(true);
		}

		public void ClosePanel()
		{
			_gameObject.SetActive(false);
		}
	}
}
