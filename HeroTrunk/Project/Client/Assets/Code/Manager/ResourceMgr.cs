using System.Collections.Generic;
using UnityEngine;

namespace MS
{
	public class ResourceMgr
	{
		private static Stack<Transform> _tapLightning = new Stack<Transform>();
		public static void PopTapLightning(Vector3 pos)
		{
			Transform tran;
			if(_tapLightning.Count > 0)
				tran = _tapLightning.Pop();
			else
				tran = ResourceLoader.LoadAssetAndInstantiate("Effect/Skill/Hit/EftHitFlash", BattleManager.GetInst().BattlePoolTrans).transform;

			tran.position = pos;
			tran.gameObject.SetActive(true);
		}

		public static void PushTapLightning(Transform tran)
		{

		}
	}
}
