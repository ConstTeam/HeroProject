namespace MS
{
	public class PlayerService : IService
	{
		public const int PLAYER_INFO	= 1;

		public override void ProcessMessage(ConnectBase conn, ByteBuffer data)
		{
			int moduleId = data.readByte();
			switch (moduleId)
			{
				case PLAYER_INFO:
					LoginPanel.GetInst().SaveAccount();
					ApplicationConst.bGM = data.readBoolean();
					PlayerInfo.PlayerId = data.readUTF();
					PlayerInfo.Nickname = data.readUTF();
					SceneLoader.LoadScene("MainScene");
					break;
			}
		}
	}
}
