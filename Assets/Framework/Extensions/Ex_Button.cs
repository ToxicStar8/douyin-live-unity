/*********************************************
 * BFramework
 * 按钮方法扩展类
 * 创建时间：2023/01/08 20:40:23
 *********************************************/
using UnityEngine.Events;
using UnityEngine.UI;

namespace Framework
{
    /// <summary>
    /// 按钮扩展类
    /// </summary>
    public static class Ex_Button
    {
        /// <summary>
        /// 添加按钮监听
        /// </summary>
        public static void AddListener(this Button btn, UnityAction onClick)
        {
            if (btn != null)
            {
                btn.onClick.AddListener(onClick);
            }
        }

        /// <summary>
        /// 清空按钮指定监听
        /// </summary>
        public static void RemoveListener(this Button btn, UnityAction onClick)
        {
            if (btn != null)
            {
                btn.onClick.RemoveListener(onClick);
            }
        }

        /// <summary>
        /// 清空按钮监听
        /// </summary>
        public static void RemoveAllListeners(this Button btn)
        {
            if (btn != null)
            {
                btn.onClick.RemoveAllListeners();
            }
        }

        /// <summary>
        /// 设置显隐
        /// </summary>
        public static void SetActive(this Button btn, bool isActive)
        {
            if (btn != null)
            {
                btn.gameObject.SetActive(isActive);
            }
        }
    }
}
