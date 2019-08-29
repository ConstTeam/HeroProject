using System.Collections;
using UnityEngine;

namespace MS
{
	public class MainThreadService : IService
	{
		public const int SCONNECT_EXCEPTION	= 4;	//短连接错误(登陆)

		public override void ProcessMessage(ConnectBase conn, ByteBuffer data)
		{
			int errCode;
			int moduleId = data.readByte();
			switch (moduleId)
			{
				case SCONNECT_EXCEPTION:
					errCode = data.readInt();
					MsgBoxPanel.MsgCallback Reconnect = () =>
					{
						ConnectShort co = (ConnectShort)conn;
						SocketHandler.GetInst().ShortSend(co.m_Data, co.m_bFlag, co.m_bNeedEncrypt);
						SocketHandler.ShortSendBackFun();
					};
					MsgBoxPanel.ShowMsgBox(string.Empty, (string)ApplicationConst.dictStaticText["22"], 1, Reconnect);
					break;
				default:
					break;
			}
		}
	}
}
