/*********************************************
 * BFramework
 * 状态机基类
 * 创建时间：2023/04/06 15:00:23
 *********************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Framework
{
    /// <summary>
    /// 状态机基类
    /// </summary>
    public abstract class FsmBase
    {
        /// <summary>
        /// 状态机编号
        /// </summary>
        public int FsmId { get; private set; }

        /// <summary>
        /// 当前状态的类型
        /// </summary>
        public sbyte CurrStateType;

        public FsmBase(int fsmId)
        {
            FsmId = fsmId; 
        }
        
        public abstract void OnUpdate();

        /// <summary>
        /// 关闭状态机
        /// </summary>
        public abstract void OnClose();
    }
}