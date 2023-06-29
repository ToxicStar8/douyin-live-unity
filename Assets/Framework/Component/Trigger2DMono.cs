/*********************************************
 * BFramework
 * 触发绑定类
 * 创建时间：2023/05/10 13:49:23
 *********************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class Trigger2DMono : MonoBehaviour
    {
        public Action<Collider2D> OnTrigger2DEnterCallback;
        public Action<Collider2D> OnTrigger2DStayCallback;
        public Action<Collider2D> OnTrigger2DExitCallback;

        //触发器原生方法
        private void OnTriggerEnter2D(Collider2D collision)
        {
            OnTrigger2DEnterCallback?.Invoke(collision);
        }
        private void OnTriggerStay2D(Collider2D collision)
        {
            OnTrigger2DStayCallback?.Invoke(collision);
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            OnTrigger2DExitCallback?.Invoke(collision);
        }
    }
}
