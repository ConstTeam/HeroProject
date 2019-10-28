using System.Collections.Generic;
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
			string playerId = ES3.FileExists("Account.es") && ES3.KeyExists(account, "Account.es") ? ES3.Load<string>(account, "Account.es") : AddPlayer(account);
			ByteBuffer data = new ByteBuffer(1);
			data.writeByte(2);
			data.writeByte(PlayerService.PLAYER_INFO);
			data.writeBoolean(true);
			data.writeUTF(playerId);
			data.writeUTF(playerId);

			string dirPath = string.Format("{0}/HeroInfo", playerId);
			string[] arr = ES3.GetFiles(dirPath);
			data.writeByte(arr.Length);
			List<int> lstHero = new List<int>();
			for(int i = 0; i < arr.Length; ++i)
			{
				string filePath = string.Format("{0}/HeroInfo/{1}", playerId, arr[i]);
				data.writeInt(ES3.Load<int>("ID", filePath));
				data.writeInt(ES3.Load<int>("Star", filePath));
				data.writeInt(ES3.Load<int>("MaxPower", filePath));
				data.writeInt(Mathf.FloorToInt(ES3.Load<float>("Attack",		filePath) * 100));
				data.writeInt(Mathf.FloorToInt(ES3.Load<float>("Defence",		filePath) * 100));
				data.writeInt(Mathf.FloorToInt(ES3.Load<float>("HP",			filePath) * 100));
				data.writeInt(Mathf.FloorToInt(ES3.Load<float>("CriticalRatio",	filePath) * 100));
				data.writeInt(Mathf.FloorToInt(ES3.Load<float>("BlockRatio",	filePath) * 100));
				data.writeInt(Mathf.FloorToInt(ES3.Load<float>("Force",			filePath) * 100));
				data.writeInt(Mathf.FloorToInt(ES3.Load<float>("Strategy",		filePath) * 100));
				data.writeInt(Mathf.FloorToInt(ES3.Load<float>("Rule",			filePath) * 100));
				data.writeInt(Mathf.FloorToInt(ES3.Load<float>("Polity",		filePath) * 100));
				data.writeInt(Mathf.FloorToInt(ES3.Load<float>("Charm",			filePath) * 100));
			}
			
			ServiceManager.PostMessageShortEx(data);
		}

		public string AddPlayer(string account)
		{
			int iPlayerId = ES3.KeyExists("IncrementPlayerID") ? ES3.Load<int>("IncrementPlayerID") + 1 : 1;
			ES3.Save<int>("IncrementPlayerID", iPlayerId);

			string sPlayerId = iPlayerId.ToString();
			ES3.Save<string>(account, sPlayerId, "Account.es");
			string filePath = string.Format("{0}/PlayerInfo.es", sPlayerId);
			ES3.Save<string>("PlayerName", sPlayerId, filePath);

			AddHero(sPlayerId, 1006);
			AddHero(sPlayerId, 1007);
			AddHero(sPlayerId, 1008);

			return sPlayerId;
		}

		public void AddHero(string playerId, int heroId)
		{
			ConfigRow row = ConfigData.GetValue("Hero_Common", heroId.ToString());
			string filePath = string.Format("{0}/HeroInfo/{1}.es", playerId, heroId);
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
	}
}
