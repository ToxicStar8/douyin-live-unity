/*********************************************
 * BFramework
 * PanelUnit基类
 * 创建时间：2023/01/08 20:40:23
 *********************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    /// <summary>
    /// PanelUnit基类
    /// </summary>
    public abstract class UnitBase : GameBase
    {
        /// <summary>
        /// Unit游戏对象
        /// </summary>
        public GameObject gameObject;

        /// <summary>
        /// Unit游戏节点
        /// </summary>
        public RectTransform rectTransform;

        /// <summary>
        /// 加载器
        /// </summary>
        public LoadHelper LoadHelper;

        /// <summary>
        /// 加载组件
        /// </summary>
        public abstract void OnCreate();

        /// <summary>
        /// 初始化
        /// </summary>
        public abstract void OnInit();
    }
}