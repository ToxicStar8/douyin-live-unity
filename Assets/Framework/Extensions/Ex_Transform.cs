/*********************************************
 * BFramework
 * Transform扩展类
 * 创建时间：2023/04/25 16:13:23
 *********************************************/
using UnityEngine;

namespace Framework
{
    /// <summary>
    /// Transform扩展类
    /// </summary>
    public static class Ex_Transform
    {
        /// <summary>
        /// 设置世界坐标
        /// </summary>
        public static Transform SetPosition(this Transform slefTr, Vector3 pos)
        {
            slefTr.position = pos;
            return slefTr;
        }

        /// <summary>
        /// 单个X左边设置
        /// </summary>
        public static Transform SetPositionX(this Transform slefTr, float x)
        {
            Vector3 pos = slefTr.position;
            pos.x = x;
            slefTr.position = pos;
            return slefTr;
        }

        /// <summary>
        /// X增量
        /// </summary>
        /// <param name="slefTr"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static Transform AddPositionX(this Transform slefTr, float x)
        {
            Vector3 pos = slefTr.position;
            pos.x += x;
            slefTr.position = pos;
            return slefTr;
        }

        /// <summary>
        /// 单个Y坐标设置
        /// </summary>
        public static Transform SetPositionY(this Transform slefTr, float y)
        {
            Vector3 pos = slefTr.position;
            pos.y = y;
            slefTr.position = pos;
            return slefTr;
        }

        /// <summary>
        /// Y增量
        /// </summary>
        public static Transform AddPositionY(this Transform slefTr, float y)
        {
            Vector3 pos = slefTr.position;
            pos.y += y;
            slefTr.position = pos;
            return slefTr;
        }

        /// <summary>
        /// 单个Z坐标设置
        /// </summary>
        public static Transform SetPositionZ(this Transform slefTr, float z)
        {
            Vector3 pos = slefTr.position;
            pos.z = z;
            slefTr.position = pos;
            return slefTr;
        }

        /// <summary>
        /// z增量
        /// </summary>
        /// <param name="slefTr"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static Transform AddPositionZ(this Transform slefTr, float z)
        {
            Vector3 pos = slefTr.position;
            pos.z += z;
            slefTr.position = pos;
            return slefTr;
        }

        /// <summary>
        /// 设置局部世界
        /// </summary>
        public static Transform SetLocalPosition(this Transform slefTr, Vector3 pos)
        {
            slefTr.localPosition = pos;
            return slefTr;
        }

        /// <summary>
        /// 单个 X Local设置
        /// </summary>
        public static Transform SetLocalPositionX(this Transform slefTr, float x)
        {
            Vector3 pos = slefTr.localPosition;
            pos.x = x;
            slefTr.localPosition = pos;
            return slefTr;
        }

        /// <summary>
        /// 单个 Y Local坐标设置
        /// </summary>
        public static Transform SetLocalPositionY(this Transform slefTr, float y)
        {
            Vector3 pos = slefTr.localPosition;
            pos.y = y;
            slefTr.localPosition = pos;
            return slefTr;
        }

        /// <summary>
        /// 单个 Z Local坐标设置
        /// </summary>
        public static Transform SetLocalPositionZ(this Transform slefTr, float z)
        {
            Vector3 pos = slefTr.localPosition;
            pos.z = z;
            slefTr.localPosition = pos;
            return slefTr;
        }

        /// <summary>
        /// x旋转设置
        /// </summary>
        public static Transform SetEulerAngleX(this Transform slefTr, float x)
        {
            Vector3 angle = slefTr.eulerAngles;
            angle.x = x;
            slefTr.eulerAngles = angle;
            return slefTr;
        }

        /// <summary>
        /// y旋转设置
        /// </summary>
        public static Transform SetEulerAngleY(this Transform slefTr, float y)
        {
            Vector3 angle = slefTr.eulerAngles;
            angle.y = y;
            slefTr.eulerAngles = angle;
            return slefTr;
        }

        /// <summary>
        /// z旋转设置
        /// </summary>
        public static Transform SetEulerAngleZ(this Transform slefTr, float z)
        {
            Vector3 angle = slefTr.eulerAngles;
            angle.z = z;
            slefTr.eulerAngles = angle;
            return slefTr;
        }

        /// <summary>
        /// x local旋转设置
        /// </summary>
        public static Transform SetLocalEulerAngleX(this Transform slefTr, float x)
        {
            Vector3 angle = slefTr.localEulerAngles;
            angle.x = x;
            slefTr.localEulerAngles = angle;
            return slefTr;
        }

        /// <summary>
        /// y local旋转设置
        /// </summary>
        public static Transform SetLocalEulerAngleY(this Transform slefTr, float y)
        {
            Vector3 angle = slefTr.localEulerAngles;
            angle.y = y;
            slefTr.localEulerAngles = angle;
            return slefTr;
        }

        /// <summary>
        /// z local旋转设置
        /// </summary>
        public static Transform SetLocalEulerAngleZ(this Transform slefTr, float z)
        {
            Vector3 angle = slefTr.localEulerAngles;
            angle.z = z;
            slefTr.localEulerAngles = angle;
            return slefTr;
        }

        /// <summary>
        /// x缩放设置
        /// </summary>
        public static Transform SetLocalScaleX(this Transform slefTr, float x)
        {
            Vector3 scale = slefTr.localScale;
            scale.x = x;
            slefTr.localScale = scale;
            return slefTr;
        }

        /// <summary>
        /// y缩放设置
        /// </summary>
        public static Transform SetLocalScaleY(this Transform slefTr, float y)
        {
            Vector3 scale = slefTr.localScale;
            scale.y = y;
            slefTr.localScale = scale;
            return slefTr;
        }

        /// <summary>
        /// xy缩放
        /// </summary>
        public static Transform SetLocalScaleXY(this Transform slefTr, float s)
        {
            Vector3 scale = slefTr.localScale;
            scale.x = s;
            scale.y = s;
            slefTr.localScale = scale;
            return slefTr;
        }

        /// <summary>
        /// 缩放
        /// </summary>
        public static Transform SetLocalScale(this Transform slefTr, Vector3 s)
        {
            slefTr.localScale = s;
            return slefTr;
        }

        /// <summary>
        /// 缩放
        /// </summary>
        public static Transform SetLocalScale(this Transform slefTr, float s)
        {
            slefTr.localScale = new Vector3(s, s, s);
            return slefTr;
        }

        /// <summary>
        /// z缩放设置
        /// </summary>
        public static Transform SetLocalScaleZ(this Transform slefTr, float z)
        {
            Vector3 pos = slefTr.localScale;
            pos.z = z;
            slefTr.localScale = pos;
            return slefTr;
        }

        /// <summary>
        /// 全局复位
        /// </summary>
        /// <param name="selfTr"></param>
        /// <returns></returns>
        public static Transform Reset(this Transform selfTr)
        {
            selfTr.position = Vector3.zero;
            selfTr.localScale = Vector3.one;
            selfTr.rotation = Quaternion.identity;

            return selfTr;
        }

        /// <summary>
        /// 局部复位
        /// </summary>
        /// <param name="selfTr"></param>
        /// <returns></returns>
        public static Transform ResetLocal(this Transform selfTr)
        {
            selfTr.localPosition = Vector3.zero;
            selfTr.localScale = Vector3.one;
            selfTr.localRotation = Quaternion.identity;

            return selfTr;
        }

        /// <summary>
        /// 隐藏所有子对象
        /// </summary>
        /// <param name="selfTr"></param>
        /// <returns></returns>
        public static Transform HideAllChildren(this Transform selfTr)
        {
            for (int i = 0; i < selfTr.childCount; i++)
            {
                selfTr.GetChild(i).SetActive(false);
            }
            return selfTr;
        }

        /// <summary>
        /// 显示子对象
        /// </summary>
        /// <param name="selfTr"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Transform ActiveChild(this Transform selfTr, string path, bool enable = true)
        {
            Transform child = selfTr.Find(path);
            child.SetActive(enable);
            return child;
        }
    }
}
