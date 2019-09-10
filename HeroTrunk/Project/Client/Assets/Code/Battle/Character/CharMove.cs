using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace MS
{
	public class CharMove : MonoBehaviour
	{

		private CharHandler _charHandler;
		private CharData _charData;
		private Transform _transform;
		private float _fReachedOffset;
		private bool _bMove;
		private bool _bSlide;
		private Vector3 _tarPos;
		private Vector3 _dir;
		private UnityEngine.AI.NavMeshAgent _agent;

		void Awake()
		{
			_bMove = false;
			_bSlide = false;
		}

		public void SetCharHandler(CharHandler handler)
		{
			_charHandler = handler;
			_charData = handler.m_CharData;
			_transform = handler.m_Transform;
			_agent = _transform.gameObject.AddComponent<NavMeshAgent>();
			_agent.autoBraking = false;
			_agent.acceleration = 100f;
			_agent.angularSpeed = 360f;
			_agent.enabled = false;
		}

		public void SetRadius(float radius)
		{
			_agent.radius = radius;
		}

		public void SetObstacleAvoidanceType(UnityEngine.AI.ObstacleAvoidanceType type)
		{
			_agent.obstacleAvoidanceType = type;
		}

		public void SetObstacleAvoidancePriority(int priority)
		{
			_agent.avoidancePriority = priority;
		}

		public void SetAgentEnable(bool enable)
		{
			_agent.speed = _charData.m_fMoveSpeed;
			_agent.enabled = enable;
		}

		void Update()
		{
			if(_bMove)
			{
				if(BeArrived())
				{
					_agent.Stop();
					_charHandler.ToIdle();
				}
			}
			else if(_bSlide)
			{
				if(BeArrived())
					_charHandler.ToIdle();
				else
				{
					_dir = (_tarPos - _transform.position).normalized;
					Slide(_dir);
				}
			}
		}

		public void Move(Vector3 dir)
		{
			_transform.Translate(dir * Time.deltaTime * _charData.m_fMoveSpeed, Space.World);
			_transform.rotation = Quaternion.Lerp(_transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * _charData.m_fRotationSpeed * Mathf.Min(dir.magnitude, 0.2f));
		}

		public void Slide(Vector3 dir)
		{
			_transform.Translate(dir * Time.deltaTime * _charData.m_fSlideSpeed, Space.World);
			_transform.rotation = Quaternion.Lerp(_transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * _charData.m_fRotationSpeed * Mathf.Min(dir.magnitude, 0.2f));
		}

		public void StartAutoMove(Vector3 tarPos, float offset)
		{
			_tarPos = tarPos;
			_fReachedOffset = offset;
			_bMove = true;
			_bSlide = false;
			_agent.SetDestination(_tarPos);
			_agent.Resume();
		}

		public void StartSlide(Transform tarTrans, float offset)
		{
			_tarPos = tarTrans.position;
			_fReachedOffset = offset;
			_bSlide = true;
			_bMove = false;
			_agent.Stop();
		}

		public void StopAutoMove(bool bAIStop = false)
		{
			if(_agent.enabled)
			{
				_bMove = false;
				_bSlide = false;
				_agent.Stop();
			}
		}

		public bool BeArrived()
		{
			return (_transform.position - _tarPos).magnitude < _fReachedOffset;
		}

		#region --绘制辅助线--------------------------------------
		private float _fTheta = 0.1f;
		void OnDrawGizmos()
		{
			// 设置颜色
			Color defaultColor = Gizmos.color;
			Gizmos.color = Color.green;

			// 绘制圆环
			Vector3 beginPoint = Vector3.zero;
			Vector3 firstPoint = Vector3.zero;
			for(float theta = 0; theta < 2 * Mathf.PI; theta += _fTheta)
			{
				float x = _charData.m_fBodyRange * Mathf.Cos(theta) + _transform.position.x;
				float z = _charData.m_fBodyRange * Mathf.Sin(theta) + _transform.position.z;
				Vector3 endPoint = new Vector3(x, _transform.position.y + 0.1f, z);

				if(theta == 0)
					firstPoint = endPoint;
				else
					Gizmos.DrawLine(beginPoint, endPoint);

				beginPoint = endPoint;
			}

			// 绘制最后一条线段
			Gizmos.DrawLine(firstPoint, beginPoint);

			Gizmos.DrawLine(_transform.position + Vector3.up * 0.1f, _transform.position + Vector3.up * 0.1f + _transform.forward * _charData.m_fAtkRange);

			// 恢复默认颜色
			Gizmos.color = defaultColor;
		}
		#endregion
	}
}

