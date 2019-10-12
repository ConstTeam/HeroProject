using UnityEngine;

namespace MS
{
	public class ResourceLoader
	{
		public static Object LoadAssets(string sPath)
		{
			return Resources.Load(sPath);
		}

		public static T LoadAsset<T>(string sPath) where T : Object 
		{	
			try
			{
				return Resources.Load<T>(sPath) as T;
			}
			catch
			{
				Debug.Log(string.Format("资源不存在{0}!", sPath));
				return null;
			}
		}

		public static GameObject LoadAssetAndInstantiate(string sPath)
		{
			try
			{
				return Object.Instantiate(Resources.Load(sPath)) as GameObject;
			}
			catch
			{
				Debug.Log(string.Format("资源不存在{0}!", sPath));
				return new GameObject();
			}
		}

		public static GameObject LoadAssetAndInstantiate(string sPath, Transform parent)
		{
			try
			{
				Object obj = Resources.Load(sPath);
				GameObject go = Object.Instantiate(obj) as GameObject;
				Transform trans = go.transform;
				trans.SetParent(parent, false);
				return go;
			}
			catch
			{
				Debug.Log(string.Format("资源不存在{0}!", sPath));
				return new GameObject();
			}
		}

		public static GameObject LoadAssetAndInstantiate(string sPath, Transform parent, Vector3 pos)
		{
			try
			{
				Object obj = Resources.Load(sPath);
				GameObject go = Object.Instantiate(obj, Vector3.zero, Quaternion.identity) as GameObject;
				Transform trans = go.transform;
				trans.SetParent(parent, false);
				trans.localPosition = pos;
				return go;
			}
			catch
			{
				Debug.Log(string.Format("资源不存在{0}!", sPath));
				return new GameObject();
			}
		}
	}
}
