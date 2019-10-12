using UnityEngine;
using UnityEngine.UI;

namespace MS
{
	public class HeroPanel : MonoBehaviour
	{
		public Transform HeroModelRoot;

		private GameObject _gameObject;
		private GameObject _curHeroGo;

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
		}

		private void OnDestroy()
		{
			_inst = null;
		}

		public void OpenPanel()
		{
			_gameObject.SetActive(true);
			if(_curHeroGo == null)
				ShowHero(HeroAll.GetHeroList()[0]);
		}

		public void ClosePanel()
		{
			_gameObject.SetActive(false);
		}

		public void ShowHero(int heroId)
		{
			if(_curHeroGo != null)
				Destroy(_curHeroGo);

			_curHeroGo = ResourceLoader.LoadAssetAndInstantiate(string.Format("Character/Hero{0}_Stand", heroId), HeroModelRoot);
			_curHeroGo.transform.Rotate(PositionMgr.vecRotY);
		}
	}
}
