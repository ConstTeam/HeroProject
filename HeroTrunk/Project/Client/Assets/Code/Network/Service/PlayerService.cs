namespace MS
{
	public class PlayerService : IService
	{
		private const int PLAYER_INFO	= 1;

		public override void ProcessMessage(ConnectBase conn, ByteBuffer data)
		{
			int moduleId = data.readByte();
			switch (moduleId)
			{
				case PLAYER_INFO:
					LoginPanel.GetInst().SaveAccount();
					ApplicationConst.bGM = data.readBoolean();
					PlayerData.PlayerId = data.readInt();
					PlayerData.Nickname = data.readUTF();
					SceneLoader.LoadScene("MainScene");
					break;
			}
		}
	}
}
