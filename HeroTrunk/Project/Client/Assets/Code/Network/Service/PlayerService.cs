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
					int heroCount = data.readByte();
					int heroId, heroLv;
					for(int i = 0; i < heroCount; ++i)
					{
						heroId	= data.readInt();
						heroLv	= data.readInt();
						HeroAll.SetHeroInfo(heroId, new HeroInfo(heroId, heroLv));
					}
					SceneLoader.LoadScene("MainScene");
					break;
			}
		}
	}
}
