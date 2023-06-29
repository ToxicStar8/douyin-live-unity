/*********************************************
 * BFramework
 * 游戏对象方法扩展类
 * 创建时间：2023/01/08 20:40:23
 *********************************************/
using UnityEngine;

namespace Framework
{
    /// <summary>
    /// 游戏对象扩展类
    /// </summary>
    public static class Ex_GameObject
    {
        public static void SetParent(this GameObject gameObject, Transform trans)
        {
            if (gameObject != null)
            {
                gameObject.transform.SetParent(trans);
            }
        }

        public static void SetScale(this GameObject gameObject, Vector3 v3)
        {
            if (gameObject != null)
            {
                gameObject.transform.localScale = v3;
            }
        }

        public static void SetLocalPos(this GameObject gameObject, Vector3 v3)
        {
            if (gameObject != null)
            {
                gameObject.transform.localPosition = v3;
            }
        }

        public static void SetLocalRotation(this GameObject gameObject, Quaternion quaternion)
        {
            if (gameObject != null)
            {
                gameObject.transform.localRotation = quaternion;
            }
        }

        public static void Destroy(this GameObject gameObject)
        {
            if (gameObject != null)
            {
                Object.Destroy(gameObject);
            }
        }
    }
}
