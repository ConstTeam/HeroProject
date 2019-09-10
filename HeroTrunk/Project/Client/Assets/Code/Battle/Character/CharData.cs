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
	}
}
