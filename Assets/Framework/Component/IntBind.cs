/*********************************************
 * BFramework
 * Int绑定
 * 创建时间：2023/01/08 20:40:23
 *********************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Framework
{
    /// <summary>
    /// Int绑定
    /// </summary>
    public class IntBind : MonoBehaviour
    {
        /// <summary>
        /// 游戏对象
        /// </summary>
        [SerializeField]
        public int Value;

        /// <summary>
        /// 说明
        /// </summary>
        [SerializeField]
        public string Desc;
    }
}
