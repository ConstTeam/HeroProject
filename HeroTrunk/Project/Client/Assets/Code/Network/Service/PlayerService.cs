namespace MS
{
	public class PlayerService : IService
	{
		public const int PLAYER_LOGIN_END	= 1;
		public const int PLAYER_INFO		= 2;
		public const int PLAYER_HERO		= 3;

		public override void ProcessMessage(ConnectBase conn, ByteBuffer data)
		{
			int moduleId = data.readByte();
			switch (moduleId)
			{
				case PLAYER_LOGIN_END:
				{
					SceneLoader.LoadScene("MainScene");
					break;
				}
				case PLAYER_INFO:
				{
					LoginPanel.GetInst().SaveAccount();
					ApplicationConst.bGM = data.readBoolean();
					PlayerInfo.PlayerId = data.readUTF();
					PlayerInfo.Nickname = data.readUTF();
					PlayerInfo.GuideStep = data.readByte();
					break;
				}
				case PLAYER_HERO:
				{
					int heroId = data.readInt();
					int star = data.readInt();
					int maxPower = data.readInt();
					float[] mainProperty = new float[10];
					for(int j = 0; j < 10; ++j)
						mainProperty[j] = data.readInt() / 100f;

					HeroAll.SetHeroInfo(heroId, new HeroInfo(heroId, star, maxPower, mainProperty));
					if(BattleHeroListPanel.m_Inst != null)
						BattleHeroListPanel.GetInst().InsertHeroItem(heroId);
					break;
				}
			}
		}
	}
}
