/*********************************************
 * BFramework
 * 事件管理器
 * 创建时间：2023/01/08 20:40:23
 *********************************************/
using MainPackage;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    /// <summary>
    /// 事件管理器
    /// </summary>
    public class EventManager : ManagerBase
    {
        private Dictionary<ushort, Action<object[]>> _eventDic;

        public override void OnStart() 
        {
            _eventDic = new Dictionary<ushort, Action<object[]>>();
        }

        /// <summary>
        /// 添加监听
        /// </summary>
        public void AddEventListener(ushort eventNo, Action<object[]> callBack)
        {
            if (_eventDic.ContainsKey(eventNo))
            {
                GameGod.Instance.Log(E_Log.Error, "事件重复监听",eventNo.ToString());
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
                GameGod.Instance.Log(E_Log.Error, "事件不存在！");
            }
            callBack?.Invoke(args);
        }

        public override void OnUpdate() { }
        public override void OnDispose() 
        {
            _eventDic.Clear();
            _eventDic = null;
        }
    }
}
