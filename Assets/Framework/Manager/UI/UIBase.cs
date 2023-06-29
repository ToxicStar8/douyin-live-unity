/*********************************************
 * BFramework
 * UI基类
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
    /// UI基类
    /// </summary>
    public abstract class UIBase : GameBase
    {
        /// <summary>
        /// UI游戏对象
        /// </summary>
        public GameObject gameObject;

        /// <summary>
        /// UI游戏节点
        /// </summary>
        public RectTransform rectTransform;

        /// <summary>
        /// UI名字
        /// </summary>
        public string uiName;

        /// <summary>
        /// 加载器
        /// </summary>
        public LoadHelper LoadHelper;

        /// <summary>
        /// 关闭UI通用方法
        /// </summary>
        public void OnClose()
        {
            //回收加载器
            LoadHelper.Recycle(LoadHelper);

            //关闭前移除全部Update回调
            if (_update != null)
            {
                GameGod.Instance.UpdateCallback -= _update;
                _update = null;
            }

            //关闭前移除全部注册事件
            if (_eventList != null)
            {
                for (int i = 0, count = _eventList.Count; i < count; i++)
                {
                    RemoveEventListener(_eventList[i]);
                }
                _eventList.Clear();
                _eventList = null;
            }

            //关闭前移除全部定时器
            if (_timerList != null)
            {
                for (int i = 0, count = _timerList.Count; i < count; i++)
                {
                    RemoveTimer(_timerList[i]);
                }
                _timerList.Clear();
                _timerList = null;
            }

            //关闭前执行
            OnBeforDestroy();
        }

        /// <summary>
        /// 创建单独的UnitPool
        /// </summary>
        public UnitPool<T> CreateSinglePool<T>()where T : UnitBase,new()
        {
            var objName = typeof(T).Name.Split('_')[1] + ".prefab";
            var obj = LoadHelper.LoadSync<GameObject>(objName);
            var pool = new UnitPool<T>(this, obj, obj.activeSelf);
            return pool;
        }

        /// <summary>
        /// 获取UI节点
        /// </summary>
        public RectTransform GetUILevelTrans(E_UILevel uiLevel)
        {
            return GameGod.Instance.GetUILevelTrans(uiLevel);
        }

        #region Update
        private Action _update;
        public void RegisterUpdate(Action updateCallback)
        {
            if (_update != null)
            {
                GameGod.Instance.Log(E_Log.Error, gameObject.name,"Update重复注册！");
                return;
            }

            _update = updateCallback;
            GameGod.Instance.UpdateCallback += _update;
        }
        #endregion

        #region Event
        private List<ushort> _eventList;

        public override void AddEventListener(ushort eventNo, Action<object[]> callBack)
        {
            if (_eventList == null)
            {
                _eventList = new List<ushort>();
            }
            _eventList.Add(eventNo);
            base.AddEventListener(eventNo, callBack);
        }
        #endregion

        #region Countdown
        private List<string> _timerList;

        /// <summary>
        /// 添加定时器监听
        /// </summary>
        public override void AddTimer(string timeName, TimerInfo countdownData)
        {
            if (_timerList == null)
            {
                _timerList = new List<string>();
            }

            _timerList.Add(timeName);
            base.AddTimer(timeName, countdownData);
        }
        #endregion

        /// <summary>
        /// 加载组件
        /// </summary>
        public abstract void OnCreate();

        /// <summary>
        /// 初始化
        /// </summary>
        public abstract void OnInit();

        /// <summary>
        /// 每次打开时调用
        /// </summary>
        public abstract void OnShow(params object[] args);

        /// <summary>
        /// 销毁前调用
        /// </summary>
        public abstract void OnBeforDestroy();
    }
}