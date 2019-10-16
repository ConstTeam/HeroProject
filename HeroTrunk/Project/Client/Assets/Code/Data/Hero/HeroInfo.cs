namespace MS
{
	public class HeroInfo
	{
		public int		ID				{ get; set; }
		public int		Star			{ get; set; }
		public int		MaxPower		{ get; set; }
		public float	AddAttack		{ get; set; }
		public float	AddDefence		{ get; set; }
		public float	AddForce		{ get; set; }
		public float	AddStrategy		{ get; set; }
		public float	AddRule			{ get; set; }
		public float	AddPolity		{ get; set; }
		public float	AddCharm		{ get; set; }
		public float	AddHP			{ get; set; }
		
		public int[]	SkillIDs		{ get; set; }
		public int[]	SkillLevels		{ get; set; }		

		public float[]	MainProperty	{ get; set; }

		public HeroInfo(int heroId, int heroStar, int maxPower, float[] mainProperty)
		{
			ID				= heroId;
			Star			= heroStar;
			MaxPower		= maxPower;
			MainProperty	= mainProperty;

			SkillIDs		= new int[5];
			SkillLevels		= new int[5];
			for(int i = 0; i < 5; ++i)
			{
				SkillIDs[i] = heroId * 100 + i + 1;
				SkillLevels[i] = 1;
			}
		}
	}
}
