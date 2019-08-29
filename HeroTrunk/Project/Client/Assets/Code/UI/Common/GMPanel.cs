using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace MS
{
	public class GMPanel : MonoBehaviour
	{
		public InputField commandInput;
		public Button closeBtn;
		public Button fpsBtn;
		public Button okBtn;
		public Button logoutBtn;
	
		private GameObject _gameObject;

		private static GMPanel _inst;
		public static GMPanel GetInst()
		{
			return _inst;
		}

		private void OnDestroy()
		{
			_inst = null;
		}

		private void Awake()
		{
			_inst = this;
			_gameObject = gameObject;
			okBtn.onClick.AddListener(OnConfirm);
			fpsBtn.onClick.AddListener(ShowConsole);
			closeBtn.onClick.AddListener(ClosePanel);
			logoutBtn.onClick.AddListener(ToLoginPanel);
			_gameObject.SetActive(false);
		}

		private void OnConfirm()
		{
			string command = commandInput.text;
			if(command.Equals(string.Empty))
				return;

			if(ClientGM(command))
				return;

			string[] param = command.Split(' ');
			ArrayList paramList = new ArrayList(){ param[0], param[1], param.Length == 3 ? param[2] : string.Empty };
			CommonCommand.ExecuteShort(Client2ServerList.GetInst().C2S_GM, paramList);
		}

		public void BeShowWnd()
		{
			_gameObject.SetActive(!_gameObject.activeSelf);
		}

		public void OpenPanel()
		{
			_gameObject.SetActive(true);
		}

		public void ClosePanel()
		{
			_gameObject.SetActive(false);
		}

		public void ToLoginPanel()
		{
			ClosePanel();
			ApplicationEntry.ToLoginScene();
		}

		private void ShowConsole()
		{
			if(null != ConsolePanel.GetInst())
				ConsolePanel.GetInst().BeShow();
		}

		private bool ClientGM(string command)
		{
			string[] arr = command.Split(' ');
			if(arr[0].Equals("100"))
			{
				switch(arr[1])
				{
					case "0": break;
				}
				return true;
			}

			return false;
		}
	}
}
