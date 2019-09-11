using System.Collections.Generic;
using UnityEngine;

namespace MS
{
	public class BattleManager : MonoBehaviour
	{
		public Camera				UICam;
		public Camera				BattleCam;
		public Transform			BattleRootTrans;
		public GameObject			BattlePoolGo;
		public Transform			HeadUIParentTran;
		public GameObject			HUDTextRes;

		public BattleCharInScene	m_CharInScene;
		public BattleTriggerManager m_TriggerManager;

		public MPData				m_MPData;
		public int					m_iEnemyPlayerLevel;

		private GameObject			_gameObject;
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
			_gameObject			= gameObject;
			BattleCam.rect		= ApplicationConst.sceneCamRect;
			_inst				= this;

			SceneLoader.LoadAddScene(m_sAddSceneName);

			_gameObject.AddComponent<HUDTextMgr>();
			_gameObject.AddComponent<BattleSceneTimer>();

			m_CharInScene		= new BattleCharInScene();
			m_TriggerManager	= new BattleTriggerManager();
			_battleScene		= new BattleSceneNormal();

			m_MPData			= new MPData("Power1");

			BattlePoolGo.AddComponent<BattleScenePool>();

			BattleScenePool.GetInst().SetHeroIdMine();
			//SetEnemyHeroIDs();
			BattleScenePool.GetInst().SetMonsterId();
		}

		private void Start()
		{
			ResourceLoader.LoadAssetAndInstantiate(m_sSpawnName);
			BattleScenePool.GetInst().LoadHero(BattleEnum.Enum_CharSide.Mine);
			_battleScene.OnBattleInit();
			Invoke("BattleStart", 3);
		}

		public void BattleStart()
		{
			BattleCam.enabled = true;
			BattleSceneTimer.GetInst().BeginTimer();
			SpawnMgr.GetInst().Begin();
			_battleScene.OnBattleStart();
		}

		public List<int> GetHeroIdsMine()
		{
			return new List<int>{ 1006, 1007, 1008 };
		}

		public List<int> GetHeroIdsEnemy()
		{
			return new List<int> { 1006, 1007, 1008 };
		}

		public MPData GetMPData()
		{
			return m_MPData;
		}

		public CharHandler GetMainHero()
		{
			if(m_CharInScene.m_listGeneralMine.Count > 0)
				return m_CharInScene.m_listGeneralMine[0];

			return null;
		}

		public CharHandler GetMainEnemy()
		{
			if(m_CharInScene.m_listGeneralEnemy.Count > 0)
				return m_CharInScene.m_listGeneralEnemy[0];

			return null;
		}

		public CharHandler GetMainHeroBySide(BattleEnum.Enum_CharSide side)
		{
			return BattleEnum.Enum_CharSide.Mine == side ? GetMainHero() : GetMainEnemy();
		}

		public HeroInfo GetHeroInfoMine(int charId)
		{
			return new HeroInfo(charId);
		}

		public HeroInfo GetHeroInfoEnemy(int charId)
		{
			return new HeroInfo(charId);
		}

		#region --被动技能相关-------------------------------------------------------------
		public void AddBattleSecTrigger(int sec, CharHandler charHandler)
		{
			m_TriggerManager.AddBattleSecTrigger(sec, charHandler);
		}

		public void AddFinalEnemyTrigger(int sec, CharHandler charHandler)
		{
			m_TriggerManager.AddFinalEnemyTrigger(sec, charHandler);
		}

		public void AddHPTrigger(int hpPercent, CharHandler charHandler, SkillEnum.TriggerType triggerType)
		{
			m_TriggerManager.AddHPTrigger(hpPercent, charHandler, triggerType);
		}

		public void AddDeadTrigger(CharHandler charHandler)
		{
			m_TriggerManager.AddDeadTrigger(charHandler);
		}

		public void FinalEnemyBorned(List<CharHandler> lstFinalEnemy)
		{
			m_TriggerManager.FinalEnemyBorned();
		}
		#endregion

		#region--静态------
		public static int m_iSectionID;
		public static string m_sAddSceneName;
		public static string m_sSpawnName;
		public static BattleEnum.Enum_BattleType m_eBattleType;

		public static void EnterBattle()
		{
			m_sAddSceneName = "BattleNormal";//SectionData.GetSceneName(m_iSectionID, m_eBattleType);
			m_sSpawnName = "Spawn/Spawn001";//SectionData.GetSceneSpawn(m_iSectionID, m_eBattleType);
			SceneLoaderMain.GetInst().LoadBattleScene();
		}
		#endregion
	}
}
