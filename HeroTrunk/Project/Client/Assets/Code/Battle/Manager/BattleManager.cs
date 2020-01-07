using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MS
{
	public class BattleManager : MonoBehaviour
	{
		public Camera				UICam;
		public Camera				BattleCam;
		public Transform			BattleRootTrans;
		public Transform			BattlePoolTrans;
		public Transform			HUDParentTran;
		public Transform			HUDSkillParentTran;
		public GameObject			HUDTextRes;

		public BattleScene			m_BattleScene;
		public BattleCharInScene	m_CharInScene;
		public BattleTriggerManager m_TriggerManager;

		public MPData				m_MPData;
		public int					m_iEnemyPlayerLevel;

		private GameObject			_gameObject;
		

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
			m_BattleScene		= _gameObject.AddComponent<BattleScene>();
			m_MPData			= new MPData("Power1");
			BattlePoolTrans.gameObject.AddComponent<BattleScenePool>();

			HUDTextMgr.GetInst().HUDParent = HUDParentTran;
			HUDTextMgr.GetInst().HUDSkillParent = HUDSkillParentTran;
		}

		private void Start()
		{
			if(PlayerInfo.GuideStep == 0)
				GuidePanel.GetInst().ShowPanel();
			else
				m_BattleScene.OnBattleInit();
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

		public CharHandler AddHero(int heroId, int heroLv, int heroIndex)
		{
			CharHandler h = SpawnHandler.GetInst().CreateHeroM(heroId, heroIndex);
			h.EnableChar();
			BattleHeroListPanel.GetInst().InsertBattleHero(heroId, heroLv, heroIndex);
			return h;
		}

		public void HeroLevelUp(int heroIndex, int heroLv)
		{
			BattleHeroListPanel.GetInst().SetHeroLevel(heroIndex, heroLv);
		}

		public HeroInfo GetHeroInfoMine(int heroId)
		{
			return HeroAll.GetHeroInfo(heroId);
		}

		public HeroInfo GetHeroInfoEnemy(int heroId)
		{
			return null;
		}

		public HeroInfo GetHeroInfo(BattleEnum.Enum_CharSide side, int charId)
		{
			return side == BattleEnum.Enum_CharSide.Mine ? GetHeroInfoMine(charId) : GetHeroInfoEnemy(charId);
		}

		public void TapLightning()
		{
			List<CharHandler> lst = m_CharInScene.GetAllMonster();
			if(lst.Count > 0)
			{
				CharHandler monster = lst[Random.Range(0, lst.Count)];
				if(monster.m_CharData.m_eState != BattleEnum.Enum_CharState.PreBorn)
				{
					ResourceMgr.GetInst().PopTapLightning(monster.m_ParentTrans.position);
					monster.BeHit(10f, null);
				}
			}
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

		public void IsWaveEnd()
		{
			if(0 == m_CharInScene.m_listMonster.Count)
				SpawnHandler.GetInst().ReleaseNextWave();	
		}

		#region--静态------
		public static string m_sAddSceneName;
		public static string m_sSpawnName;

		public static void EnterBattle()
		{
			m_sAddSceneName = "BattleNormal";//SectionData.GetSceneName(m_iSectionID, m_eBattleType);
			m_sSpawnName = "Spawn/Spawn001";//SectionData.GetSceneSpawn(m_iSectionID, m_eBattleType);
			SceneLoaderMain.GetInst().LoadBattleScene();
		}
		#endregion
	}
}
