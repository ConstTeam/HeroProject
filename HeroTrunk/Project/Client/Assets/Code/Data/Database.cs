using System.Collections;
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
				data.writeInt(ES3.Load<int>("HeroID", filePath));
				data.writeInt(ES3.Load<int>("HeroLevel", filePath));
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
			string filePath = string.Format("{0}/HeroInfo/{1}.es", playerId, heroId);
			ES3.Save<int>("HeroID", heroId, filePath);
			ES3.Save<int>("HeroLevel", 1, filePath);
		}
	}
}
