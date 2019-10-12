using System.Collections;
using UnityEngine;

namespace MS
{
	public class Client2ServerList
	{
		public ArrayList C2S_GM;

		//--大厅服---------------------------------------------------------------------------------------
		public ArrayList C2S_LOGIN_SHORT;

		//--战斗服---------------------------------------------------------------------------------------
		public ArrayList C2S_BATTLE_LOGIN;

		//*******************************************************************************************

		public static string DeviceIdentifier;
		public static Client2ServerList _Inst = null;

		public static Client2ServerList GetInst()
		{
			if (null == _Inst)
				_Inst = new Client2ServerList();
			return _Inst;
		}

		public Client2ServerList()
		{
			//短连接全加上设备ID用以保证，唯一程序唯一设备中运行; 当使用长连接时，可以考虑去掉。
			DeviceIdentifier = SystemInfo.deviceUniqueIdentifier;

			C2S_GM						= new ArrayList() { "sss",		ModuleDataFirst.MODULE_GM,				(byte)1 };

			//--大厅服---------------------------------------------------------------------------------------
			C2S_LOGIN_SHORT				= new ArrayList() { "sssssss",	ModuleDataFirst.MODULE_LOGIN,			(byte)1 };

			//--战斗服---------------------------------------------------------------------------------------

			
		}
	}
}
