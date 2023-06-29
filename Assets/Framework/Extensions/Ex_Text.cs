/*********************************************
 * BFramework
 * 文本方法扩展类
 * 创建时间：2023/02/03 14:46:24
 *********************************************/
using UnityEngine.Events;
using UnityEngine.UI;

namespace Framework
{
    /// <summary>
    /// 文本扩展类
    /// </summary>
    public static class Ex_Text
    {
        /// <summary>
        /// 设置显隐
        /// </summary>
        public static void SetActive(this Text txt, bool isActive)
        {
            if (txt != null)
            {
                txt.gameObject.SetActive(isActive);
            }
        }
    }
}
