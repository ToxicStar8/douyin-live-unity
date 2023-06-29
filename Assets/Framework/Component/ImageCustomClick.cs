/*********************************************
 * BFramework
 * 为点击不规则图形而生,参与渲染
 * 创建时间：2023/01/08 20:40:23
 *********************************************/
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 为点击不规则图形而生,参与渲染
/// </summary>
public class ImageCustomClick : Image
{
    private Collider2D[] mColliders;   // 可以带有多种碰撞区域

    protected override void Awake()
    {
        base.Awake();
        mColliders = GetComponents<Collider2D>();
        if ((mColliders != null) && (mColliders.Length == 0))
        {
            mColliders = null;
        }
        else
        {
            for (int i = 0; i < mColliders.Length; i++)
            {
                mColliders[i].enabled = false;      // 等需要判断的时候打开，减少性能消耗
            }
        }
        useLegacyMeshGeneration = false;
    }

    public override bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
    {
        if (mColliders != null)
        {
            bool valid = false;
            for (int i = 0; i < mColliders.Length; i++)
            {
                var collider = mColliders[i];
                collider.enabled = true;
                valid = collider.OverlapPoint(eventCamera.ScreenToWorldPoint(screenPoint));
                collider.enabled = false;
                if (valid)
                {
                    break;
                }
            }
            return valid;
        }
        return base.IsRaycastLocationValid(screenPoint, eventCamera);
    }
}
