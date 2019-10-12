using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

namespace MS
{
	public delegate void ShortSendExcute();
	public delegate void ShortSendBack();
	public delegate void LongSendExcute();
	public class SocketHandler : MonoBehaviour
	{
		public static string			socketResponse		= "";
		public static float				shortTimeout		= 10;
		public static ShortSendExcute	ShortSendExcuteFun;
		public static ShortSendBack		ShortSendBackFun;
	
		private List<ConnectShort>		_lstShortConn = new List<ConnectShort>();
		private string					_sShortIp;
		private int						_sShortPort;

		private string[][]				_arrIpInfo;

		private static int m_iMultiCode = 0;
		public static int GetMultiCode()
		{
			return ++m_iMultiCode;
		}

		private static SocketHandler m_Inst = null;
		public static SocketHandler GetInst()
		{
			return m_Inst;
		}

		void OnDestroy()
		{
			m_Inst = null;
		}
	
		void Awake()
		{
			m_Inst = this;
		}

		public void ShortSetUrl(string sIp, int iPort)
		{
			IPAddress[] ips	= Dns.GetHostAddresses(sIp);
			_sShortIp		= ConnectBase.SetIpType(ips[0].ToString());
			_sShortPort		= iPort;
		}

		public void ShortSend(ByteBuffer data, bool flag = false, bool bEncrypt = true)
		{
			if(flag && null != ShortSendExcuteFun)
				ShortSendExcuteFun();

			ConnectShort conn = new ConnectShort(data, flag, bEncrypt);
			_lstShortConn.Add(conn);
			conn.ConnectToServer(_sShortIp, _sShortPort);

			StartCoroutine(ShortTimeout(conn));
		}

		private IEnumerator ShortTimeout(ConnectShort conn)
		{
			yield return new WaitForSeconds(shortTimeout);
			int index = _lstShortConn.IndexOf(conn);
			if(index >= 0)
			{
				conn.PostError(0);
			}
		}
	
		public void ShortClose(ConnectShort conn)
		{
			int index = _lstShortConn.IndexOf(conn);
			if(index >= 0)
			{
				_lstShortConn[index].Close();
				_lstShortConn.RemoveAt(index);
			}
		}

		public int GetShortConnListCount()
		{
			return _lstShortConn.Count;
		}
	}
}
