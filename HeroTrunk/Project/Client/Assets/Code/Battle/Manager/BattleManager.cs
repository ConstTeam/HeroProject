using System.Collections.Generic;
using UnityEngine;

namespace MS
{
	public class BattleManager : MonoBehaviour
	{
		public Camera				BattleCam;
		public Transform			BattleRootTrans;
		public GameObject			BattlePoolGo;

		public BattleScenePool		m_ScenePoor;
		public BattleCharInScene	m_CharInScene;
		public BattleTriggerManager m_TriggerManager;
		public BattleSceneTimer		m_SceneTimer;

		public MPData m_MPData;

		private BattleSceneBase		_battleScene;

		private static BattleManager _inst;
		public static BattleManager GetInst()
		{
			return _inst;
		}

		private void OnDestroy()
		{
			_inst = null;
		}

		private void Awake()
		{
			BattleCam.rect = ApplicationConst.sceneCamRect;
			_inst = this;

			m_SceneTimer		= gameObject.AddComponent<BattleSceneTimer>();
			m_CharInScene		= new BattleCharInScene();
			m_TriggerManager	= new BattleTriggerManager(m_SceneTimer);
			_battleScene		= new BattleSceneNormal();

			m_MPData			= new MPData("Power1");
		}

		private void Start()
		{
			Load();
		}

		private void Load()
		{
			m_ScenePoor = BattlePoolGo.AddComponent<BattleScenePool>();
		}

		public void BattleInit()
		{

		}

		public CharHandler GetMainHero()
		{
			if(m_CharInScene.m_listGeneralMine.Count > 0)
				return m_CharInScene.m_listGeneralMine[0];

			return null;
		}

		public List<int> GetHeroIdsMine()
		{
			return new List<int>{ 1006, 1007, 1008 };
		}

		public MPData GetMPData()
		{
			return m_MPData;
		}
	}
}
