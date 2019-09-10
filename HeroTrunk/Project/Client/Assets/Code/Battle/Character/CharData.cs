using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MS
{
	public class CharData : MonoBehaviour
	{
		public delegate void MPChangeCbFunc(float curMp, float maxMp);
		public delegate void HPChangeCbFunc(float curHp, float maxHp);

		public MPChangeCbFunc FuncMPChangeCb;
		public HPChangeCbFunc FuncHPChangeCb;

		public BattleEnum.Enum_CharSide		m_eSide;
		public BattleEnum.Enum_CharState	m_eState;
		public BattleEnum.Enum_CharType		m_eType;
		public BattleEnum.Enum_AttackType	m_eAtkType;

		public int		m_iCharID			= 0;
		public string	m_sHeroName			= "";
		public float	m_fMoveSpeed		= 0f;
		public float	m_fSlideSpeed		= 50f;
		public float	m_fRotationSpeed	= 50f;
		public float	m_fBodyRange		= 0.8f;
		public float	m_fAtkRange			= 0.8f;
		public float	m_fAtkRadian		= 0.0f;		//攻击弧度
		public int		m_iAtkCount			= 0;		//普攻动作个数
		public float	m_fAtkX				= 1;		//攻击系数

		public float	m_fOriHP			= 0f;		//初始血量
		public float	m_fOriAtk			= 0f;		//初始攻击
		public float	m_fOriDef			= 0f;		//初始防御

		//-----------------------------------------------------------------------------------------
		public int		m_iCurSkillID		= 0;		//正在释放的技能
		//-----------------------------------------------------------------------------------------

		private ConfigRow _heroRow;

		//技能ID
		[SerializeField] private int[] _skillIDs = new int[5];
		public int[] SkillIDs
		{
			get { return _skillIDs; }
		}

		private Dictionary<int, int> _skillLevels = new Dictionary<int, int>();
		public int GetSkillLv(int skillId)
		{
			return _skillLevels[skillId];
		}

		[SerializeField] private int _iCurLevel = 0;
		public int CurLevel
		{
			get { return _iCurLevel; }
			set { _iCurLevel = value; }
		}

		#region --攻防血魔治疗------------------------------------------------------------------------
		//当前血量
		[SerializeField] private float _fCurHP;
		public float CurHP
		{
			get { return _fCurHP; }
			set
			{
				_fCurHP = Mathf.Min(MaxHP, value);
				if(null != FuncHPChangeCb)
				{
					FuncHPChangeCb(_fCurHP, MaxHP);
				}
			}
		}

		//当前魔量
		[SerializeField] private float _fCurMP;
		public float CurMP
		{
			get { return _fCurMP; }
			set
			{
				_fCurMP = Mathf.Min(MaxMP, value);
				if(null != FuncMPChangeCb)
					FuncMPChangeCb(_fCurMP, MaxMP);
			}
		}

		//最大血量
		[SerializeField] private float _fMaxHP;
		public float MaxHP
		{
			get { return _fMaxHP; }
			set { _fMaxHP = value; }
		}

		//最大魔量
		[SerializeField] private float _fMaxMP;
		public float MaxMP
		{
			get { return _fMaxMP; }
			set { _fMaxMP = value; }
		}

		//当前攻击
		[SerializeField] private float _fCurAtk;
		public float CurAttack
		{
			get { return _fCurAtk; }
			set { _fCurAtk = Mathf.Max(0, value); }
		}

		//当前防御
		[SerializeField] private float _fCurDef;
		public float CurDefence
		{
			get { return _fCurDef; }
			set { _fCurDef = Mathf.Max(0, value); }
		}

		//治疗提升
		[SerializeField] private float _fCureUpRatio = 0;
		public float CureUpRatio
		{
			get { return _fCureUpRatio; }
			set { _fCureUpRatio = value; }
		}

		//治疗压制
		[SerializeField] private float _fCureDownRatio = 0;
		public float CureDownRatio
		{
			get { return _fCureDownRatio; }
			set { _fCureDownRatio = value; }
		}
		#endregion

		#region --三维属性----------------------------------------------------------------------------
		[SerializeField] private float[] _properties = new float[3];    //三维属性
		[SerializeField] private float[] _propertiesRatio = new float[3];   //三维附加率属性

		//武
		public float Force
		{
			get { return _properties[0]; }
			set { _properties[0] = Mathf.Max(0, value); }
		}

		//魂
		public float Soul
		{
			get { return _properties[1]; }
			set { _properties[1] = Mathf.Max(0, value); }
		}

		//法
		public float Magic
		{
			get { return _properties[2]; }
			set { _properties[2] = Mathf.Max(0, value); }
		}

		//武附加率
		public float ForceRatio
		{
			get { return 1f + _propertiesRatio[0]; }
			set { _propertiesRatio[0] = Mathf.Max(0, value); }
		}

		//魂附加率
		public float SoulRatio
		{
			get { return 1f + _propertiesRatio[1]; }
			set { _propertiesRatio[1] = Mathf.Max(0, value); }
		}

		//法附加率
		public float MagicRatio
		{
			get { return 1f + _propertiesRatio[2]; }
			set { _propertiesRatio[2] = Mathf.Max(0, value); }
		}
		#endregion

		#region --武功魂技法术提升免伤----------------------------------------------------------------
		[SerializeField] private float[] _propertiesAtkRatio = new float[3];    //武功魂技法术提升
		[SerializeField] private float[] _propertiesDefRatio = new float[3];    //武功魂技法术免伤

		//武功提升率
		public float ForceSkillAtkRatio
		{
			get { return _propertiesAtkRatio[0]; }
			set { _propertiesAtkRatio[0] = value; }
		}

		//魂技提升率
		public float SoulSkillAtkRatio
		{
			get { return _propertiesAtkRatio[1]; }
			set { _propertiesAtkRatio[1] = value; }
		}

		//法术提升率
		public float MagicSkillAtkRatio
		{
			get { return _propertiesAtkRatio[2]; }
			set { _propertiesAtkRatio[2] = value; }
		}


		//武功免伤率
		public float ForceSkillDefRatio
		{
			get { return _propertiesDefRatio[0]; }
			set { _propertiesDefRatio[0] = value; }
		}

		//魂技免伤率
		public float SoulSkillDefRatio
		{
			get { return _propertiesDefRatio[1]; }
			set { _propertiesDefRatio[1] = value; }
		}

		//法术免伤率
		public float MagicSkillDefRatio
		{
			get { return _propertiesDefRatio[2]; }
			set { _propertiesDefRatio[2] = value; }
		}
		#endregion

		#region --基础率------------------------------------------------------------------------------
		//暴击率
		[SerializeField] private float _fCriticalRatio = 0.0f;
		public float CriticalRatio
		{
			get { return _fCriticalRatio; }
			set { _fCriticalRatio = value; }
		}

		//格挡率
		[SerializeField] private float _fBlockRatio = 0.0f;
		public float BlockRatio
		{
			get { return _fBlockRatio; }
			set { _fBlockRatio = value; }
		}

		//抗晕率
		[SerializeField] private float _fStunDefRatio = 0.0f;
		public float StunDefRatio
		{
			get { return _fStunDefRatio; }
			set { _fStunDefRatio = value; }
		}

		//抗封率
		[SerializeField] private float _fForbidDefRatio = 0.0f;
		public float ForbidDefRatio
		{
			get { return _fForbidDefRatio; }
			set { _fForbidDefRatio = Mathf.Max(-1.0f, _fForbidDefRatio + value); }
		}
		#endregion

		public BuffFloat Rebound			= new BuffFloat();	//吸收伤害
		public BuffFloat Absorb				= new BuffFloat();	//反弹伤害
		public BuffFloat AbsorbHP			= new BuffFloat();	//吸血
		public BuffBool SkillForbid			= new BuffBool();	//禁用技能
		public BuffBool SoulSkillInvalid	= new BuffBool();	//魂技失效
		public BuffBool Stun				= new BuffBool();	//晕眩
		public BuffBool UnBeatable			= new BuffBool();	//无敌
		public BuffBool Concentrate			= new BuffBool();	//集火

		public void Init(int iIndex)
		{
			m_eState = BattleEnum.Enum_CharState.Idle;
			if(-1 != iIndex) //非小怪
			{
				SetConfigInfo();
				SetSkillInfo();
			}
		}

		private void SetConfigInfo()
		{
			_heroRow		= ConfigData.GetValue("Hero_Common", m_iCharID.ToString());
			m_fAtkRange		= float.Parse(_heroRow.GetValue("AttackRange"));
			m_fBodyRange	= float.Parse(_heroRow.GetValue("BodyRange"));
			m_fMoveSpeed	= float.Parse(_heroRow.GetValue("RunSpeed"));
			m_fAtkRadian	= float.Parse(_heroRow.GetValue("AttackRad"));
			m_iAtkCount		= int.Parse(_heroRow.GetValue("AttackCount"));
			m_fAtkX			= float.Parse(_heroRow.GetValue("AttackX"));
			m_sHeroName		= _heroRow.GetValue("HeroName");
			m_eAtkType		= "0" == _heroRow.GetValue("AttackType") ? BattleEnum.Enum_AttackType.Close : BattleEnum.Enum_AttackType.Distant;
		}

		private void SetSkillInfo()
		{
			//HeroInfo heroInfo = m_eSide == BattleEnum.Enum_CharSide.Mine ? FightSceneMgr.GetInst().GetMineHeroInfo(m_iCharID) : FightSceneMgr.GetInst().GetEnemyHeroInfo(m_iCharID);
			//int skillId = heroInfo.MainSkillID;
			//_skillIDs[0] = skillId;
			//_skillLevels.Add(skillId, heroInfo.Skills[1]);

			//List<int> triggerIDs = heroInfo.GetActiveSkills();
			//for(int i = 0; i < triggerIDs.Count; ++i)
			//{
			//	_skillIDs[i + 1] = triggerIDs[i];
			//	_skillLevels.Add(triggerIDs[i], heroInfo.Skills[i + 2]);
			//}
		}

		public BattleEnum.Enum_CharSide GetOppositeSide()
		{
			return m_eSide == BattleEnum.Enum_CharSide.Mine ? BattleEnum.Enum_CharSide.Enemy : BattleEnum.Enum_CharSide.Mine;
		}

		public void SetCharData(BattleEnum.Enum_CharType type)
		{
			if(m_eSide == BattleEnum.Enum_CharSide.Mine)
				SetCharDataMine(type);
			else
				SetCharDataEnemy(type);
		}

		private void SetCharDataMine(BattleEnum.Enum_CharType type)
		{
			//HeroInfo heroInfo = FightSceneMgr.GetInst().GetMineHeroInfo(m_iCharID);
			//_SetCharData(heroInfo, type);
		}

		private void SetCharDataEnemy(BattleEnum.Enum_CharType type)
		{
			//HeroInfo heroInfo = FightSceneMgr.GetInst().GetEnemyHeroInfo(m_iCharID);
			//_SetCharData(heroInfo, type);
		}

		private void _SetCharData(HeroInfo heroInfo, BattleEnum.Enum_CharType type)
		{
			m_eType			= type;
			CurLevel		= heroInfo.Level;
			CurAttack		= heroInfo.Attack + heroInfo.AddAttack;
			CurDefence		= heroInfo.Defence + heroInfo.AddDefence;
			CriticalRatio	= heroInfo.CriticalRatio;
			BlockRatio		= heroInfo.BlockRatio;
			Force			= heroInfo.Force + heroInfo.AddForce;
			Magic			= heroInfo.Magic + heroInfo.AddMagic;
			Soul			= heroInfo.Soul + heroInfo.AddSoul;
			MaxHP			= heroInfo.HP + heroInfo.AddHP;
			CurHP			= MaxHP;
			MaxMP			= heroInfo.MaxPower;
			CurMP			= MaxMP;
			m_fOriHP		= MaxHP;
			m_fOriDef		= CurDefence;
			m_fOriAtk		= CurAttack;
		}
	}
}