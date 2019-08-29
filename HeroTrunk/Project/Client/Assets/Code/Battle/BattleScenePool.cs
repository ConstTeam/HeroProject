using System.Collections.Generic;
using UnityEngine;

namespace MS
{
	public class BattleScenePool : MonoBehaviour
	{
		private Transform _transform;

		public List<int> m_lstCharMineID = new List<int>();

		private void Awake()
		{
			_transform = transform;
		}

		private void LoadHero()
		{
			List<int> charIds = GroupData.m_lstNormalGroup;

			GameObject charGo;
			GameObject handlerGo;
			int charId;
			for(int i = 0; i < charIds.Count; ++i)
			{
				charId = charIds[i];
				charGo = ResourceLoader.LoadAssetAndInstantiate(string.Format("Character/Hero{0}_Stand", charId), _transform, PositionMgr.vecHidePos);
				handlerGo = new GameObject("Handler");  //把其他代码和CharAnimCallback分开放
				handlerGo.transform.SetParent(charGo.transform);
				handlerGo.transform.localPosition = Vector3.zero;
				CharHandler charHandler = handlerGo.AddComponent<CharHandler>();
				charHandler.Init(charId, BattleEnum.Enum_CharSide.Mine, i);
			}
		}
	}
}
