using System.Collections;
using UnityEngine;

namespace MS
{
	public class ApplicationConst : MonoBehaviour
	{
		public static bool			bDynamicRes			= true;
		public static bool			bGM;
		public static bool			bAppRunning			= true;
		public static string		sSvnVersion			= "";
		public static string		sAccessToken;
		public static string		sServerId;
		public static string		sServerStartTime	= "";

		public static Hashtable		dictStaticInfo;
		public static Hashtable		dictStaticText;

		public static float			screenRatio;
		public static Rect			sceneCamRect;
		public static float			matchWidthOrHeight;

		public static bool			bFreeSkill			= false;
		public static float			fFightSlideMin;
		public static float			fFightSlideMax;
		public static float			fBackwardCD;

		public static string BundleVersion
		{
			get{ return dictStaticInfo["BundleVer"] as string; }
		}

		public static string SvnVersion
		{
			get{ return dictStaticInfo["SvnVer"] as string; }
		}

		public static string PlatformID
		{
			get{ return dictStaticInfo["PlatformID"] as string; }
		}

		public static string ChannelID
		{
			get{ return dictStaticInfo["ChannelID"] as string; }
		}

		void Awake()
		{
			TextAsset text = Resources.Load<TextAsset>("Text/StaticInfo");
			dictStaticInfo = MiniJSON.jsonDecode(text.text) as Hashtable;

			text = Resources.Load<TextAsset>("Text/StaticText");
			dictStaticText = MiniJSON.jsonDecode(text.text) as Hashtable;

			SetSceneCamRect();
		}

		public void OnConfigLoadEnd()
		{
			new SkillHandler();

			ConfigTable tbl		= ConfigData.GetValue("InitValues_Common");
			string[] slideDis	= tbl.GetValue("FIGHT_SLIDE", "Value").Split(',');
			fFightSlideMin		= int.Parse(slideDis[0]);
			fFightSlideMax		= int.Parse(slideDis[1]);
			fBackwardCD			= float.Parse(tbl.GetValue("BACKWARD_CD", "Value"));
		}

		private void SetSceneCamRect()
		{
			screenRatio = (1080f * Screen.width) / (1920f * Screen.height);
			if(screenRatio > 1f)
			{
				screenRatio = 1f;
				matchWidthOrHeight = 1f;
			}
			else
			{
				screenRatio = 1f / screenRatio;
				matchWidthOrHeight = 0f;
			}
			sceneCamRect = new Rect(0f, (1f - screenRatio) / 2f, 1f, screenRatio);
		}
	}
}
