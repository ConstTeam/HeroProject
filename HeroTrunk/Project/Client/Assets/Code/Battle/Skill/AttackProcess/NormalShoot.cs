using System.Collections.Generic;

namespace MS
{
	public class NormalShoot : AttackProcess
	{
		protected override List<CharHandler> GetTargets(CharHandler charHandler, SkillDataSon skillDataSon = null, int index = -1)
		{
			List<CharHandler> ret = new List<CharHandler>();

			CharHandler ch = GetConcentrate(charHandler, SkillEnum.AimSide.Aim);
			if(null == ch)
			{
				float dis = 0xffff;
				ch = BattleManager.GetInst().m_CharInScene.GetNearestChar(charHandler, charHandler.m_CharData.GetOppositeSide(), ref dis);

				if(null != ch)
					ret.Add(ch);
			}
			else
				ret.Add(ch);

			return ret;
		}

		protected override void Excute(CharHandler charHandler, int index = -1, int skillInstId = -1)
		{
			List<CharHandler> targets = GetTargets(charHandler);
			if(targets.Count > 0)
			{
				for(int i = 0; i < targets.Count; ++i)
				{
					CharHandler aimHandler = targets[i];
					BulletBase bullet = BattleScenePool.GetInst().PopBullet(charHandler);
					bullet.Shoot(charHandler, targets[i], () => { BeHitCallback(charHandler, aimHandler); });
				}
			}
			else
			{
				BulletBase bullet = BattleScenePool.GetInst().PopBullet(charHandler);
				bullet.Shoot(charHandler);
			}
		}

		private void BeHitCallback(CharHandler charHandler, CharHandler aimHandler)
		{
			float hurt;
			if(BattleEnum.Enum_CharType.Monster == charHandler.m_CharData.m_eType)
				hurt = BattleCalculate.MonsterNormalHurt(charHandler.m_CharData, aimHandler.m_CharData);
			else
				hurt = BattleCalculate.HeroNormalHurt(charHandler.m_CharData, aimHandler.m_CharData);

			hurt = ChangeProperty.RatioCoefficient(hurt, SkillEnum.SkillKind.Force, charHandler, aimHandler);
			aimHandler.BeHit(hurt, charHandler);
		}
	}
}
