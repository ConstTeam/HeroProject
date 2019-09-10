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
			SetMyHeroIDs();
			//SetEnemyHeroIDs();
			//SetMonsterIDs(FightSceneMgr.m_eBattleType);
		}

		private void Start()
		{
			LoadHero();
		}

		private void SetMyHeroIDs()
		{
			m_lstCharMineID = BattleManager.GetInst().GetHeroIdsMine();
		}

		private void LoadHero()
		{
			List<string> charIds = GroupInfo.m_lstNormalGroup;

			GameObject charGo;
			GameObject handlerGo;
			string charId;
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
	}
}
