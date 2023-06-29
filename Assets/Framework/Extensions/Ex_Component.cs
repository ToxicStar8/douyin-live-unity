/*********************************************
 * BFramework
 * Component扩展类
 * 创建时间：2023/04/25 16:16:23
 *********************************************/
using UnityEngine;

namespace Framework
{
    public static class Ex_Component
    {
        /// <summary>
        /// 激活/隐藏对象
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        public static Component SetActive(this Component comp , bool isActive)
        {
            if (comp != null)
            {
                var go = comp.gameObject;
                if (go.activeSelf != isActive)
                {
                    comp.gameObject.SetActive(isActive);
                }
            }
            return comp;
        }

        /// <summary>
        /// 反转激活/隐藏对象
        /// </summary>
        /// <param name="comp"></param>
        /// <returns></returns>
        public static Component SetReverseActive(this Component comp)
        {
            if (comp != null)
            {
                comp.gameObject.SetActive(!comp.gameObject.activeSelf);
            }
            return comp;
        }

        /// <summary>
        /// 激活/隐藏父对象
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        public static Component SetParentActive(this Component comp, bool isActive)
        {
            if (comp != null)
            {
                comp.transform.parent.SetActive(isActive);
            }
            return comp;
        }

        /// <summary>
        /// 销毁自己
        /// </summary>
        /// <param name="comp"></param>
        public static void DestroySelf(this Component comp)
        {
            if (comp != null)
            {
                Object.Destroy(comp);
            }
        }

        /// <summary>
        /// 销毁所有子对象
        /// </summary>
        public static void DestroyAllChildren(this Component comp)
        {
            Transform tr = comp.transform;
            for (int i = tr.childCount - 1; i >= 0; i--)
            {
                Object.DestroyImmediate(tr.GetChild(i).gameObject);
            }
        }

        /// <summary>
        /// 销毁对象
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        public static void DestroyGameobject(this Component comp)
        {
            if (comp != null)
            {
                Object.Destroy(comp.gameObject);
            }
        }

        /// <summary>
        /// 初始对象
        /// </summary>
        /// <param name="comp"></param>
        public static void SetLocalIdentity(this Component comp)
        {
            Transform tr = comp as Transform ?? comp.transform;
            tr.localPosition = Vector3.zero;
            tr.localRotation = Quaternion.identity;
            tr.localScale = Vector3.one;
        }

        /// <summary>
        /// 初始对象
        /// </summary>
        /// <param name="comp"></param>
        public static void SetIdentity(this Component comp)
        {
            Transform tr = comp as Transform ?? comp.transform;
            tr.position = Vector3.zero;
            tr.rotation = Quaternion.identity;
            tr.localScale = Vector3.one;
        }

        /// <summary>
        /// 组件上添加脚本
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="comp"></param>
        /// <returns></returns>
        //public static T AddComponent<T>(this Component comp) where T: Component
        //{
        //    if(comp == null)
        //    {
        //        return null;
        //    }
        //    return comp.gameObject.AddComponent<T>() as T;
        //}

        /// <summary>
        /// 是否激活自己
        /// </summary>
        public static bool ActiveSelf(this Component comp)
        { 
            if (comp == null)
            {
                return false;
            }
            return comp.gameObject.activeSelf;
        }

        /// <summary>
        /// 是否在面板上激活自己
        /// </summary>
        public static bool ActiveInHierarchy(this Component comp)
        {
            if (comp == null)
            {
                return false;
            }
            return comp.gameObject.activeInHierarchy;
        }

        /// <summary>
        /// 销毁自己
        /// </summary>
        /// <param name="comp"></param>
        public static void Destory(this Component comp)
        {
            if (comp != null)
            {
                Object.DestroyImmediate(comp);
            }
        }

        public static T FindChild<T>(this Component comp, string path) where T : Component
        {
            Transform tr = comp.transform.Find(path);
            if(tr != null)
            {
                return tr.GetComponent<T>();
            }
            return null;
        }
    }
}
