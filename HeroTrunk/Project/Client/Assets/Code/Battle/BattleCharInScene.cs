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
	}
}
