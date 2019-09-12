namespace MS
{
	public class SectionData
	{
		private static string GetConfigName(BattleEnum.Enum_BattleType battleType)
		{
			string cfgName = string.Empty;
			switch(battleType)
			{
				case BattleEnum.Enum_BattleType.Normal:
					cfgName = "City_Common";
					break;
			}
			return cfgName;
		}

		private static string GetLanConfigName(BattleEnum.Enum_BattleType battleType)
		{
			string cfgName = string.Empty;
			switch(battleType)
			{
				case BattleEnum.Enum_BattleType.Normal:
					cfgName = "Lan_City_Common";
					break;
			}
			return cfgName;
		}

		public static ConfigRow GetSectionRow(int iSectionId, BattleEnum.Enum_BattleType battleType)
		{
			string cfgName = GetConfigName(battleType);
			return ConfigData.GetValue(cfgName, iSectionId.ToString());
		}

		public static string GetSectionName(int iSectionId, BattleEnum.Enum_BattleType battleType)
		{
			string cfgName = GetLanConfigName(battleType);
			return ConfigData.GetValue(cfgName, iSectionId.ToString(), "PassName");
		}

		public static string GetSectionDescription(int iSectionId, BattleEnum.Enum_BattleType battleType)
		{
			string cfgName = GetLanConfigName(battleType);
			return ConfigData.GetValue(cfgName, iSectionId.ToString(), "PassDes");
		}

		public static string GetSectionType(int iSectionId, BattleEnum.Enum_BattleType battleType)
		{
			string cfgName = GetConfigName(battleType);
			return ConfigData.GetValue(cfgName, iSectionId.ToString(), "PassType");
		}

		public static string GetMonsterSpawns(int iSectionId, BattleEnum.Enum_BattleType battleType)
		{
			//string cfgName = GetConfigName(battleType);
			return "2006;3|2016;1";//ConfigData.GetValue(cfgName, iSectionId.ToString(), "MonsterSpawns");
		}

		public static string GetBossSpawns(int iSectionId, BattleEnum.Enum_BattleType battleType)
		{
			string cfgName = GetConfigName(battleType);
			return ConfigData.GetValue(cfgName, iSectionId.ToString(), "BossSpawns");
		}

		public static string GetSceneName(int iSectionId, BattleEnum.Enum_BattleType battleType)
		{
			string cfgName = GetConfigName(battleType);
			return ConfigData.GetValue(cfgName, iSectionId.ToString(), "BattleScene");
		}

		public static string GetSceneSpawn(int iSectionId, BattleEnum.Enum_BattleType battleType)
		{
			string cfgName = GetConfigName(battleType);
			return ConfigData.GetValue(cfgName, iSectionId.ToString(), "SpawnPath");
		}

		public static string GetExpGain(int iSectionId, BattleEnum.Enum_BattleType battleType)
		{
			string cfgName = GetConfigName(battleType);
			return ConfigData.GetValue(cfgName, iSectionId.ToString(), "ExpGain");
		}

		public static string GetLevel(int iSectionId, BattleEnum.Enum_BattleType battleType)
		{
			string cfgName = GetConfigName(battleType);
			return ConfigData.GetValue(cfgName, iSectionId.ToString(), "Level");
		}

		public static string GetQuality(int iSectionId, BattleEnum.Enum_BattleType battleType)
		{
			string cfgName = GetConfigName(battleType);
			return ConfigData.GetValue(cfgName, iSectionId.ToString(), "Quality");
		}

		public static string GetStar(int iSectionId, BattleEnum.Enum_BattleType battleType)
		{
			string cfgName = GetConfigName(battleType);
			return ConfigData.GetValue(cfgName, iSectionId.ToString(), "StarLevel");
		}
	
		public static string GetLimitCount(int iSectionId, BattleEnum.Enum_BattleType battleType)
		{
			string cfgName = GetConfigName(battleType);
			return ConfigData.GetValue(cfgName, iSectionId.ToString(), "LimitCount");
		}
		public static string GeHardInfoId(int iSectionId, BattleEnum.Enum_BattleType battleType)
		{
			string cfgName = GetConfigName(battleType);
			return ConfigData.GetValue(cfgName, iSectionId.ToString(), "HardInfoId");
		}
		public static string GetGainDescription(int iSectionId, BattleEnum.Enum_BattleType battleType)
		{
			string cfgName = GetLanConfigName(battleType);
			return ConfigData.GetValue(cfgName, iSectionId.ToString(), "Description");
		}

		public static string GetLimitQuest(int iSectionId, BattleEnum.Enum_BattleType battleType)
		{
			string cfgName = GetConfigName(battleType);
			return ConfigData.GetValue(cfgName, iSectionId.ToString(), "LimitQuest");
		}
	}
}
