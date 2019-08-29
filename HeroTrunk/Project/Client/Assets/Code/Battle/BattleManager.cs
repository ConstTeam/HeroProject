using UnityEngine;

namespace MS
{
	public class BattleManager : MonoBehaviour
	{
		public Camera BattleCam;
		public Transform BattleRootTrans;
		public GameObject BattlePoolGo;


		[HideInInspector]
		public BattleScenePool m_BattlePoor;

		[HideInInspector]
		public BattleCharInScene m_CharInScene;

		private BattleSceneBase _battleScene;

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

			m_CharInScene	= new BattleCharInScene();
			_battleScene	= new BattleSceneNormal();
		}

		private void Start()
		{
			Load();
		}

		private void Load()
		{
			m_BattlePoor = BattlePoolGo.AddComponent<BattleScenePool>();
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
	}
}
