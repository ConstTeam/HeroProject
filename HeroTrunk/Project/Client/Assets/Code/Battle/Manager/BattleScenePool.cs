using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MS
{
	public class BattleScenePool : MonoBehaviour
	{
		public List<int>	m_lstCharMineID		= new List<int>();
		public List<int>	m_lstCharEnemyID	= new List<int>();
		public List<int>	m_lstMonsterID		= new List<int>();

		private Dictionary<int, CharHandler>			_dicCharMinePool		= new Dictionary<int, CharHandler>();
		private Dictionary<int, CharHandler>			_dicCharEnemyPool		= new Dictionary<int, CharHandler>();
		private Dictionary<int, Stack<CharHandler>>		_dicMonsterPool			= new Dictionary<int, Stack<CharHandler>>();
		private Dictionary<int, Stack<BulletBase>>		_dicHeroBulletPool		= new Dictionary<int, Stack<BulletBase>>();
		private Dictionary<int, Stack<BulletBase>>		_dicMonsterBulletPool	= new Dictionary<int, Stack<BulletBase>>();
		private Dictionary<string, Stack<Transform>>	_dicEffects				= new Dictionary<string, Stack<Transform>>();

		private Transform	_transform;
		private ConfigTable	_monsterConfig;

		private static BattleScenePool _inst;
		public static BattleScenePool GetInst()
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
			_transform = transform;
			_monsterConfig = ConfigData.GetValue("Monster_Client");
		}

		public void SetHeroIdMine()
		{
			m_lstCharMineID = BattleManager.GetInst().GetHeroIdsMine();
		}

		public void LoadHero(BattleEnum.Enum_CharSide side)
		{
			bool bMine = BattleEnum.Enum_CharSide.Mine == side;
			List<int> charIds = bMine ? m_lstCharMineID : m_lstCharEnemyID;
			Dictionary<int, CharHandler> dicCharPool = bMine ? _dicCharMinePool : _dicCharEnemyPool;

			GameObject charGo;
			GameObject handlerGo;
			int charId;
			for(int i = 0; i < m_lstCharMineID.Count; ++i)
			{
				charId = charIds[i];
				charGo = ResourceLoader.LoadAssetAndInstantiate(string.Format("Character/Hero{0}_Stand", charId.ToString()), _transform, PositionMgr.vecHidePos);
				handlerGo = new GameObject("Handler");  //把其他代码和CharAnimCallback分开放
				handlerGo.transform.SetParent(charGo.transform);
				handlerGo.transform.localPosition = Vector3.zero;
				CharHandler charHandler = handlerGo.AddComponent<CharHandler>();
				charHandler.Init(charId, BattleEnum.Enum_CharSide.Mine, i);
				dicCharPool.Add(charId, charHandler);
				//PreloadBulletHero(charHandler);
			}
		}

		public Transform PopEffect(string key)
		{
			Transform ret;
			if(_dicEffects.ContainsKey(key))
			{
				Stack<Transform> st = _dicEffects[key];
				if(st.Count > 0)
					ret = st.Pop();
				else
					ret = ResourceLoader.LoadAssetAndInstantiate(key, _transform).transform;
			}
			else
			{
				Stack<Transform> st = new Stack<Transform>();
				_dicEffects.Add(key, st);
				ret = ResourceLoader.LoadAssetAndInstantiate(key, _transform).transform;
			}

			ret.gameObject.SetActive(true);
			return ret;
		}

		public Transform PopEffect<T>(string key) where T : MonoBehaviour
		{
			Transform ret;
			if(_dicEffects.ContainsKey(key))
			{
				Stack<Transform> st = _dicEffects[key];
				if(st.Count > 0)
					ret = st.Pop();
				else
				{
					ret = ResourceLoader.LoadAssetAndInstantiate(key, _transform).transform;
					ret.gameObject.AddComponent<T>();
				}
			}
			else
			{
				Stack<Transform> st = new Stack<Transform>();
				_dicEffects.Add(key, st);
				ret = ResourceLoader.LoadAssetAndInstantiate(key, _transform).transform;
				ret.gameObject.AddComponent<T>();
			}

			ret.gameObject.SetActive(true);
			return ret;
		}

		public void PushEffect(string key, Transform effect, float delay = 0)
		{
			if(0 == delay)
				__PushEffect(key, effect);
			else
				StartCoroutine(_PushEffect(key, effect, delay));
		}

		private IEnumerator _PushEffect(string key, Transform effect, float delay)
		{
			yield return new WaitForSeconds(delay);
			__PushEffect(key, effect);
		}

		private void __PushEffect(string key, Transform effect)
		{
			effect.SetParent(_transform);
			effect.gameObject.SetActive(false);
			_dicEffects[key].Push(effect);
		}

		public CharHandler GetCharHandler(BattleEnum.Enum_CharSide side, int charId)
		{
			return BattleEnum.Enum_CharSide.Mine == side ? _dicCharMinePool[charId] : _dicCharEnemyPool[charId];
		}

		#region --小怪相关------------------------------------------------------------------
		public void SetMonsterId()
		{
			string strIds = SectionData.GetMonsterSpawns(BattleManager.m_iSectionID, BattleManager.m_eBattleType);
			if(string.Empty == strIds)
				return;

			string[] monsterInfos = strIds.Split('|');
			string[] ids;
			for(int i = 0; i < monsterInfos.Length; ++i)
			{
				ids = monsterInfos[i].Split(';');
				int monsterId = int.Parse(ids[0]);
				if(!m_lstMonsterID.Contains(monsterId))
				{
					m_lstMonsterID.Add(monsterId);
					_dicMonsterPool.Add(monsterId, new Stack<CharHandler>());
					//PreloadBulletMonster(charId);
				}
			}
		}

		private CharHandler LoaderMonster(Object obj, int charId)
		{
			Transform tempTrans;

			GameObject charGo = Instantiate(obj) as GameObject;
			tempTrans = charGo.transform;
			tempTrans.SetParent(_transform);
			tempTrans.position = PositionMgr.vecHidePos;

			GameObject handlerGo = new GameObject("Handler");   //把其他代码和CharAnimCallback分开放
			tempTrans = handlerGo.transform;
			tempTrans.SetParent(charGo.transform, false);
			tempTrans.localPosition = Vector3.zero;

			CharHandler ch = handlerGo.AddComponent<CharHandler>();
			ch.Init(charId, BattleEnum.Enum_CharSide.Enemy);
			return ch;
		}

		public void PushMonsterHandler(int monsterId, CharHandler charHandler)
		{
			_dicMonsterPool[monsterId].Push(charHandler);
		}

		public CharHandler PopMonsterHandler(int monsterId)
		{
			if(_dicMonsterPool[monsterId].Count > 0)
				return _dicMonsterPool[monsterId].Pop();
			else
			{
				Object obj = ResourceLoader.LoadAssets(_monsterConfig.GetValue(monsterId.ToString(), "PrefabPath"));
				CharHandler ch = LoaderMonster(obj, monsterId);
				return ch;
			}
		}
		#endregion

		#region --子弹相关------------------------------------------------------------------
		private void PreloadBulletHero(CharHandler handler)
		{
			if(BattleEnum.Enum_AttackType.Distant == handler.m_CharData.m_eAtkType)
			{
				string bulletPath = string.Format("Effect/Prefabs_Character/Hero{0}/Hero{0}_Fly", handler.m_CharData.m_iCharID);
				_PreloadBullet(handler.m_CharData.m_iCharID, bulletPath, _dicHeroBulletPool);
			}
		}

		private void PreloadBulletMonster(int monsterId)
		{
			string bulletPath = _monsterConfig.GetValue(monsterId.ToString(), "BulletPath");

			if(string.Empty != bulletPath)
				_PreloadBullet(monsterId, bulletPath, _dicMonsterBulletPool);
		}

		private void _PreloadBullet(int charId, string bulletPath, Dictionary<int, Stack<BulletBase>> dicBullet)
		{
			GameObject go;
			if(!dicBullet.ContainsKey(charId))
			{
				go = ResourceLoader.LoadAssetAndInstantiate(bulletPath, _transform, PositionMgr.vecHidePos);
				go.SetActive(false);
				BulletTrace bullet = go.AddComponent<BulletTraceEx>();
				dicBullet.Add(charId, new Stack<BulletBase>());
				dicBullet[charId].Push(bullet);
			}

			go = dicBullet[charId].Pop().gameObject;
			GameObject clone;
			for(int j = 0; j < 5; ++j)
			{
				clone = Instantiate(go, _transform) as GameObject;
				dicBullet[charId].Push(clone.GetComponent<BulletTraceEx>());
			}
		}

		public void PushBullet(CharHandler charHandler, BulletBase bullet)
		{
			CharData charData = charHandler.m_CharData;
			if(BattleEnum.Enum_CharType.Monster == charData.m_eType)
				_dicMonsterBulletPool[charData.m_iCharID].Push(bullet);
			else
				_dicHeroBulletPool[charData.m_iCharID].Push(bullet);
		}

		public BulletBase PopBullet(CharHandler charHandler)
		{
			CharData charData = charHandler.m_CharData;
			Dictionary<int, Stack<BulletBase>> dicBullet = BattleEnum.Enum_CharType.Monster == charData.m_eType ? _dicMonsterBulletPool : _dicHeroBulletPool;
			if(1 == dicBullet[charData.m_iCharID].Count)
			{
				BulletBase bullet = dicBullet[charData.m_iCharID].Peek();
				GameObject clone = Instantiate(bullet.gameObject, _transform) as GameObject;
				return clone.GetComponent<BulletBase>();
			}
			else
				return dicBullet[charData.m_iCharID].Pop();

		}
		#endregion
	}
}
