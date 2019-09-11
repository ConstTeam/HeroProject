using System.Collections.Generic;
using UnityEngine;

namespace MS
{
	public class HUDTextMgr : MonoBehaviour
	{
		public enum HUDTextType
		{
			NormalHit,			//普通攻击伤害数字
			SkillHit,			//技能伤害数字
			MainSkillName,		//魂技能技能名字显示信息
			NormalSkillName,	//普通技能
			AbsorbHP,			//回复HP数字
			BUFF				//Buff类型信息
		}

		public Transform HeadHUDParent;
		public Transform HeadHUDSkillParent;

		private GameObject TextPrefab;
		private Queue<HUDTextItem> pool = new Queue<HUDTextItem>();
		private Queue<HUDTextItem> poolSkill = new Queue<HUDTextItem>();  //技能名字,显示在最上面

		private static HUDTextMgr _inst;
		public static HUDTextMgr GetInst()
		{
			return _inst;
		}

		private void OnDestroy()
		{
			_inst = null;
		}

		private void Awake()
		{
			_inst = this;
			TextPrefab = ResourceLoader.LoadAsset<GameObject>("PrefabUI/Battle/HUDText");
		}

		public void NewText(string text, CharHandler handler, HUDTextType type)
		{
			HUDTextItem item = GetText(type);
			Vector3 createPos = handler.UIFollowGo.transform.position;
			item.Show(text, handler, type, createPos);
		}

		//扩展池子
		private void ExpandPool(int count, HUDTextType type)
		{
			GameObject obj = null;
			if(type == HUDTextType.MainSkillName || type == HUDTextType.NormalSkillName)
			{
				for(int i = 0; i < count; i++)
				{
					obj = Instantiate(TextPrefab);
					obj.transform.SetParent(HeadHUDSkillParent);
					obj.transform.localScale = Vector3.one;
					poolSkill.Enqueue(obj.GetComponentInChildren<HUDTextItem>());
				}
			}
			else
			{
				for(int i = 0; i < count; i++)
				{
					obj = Instantiate(TextPrefab);
					obj.transform.SetParent(HeadHUDParent);
					obj.transform.localScale = Vector3.one;
					pool.Enqueue(obj.GetComponentInChildren<HUDTextItem>());
				}
			}

		}

		//得到一个HUD对象
		private HUDTextItem GetText(HUDTextType type)
		{
			HUDTextItem item;
			if(type == HUDTextType.MainSkillName || type == HUDTextType.NormalSkillName)
			{
				if(poolSkill.Count <= 0)
					ExpandPool(6, type);
				item = poolSkill.Dequeue();
			}
			else
			{
				if(pool.Count <= 0)
					ExpandPool(10, type);
				item = pool.Dequeue();
			}
			item.gameObject.SetActive(true);
			return item;
		}

		//回收一个HUD对象
		public void GetBackText(HUDTextItem item)
		{
			item.gameObject.SetActive(false);
			if(pool.Contains(item))
				return;

			pool.Enqueue(item);
		}

		//回收一个HUD SkillName对象
		public void GetBackSkillText(HUDTextItem item)
		{
			item.gameObject.SetActive(false);
			if(poolSkill.Contains(item))
				return;

			poolSkill.Enqueue(item);
		}
	}
}
