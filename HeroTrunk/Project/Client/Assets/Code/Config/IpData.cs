using UnityEngine;
using System.Collections.Generic;

namespace MS
{
	public class IpData
	{
		private static string[] _sLastServerInfo = null;
		private static string[] _sCurServerInfo = null;
		private static List<string[]> _listLogin = new List<string[]>();

		public static void LoadIp(string ipData)
		{
			_listLogin.Clear();
			string lastServerIndex = PlayerPrefs.GetString("LastLoginServer", "");

			string[] data = ipData.Split('\n');
			for(int i = data.Length-1; i >= 0; --i)
			{
				if("" == data[i])
					continue;

				string[] info = data[i].Replace("\r", "").Split('|');
				_listLogin.Add(info);

				if(info[0] == lastServerIndex)
					_sCurServerInfo = _sLastServerInfo = info;
			}

			if(null == _sCurServerInfo || _sCurServerInfo.Length <= 0)
				_sCurServerInfo = _listLogin[data.Length - 1];

			SetUrl(); 
		}

		public static void SetUrl()
		{
			if(null == _sCurServerInfo)
				Debug.LogError("LoginServer has not defined in ip.xml!");

			SocketHandler.GetInst().ShortSetUrl(_sCurServerInfo[1], int.Parse(_sCurServerInfo[2]));
		}

		public static List<string[]> GetServerList()
		{
			return _listLogin;
		}

		public static int GetServerCount()
		{
			return _listLogin.Count;
		}

		public static string[] GetCurServerData()
		{
			return _sCurServerInfo;
		}

		public static void SetCurServerData(int index)
		{
			_sCurServerInfo = _listLogin[index];
			SetUrl();
		}

		public static void WriteLastServerIndex()
		{
			PlayerPrefs.SetString("LastLoginServer", _sCurServerInfo[0]);	
		}

        public static string[] GetLastServerData()
        {
            return _sLastServerInfo;
        }
    }
}
