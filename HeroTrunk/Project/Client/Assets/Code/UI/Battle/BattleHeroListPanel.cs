using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MS
{
	public class BattleHeroListPanel : MonoBehaviour
	{
		public Button ShowBtn;
		public Transform ContentTrans;
		public Transform AddContentTrans;
		public GameObject AddPanel;
		public Button AddCloseBtn;
		public ToggleGroup Group;

		private List<BattleHeroListItem> _lstHeroItem = new List<BattleHeroListItem>();
		private List<BattleHeroListInfoItem> _lstHeroInfoItem = new List<BattleHeroListInfoItem>();
		private BattleHeroListAddItem _addHeroItem;
		private Animation _anim;
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
			_anim = GetComponent<Animation>();
			BattleHeroListItem item;
			for(int i = 0; i < 5; ++i)
			{
				item = ResourceLoader.LoadAssetAndInstantiate("PrefabUI/Battle/HeroList/BattleHeroListItem", ContentTrans).GetComponent<BattleHeroListItem>();
				item.HeroIndex = i;
				_lstHeroItem.Add(item);
			}
			_addHeroItem = ResourceLoader.LoadAssetAndInstantiate("PrefabUI/Battle/HeroList/BattleHeroListAddItem", ContentTrans).GetComponent<BattleHeroListAddItem>();
			ShowBtn.onClick.AddListener(ShowOrHide);
			AddCloseBtn.onClick.AddListener(CloseAddPanel);

			SetAddPanel();

			SetActive(PlayerInfo.GuideStep > 1);
		}

		private void Start()
		{
			Refresh();
		}

		public void SetActive(bool bActive)
		{
			_gameObject.SetActive(bActive);
		}

		public void ShowOrHide()
		{
			_anim.Play(_bShow ? "BattleHeroListClose" : "BattleHeroListOpen");
			_bShow = !_bShow;
		}

		public void Refresh()
		{
			List<CharHandler> lst1 = BattleManager.GetInst().m_CharInScene.GetGeneral(BattleEnum.Enum_CharSide.Mine);
			List<CharHandler> lst2 = BattleManager.GetInst().m_CharInScene.GetOfficial(BattleEnum.Enum_CharSide.Mine);
			int heroCount = lst1.Count + lst2.Count;

			int i = 0;
			for(; i < lst1.Count; ++i)
				_lstHeroItem[i].ShowHero(lst1[i].m_CharData.m_iCharID);
			for(; i < heroCount; ++i)
				_lstHeroItem[i].ShowHero(lst2[i].m_CharData.m_iCharID);
			_addHeroItem.SetState(i);
		}

		private void OnDestroy()
		{
			_inst = null;
		}

		public void SyncCoin()
		{
			for(int i = 0; i < _lstHeroItem.Count; ++i)
				_lstHeroItem[i].SetBtnState();
			_addHeroItem.SetBtnState();
		}

		public Vector3 GetShowBtnPos()
		{
			return ShowBtn.transform.position;
		}

		#region --Add Panel---------
		public void ShowAddPanel(bool bShow)
		{
			if(bShow)
			{
				_anim["BattleHeroListChange"].speed = 1;
				_anim["BattleHeroListChange"].time = 0;
			}
			else
			{
				_anim["BattleHeroListChange"].speed = -1;
				_anim["BattleHeroListChange"].time = _anim["BattleHeroListChange"].length;
			}
			_anim.Play("BattleHeroListChange");
		}

		private void AddHero()
		{
			//BattleManager.GetInst().m_BattleScene.Coin -= _iNeedCoin;
			//BattleManager.GetInst().AddHero(_curIndex);
		}

		private void CloseAddPanel()
		{
			ShowAddPanel(false);
		}

		private void SetAddPanel()
		{
			List<int> lst = HeroAll.GetHeroList();
			BattleHeroListInfoItem item;
			for(int i = 0; i < lst.Count; ++i)
			{
				item = ResourceLoader.LoadAssetAndInstantiate("PrefabUI/Battle/HeroList/BattleHeroListInfoItem", AddContentTrans).GetComponent<BattleHeroListInfoItem>();
				item.Init(lst[i], Group);
				_lstHeroInfoItem.Add(item);
			}
		}
		#endregion
	}
}
