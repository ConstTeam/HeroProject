using LuaInterface;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace MS
{
	public class FormulaManager : MonoBehaviour
	{
		public static LuaState luaState;
		private static Dictionary<string, LuaFunction> dicFunction = new Dictionary<string, LuaFunction>();

		private void Awake()
		{
			TextAsset scriptFile = Resources.Load<TextAsset>("Text/FormulaMgr.lua");
			luaState = new LuaState();
			luaState.DoString(scriptFile.text);
		}

		public static LuaFunction LoadFunction(string sName)
		{
			if(dicFunction.ContainsKey(sName))
				return dicFunction[sName];
			else
			{
				LuaFunction f = luaState.GetFunction(sName);
				if(f == null)
				{
					Debug.LogError(string.Format("未在FormulaMgr文件中找到名字为:{0}的函数", sName));
					return null;
				}
				dicFunction.Add(sName, f);
				return f;
			}
		}

		public static float Arithmetic(string express)
		{
			float ret = 0.0f;
			try
			{
				if(Regex.IsMatch(express, "[a-zA-Z]"))
					return 0.0f;

				object[] r = luaState.DoString(string.Format("return {0}", express));
				if(r.Length > 0)
				{
					ret = float.Parse(r[0].ToString());
					if(float.IsNaN(ret) || float.IsInfinity(ret))
						ret = 0.0f;
				}
			}
			catch
			{
				Debug.LogError(express);
			}

			return ret;
		}
	}
}
