namespace MS
{
	public class SpawnBoss : SpawnHeroBase
	{
		protected override void SetApplyRootMotion(CharHandler charHandler)
		{
			charHandler.m_CharAnim.SetApplyRootMotion(false);
		}

		protected override void SetRingLight(CharHandler charHandler)
		{
			charHandler.m_CharEffect.SetRingLight("Effect/PassiveSkill/RingLight_Boss");
		}

		protected override void SetObstacleAvoidance(CharHandler handler)
		{
			handler.m_CharMove.SetObstacleAvoidanceType(UnityEngine.AI.ObstacleAvoidanceType.LowQualityObstacleAvoidance);
			handler.m_CharMove.SetObstacleAvoidancePriority(37);
		}
	}
}
