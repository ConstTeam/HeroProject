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
					PlayerInfo.GuideStep = data.readByte();

					int heroCount = data.readByte();
					int id, star, maxPower;
					for(int i = 0; i < heroCount; ++i)
					{
						id = data.readInt();
						star = data.readInt();
						maxPower = data.readInt();
						float[] mainProperty = new float[10];
						for(int j = 0; j < 10; ++j)
							mainProperty[j] = data.readInt() / 100f;

						HeroAll.SetHeroInfo(id, new HeroInfo(id, star, maxPower, mainProperty));
					}

					BattleScene.CurCoin = data.readInt();
					BattleScene.CurBigLv = data.readInt();
					BattleScene.CurSmallLv = data.readInt();
					BattleScene.m_lstHeroId.Clear();
					BattleScene.m_lstHeroLv.Clear();
					int size = data.readByte();
					for(int i = 0; i < size; ++i)
					{
						BattleScene.m_lstHeroId.Add(data.readInt());
						BattleScene.m_lstHeroLv.Add(data.readInt());
					}

					SceneLoader.LoadScene("MainScene");
					break;
			}
		}
	}
}
