using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MS
{
	public class Database : MonoBehaviour
	{
		private Database _inst;
		public Database GetInst()
		{
			return _inst;
		}

		private void Awake()
		{
			_inst = this;
			if(!ES3.KeyExists("HeroList"))
			{
				ES3.Save<List<int>>("HeroList", 1006);
			}

		}

		private void OnDestroy()
		{
			_inst = null;
		}

		public void AddHero(int heroId)
		{

		}
	}
}
