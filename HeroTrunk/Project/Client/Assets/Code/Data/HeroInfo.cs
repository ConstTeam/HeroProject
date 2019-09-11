using System.Collections.Generic;

namespace MS
{
	public class HeroInfo
	{
		public int			ID				{ get; set; }
		public int			Level			{ get; set; }
		public float		Attack			{ get; set; }
		public float		AddAttack		{ get; set; }
		public float		Defence			{ get; set; }
		public float		AddDefence		{ get; set; }
		public float		Force			{ get; set; }
		public float		AddForce		{ get; set; }
		public float		Magic			{ get; set; }
		public float		AddMagic		{ get; set; }
		public float		Soul			{ get; set; }
		public float		AddSoul			{ get; set; }
		public float		HP				{ get; set; }
		public float		AddHP			{ get; set; }
		public float		MaxPower		{ get; set; }
		public float		CriticalRatio	{ get; set; }
		public float		BlockRatio		{ get; set; }
		public int[]		SkillIDs		{ get; set; }
		public int[]		SkillLevels		{ get; set; }		

		public HeroInfo(int charId)
		{
			ID				= charId;
			ConfigRow row	= ConfigData.GetValue("Hero_Common", charId.ToString());
			Level			= 1;
			Attack			= float.Parse(row.GetValue("Attack"));
			Defence			= float.Parse(row.GetValue("Defence"));
			Force			= float.Parse(row.GetValue("Force"));
			Magic			= float.Parse(row.GetValue("Resourcefulness"));
			Soul			= float.Parse(row.GetValue("RuleWorld"));
			HP				= float.Parse(row.GetValue("Hp"));
			MaxPower		= float.Parse(row.GetValue("MaxPower"));
			CriticalRatio	= float.Parse(row.GetValue("CriticalRatio"));
			BlockRatio		= float.Parse(row.GetValue("BlockRatio"));

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
