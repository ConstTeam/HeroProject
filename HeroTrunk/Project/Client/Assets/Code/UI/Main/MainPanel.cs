using UnityEngine;
using UnityEngine.UI;

namespace MS
{
	public class MainPanel : MonoBehaviour
	{
		public Button NormalBtn;

		private void Awake()
		{
			NormalBtn.onClick.AddListener(OnClickNormal);
		}

		private void OnClickNormal()
		{
			//SceneLoaderMain.GetInst().LoadBattleScene();
		}
	}
}
