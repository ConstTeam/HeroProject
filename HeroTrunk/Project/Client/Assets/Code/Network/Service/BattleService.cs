namespace MS
{
	public class BattleService : IService
	{
		private const int LOAD = 1;

		public override void ProcessMessage(ConnectBase conn, ByteBuffer data)
		{
			int moduleId = data.readByte();
			switch(moduleId)
			{
				case LOAD:
				{
					break;
				}
			}
		}
	}
}
