using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MS
{
	public class BattleHeroListPanel : MonoBehaviour
	{
		public Button ShowBtn;
		public Transform ContentTrans;
		public Animation Anim;

		private List<BattleHeroListItem> _lstHeroItem = new List<BattleHeroListItem>();
		private GameObject _gameObject;
		private bool _bShow = false;

		private static BattleHeroListPanel _inst;
		public static BattleHeroListPanel GetInst()
		{
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
				item.HeroIndex = i;
				item.Show(false);
				_lstHeroItem.Add(item);
			}
			ShowBtn.onClick.AddListener(ShowOrHide);
		}

		private void Start()
		{
			Refresh();
		}

		private void ShowOrHide()
		{
			Anim.Play(_bShow ? "BattleHeroListClose" : "BattleHeroListOpen");
			_bShow = !_bShow;
		}

		public void Refresh()
		{
			List<CharHandler> lst1 = BattleManager.GetInst().m_CharInScene.GetGeneral(BattleEnum.Enum_CharSide.Mine);
			List<CharHandler> lst2 = BattleManager.GetInst().m_CharInScene.GetOfficial(BattleEnum.Enum_CharSide.Mine);
			int heroCount = lst1.Count + lst2.Count;
			int i = 0;
			for(; i < lst1.Count; ++i)
			{
				_lstHeroItem[i].Show(true);
				_lstHeroItem[i].ShowHero(lst1[i].m_CharData.m_iCharID);
			}
			for(; i < heroCount; ++i)
			{
				_lstHeroItem[i].Show(true);
				_lstHeroItem[i].ShowHero(lst2[i].m_CharData.m_iCharID);
			}
			if(heroCount < 5)
				_lstHeroItem[i].Show(true);
		}

		private void OnDestroy()
		{
			_inst = null;
		}

		public bool BeShowPanel()
		{
			bool ret = !_gameObject.activeSelf;
			_gameObject.SetActive(ret);
			return ret;
		}

		public Vector3 GetShowBtnPos()
		{
			return ShowBtn.transform.position;
		}
	}
}
