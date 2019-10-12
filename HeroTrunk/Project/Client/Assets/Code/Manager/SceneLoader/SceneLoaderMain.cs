using System.Collections;
using UnityEngine;

namespace MS
{
	public class SceneLoaderMain : MonoBehaviour
	{
		private static SceneLoaderMain _inst;

		public static SceneLoaderMain GetInst()
		{
			return _inst;
		}

		public GameObject	mainUICanvas;
		public Camera		mainUICamera;
		public Transform	mainUIRoot;

		public GameObject	battleUICanvas;
		public Camera		battleUICamera;
		public Transform	battleUIRoot;

		private void Awake()
		{
			_inst = this;
			StartCoroutine(LoadPanel());
		}

		private void OnDestroy()
		{
			_inst = null;
		}

		private IEnumerator LoadPanel()
		{
			SceneLoader.m_fSpeed = 0.1f;
			yield return LoadPanelProgress("PrefabUI/Main/MainPanel");
			yield return SceneLoader.SetProgress(SceneLoader.m_fProgress, 1f);
		}

		private IEnumerator LoadPanelProgress(string path)
		{
			ResourceLoader.LoadAssetAndInstantiate(path, mainUIRoot);
			yield return SceneLoader.SetProgress(0.01f, 0.99f);
		}

		public void LoadBattleScene()
		{
			mainUICanvas.SetActive(false);
			battleUICanvas.SetActive(true);
			SceneLoader.LoadBattleScene();
		}

		public void DestroyBattleUI()
		{
			battleUICanvas.SetActive(false);
			GameObject go;
			int count = battleUIRoot.childCount;
			for(int i = count - 1; i >=0; --i)
			{
				go = battleUIRoot.GetChild(i).gameObject;
				Destroy(go);
			}
		}

		public void ShowMainScene()
		{
			mainUICanvas.SetActive(true);
		}
	}
}
