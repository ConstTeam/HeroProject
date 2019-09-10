using System.Collections.Generic;

namespace MS
{
	public class BuffBool
	{
		private List<int> _lstInstID = new List<int>();
		private List<bool> _lstValue = new List<bool>();

		public void AddNew(int instId, bool value)
		{
			_lstInstID.Add(instId);
			_lstValue.Add(value);
		}

		public int InstanceID
		{
			get { return _lstInstID[_lstInstID.Count - 1]; }
		}

		public bool Value
		{
			get { return GetValue(_lstValue); }
			set { SetValue(value, _lstValue, _lstInstID); }
		}

		private bool GetValue(List<bool> lst)
		{
			return lst.Count > 0;
		}

		private void SetValue(bool value, List<bool> lst, List<int> instIDs)
		{
			int count = lst.Count;
			if(count > 0)
			{
				if(value)
					lst[lst.Count - 1] = value;
				else
				{
					instIDs.RemoveAt(count - 1);
					lst.RemoveAt(count - 1);
				}
			}
			else if(value)
				lst.Add(value);
		}
	}
}
