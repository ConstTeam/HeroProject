using UnityEngine;

namespace MS
{
	public class SceneLoaderLogin : MonoBehaviour
	{
		public RectTransform uiRoot;

		private void Awake()
		{
			uiRoot.sizeDelta = ApplicationConst.uiSize;
			LoadPanel();
		}

		private void LoadPanel()
		{
			ResourceLoader.LoadAssetAndInstantiate("PrefabUI/Login/LoginPanel",			uiRoot);
			ResourceLoader.LoadAssetAndInstantiate("PrefabUI/Login/ServerListPanel",	uiRoot);
			ResourceLoader.LoadAssetAndInstantiate("PrefabUI/Login/CheckResPanel",		uiRoot);
		}
	}
}
