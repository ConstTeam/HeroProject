using UnityEngine;

namespace MS
{
	public class BattleSceneTimer : MonoBehaviour
	{
		public delegate void SceneTimerFunc(int sec);
		public SceneTimerFunc FuncSceneTick;
		public int m_iSec = -1;

		private float _fDeltaTime = 0.0f;

		private static BattleSceneTimer _inst;
		public static BattleSceneTimer GetInst()
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
			m_iSec = -1;
			PauseTimer();
		}

		private void Update()
		{
			_fDeltaTime += Time.deltaTime;
			if(_fDeltaTime >= 1.0f)
			{
				FuncSceneTick(++m_iSec);
				_fDeltaTime = 0;
			}
		}

		public void BeginTimer()
		{
			m_iSec = 0;
			enabled = true;
		}

		public void PauseTimer()
		{
			enabled = false;
		}

		public void AddTickFunc(SceneTimerFunc func)
		{
			FuncSceneTick += func;
		}
	}
}
