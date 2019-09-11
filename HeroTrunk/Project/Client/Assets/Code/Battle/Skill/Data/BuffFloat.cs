using System.Collections.Generic;

namespace MS
{
	public class BuffFloat
	{
		private List<int> _lstInstID = new List<int>();
		private List<float> _lstValue = new List<float>();

		public void AddNew(int instId, float value)
		{
			_lstInstID.Add(instId);
			_lstValue.Add(value);
		}

		public int InstanceID
		{
			get { return _lstInstID[_lstInstID.Count - 1]; }
		}

		public float Value
		{
			get { return GetValue(_lstValue); }
			set { SetValue(value, _lstValue, _lstInstID); }
		}

		private float GetValue(List<float> lst)
		{
			int count = lst.Count;
			if(count > 0)
				return lst[lst.Count - 1];
			else
				return 0;
		}

		private void SetValue(float value, List<float> lst, List<int> instIDs)
		{
			int count = lst.Count;
			if(count > 0)
			{
				if(value > 0)
					lst[lst.Count - 1] = value;
				else
				{
					instIDs.RemoveAt(count - 1);
					lst.RemoveAt(count - 1);
				}
			}
			else if(value > 0)
				lst.Add(value);
		}
	}
}
