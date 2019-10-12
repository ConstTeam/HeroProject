using UnityEngine;
using UnityEngine.UI;

namespace MS
{
	public class HeroPanel : MonoBehaviour
	{
		public Transform HeroModelRoot;
		public Button PreBtn;
		public Button NextBtn;

		private GameObject _gameObject;
		private GameObject _curHeroGo;
		private int _curHeroIndex = 0;

		private static HeroPanel _inst;
		public static HeroPanel GetInst()
		{
			if(_inst == null)
				ResourceLoader.LoadAssetAndInstantiate("PrefabUI/Main/HeroPanel", SceneLoaderMain.GetInst().mainUIRoot);

			return _inst;
		}

		private void Awake()
		{
			_inst = this;
			_gameObject = gameObject;
			PreBtn.onClick.AddListener(OnClickPre);
			NextBtn.onClick.AddListener(OnClickNext);
		}

		private void OnDestroy()
		{
			_inst = null;
		}

		public void OpenPanel()
		{
			_gameObject.SetActive(true);
			ShowHero(_curHeroIndex);
		}

		public void ClosePanel()
		{
			_gameObject.SetActive(false);
			DestroyHero();
		}

		public void DestroyHero()
		{
			if(_curHeroGo != null)
				Destroy(_curHeroGo);	
		}

		public void ShowHero(int heroIndex)
		{
			DestroyHero();
			_curHeroIndex = heroIndex;
			int heroId = HeroAll.GetHeroList()[_curHeroIndex];
			_curHeroGo = ResourceLoader.LoadAssetAndInstantiate(string.Format("Character/Hero{0}_Stand", heroId), HeroModelRoot);
			_curHeroGo.transform.Rotate(PositionMgr.vecRotY);
		}

		private void OnClickPre()
		{
			if(_curHeroIndex > 0)
				ShowHero(--_curHeroIndex);
		}

		private void OnClickNext()
		{
			if(_curHeroIndex < HeroAll.GetHeroList().Count - 1)
				ShowHero(++_curHeroIndex);
		}
	}
}
