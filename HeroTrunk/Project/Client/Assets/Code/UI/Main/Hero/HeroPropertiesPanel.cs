using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MS
{
	public class HeroPropertiesPanel : MonoBehaviour
	{
		public Text[] Labels;
		public Text[] Values;

		private void Awake()
		{
			for(int i = 0; i < 10; ++i)
			{
				Labels[i].text = ConfigData.GetStaticText((10000 + i).ToString());
			}
		}
	}
}
