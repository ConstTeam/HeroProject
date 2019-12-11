namespace MS
{
	public class BattleEnum
	{
		public enum Enum_CharSide
		{
			Mine,
			Enemy
		}

		public enum Enum_CharState
		{
			Idle,
			Run,
			Attack,
			Dead,
			DoSkill,
			Dizzy,
			PreBorn,
			Born,
			Slide
		}

		public enum Enum_CharType
		{
			General,
			Official,
			Monster
		}

		public enum Enum_AttackType
		{
			Close,
			Distant
		}

		public enum Enum_BattleType
		{
			Normal,
			Elite
		}
	}
}
