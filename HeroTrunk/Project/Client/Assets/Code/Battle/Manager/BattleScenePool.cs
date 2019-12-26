using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MS
{
	public class BattleScenePool : MonoBehaviour
	{
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

		public CharHandler LoadHero(BattleEnum.Enum_CharSide side, int heroId, int heroIndex)
		{
			GameObject charGo = ResourceLoader.LoadAssetAndInstantiate(string.Format("Character/Hero{0}_Stand", heroId.ToString()), _transform, PositionMgr.vecHidePos);
			GameObject handlerGo = new GameObject("Handler");  //把其他代码和CharAnimCallback分开放
			handlerGo.transform.SetParent(charGo.transform);
			handlerGo.transform.localPosition = Vector3.zero;
			CharHandler charHandler = handlerGo.AddComponent<CharHandler>();
			charHandler.Init(heroId, side, heroIndex);
			//PreloadBulletHero(charHandler);
			return charHandler;
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
		private CharHandler LoaderMonster(GameObject go, int charId)
		{
			GameObject handlerGo = new GameObject("Handler");   //把其他代码和CharAnimCallback分开放
			Transform tempTrans = handlerGo.transform;
			tempTrans.SetParent(go.transform, false);
			tempTrans.localPosition = Vector3.zero;

			CharHandler ch = handlerGo.AddComponent<CharHandler>();
			ch.Init(charId, BattleEnum.Enum_CharSide.Enemy);
			return ch;
		}

		public void PushMonsterHandler(CharHandler charHandler)
		{
			charHandler.Hide();
			_dicMonsterPool[charHandler.m_CharData.m_iCharID].Push(charHandler);
		}

		public CharHandler PopMonsterHandler(int monsterId)
		{
			if(!_dicMonsterPool.ContainsKey(monsterId))
				_dicMonsterPool.Add(monsterId, new Stack<CharHandler>());

			if(_dicMonsterPool[monsterId].Count > 0)
				return _dicMonsterPool[monsterId].Pop();
			else
			{
				GameObject go = ResourceLoader.LoadAssetAndInstantiate(_monsterConfig.GetValue(monsterId.ToString(), "PrefabPath"), _transform, PositionMgr.vecHidePos);
				CharHandler ch = LoaderMonster(go, monsterId);
				return ch;
			}
		}
		#endregion

		#region --子弹相关------------------------------------------------------------------
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

			if(!dicBullet.ContainsKey(charData.m_iCharID))
				dicBullet.Add(charData.m_iCharID, new Stack<BulletBase>());

			if(dicBullet[charData.m_iCharID].Count > 0)
				return dicBullet[charData.m_iCharID].Pop();
			else
			{
				string bulletPath = BattleEnum.Enum_CharType.Monster == charData.m_eType ? _monsterConfig.GetValue(charData.m_iCharID.ToString(), "BulletPath") : string.Format("Effect/Prefabs_Character/Hero{0}/Hero{0}_Fly", charData.m_iCharID);
				GameObject go = ResourceLoader.LoadAssetAndInstantiate(bulletPath, _transform, PositionMgr.vecHidePos);
				return go.AddComponent<BulletTraceEx>();
			}		
		}
		#endregion
	}
}
