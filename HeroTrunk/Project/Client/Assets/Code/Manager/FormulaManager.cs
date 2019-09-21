using LuaInterface;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace MS
{
	public class FormulaManager : MonoBehaviour
	{
		public static LuaState luaState;

		private void Awake()
		{
			luaState = new LuaState();
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
