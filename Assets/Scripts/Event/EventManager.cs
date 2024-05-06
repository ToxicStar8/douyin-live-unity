/*********************************************
 * BFramework
 * 事件管理器
 * 创建时间：2023/01/08 20:40:23
 *********************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    /// <summary>
    /// 事件管理器
    /// </summary>
    public class EventManager : InstanceBase<EventManager>
    {
        private Dictionary<ushort, Action<object[]>> _eventDic = new Dictionary<ushort, Action<object[]>>();

        /// <summary>
        /// 添加监听
        /// </summary>
        public void AddEventListener(ushort eventNo, Action<object[]> callBack)
        {
            if (_eventDic.ContainsKey(eventNo))
            {
                Debug.LogError("事件重复监听===>" + eventNo.ToString());
                _eventDic.Remove(eventNo);
            }
            _eventDic.Add(eventNo, callBack);
        }

        /// <summary>
        /// 移除监听
        /// </summary>
        public void RemoveEventListener(ushort eventNo, Action<object[]> callBack = null)
        {
            if (_eventDic.ContainsKey(eventNo))
            {
                _eventDic.Remove(eventNo);
            }
        }

        /// <summary>
        /// 发送事件
        /// </summary>
        public void SendEven(ushort eventNo,params object[] args)
        {
            if (!_eventDic.TryGetValue(eventNo, out var callBack))
            {
                Debug.LogError("事件不存在！");
            }
            callBack?.Invoke(args);
        }
    }
}
