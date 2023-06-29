/*********************************************
 * BFramework
 * 状态机状态类
 * 创建时间：2023/04/06 15:00:23
 *********************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Framework
{
    /// <summary>
    /// 状态机的状态
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class FsmState<T> where T : class
    {
        /// <summary>
        /// 持有该状态的状态机
        /// </summary>
        public Fsm<T> CurrFsm;

        public abstract void OnInit();
        /// <summary>
        /// 进入状态
        /// </summary>
        public abstract void OnEnter();

        /// <summary>
        /// 执行状态
        /// </summary>
        public abstract void OnUpdate();

        /// <summary>
        /// 离开状态
        /// </summary>
        public abstract void OnLeave();

        /// <summary>
        /// 状态机销毁时调用
        /// </summary>
        public abstract void OnDestroy();

    }
}