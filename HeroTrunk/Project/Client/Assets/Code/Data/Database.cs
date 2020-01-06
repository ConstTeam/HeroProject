using UnityEngine;

namespace MS
{
	public class Database : MonoBehaviour
	{
		private static Database _inst;
		public static Database GetInst()
		{
			return _inst;
		}

		private void Awake()
		{
			_inst = this;
		}

		private void OnDestroy()
		{
			_inst = null;
		}

		public void OnLogin(string account)
		{
			string playerId = ES3.FileExists("Account") && ES3.KeyExists(account, "Account") ? ES3.Load<string>(account, "Account") : AddPlayer(account);
			SyncPlayerInfo(playerId);

			string dirPath = string.Format("{0}/HeroInfo", playerId);
			string[] arr = ES3.GetFiles(dirPath);
			for(int i = 0; i < arr.Length; ++i)
				SyncHero(playerId, int.Parse(arr[i]));

			ByteBuffer data = new ByteBuffer();
			data.writeByte(2);
			data.writeByte(PlayerService.PLAYER_LOGIN_END);
			ServiceManager.PostMessageShortEx(data);
		}

		public string AddPlayer(string account)
		{
			int iPlayerId = ES3.KeyExists("IncrementPlayerID") ? ES3.Load<int>("IncrementPlayerID") + 1 : 1;
			ES3.Save<int>("IncrementPlayerID", iPlayerId);

			string sPlayerId = iPlayerId.ToString();
			ES3.Save<string>(account, sPlayerId, "Account");
			string filePath = string.Format("{0}/PlayerInfo", sPlayerId);
			ES3.Save<string>("PlayerName", sPlayerId, filePath);
			ES3.Save<int>("GuideStep", 0, filePath);

			AddHero(sPlayerId, 1006);
			AddHero(sPlayerId, 1020);

			filePath = string.Format("{0}/NormalBattle", sPlayerId);
			ES3.Save<int>("Coin", 0, filePath);
			ES3.Save<int>("CurSpawn", 0, filePath);
			ES3.Save<int>("CurWave", 0, filePath);
			ES3.Save<int>("HighestSpawn", 0, filePath);

			return sPlayerId;
		}

		public void AddHero(string playerId, int heroId)
		{
			ConfigRow row = ConfigData.GetValue("Hero_Common", heroId.ToString());
			string filePath = string.Format("{0}/HeroInfo/{1}", playerId, heroId);
			ES3.Save<int>("ID",					heroId, filePath);
			ES3.Save<int>("Star",				0, filePath);
			ES3.Save<int>("MaxPower",			int.Parse(row.GetValue("MaxPower")), filePath);
			ES3.Save<float>("Attack",			float.Parse(row.GetValue("Attack")), filePath);
			ES3.Save<float>("Defence",			float.Parse(row.GetValue("Defence")), filePath);
			ES3.Save<float>("HP",				float.Parse(row.GetValue("Hp")), filePath);
			ES3.Save<float>("CriticalRatio",	float.Parse(row.GetValue("BlockRatio")), filePath);
			ES3.Save<float>("BlockRatio",		float.Parse(row.GetValue("BlockRatio")), filePath);
			ES3.Save<float>("Force",			float.Parse(row.GetValue("Force")), filePath);
			ES3.Save<float>("Strategy",			float.Parse(row.GetValue("Strategy")), filePath);
			ES3.Save<float>("Rule",				float.Parse(row.GetValue("Rule")), filePath);
			ES3.Save<float>("Polity",			float.Parse(row.GetValue("Polity")), filePath);
			ES3.Save<float>("Charm",			float.Parse(row.GetValue("Charm")), filePath);
		}
		
		private void SyncPlayerInfo(string playerId)
		{
			ByteBuffer data = new ByteBuffer();
			data.writeByte(2);
			data.writeByte(PlayerService.PLAYER_INFO);
			data.writeBoolean(true);
			data.writeUTF(playerId);
			string filePath = string.Format("{0}/PlayerInfo", playerId);
			data.writeUTF(ES3.Load<string>("PlayerName", filePath));
			data.writeByte(ES3.Load<int>("GuideStep", filePath));
			ServiceManager.PostMessageShortEx(data);
		}

		private void SyncHero(string playerId, int heroId)
		{
			ByteBuffer data = new ByteBuffer();
			data.writeByte(2);
			data.writeByte(PlayerService.PLAYER_HERO);
			string filePath = string.Format("{0}/HeroInfo/{1}", playerId, heroId);
			data.writeInt(ES3.Load<int>("ID", filePath));
			data.writeInt(ES3.Load<int>("Star", filePath));
			data.writeInt(ES3.Load<int>("MaxPower", filePath));
			data.writeInt(Mathf.FloorToInt(ES3.Load<float>("Attack", filePath) * 100));
			data.writeInt(Mathf.FloorToInt(ES3.Load<float>("Defence", filePath) * 100));
			data.writeInt(Mathf.FloorToInt(ES3.Load<float>("HP", filePath) * 100));
			data.writeInt(Mathf.FloorToInt(ES3.Load<float>("CriticalRatio", filePath) * 100));
			data.writeInt(Mathf.FloorToInt(ES3.Load<float>("BlockRatio", filePath) * 100));
			data.writeInt(Mathf.FloorToInt(ES3.Load<float>("Force", filePath) * 100));
			data.writeInt(Mathf.FloorToInt(ES3.Load<float>("Strategy", filePath) * 100));
			data.writeInt(Mathf.FloorToInt(ES3.Load<float>("Rule", filePath) * 100));
			data.writeInt(Mathf.FloorToInt(ES3.Load<float>("Polity", filePath) * 100));
			data.writeInt(Mathf.FloorToInt(ES3.Load<float>("Charm", filePath) * 100));
			ServiceManager.PostMessageShortEx(data);
		}

		public void SyncBattleInfo(string playerId)
		{
			string filePath = string.Format("{0}/NormalBattle", playerId);
			SyncCoin(ES3.Load<int>("Coin", filePath));
			SyncCurSpawn(ES3.Load<int>("CurSpawn", filePath));
			SyncHighestSpawn(ES3.Load<int>("HighestSpawn", filePath));

			int count = 0;
			for(int i = 0; i < 5; ++i)
			{
				if(ES3.KeyExists(string.Format("HeroID{0}", i), filePath))
					++count;
			}
			for(int i = 0; i < count; ++i)
				SyncBattleHero(playerId, i);

			ByteBuffer data = new ByteBuffer();
			data.writeByte(102);
			data.writeByte(BattleService.BATTLE_START);
			ServiceManager.PostMessageShortEx(data);

			SyncCurWave(ES3.Load<int>("CurWave", filePath));
		}

		#region --Guide------
		public void SetGuideStep(string playerId, int guideStep)
		{
			string filePath = string.Format("{0}/PlayerInfo", playerId);
			ES3.Save<int>("GuideStep", guideStep, filePath);
		}
		#endregion

		#region --Normal Battle------
		public void NormalBattleChangeCoin(string playerId, int v)
		{
			string filePath = string.Format("{0}/NormalBattle", playerId);
			int coin = ES3.Load<int>("Coin", filePath) + v;
			ES3.Save<int>("Coin", coin, filePath);
			SyncCoin(coin);
		}

		private void SyncCoin(int coin)
		{
			ByteBuffer data = new ByteBuffer();
			data.writeByte(102);
			data.writeByte(BattleService.BATTLE_COIN);
			data.writeInt(coin);
			ServiceManager.PostMessageShortEx(data);
		}

		public void NormalBattleSaveCurSpawn(string playerId, int curSpawn)
		{
			string filePath = string.Format("{0}/NormalBattle", playerId);
			ES3.Save<int>("CurSpawn", curSpawn, filePath);
			SyncCurSpawn(curSpawn);
			NormalBattleSaveCurWave(playerId, 0);

			filePath = string.Format("{0}/NormalBattle", playerId);
			if(curSpawn > ES3.Load<int>("HighestSpawn", filePath))
			{
				ES3.Save<int>("HighestSpawn", curSpawn, filePath);
				ConfigTable tbl = ConfigData.GetValue("NormalTask_Client");
				if(tbl.m_Data.ContainsKey(curSpawn.ToString()))
				{
					ConfigRow row = tbl.GetRow(curSpawn.ToString());
					switch(row.GetValue("Type"))
					{
						case "1":
						{
							int heroId = int.Parse(row.GetValue("Value"));
							AddHero(playerId, heroId);
							SyncHero(playerId, heroId);
							break;
						}
					}
				}
			}
		}

		private void SyncCurSpawn(int curSpawn)
		{
			ByteBuffer data = new ByteBuffer();
			data.writeByte(102);
			data.writeByte(BattleService.BATTLE_CUR_SPAWN);
			data.writeInt(curSpawn);
			ServiceManager.PostMessageShortEx(data);
		}

		public void NormalBattleSaveCurWave(string playerId, int curWave)
		{
			string filePath = string.Format("{0}/NormalBattle", playerId);
			ES3.Save<int>("CurWave", curWave, filePath);
			SyncCurWave(curWave);
		}

		private void SyncCurWave(int curWave)
		{
			ByteBuffer data = new ByteBuffer();
			data.writeByte(102);
			data.writeByte(BattleService.BATTLE_CUR_WAVE);
			data.writeInt(curWave);
			ServiceManager.PostMessageShortEx(data);
		}

		public void NormalBattleSaveHighestSpawn(string playerId, int highestSpawn)
		{
			string filePath = string.Format("{0}/NormalBattle", playerId);
			ES3.Save<int>("HighestSpawn", highestSpawn, filePath);
			SyncHighestSpawn(highestSpawn);
		}

		private void SyncHighestSpawn(int highestSpawn)
		{
			ByteBuffer data = new ByteBuffer();
			data.writeByte(102);
			data.writeByte(BattleService.BATTLE_HIGHEST_SPAWN);
			data.writeInt(highestSpawn);		
			ServiceManager.PostMessageShortEx(data);
		}

		public void NormalBattleAddHero(string playerId, int heroId, int heroIndex)
		{
			NormalBattleChangeCoin(playerId, -ConfigMgr.BattleHeroLevelUpCoin(heroIndex, 0));

			string filePath = string.Format("{0}/NormalBattle", playerId);
			ES3.Save<int>(string.Format("HeroID{0}", heroIndex), heroId, filePath);
			ES3.Save<int>(string.Format("HeroLevel{0}", heroIndex), 1, filePath);

			SyncBattleHero(playerId, heroIndex);
		}

		public void SyncBattleHero(string playerId, int index)
		{
			ByteBuffer data = new ByteBuffer();
			data.writeByte(102);
			data.writeByte(BattleService.BATTLE_HERO);
			string filePath = string.Format("{0}/NormalBattle", playerId);
			data.writeInt(ES3.Load<int>(string.Format("HeroID{0}", index), filePath));
			data.writeInt(ES3.Load<int>(string.Format("HeroLevel{0}", index), filePath));
			data.writeByte(index);
			ServiceManager.PostMessageShortEx(data);
		}
		#endregion
	}
}
