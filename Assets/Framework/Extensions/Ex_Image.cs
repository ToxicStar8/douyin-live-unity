/*********************************************
 * BFramework
 * 图片方法扩展类
 * 创建时间：2023/01/08 20:40:23
 *********************************************/
using UnityEngine.Events;
using UnityEngine.UI;

namespace Framework
{
    /// <summary>
    /// 图片扩展类
    /// </summary>
    public static class Ex_Image
    {
        /// <summary>
        /// 设置显隐
        /// </summary>
        public static void SetActive(this Image img, bool isActive)
        {
            if (img != null)
            {
                img.gameObject.SetActive(isActive);
            }
        }
    }
}
