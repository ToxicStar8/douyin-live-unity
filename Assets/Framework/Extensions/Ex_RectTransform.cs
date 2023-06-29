/*********************************************
 * BFramework
 * RectTransform方法扩展类
 * 创建时间：2023/01/08 20:40:23
 *********************************************/
using UnityEngine;

namespace Framework
{
    /// <summary>
    /// RectTransform扩展类
    /// </summary>
    public static class Ex_RectTransform
    {
        /// <summary>
        /// 设置 Anchor pos x
        /// </summary>
        public static RectTransform SetAnchorPosX(this RectTransform slefTr, float x)
        {
            Vector3 pos = slefTr.anchoredPosition;
            pos.x = x;
            slefTr.anchoredPosition = pos;
            return slefTr;
        }

        /// <summary>
        /// 设置 Anchor pos y
        /// </summary>
        public static RectTransform SetAnchorPosY(this RectTransform slefTr, float y)
        {
            Vector3 pos = slefTr.anchoredPosition;
            pos.y = y;
            slefTr.anchoredPosition = pos;
            return slefTr;
        }

        /// <summary>
        /// + Anchor pos x
        /// </summary>
        public static RectTransform AddAnchorPosX(this RectTransform slefTr, float x)
        {
            slefTr.anchoredPosition += new Vector2(x, 0);
            return slefTr;
        }

        /// <summary>
        /// + Anchor pos y
        /// </summary>
        public static RectTransform AddAnchorPosY(this RectTransform slefTr, float y)
        {
            slefTr.anchoredPosition += new Vector2(0, y);
            return slefTr;
        }

        /// <summary>
        /// 设置中心点 x
        /// </summary>
        public static RectTransform SetPiovtX(this RectTransform slefTr, float x)
        {
            Vector2 pos = slefTr.pivot;
            pos.x = x;
            slefTr.anchoredPosition = pos;
            return slefTr;
        }

        /// <summary>
        /// 设置中心点 y
        /// </summary>
        public static RectTransform SetPiovtY(this RectTransform slefTr, float y)
        {
            Vector2 pos = slefTr.pivot;
            pos.y = y;
            slefTr.anchoredPosition = pos;
            return slefTr;
        }

        /// <summary>
        /// 设置中心点 x
        /// </summary>
        public static RectTransform SetSizeDeltaX(this RectTransform slefTr, float x)
        {
            Vector2 pos = slefTr.sizeDelta;
            pos.x = x;
            slefTr.sizeDelta = pos;
            return slefTr;
        }

        /// <summary>
        /// 设置中心点 y
        /// </summary>
        public static RectTransform SetSizeDeltaY(this RectTransform slefTr, float y)
        {
            Vector2 pos = slefTr.sizeDelta;
            pos.y = y;
            slefTr.sizeDelta = pos;
            return slefTr;
        }

        /// <summary>
        /// RectTransform 随机一个点,必须是以中心点为锚点
        /// </summary>
        public static Vector2 RandomAnchorPosInRect(this RectTransform slefTr)
        {
            float x = UnityEngine.Random.Range(-slefTr.rect.width / 2, slefTr.rect.width / 2);
            float y = UnityEngine.Random.Range(-slefTr.rect.height / 2, slefTr.rect.height / 2);
            return new Vector2(x, y);
        }
    }
}
