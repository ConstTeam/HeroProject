using System.Collections.Generic;
using UnityEngine;

namespace MS
{
	public struct SkillEffectStruct
	{
		public GameObject m_SkillEffectObj;
		public Animator m_Animator;
		public ParticleEffect m_SkillEffectParticle;

		public SkillEffectStruct(GameObject obj)
		{
			m_SkillEffectObj = obj;
			m_Animator = obj.GetComponent<Animator>();
			m_SkillEffectParticle = obj.AddComponent<ParticleEffect>();
		}
	}

	public class CharEffect : MonoBehaviour
	{
		private Dictionary<int, SkillEffectStruct> _dicSkillEffect = new Dictionary<int, SkillEffectStruct>();
		private GameObject		_dizzyEffect;
		private GameObject[]	_attackEffects;
		private Transform		_transform;
		private CharHandler		_charHandler;
		private ConfigTable		_skillEffectConfig;
		private ConfigTable		_skillEffectGeneralConfig;

		public GameObject		_RingLightEffect;

		private void Awake()
		{
			_skillEffectConfig = ConfigData.GetValue("SkillEffect_Client");
			_skillEffectGeneralConfig = ConfigData.GetValue("SkillEffectGeneral_Client");
		}

		public void SetCharHandler(CharHandler handler)
		{
			_charHandler = handler;
			_transform = handler.m_Transform;

			//LoadHormalHitEffect();
			//LoadSkillEffect();
			//LoadCommonEffect();
		}

		private void LoadSkillEffect()
		{
			int skillId;
			string path;

			Transform trans;
			for(int i = 0; i < _charHandler.m_CharData.SkillIDs.Length; ++i)
			{
				skillId = _charHandler.m_CharData.SkillIDs[i];
				if(0 == skillId)
					break;

				path = _skillEffectConfig.GetValue(skillId.ToString(), "EffectPath");
				if(null != path && !path.Equals(string.Empty))
				{
					GameObject go = ResourceLoader.LoadAssetAndInstantiate(path, _transform);
					trans = go.transform;
					trans.localScale = go.transform.localScale / _charHandler.m_Transform.localScale.z;
					trans.rotation = _charHandler.m_Transform.rotation;
					go.SetActive(false);
					_dicSkillEffect.Add(skillId, new SkillEffectStruct(go));
				}
			}
		}

		private Vector3 dizzyPos = new Vector3(0f, 3f, 0f);
		private void LoadCommonEffect()
		{
			_dizzyEffect = ResourceLoader.LoadAssetAndInstantiate(_skillEffectGeneralConfig.GetValue("1", "Path"), _transform, dizzyPos);
			_dizzyEffect.transform.localScale = _dizzyEffect.transform.localScale / _charHandler.m_Transform.localScale.z;
			_dizzyEffect.SetActive(false);
		}

		private void LoadHormalHitEffect()
		{
			int charId = _charHandler.m_CharData.m_iCharID;
			int len = _charHandler.m_CharData.m_iAtkCount;
			_attackEffects = new GameObject[len];
			for(int i = 0; i < len; ++i)
			{
				string path = string.Format("Effect/Prefabs_Character/Hero{0}/Hero{0}_Attack{1:00}", charId, i + 1);
				_attackEffects[i] = ResourceLoader.LoadAssetAndInstantiate(path, _transform);
				_attackEffects[i].transform.localScale = _attackEffects[i].transform.localScale / _charHandler.m_Transform.localScale.z;
				_attackEffects[i].SetActive(false);
			}
		}

		public Transform GetSkillBulletObj(int skillId)
		{
			return BattleScenePool.GetInst().PopEffect<BulletTrace>(SkillHandler.GetInst().GetSkillDataByID(skillId).m_sBulletPath);
		}

		public void ShowSkillEffect(int skillId)
		{
			if(_dicSkillEffect.ContainsKey(skillId))
			{
				SkillEffectStruct effect = _dicSkillEffect[skillId];
				effect.m_SkillEffectObj.SetActive(false);
				effect.m_SkillEffectObj.SetActive(true);
				if(null != effect.m_Animator)
					effect.m_Animator.Update(0f);
			}
		}

		public void HideSkillEffect(int skillId)
		{
			if(_dicSkillEffect.ContainsKey(skillId))
			{
				_dicSkillEffect[skillId].m_SkillEffectObj.SetActive(false);
			}
		}

		public void BeShowDizzyEffect(bool bShow)
		{
			_dizzyEffect.SetActive(bShow);
		}

		public void ShowNormalHitEffect(int index)
		{
			if(null == _attackEffects)
				return;

			if(BattleEnum.Enum_CharType.Monster != _charHandler.m_CharData.m_eType)
			{
				_attackEffects[index].SetActive(false);
				_attackEffects[index].SetActive(true);
			}
		}

		public void SetRingLight(string path)
		{
			_RingLightEffect = ResourceLoader.LoadAssetAndInstantiate(path, _charHandler.m_Transform);
			_RingLightEffect.transform.localScale = _RingLightEffect.transform.localScale / _charHandler.m_Transform.localScale.z;
		}

		public void ChangeRingLight(GameObject RingLightEffect)
		{
			_RingLightEffect = RingLightEffect;
			_RingLightEffect.transform.SetParent(_charHandler.m_Transform);
			_RingLightEffect.transform.localPosition = Vector3.zero;
			_RingLightEffect.transform.localScale = Vector3.one;
			_RingLightEffect.transform.localScale = _RingLightEffect.transform.localScale / _charHandler.m_Transform.localScale.z;
		}
	}
}
