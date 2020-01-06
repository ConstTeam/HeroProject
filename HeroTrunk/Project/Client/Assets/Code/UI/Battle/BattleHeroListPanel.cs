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

		private List<BattleHeroListItem> _lstHeroItem = new List<BattleHeroListItem>();
		private BattleHeroListAddItem _addHeroItem;
		private Animation _anim;
		private GameObject _gameObject;
		private bool _bShow = false;

		public static BattleHeroListPanel m_Inst;
		public static BattleHeroListPanel GetInst()
		{
			return m_Inst;
		}

		private void OnDestroy()
		{
			m_Inst = null;
		}

		private void Awake()
		{
			m_Inst = this;
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

			InitAddPanel();

			SetActive(PlayerInfo.GuideStep > 1);
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

		public void InsertBattleHero(int heroId, int heroLv, int heroIndex)
		{
			_lstHeroItem[heroIndex].ShowHero(heroId);
			_dicHeroInfoItem[heroId].DisableToggle();
			_addHeroItem.SetState(heroIndex);
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
		public Button AddCloseBtn;
		public Button AddOkBtn;
		public Text AddNeedCoinText;
		public ToggleGroup Group;

		private Dictionary<int, BattleHeroListInfoItem> _dicHeroInfoItem = new Dictionary<int, BattleHeroListInfoItem>();
		private int _iCurAddIndex;

		private void InitAddPanel()
		{
			List<int> lst = HeroAll.GetHeroList();
			for(int i = 0; i < lst.Count; ++i)
				InsertHeroItem(lst[i]);

			AddCloseBtn.onClick.AddListener(CloseAddPanel);
			AddOkBtn.onClick.AddListener(AddOk);
		}

		public void InsertHeroItem(int heroId)
		{
			BattleHeroListInfoItem item = ResourceLoader.LoadAssetAndInstantiate("PrefabUI/Battle/HeroList/BattleHeroListInfoItem", AddContentTrans).GetComponent<BattleHeroListInfoItem>();
			item.Init(heroId, Group);
			_dicHeroInfoItem.Add(heroId, item);
		}

		public void ShowAddPanel(bool bShow, int index, int coin)
		{
			_iCurAddIndex = index;
			AddNeedCoinText.text = coin.ToString();
			_anim["BattleHeroListChange"].speed = 1;
			_anim["BattleHeroListChange"].time = 0;
			_anim.Play("BattleHeroListChange");
		}

		private void CloseAddPanel()
		{
			_anim["BattleHeroListChange"].speed = -1;
			_anim["BattleHeroListChange"].time = _anim["BattleHeroListChange"].length;
			_anim.Play("BattleHeroListChange");
		}

		private void AddOk()
		{
			foreach(BattleHeroListInfoItem v in _dicHeroInfoItem.Values)
			{
				if(v.IsToggleOn())
				{
					Database.GetInst().NormalBattleAddHero(PlayerInfo.PlayerId, v.HeroId, _iCurAddIndex);
					break;
				}
			}
			CloseAddPanel();
		}
		#endregion
	}
}
