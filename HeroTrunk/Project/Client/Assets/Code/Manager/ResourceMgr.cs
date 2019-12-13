using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MS
{
	public class ResourceMgr : MonoBehaviour
	{
		private static ResourceMgr _inst;
		public static ResourceMgr GetInst()
		{
			return _inst;
		}

		private void Awake()
		{
			_inst = this;
		}

		private void OnDestroy()
		{
			_inst = null;
		}

		private Stack<Transform> _stkTapLightning = new Stack<Transform>();
		public void PopTapLightning(Vector3 pos)
		{
			Transform trans = _stkTapLightning.Count > 0 ? _stkTapLightning.Pop() : ResourceLoader.LoadAssetAndInstantiate("Effect/Skill/Hit/EftHitFlash", BattleManager.GetInst().BattlePoolTrans).transform;
			trans.position = pos;
			trans.gameObject.SetActive(false);
			trans.gameObject.SetActive(true);
			StartCoroutine(PushTapLightning(trans));
		}

		private IEnumerator PushTapLightning(Transform trans)
		{
			yield return new WaitForSeconds(0.55f);
			_stkTapLightning.Push(trans);
		}

		private Stack<Transform> _stkSoulBall = new Stack<Transform>();
		public void PopSoulBall(CharHandler h, int count)
		{
			for(int i = 0; i < count; ++i)
			{
				Transform trans = _stkSoulBall.Count > 0 ? _stkSoulBall.Pop() : ResourceLoader.LoadAssetAndInstantiate("Effect/__/SoulBall", BattleManager.GetInst().BattlePoolTrans).transform;
				trans.position = h.m_CharBody.Chest.position;
				trans.gameObject.SetActive(false);
				trans.gameObject.SetActive(true);
				trans.Rotate(Vector3.up, Random.Range(0, 360));
				trans.DOMove(h.m_ParentTrans.position + Vector3.up * 0.5f + trans.forward, 1f).SetEase(Ease.OutCirc);
				StartCoroutine(PushSoulBall(trans, 5));
			}
		}

		private Vector3 _vecSoulBallPos = new Vector3(22f, 1f, 10f);
		private IEnumerator PushSoulBall(Transform trans, int v)
		{
			yield return new WaitForSeconds(2f);
			trans.DOMove(_vecSoulBallPos, 0.3f).OnComplete(()=>{ _PushSoulBall(trans, v); });
		}

		private void _PushSoulBall(Transform trans, int v)
		{
			trans.gameObject.SetActive(false);
			_stkSoulBall.Push(trans);
			BattleManager.GetInst().m_BattleScene.Coin += v;
		}
	}
}
