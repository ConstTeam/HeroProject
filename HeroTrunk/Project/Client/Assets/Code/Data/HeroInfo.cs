namespace MS
{
	public class HeroInfo
	{
		public int		ID					{ get; set; }
		public int		Level				{ get; set; }
		public float	Attack				{ get; set; }
		public float	AddAttack			{ get; set; }
		public float	Defence				{ get; set; }
		public float	AddDefence			{ get; set; }
		public float	Force				{ get; set; }
		public float	AddForce			{ get; set; }
		public float	Resourcefulness		{ get; set; }
		public float	AddResourcefulness	{ get; set; }
		public float	RuleWorld			{ get; set; }
		public float	AddRuleWorld		{ get; set; }
		public float	Polity				{ get; set; }
		public float	AddPolity			{ get; set; }
		public float	Charm				{ get; set; }
		public float	AddCharm			{ get; set; }
		public float	HP					{ get; set; }
		public float	AddHP				{ get; set; }
		public float	MaxPower			{ get; set; }
		public float	CriticalRatio		{ get; set; }
		public float	BlockRatio			{ get; set; }
		public int[]	SkillIDs			{ get; set; }
		public int[]	SkillLevels			{ get; set; }		

		public HeroInfo(int charId)
		{
			ID					= charId;
			ConfigRow row		= ConfigData.GetValue("Hero_Common", charId.ToString());
			Level				= 1;
			Attack				= float.Parse(row.GetValue("Attack"));
			Defence				= float.Parse(row.GetValue("Defence"));
			Force				= float.Parse(row.GetValue("Force"));
			Resourcefulness		= float.Parse(row.GetValue("Resourcefulness"));
			RuleWorld			= float.Parse(row.GetValue("RuleWorld"));
			Polity				= float.Parse(row.GetValue("Polity"));
			Charm				= float.Parse(row.GetValue("Charm"));
			HP					= float.Parse(row.GetValue("Hp"));
			MaxPower			= float.Parse(row.GetValue("MaxPower"));
			CriticalRatio		= float.Parse(row.GetValue("CriticalRatio"));
			BlockRatio			= float.Parse(row.GetValue("BlockRatio"));

			SkillIDs		= new int[5];
			SkillLevels		= new int[5];
			for(int i = 0; i < 5; ++i)
			{
				SkillIDs[i] = charId * 100 + i + 1;
				SkillLevels[i] = 1;
			}
		}
	}
}
