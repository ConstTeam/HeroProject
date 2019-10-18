using System.Collections.Generic;
using UnityEngine;

namespace MS
{
	public class BattleHeroListPanel : MonoBehaviour
	{
		public Transform ContentTrans;

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
			BattleHeroListItem item;
			for(int i = 0; i < 5; ++i)
			{
				item = ResourceLoader.LoadAssetAndInstantiate("PrefabUI/Battle/BattleHeroListItem", ContentTrans).GetComponent<BattleHeroListItem>();
				item.Index = i;
				item.Show(false);
				_lstHeroItem.Add(item);
			}
			_gameObject.SetActive(false);
		}

		private void Start()
		{
			Refresh();
		}

		public void Refresh()
		{
			int heroCount = BattleManager.GetInst().m_CharInScene.GetHeroCount(BattleEnum.Enum_CharSide.Mine);
			int i = 0;
			for(; i < heroCount; ++i)
			{
				_lstHeroItem[i].Show(true);
				_lstHeroItem[i].ShowHero();
			}
			if(heroCount < 5)
				_lstHeroItem[i].Show(true);
		}

		private void OnDestroy()
		{
			_inst = null;
		}

		public void BeShowPanel()
		{
			_gameObject.SetActive(!_gameObject.activeSelf);
		}
	}
}
