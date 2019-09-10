using System.Collections.Generic;
using UnityEngine;

namespace MS
{
	public class BattleCharInScene : MonoBehaviour
	{
		public List<CharHandler> m_listGeneralMine				= new List<CharHandler>();
		public List<CharHandler> m_listGeneralEnemy				= new List<CharHandler>();
		public List<CharHandler> m_listOfficialMine				= new List<CharHandler>();
		public List<CharHandler> m_listOfficialEnemy			= new List<CharHandler>();
		public List<CharHandler> m_listMonster					= new List<CharHandler>();
		public List<CharHandler> m_listDeadMine					= new List<CharHandler>();
		public List<CharHandler> m_listDeadEnemy				= new List<CharHandler>();
		public List<CharHandler> m_listOfficialPresenceMine		= new List<CharHandler>();
		public List<CharHandler> m_listOfficialPresenceEnemy	= new List<CharHandler>();

		public void AddChar(CharHandler handler)
		{
			if(BattleEnum.Enum_CharSide.Mine == handler.m_CharData.m_eSide)
			{
				switch(handler.m_CharData.m_eType)
				{
					case BattleEnum.Enum_CharType.General:
						m_listGeneralMine.Add(handler);
						break;
					case BattleEnum.Enum_CharType.Official:
						m_listOfficialMine.Add(handler);
						break;
				}
			}
			else
			{
				switch(handler.m_CharData.m_eType)
				{
					case BattleEnum.Enum_CharType.General:
						m_listGeneralEnemy.Add(handler);
						break;
					case BattleEnum.Enum_CharType.Official:
						m_listOfficialEnemy.Add(handler);
						break;
					case BattleEnum.Enum_CharType.Monster:
						m_listMonster.Add(handler);
						break;
				}
			}
		}

		public void RemoveChar(CharHandler handler)
		{
			if(BattleEnum.Enum_CharSide.Mine == handler.m_CharData.m_eSide)
			{
				switch(handler.m_CharData.m_eType)
				{
					case BattleEnum.Enum_CharType.General:
						m_listGeneralMine.Remove(handler);
						m_listDeadMine.Add(handler);
						break;
					case BattleEnum.Enum_CharType.Official:
						m_listOfficialMine.Remove(handler);
						break;
				}
			}
			else
			{
				switch(handler.m_CharData.m_eType)
				{
					case BattleEnum.Enum_CharType.General:
						m_listGeneralEnemy.Remove(handler);
						m_listDeadEnemy.Add(handler);
						break;
					case BattleEnum.Enum_CharType.Official:
						m_listOfficialEnemy.Remove(handler);
						break;
					case BattleEnum.Enum_CharType.Monster:
						m_listMonster.Remove(handler);
						break;
				}
			}
		}

		public void ReAddChar(CharHandler handler)
		{
			if(BattleEnum.Enum_CharSide.Mine == handler.m_CharData.m_eSide)
			{
				m_listDeadMine.Remove(handler);
				if(BattleCam.GetInst().m_CharHandler == handler)     //˾������Ҳ������佫ʱ��,���������˳������
					m_listGeneralMine.Insert(0, handler);
				else
					m_listGeneralMine.Add(handler);
			}
			else
			{
				m_listDeadEnemy.Remove(handler);
				m_listGeneralEnemy.Add(handler);
			}
		}

		public CharHandler GetCharByID(BattleEnum.Enum_CharSide side, int charId)
		{
			List<CharHandler> lst = BattleEnum.Enum_CharSide.Mine == side ? m_listGeneralMine : m_listGeneralEnemy;
			for(int i = 0; i < lst.Count; ++i)
			{
				if(lst[i].m_CharData.m_iCharID == charId)
					return lst[i];
			}

			lst = BattleEnum.Enum_CharSide.Mine == side ? m_listOfficialMine : m_listOfficialEnemy;
			for(int i = 0; i < lst.Count; ++i)
			{
				if(lst[i].m_CharData.m_iCharID == charId)
					return lst[i];
			}

			return null;
		}

		public int GetHeroCount(BattleEnum.Enum_CharSide side)
		{
			return BattleEnum.Enum_CharSide.Mine == side ? m_listGeneralMine.Count + m_listOfficialMine.Count : m_listGeneralEnemy.Count + m_listOfficialEnemy.Count;
		}

		public void DisableAllCharacters()
		{
			_DisableAllCharacters(m_listGeneralMine);
			_DisableAllCharacters(m_listGeneralEnemy);
			for(int i = 0; i < m_listMonster.Count; ++i)
			{
				m_listMonster[i].m_CharState.enabled = false;
				m_listMonster[i].m_CharMove.SetAgentEnable(false);
			}
		}

		public void _DisableAllCharacters(List<CharHandler> lst)
		{
			for(int i = 0; i < lst.Count; ++i)
			{
				lst[i].m_CharState.enabled = false;
				lst[i].m_CharMove.SetAgentEnable(false);
				lst[i].m_CharSkill.RunCD(false);
			}
		}

		public void ShowAllCharacters(bool bShow)
		{
			if(bShow == BattleCam.GetInst().enabled)
				return;

			BattleCam.GetInst().enabled = bShow;
			_ShowAllCharacters(m_listGeneralMine, bShow);
			_ShowAllCharacters(m_listGeneralEnemy, bShow);
			_ShowAllCharacters(m_listDeadMine, bShow);
			_ShowAllCharacters(m_listDeadEnemy, bShow);
			_ShowAllCharacters(m_listMonster, bShow);
		}

		public void _ShowAllCharacters(List<CharHandler> lst, bool bShow)
		{
			for(int i = 0; i < lst.Count; ++i)
			{
				lst[i].m_Transform.position += Vector3.down * 1000 * (bShow ? -1 : 1);
			}
		}

		//--��ȡս���еĶ���------------------------------------------------------------------------------------
		//��ȡĳ���� ���ж���
		public void GetAllChar(BattleEnum.Enum_CharSide side, List<List<CharHandler>> lst)
		{
			GetAllHero(side, lst);
			if(BattleEnum.Enum_CharSide.Enemy == side)
				GetAllMonster(lst);
		}

		//��ȡĳ���� ����Ӣ��
		public void GetAllHero(BattleEnum.Enum_CharSide side, List<List<CharHandler>> lst)
		{
			if(BattleEnum.Enum_CharSide.Mine == side)
			{
				lst.Add(m_listGeneralMine);
				lst.Add(m_listOfficialMine);
			}
			else
			{
				lst.Add(m_listGeneralEnemy);
				lst.Add(m_listOfficialEnemy);
			}
		}

		//��ȡĳ���� �ڳ�Ӣ��
		public void GetPresence(BattleEnum.Enum_CharSide side, List<List<CharHandler>> lst)
		{
			if(BattleEnum.Enum_CharSide.Mine == side)
			{
				lst.Add(m_listGeneralMine);
				lst.Add(m_listOfficialPresenceMine);
			}
			else
			{
				lst.Add(m_listGeneralEnemy);
				lst.Add(m_listOfficialPresenceEnemy);
			}
		}

		//��ȡĳ�������
		public void GetGeneral(BattleEnum.Enum_CharSide side, List<List<CharHandler>> lst)
		{
			lst.Add(BattleEnum.Enum_CharSide.Mine == side ? m_listGeneralMine : m_listGeneralEnemy);
		}

		//��ȡĳ�������
		public List<CharHandler> GetGeneral(BattleEnum.Enum_CharSide side)
		{
			return BattleEnum.Enum_CharSide.Mine == side ? m_listGeneralMine : m_listGeneralEnemy;
		}

		//��ȡĳ�����Ĺ�
		public void GetOfficial(BattleEnum.Enum_CharSide side, List<List<CharHandler>> lst)
		{
			lst.Add(BattleEnum.Enum_CharSide.Mine == side ? m_listOfficialMine : m_listOfficialEnemy);
		}

		//��ȡĳ�����Ĺ�
		public List<CharHandler> GetOfficial(BattleEnum.Enum_CharSide side)
		{
			return BattleEnum.Enum_CharSide.Mine == side ? m_listOfficialMine : m_listOfficialEnemy;
		}

		//��ȡ����С��
		public void GetAllMonster(List<List<CharHandler>> lst)
		{
			lst.Add(m_listMonster);
		}

		//��ȡĳ�����������
		public void GetDeadGeneral(BattleEnum.Enum_CharSide side, List<List<CharHandler>> lst)
		{
			lst.Add(BattleEnum.Enum_CharSide.Mine == side ? m_listDeadMine : m_listDeadEnemy);
		}

		//��ȡĳ���ļ���Ŀ��
		public CharHandler GetConcentrate(BattleEnum.Enum_CharSide side)
		{
			List<CharHandler> lst = BattleEnum.Enum_CharSide.Mine == side ? m_listGeneralMine : m_listGeneralEnemy;
			for(int i = 0; i < lst.Count; ++i)
			{
				if(lst[i].m_CharData.Concentrate.Value)
					return lst[i];
			}

			return null;
		}

		//���
		public CharHandler GetNearestChar(CharHandler charHandler, BattleEnum.Enum_CharSide side, ref float dis)
		{
			List<CharHandler> lst = BattleEnum.Enum_CharSide.Mine == side ? m_listGeneralMine : m_listGeneralEnemy;
			Vector3 pos = charHandler.m_Transform.position;
			CharHandler ret = null;
			for(int i = 0; i < lst.Count; ++i)
			{
				CharHandler tmpChar = lst[i];
				if(tmpChar == charHandler || tmpChar.IsInexistence())
					continue;

				float temp = Vector3.Distance(tmpChar.m_Transform.position, pos);
				if(temp < dis)
				{
					dis = temp;
					ret = tmpChar;
				}
			}

			if(BattleEnum.Enum_CharSide.Mine != side)
			{
				CharHandler ch = GetNearestMonster(charHandler, ref dis);
				if(null != ch)
					ret = ch;
			}

			return ret;
		}

		//���
		public CharHandler GetNearestMonster(CharHandler handler, ref float dis)
		{
			List<CharHandler> lst = m_listMonster;
			Vector3 pos = handler.m_Transform.position;
			CharHandler ret = null;
			for(int i = 0; i < lst.Count; ++i)
			{
				CharHandler tmpChar = lst[i];
				if(tmpChar == handler || tmpChar.IsInexistence())
					continue;

				float temp = (tmpChar.m_Transform.position - pos).magnitude;
				if(temp < dis)
				{
					dis = temp;
					ret = tmpChar;
				}
			}

			return ret;
		}

		//--ȫ�ּ�ħ-------------------------------------------------------
		public void AddMPAll(float mp)
		{
			for(int i = 0; i < m_listGeneralMine.Count; ++i)
			{
				m_listGeneralMine[i].m_CharData.CurMP += mp;
			}
			for(int i = 0; i < m_listOfficialMine.Count; ++i)
			{
				m_listOfficialMine[i].m_CharData.CurMP += mp;
			}
			for(int i = 0; i < m_listGeneralEnemy.Count; ++i)
			{
				m_listGeneralEnemy[i].m_CharData.CurMP += mp;
			}
			for(int i = 0; i < m_listOfficialEnemy.Count; ++i)
			{
				m_listOfficialEnemy[i].m_CharData.CurMP += mp;
			}
		}
	}
}
