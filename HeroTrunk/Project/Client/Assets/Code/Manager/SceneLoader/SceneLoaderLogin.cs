using UnityEngine;

namespace MS
{
	public class SceneLoaderLogin : MonoBehaviour
	{
		public Transform uiRoot;

		private void Awake()
		{
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
