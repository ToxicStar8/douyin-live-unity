/*********************************************
 * BFramework
 * UI代码生成绑定
 * 创建时间：2023/01/08 20:40:23
 *********************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Framework
{
    /// <summary>
    /// UI代码生成绑定
    /// </summary>
    public class UIBind : MonoBehaviour
    {
#if UNITY_EDITOR
        /// <summary>
        /// 游戏对象
        /// </summary>
        [SerializeField]
        public Object BindComponent;

        /// <summary>
        /// 绑定类型
        /// </summary>
        [SerializeField]
        public BindType Type;

        /// <summary>
        /// 备注
        /// </summary>
        [SerializeField]
        public string Content;

        private void Reset()
        {
            Type = BindType.Component;
            ResetBind();
        }

        public void ResetBind()
        {
            BindComponent = null;
            BindComponent = BindComponent ?? GetComponent<LoopScrollRect>();
            BindComponent = BindComponent ?? GetComponent<ScrollRect>();
            BindComponent = BindComponent ?? GetComponent<Button>();
            BindComponent = BindComponent ?? GetComponent<InputField>();
            BindComponent = BindComponent ?? GetComponent<Image>();
            BindComponent = BindComponent ?? GetComponent<Toggle>();
            BindComponent = BindComponent ?? GetComponent<Slider>();
            BindComponent = BindComponent ?? GetComponent<Text>();
            BindComponent = BindComponent ?? GetComponent<RectTransform>();
        }
#endif
    }

    public enum BindType
    {
        Component,
        Unit,
    }
}
