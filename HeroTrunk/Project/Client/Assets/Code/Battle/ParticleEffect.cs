using UnityEngine;

namespace MS
{
	public class ParticleEffect : MonoBehaviour
	{
		public bool m_bPause;           //是否受timescale影响--true为受影响，false为不受影响
		private float _timeAtLastFrame;
		private float _deltaTime;
		private ParticleSystem[] _particleSystem;
		private Animator[] _effectAnimator;


		void Awake()
		{
			_particleSystem = gameObject.GetComponentsInChildren<ParticleSystem>();
			_effectAnimator = gameObject.GetComponentsInChildren<Animator>();
		}

		void Update()
		{
			if(!m_bPause && 0 == Time.timeScale)
			{
				_deltaTime = Time.realtimeSinceStartup - _timeAtLastFrame;
				_timeAtLastFrame = Time.realtimeSinceStartup;
				foreach(ParticleSystem par in _particleSystem)
				{
					if(par.gameObject.activeInHierarchy)
					{
						par.Simulate(_deltaTime, false, false);
						par.Play();
					}
				}
			}
			else
				_timeAtLastFrame = Time.realtimeSinceStartup;
		}

		public void Replay()
		{
			_timeAtLastFrame = Time.realtimeSinceStartup;
		}

		public void ReplayEffect()
		{
			for(int i = 0; i < _particleSystem.Length; ++i)
			{
				_particleSystem[i].Clear();
			}
		}

		public void PauseEffect()
		{
			for(int i = 0; i < _effectAnimator.Length; ++i)
			{
				_effectAnimator[i].speed = 0;
			}
		}

		public void PlayEffect()
		{
			for(int i = 0; i < _effectAnimator.Length; ++i)
			{
				_effectAnimator[i].speed = 1;
			}
		}

		public void UnPauseInScale()
		{
			m_bPause = false;
			for(int i = 0; i < _effectAnimator.Length; ++i)
			{
				_effectAnimator[i].updateMode = AnimatorUpdateMode.UnscaledTime;
			}
			Replay();
		}

		public void PauseInScale()
		{
			m_bPause = true;
			for(int i = 0; i < _effectAnimator.Length; ++i)
			{
				_effectAnimator[i].updateMode = AnimatorUpdateMode.Normal;
			}
		}

		public void SkillEffectEnd()
		{
			gameObject.SetActive(false);
		}
	}
}
