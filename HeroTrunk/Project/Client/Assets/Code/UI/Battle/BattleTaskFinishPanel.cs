using UnityEngine;
using UnityEngine.UI;

namespace MS
{
	public class BattleTaskFinishPanel : MonoBehaviour
	{
		public Transform ModelRootTran;
		public Button OkBtn;

		private GameObject _gameObject;
		private GameObject _ModelRootGo;
		private GameObject _curHeroGo;

		private static BattleTaskFinishPanel _inst;
		public static BattleTaskFinishPanel GetInst()
		{
			if(_inst == null)
				ResourceLoader.LoadAssetAndInstantiate("PrefabUI/Battle/BattleTaskFinishPanel", SceneLoaderMain.GetInst().battleUIRoot);

			return _inst;
		}

		private void OnDestroy()
		{
			_inst = null;
		}

		private void Awake()
		{
			_inst = this;
			_gameObject = gameObject;
			_ModelRootGo = ModelRootTran.gameObject;
			OkBtn.onClick.AddListener(Close);
		}

		public void Close()
		{
			DestroyHero();
			_gameObject.SetActive(false);
		}

		public void Show(int taskId)
		{
			ConfigRow row = ConfigData.GetValue("NormalTask_Client", taskId.ToString());
			switch(row.GetValue("Type"))
			{
				case "1":
				{
					ShowHero(int.Parse(row.GetValue("Value")));
					break;
				}
			}
		}

		public void DestroyHero()
		{
			if(_curHeroGo != null)
				Destroy(_curHeroGo);
			_ModelRootGo.SetActive(false);
		}

		public void ShowHero(int heroId)
		{
			DestroyHero();
			_curHeroGo = ResourceLoader.LoadAssetAndInstantiate(string.Format("Character/Hero{0}_Stand", heroId), ModelRootTran);
			_curHeroGo.transform.Rotate(PositionMgr.vecRotY);
			_ModelRootGo.SetActive(true);
		}
	}
}
