using UnityEngine;
using UnityEngine.UI;

namespace MS
{
	public class MainPanel : MonoBehaviour
	{
		public Button SingleBtn;

		private void Awake()
		{
			//SingleBtn.onClick.AddListener(OnClickSingle);
		}

		private void OnClickSingle()
		{
			//SceneLoaderMain.GetInst().LoadBattleScene();
		}
	}
}
