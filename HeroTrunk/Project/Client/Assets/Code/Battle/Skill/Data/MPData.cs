namespace MS
{
	public class MPData
	{
		//全局增魔
		public float m_fHPLosePercent;          //损血百分比
		public float m_fAddMPFromHPLose;        //损血m_fHPLosePercent的增魔值
		public float m_fAddMPFromHeroDead;      //杀将增魔值
		public float m_fAddMPFromMonsterDead;   //杀怪增魔值

		//单人增魔
		public float m_fHPLosePercentSingle;    //损血百分比
		public float m_fAddMPFromHPLoseSingle;  //损血m_fHPLosePercentSingle的增魔值

		public MPData(string id)
		{
			ConfigRow row				= ConfigData.GetValue("SkillPower_Client", id);
			m_fHPLosePercent			= float.Parse(row.GetValue("Percent"));
			m_fAddMPFromHPLose			= float.Parse(row.GetValue("LoseHp"));
			m_fAddMPFromHeroDead		= float.Parse(row.GetValue("HeroDead"));
			m_fAddMPFromMonsterDead		= float.Parse(row.GetValue("MonsterDead"));
			m_fHPLosePercentSingle		= float.Parse(row.GetValue("PercentSingle"));
			m_fAddMPFromHPLoseSingle	= float.Parse(row.GetValue("LoseHpSingle"));
		}
	}
}
