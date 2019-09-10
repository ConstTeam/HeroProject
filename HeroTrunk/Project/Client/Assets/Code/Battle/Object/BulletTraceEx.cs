namespace MS
{
	public class BulletTraceEx : BulletTrace
	{
		protected override void Hide()
		{
			base.Hide();
			BattleScenePool.GetInst().PushBullet(m_CharHandler, this);
		}
	}
}
