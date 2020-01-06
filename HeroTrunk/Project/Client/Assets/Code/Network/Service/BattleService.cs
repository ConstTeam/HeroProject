namespace MS
{
	public class BattleService : IService
	{
		public const int BATTLE_START			= 1;
		public const int BATTLE_COIN			= 2;
		public const int BATTLE_CUR_SPAWN		= 3;
		public const int BATTLE_CUR_WAVE		= 4;
		public const int BATTLE_HIGHEST_SPAWN	= 5;
		public const int BATTLE_HERO			= 6;

		public override void ProcessMessage(ConnectBase conn, ByteBuffer data)
		{
			int moduleId = data.readByte();
			switch(moduleId)
			{
				case BATTLE_START:
				{
					BattleManager.GetInst().m_BattleScene.OnBattleStart();
					break;
				}
				case BATTLE_COIN:
				{
					BattleManager.GetInst().m_BattleScene.Coin = data.readInt();
					break;
				}
				case BATTLE_CUR_SPAWN:
				{
					SpawnHandler.GetInst().SetSpawnInfo(data.readInt());
					break;
				}
				case BATTLE_CUR_WAVE:
				{
					SpawnHandler.GetInst().CurWave = data.readInt();
					SpawnHandler.GetInst().ReleaseCurWave();
					break;
				}
				case BATTLE_HIGHEST_SPAWN:
				{
					int highestLv = data.readInt();
					//
					break;
				}
				case BATTLE_HERO:
				{
					BattleManager.GetInst().AddHero(data.readInt(), data.readInt(), data.readByte());
					break;
				}
			}
		}
	}
}
