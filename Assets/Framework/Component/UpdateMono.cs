/*********************************************
 * BFramework
 * Update绑定类
 * 创建时间：2023/01/08 20:40:23
 *********************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class UpdateMono : MonoBehaviour
    {
        public Action UpdateCallBack;

        void Update()
        {
            UpdateCallBack?.Invoke();
        }
    }
}
