namespace MS
{
	public class SkillEnum
	{
		//技能大类 武、魂、法---------------------------------------------
		public enum SkillKind
		{
			Force,
			Soul,
			Magic
		}

		//--技能类型、子类型----------------------------------------------
		public enum SkillType
		{
			Property,       //属性更改
			Buff,
			Debuff,
			SuperBuff,      //不被驱散的Buff
			SuperDebuff,    //不被驱散的Debuff
			Special,        //特殊技
			Bullet          //子弹
		}

		public enum SkillSonType
		{
			//Buff---------------------------------------------
			Property,               //属性
			Stun,                   //晕眩
			SkillForbid,            //禁用技能
			SoulSkillInvalid,       //技能失效

			//Special------------------------------------------
			SkillRelease,           //立即释放技能
			CleanBuff,              //清除Buff
			CleanDebuff,            //清除Debuff
			CleanBuffDebuff,        //清除Buff和Debuff
			ReviveHero,             //复活

			//属性---------------------------------------------
			Attack,                 //攻
			Defence,                //防
			HP,                     //血
			MP,                     //魔
			Force,                  //武
			Soul,                   //魂
			Magic,                  //法
			ForceRatio,             //武提升
			SoulRatio,              //魂提升
			MagicRatio,             //法提升
			ForceSkillAtkRatio,     //武功提升
			SoulSkillAtkRatio,      //魂技提升
			MagicSkillAtkRatio,     //术法提升
			ForceSkillDefRatio,     //武功免伤
			SoulSkillDefRatio,      //魂技免伤
			MagicSkillDefRatio,     //术法免伤
			CriticalRatio,          //暴击率
			DamageRatio,            //输出伤害比例
			Absorb,                 //吸收伤害
			SuperAbsorb,            //到时间未破则回满血
			Unbeatable,             //无敌
			CureUpRatio,            //治疗提升
			CureDownRatio,          //治疗压制
			Rebound,                //反弹
			Concentrate,            //集火
			AbsorbHP,               //吸血
			Connect                 //伤害锁链
		}

		//--技能接收方---------------------------------------------------------
		public enum AimSide
		{
			Self,           //己方
			Aim             //对方
		}

		public enum AimSideType
		{
			All,            //全体
			Hero,           //所有英雄
			General,        //武官
			Official,       //文官
			Self,           //自己
			Presence,       //场上的英雄(包括正在放技能的文官)
			Monster,        //小怪
			Dead            //阵亡武将
		}

		//捕获目标方式
		public enum CatchType
		{
			All,            //所有
			Nearest,        //最近
			Randoms,        //随机
			Scope,          //扇形范围
			HPLeast,        //血量最少的
			Friend,         //除了自己
			Rect,           //矩形范围
			DistanceNear,   //最近目标地点技能
			DistanceFixed   //固定距离地点技能
		}

		//--被动技能触发类型---------------------------------------------------
		public enum TriggerType
		{
			None,
			BattleStart,    //战斗开始后x秒
			AimHeroBorn,    //对方武将进场x秒后
			SelfCurHP,      //己方首次任意武将血量低于x%
			AimCurHP,       //对方首次任意武将血量低于x%
			SelfOtherDie,   //己方首次武将阵亡
			MainSkill,      //自己释放主动技能时
			SelfBeDebuff,   //己方首次受到负面状态影响
			SelfDie         //自己阵亡
		}
	}
}
