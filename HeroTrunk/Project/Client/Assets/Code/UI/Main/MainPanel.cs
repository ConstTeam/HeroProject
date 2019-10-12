using UnityEngine;
using UnityEngine.UI;

namespace MS
{
	public class MainPanel : MonoBehaviour
	{
		public Button NormalBtn;
		public Button HeroBen;

		private void Awake()
		{
			NormalBtn.onClick.AddListener(OnClickNormal);
			HeroBen.onClick.AddListener(OnClickHero);
		}

		private void OnClickNormal()
		{
			BattleManager.EnterBattle();
		}

		private void OnClickHero()
		{
			HeroPanel.GetInst().OpenPanel();
		}
	}
}
