using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MS
{
	public class HUDTextItem : MonoBehaviour
	{
		private Camera MCamera;
		private Camera UICamera;

		private Text m_text;
		private Gradient gradient;
		private Vector3 m_createPos;

		private Color enemyNorHitColTop;
		private Color enemyNorHitColBtm;

		private Color enemySkiHitColTop;
		private Color enemySkiHitColBtm;

		private Color mineHitColTop;
		private Color mineHitColBtm;

		private Color hpColTop;
		private Color hpColBtm;

		private Color buffColTop;
		private Color buffColBtm;

		private void Awake()
		{

			MCamera = GameObject.FindGameObjectWithTag("FightCamera").GetComponent<Camera>();
			UICamera = GameObject.FindGameObjectWithTag("UICamera").GetComponent<Camera>();

			m_text = GetComponent<Text>();
			gradient = GetComponent<Gradient>();

			enemyNorHitColTop = new Color(134 / 255.0f, 197 / 255.0f, 255 / 255.0f, 1);
			enemyNorHitColBtm = new Color(191 / 255.0f, 230 / 255.0f, 255 / 255.0f, 1);

			mineHitColTop = new Color(218 / 255.0f, 11 / 255.0f, 11 / 255.0f, 1);
			mineHitColBtm = new Color(255 / 255.0f, 104 / 255.0f, 104 / 255.0f, 1);

			enemySkiHitColTop = new Color(255 / 255.0f, 233 / 255.0f, 91 / 255.0f, 1);
			enemySkiHitColBtm = new Color(255 / 255.0f, 230 / 255.0f, 156 / 255.0f, 1);

			hpColTop = new Color(20 / 255.0f, 178 / 255.0f, 24 / 255.0f, 1);
			hpColBtm = new Color(133 / 255.0f, 214 / 255.0f, 91 / 255.0f, 1);

			buffColTop = new Color(60 / 255.0f, 190 / 255.0f, 175 / 255.0f, 1);
			buffColBtm = new Color(127 / 255.0f, 244 / 255.0f, 254 / 255.0f, 1);
		}

		private void LateUpdate()
		{
			if(this.gameObject.activeSelf)
			{
				Vector3 pos = MCamera.WorldToViewportPoint(m_createPos);
				Vector2 screenToWorldPoint = UICamera.ViewportToWorldPoint(pos);
				transform.parent.position = screenToWorldPoint;
			}
		}

		private void Init(string text, Vector3 createPos)
		{
			m_createPos = createPos + Vector3.up;    //与血条分开,比血条高一点
			transform.localPosition = Vector3.zero;  //重置位置

			Color c = m_text.color;     //重置alpha为1,开始显示
			c.a = 1;
			m_text.color = c;

			m_text.text = text;
		}

		public void Show(string text, CharHandler handler, HUDTextMgr.HUDTextType type, Vector3 createPos)
		{
			Init(text, createPos);

			if(text.IndexOf(ConfigData.GetStaticText("10009")) > -1)//士气特殊显示
			{
				MPShow();
				return;
			}

			switch(type)
			{
				case HUDTextMgr.HUDTextType.NormalHit:
					NormalHitShow(text, handler);
					break;
				case HUDTextMgr.HUDTextType.SkillHit:
					SkillHitShow(text, handler);
					break;
				case HUDTextMgr.HUDTextType.MainSkillName:
					MainSkillNameShow(text, handler);
					break;
				case HUDTextMgr.HUDTextType.NormalSkillName:
					NormalSkillNameShow(text, handler);
					break;
				case HUDTextMgr.HUDTextType.AbsorbHP:
					AbsorbHPShow(text, handler);
					break;
				case HUDTextMgr.HUDTextType.BUFF:
					BuffShow(text, handler);
					break;
			}

		}

		private void MPShow()
		{
			m_text.fontSize = 30;

			gradient.topColor = enemySkiHitColTop;
			gradient.bottomColor = enemySkiHitColBtm;

			TextAnimetor(m_text);
		}

		private void NormalHitShow(string text, CharHandler handler)
		{
			m_text.fontSize = 30;
			if(handler.m_CharData.m_eSide == BattleEnum.Enum_CharSide.Mine)
			{
				gradient.topColor = mineHitColTop;
				gradient.bottomColor = mineHitColBtm;
			}
			else
			{
				gradient.topColor = enemyNorHitColTop;
				gradient.bottomColor = enemyNorHitColBtm;
			}

			TextAnimetor(m_text);
		}

		private void SkillHitShow(string text, CharHandler handler)
		{
			m_text.fontSize = 30;
			if(handler.m_CharData.m_eSide == BattleEnum.Enum_CharSide.Mine)
			{
				gradient.topColor = mineHitColTop;
				gradient.bottomColor = mineHitColBtm;
			}
			else
			{
				gradient.topColor = enemySkiHitColTop;
				gradient.bottomColor = enemySkiHitColBtm;
			}

			TextAnimetor(m_text);
		}

		private void MainSkillNameShow(string text, CharHandler handler)
		{
			m_text.fontSize = 36;
			if(handler.m_CharData.m_eSide == BattleEnum.Enum_CharSide.Mine)
			{
				gradient.topColor = Color.yellow;
				gradient.bottomColor = Color.yellow;
			}
			else
			{
				gradient.topColor = Color.yellow;
				gradient.bottomColor = Color.yellow;
			}

			SkillAnimetor(m_text);
		}

		private void NormalSkillNameShow(string text, CharHandler handler)
		{
			m_text.fontSize = 34;
			if(handler.m_CharData.m_eSide == BattleEnum.Enum_CharSide.Mine)
			{
				gradient.topColor = Color.cyan;
				gradient.bottomColor = Color.cyan;
			}
			else
			{
				gradient.topColor = Color.cyan;
				gradient.bottomColor = Color.cyan;
			}

			SkillAnimetor(m_text);
		}

		private void AbsorbHPShow(string text, CharHandler handler)
		{
			m_text.fontSize = 30;

			gradient.topColor = hpColTop;
			gradient.bottomColor = hpColBtm;

			TextAnimetor(m_text);
		}

		private void BuffShow(string text, CharHandler handler)
		{
			m_text.fontSize = 32;

			gradient.topColor = buffColTop;
			gradient.bottomColor = buffColBtm;

			TextAnimetor(m_text);
		}


		private void TextAnimetor(Text text)
		{
			Sequence mySequence = DOTween.Sequence();

			mySequence.Append(text.transform.DOScale(Vector3.one * 2f, 0.1f));
			mySequence.Append(text.transform.DOScale(Vector3.one, 0.1f));
			mySequence.AppendInterval(0.1f);
			mySequence.Append(text.transform.DOLocalMoveY(text.transform.localPosition.y + 30, 0.2f).SetEase(Ease.Linear));
			mySequence.Join(text.DOFade(0.7f, 0.2f));
			mySequence.Append(text.transform.DOLocalMoveY(text.transform.localPosition.y + 50, 0.2f).SetEase(Ease.Linear));
			mySequence.Join(text.DOFade(0, 0.2f));
			mySequence.OnComplete(() => HUDTextMgr.GetInst().GetBackText(this));
		}

		private void SkillAnimetor(Text text)
		{
			Sequence mySequence = DOTween.Sequence();
			mySequence.AppendInterval(1f);

			mySequence.Append(text.DOFade(0, 0.2f));
			mySequence.OnComplete(() => HUDTextMgr.GetInst().GetBackSkillText(this));
		}
	}
}
