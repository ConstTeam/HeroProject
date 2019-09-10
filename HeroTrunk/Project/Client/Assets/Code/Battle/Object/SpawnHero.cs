namespace MS
{
	public class SpawnHero : SpawnHeroBase
	{
		protected override void SetRingLight(CharHandler charHandler)
		{
			string ring = BattleEnum.Enum_CharSide.Mine == charHandler.m_CharData.m_eSide ? BattleManager.GetInst().GetMainHero() == charHandler ? "Effect/PassiveSkill/RingLight_Mine" : "Effect/PassiveSkill/RingLight_MineAI" : "Effect/PassiveSkill/RingLight_Boss";
			charHandler.m_CharEffect.SetRingLight(ring);
		}

		protected override void SetObstacleAvoidance(CharHandler handler)
		{
			if(handler.m_iIndex.Equals(0))
			{
				handler.m_CharMove.SetObstacleAvoidanceType(UnityEngine.AI.ObstacleAvoidanceType.NoObstacleAvoidance);
				handler.m_CharMove.SetObstacleAvoidancePriority(39);
			}
			else
			{
				handler.m_CharMove.SetObstacleAvoidanceType(UnityEngine.AI.ObstacleAvoidanceType.LowQualityObstacleAvoidance);
				handler.m_CharMove.SetObstacleAvoidancePriority(38);
			}
		}
	}
}
