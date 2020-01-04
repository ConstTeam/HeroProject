namespace MS
{
	public class CommonService : IService
	{
		private const int SERVER_MESSAGEBOX		= 1;
		private const int SERVER_STRING			= 2;

		public override void ProcessMessage(ConnectBase conn, ByteBuffer data)
		{
			int moduleId = data.readByte();
			switch (moduleId)
			{
				case SERVER_MESSAGEBOX:
					int boxType = data.readByte();
					int key = data.readInt();
					int size = data.readByte();
					string value = ConfigData.GetStaticText(key.ToString());
					string[] c = new string[size];
					for (int i = 0; i < size; ++i)
					{
						int a = data.readByte();
						int b = data.readInt();
						c[i] = (0 == a) ? b.ToString() : ConfigData.GetStaticText(b.ToString());
					}
					MsgBoxPanel.ShowMsgBox(string.Empty, string.Format(value, c), boxType);
					break;
				case SERVER_STRING:
					string msg = data.readUTF();
					MsgBoxPanel.ShowMsgBox(string.Empty, msg, 1);
					break;
			}
		}
	}
}
