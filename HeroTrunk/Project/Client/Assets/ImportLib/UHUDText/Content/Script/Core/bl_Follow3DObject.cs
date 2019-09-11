using UnityEngine;
using System.Collections;
using MS;

/// <summary>
/// 物体血条跟随
/// </summary>
public class bl_Follow3DObject : MonoBehaviour {

    public delegate void OnVisibilityChange(bool isVisible);
    /// <summary>
    /// 显示隐藏事件.
    /// </summary>

    public OnVisibilityChange onChange;
    int mIsVisible = -1;

    /// <summary>
    /// 判断当前物体是否显示.
    /// </summary>

    public bool isVisible { get { return mIsVisible == 1; } }
    /// <summary>
    /// 跟随物
    /// </summary>
    public Transform target;

    public Camera MCamera;
    public Camera UICamera;

	private Transform _transform;

	private void Awake()
	{
		_transform = transform;
	}

	void Start()
    {
        if(MCamera==null)
            MCamera = BattleManager.GetInst().BattleCam;

        if(UICamera==null)
            UICamera = BattleManager.GetInst().UICam;
    }

    void LateUpdate()
    {      
        if (target != null)
        {
            Vector3 pos = MCamera.WorldToViewportPoint(target.position);

            //当前物体是否在相机内
            int isVisible = (MCamera.orthographic || pos.z > 0f) && (pos.x > 0f && pos.x < 1f && pos.y > 0f && pos.y < 1f) ? 1 : 0;
            bool vis = (isVisible == 1);
          
            Vector2 screenToWorldPoint = UICamera.ViewportToWorldPoint(pos);

			_transform.position = screenToWorldPoint;

            if (mIsVisible != isVisible)
            {
                mIsVisible = isVisible;
                for (int i = 0, imax = _transform.childCount; i < imax; ++i)
					_transform.GetChild(i).gameObject.SetActive(vis);
                // 发送一个显示改变事件
                if (onChange != null) onChange(vis);
            }
        }
    }
}
