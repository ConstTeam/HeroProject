using System.Collections.Generic;
using UnityEngine;

namespace MS
{
	public class AttackProcess
	{
		protected SkillBuff _skillBuff;
		protected SkillProperty _skillProperty;
		protected SkillBullet _skillBullet;
		protected SkillSpecial _skillSpecial;

		public AttackProcess()
		{
			_skillBuff = new SkillBuff();
			_skillProperty = new SkillProperty();
			_skillBullet = new SkillBullet();
			_skillSpecial = new SkillSpecial();
		}

		public void OnProcess(CharHandler charHandler, int index = -1)
		{
			if(CheckCondition(charHandler))
				Excute(charHandler, index);
		}

		protected virtual bool CheckCondition(CharHandler charHandler)
		{
			return true;
		}

		protected virtual bool CheckDead(CharHandler charHandler)
		{
			if(BattleEnum.Enum_CharState.Dead == charHandler.m_CharData.m_eState)
				return false;

			return true;
		}

		protected virtual List<CharHandler> GetTargets(CharHandler charHandler, SkillDataSon skillDataSon = null, int index = -1)
		{
			List<CharHandler> targets = new List<CharHandler>();
			return targets;
		}

		protected virtual void Excute(CharHandler charHandler, int index = -1, int skillInstId = -1)
		{

		}

		protected void GetCharInSector(CharHandler charHandler, float angle, float raidus, List<CharHandler> srcLst, List<CharHandler> retLst)
		{
			for(int i = 0; i < srcLst.Count; ++i)
			{
				CharHandler tmpChar = srcLst[i];
				if(null == charHandler || tmpChar.IsInexistence())
					continue;

				float tmpAngle = Vector3.Angle(charHandler.m_Transform.localRotation * Vector3.forward, tmpChar.m_Transform.position - charHandler.m_Transform.position);
				float tmpDis = Vector3.Distance(charHandler.m_Transform.position, tmpChar.m_Transform.position);
				if(tmpAngle <= angle / 2f && tmpDis < raidus)
				{
					retLst.Add(tmpChar);
				}
			}
		}

		protected void GetCharDistance(CharHandler charHandler, Vector3 centerPos, float angle, float raidus, float distance, List<CharHandler> srcLst, List<CharHandler> retLst)
		{
			for(int i = 0; i < srcLst.Count; ++i)
			{
				CharHandler tmpChar = srcLst[i];
				if(null == charHandler || tmpChar.IsInexistence())
					continue;

				float tmpAngle = Vector3.Angle(charHandler.m_Transform.localRotation * Vector3.forward, tmpChar.m_Transform.position - centerPos);
				float tmpDis = Vector3.Distance(centerPos, tmpChar.m_Transform.position);
				if(tmpAngle <= angle / 2f && tmpDis < raidus)
				{
					retLst.Add(tmpChar);
				}
			}
		}

		protected void GetCharNearest(CharHandler charHandler, float angle, float raidus, List<CharHandler> srcLst, List<CharHandler> retLst)
		{
			float retDis = 0xffff;
			CharHandler retChar = null;
			for(int i = 0; i < srcLst.Count; ++i)
			{
				CharHandler tmpChar = srcLst[i];
				if(null == charHandler || tmpChar.IsInexistence())
					continue;

				float tmpAngle = Vector3.Angle(charHandler.m_Transform.localRotation * Vector3.forward, tmpChar.m_Transform.position - charHandler.m_Transform.position);
				float tmpDis = Vector3.Distance(charHandler.m_Transform.position, tmpChar.m_Transform.position);

				if(tmpAngle <= angle / 2f && tmpDis < raidus && tmpDis < retDis)
				{
					retDis = tmpDis;
					retChar = tmpChar;
				}
			}

			if(null != retChar)
				retLst.Add(retChar);
		}

		protected void GetCharInRect(CharHandler charHandler, float sizeX, float sizeY, List<CharHandler> srcLst, List<CharHandler> retLst)
		{
			for(int i = 0; i < srcLst.Count; ++i)
			{
				CharHandler tmpChar = srcLst[i];
				if(null == charHandler || tmpChar.IsInexistence())
					continue;

				Vector3[] polygon = new Vector3[4];
				polygon[0] = charHandler.m_Transform.position - charHandler.m_Transform.right * sizeX / 2;
				polygon[1] = charHandler.m_Transform.position + charHandler.m_Transform.right * sizeX / 2;
				polygon[2] = polygon[1] + charHandler.m_Transform.forward * sizeY;
				polygon[3] = polygon[0] + charHandler.m_Transform.forward * sizeY;

				if(IsPointInPolygon(tmpChar.m_Transform.position, polygon))
					retLst.Add(tmpChar);
			}
		}

		private bool IsPointInPolygon(Vector3 point, Vector3[] polygon)
		{
			int polygonLength = polygon.Length, i = 0;
			bool inside = false;
			float pointX = point.x, pointZ = point.z;
			float startX, startZ, endX, endZ;
			Vector3 endPoint = polygon[polygonLength - 1];
			endX = endPoint.x;
			endZ = endPoint.z;
			while(i < polygonLength)
			{
				startX = endX;
				startZ = endZ;
				endPoint = polygon[i++];
				endX = endPoint.x;
				endZ = endPoint.z;
				inside ^= (endZ > pointZ ^ startZ > pointZ) && ((pointX - endX) < (pointZ - endZ) * (startX - endX) / (startZ - endZ));
			}
			return inside;
		}

		protected CharHandler GetConcentrate(CharHandler charHandler, SkillEnum.AimSide aimSide)
		{
			CharHandler ConcentrateChar = null;
			BattleEnum.Enum_CharSide oppSide = charHandler.m_CharData.GetOppositeSide();
			if(SkillEnum.AimSide.Aim == aimSide)
				ConcentrateChar = BattleManager.GetInst().m_CharInScene.GetConcentrate(oppSide);

			return ConcentrateChar;
		}
	}
}
