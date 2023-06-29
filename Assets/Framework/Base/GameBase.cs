/*********************************************
 * BFramework
 * 所有游戏对象的基类 存放通用方法
 * 创建时间：2023/01/08 20:40:23
 *********************************************/
using MainPackage;
using System;
using UnityEngine;

namespace Framework
{
    /// <summary>
    /// 所有游戏对象的基类 存放通用方法
    /// </summary>
    public abstract class GameBase
    {
        #region Event 
        public virtual void SendEven(ushort eventNo, params object[] args)
        {
            GameGod.Instance.EventManager.SendEven(eventNo, args);
        }
        public virtual void AddEventListener(ushort eventNo, Action<object[]> callBack)
        {
            GameGod.Instance.EventManager.AddEventListener(eventNo, callBack);
        }
        public virtual void RemoveEventListener(ushort eventNo, Action<object[]> callBack = null)
        {
            GameGod.Instance.EventManager.RemoveEventListener(eventNo, callBack);
        }
        #endregion

        #region Pool
        /// <summary>
        /// 创建类对象池
        /// </summary>
        public virtual ClassObjectPool<T> CreateClassPool<T>() where T : class, new()
        {
            return GameGod.Instance.PoolManager.CreateClassObjectPool<T>();
        }
        /// <summary>
        /// 在对象池中创建类对象
        /// </summary>
        public virtual T CreateClassObj<T>() where T : class, new()
        {
            return GameGod.Instance.PoolManager.CreateClassObj<T>();
        }
        /// <summary>
        /// 回收类到池中
        /// </summary>
        public virtual void RecycleClassObj<T>(T obj) where T : class, new()
        {
            GameGod.Instance.PoolManager.RecycleClassObj<T>(obj);
        }

        /// <summary>
        /// 创建游戏对象池
        /// </summary>
        public virtual GameObjectPool CreateGameObjectPool(string objName)
        {
            return GameGod.Instance.PoolManager.CreateGameObjectPool(objName);
        }
        /// <summary>
        /// 在对象池中创建游戏对象
        /// </summary>
        public virtual GameObject CreateGameObject(string objName, Transform trans = null)
        {
            return GameGod.Instance.PoolManager.CreateGameObject(objName, trans);
        }
        /// <summary>
        /// 回收游戏对象到池中
        /// </summary>
        public virtual void RecycleGameObject(GameObject go)
        {
            GameGod.Instance.PoolManager.RecycleGameObject(go);
        }
        #endregion

        #region Table
        /// <summary>
        /// 获取配表
        /// </summary>
        public virtual T GetTableCtrl<T>() where T : class, ITableCtrlBase
        {
            return GameGod.Instance.TableManager.GetTableCtrl<T>();
        }
        #endregion

        #region Sprite
        /// <summary>
        /// 加载Sprite
        /// </summary>
        public virtual Sprite GetSprite(LoadHelper loadHelper, string atlasName, string spriteName)
        {
            var sp = loadHelper.GetSprite(atlasName, spriteName);
            return sp;
        }
        #endregion

        #region UI
        /// <summary>
        /// 打开UI
        /// </summary>
        public virtual void OpenUI<T>(E_UILevel uiLevel = E_UILevel.Common, params object[] args) where T : UIBase, new()
        {
            GameGod.Instance.UIManager.OpenUI<T>(uiLevel, args);
        }

        /// <summary>
        /// 隐藏UI
        /// </summary>
        public virtual void HideUI<T>() where T : UIBase, new()
        {
            GameGod.Instance.UIManager.HideUI<T>();
        }

        /// <summary>
        /// 隐藏UI
        /// </summary>
        public virtual void HideUI(string uiName)
        {
            GameGod.Instance.UIManager.HideUI(uiName);
        }

        /// <summary>
        /// 关闭UI
        /// </summary>
        public virtual void CloseUI<T>() where T : UIBase, new()
        {
            GameGod.Instance.UIManager.CloseUI<T>();
        }

        /// <summary>
        /// 关闭UI
        /// </summary>
        public virtual void CloseUI(string uiName)
        {
            GameGod.Instance.UIManager.CloseUI(uiName);
        }
        #endregion

        #region Audio
        public virtual void PlayBGM(string audioName)
        {
            GameGod.Instance.AudioManager.PlayBackground(audioName);
        }

        public virtual void PlaySound(string audioName)
        {
            GameGod.Instance.AudioManager.PlaySound(audioName);
        }
        #endregion

        #region Timer
        /// <summary>
        /// 添加定时器监听
        /// </summary>
        public virtual void AddTimer(string timeName, TimerInfo timerInfo)
        {
            GameGod.Instance.TimeManager.AddTimer(timeName, timerInfo);
        }
        /// <summary>
        /// 添加一次性定时器监听，执行次数永远不能为-1，即无限，否则无限循环无法跳出
        /// </summary>
        public virtual void AddTempTimer(TimerInfo timerInfo)
        {
            GameGod.Instance.TimeManager.AddTempTimer(timerInfo);
        }
        /// <summary>
        /// 获取定时器信息
        /// </summary>
        public virtual TimerInfo GetTimerInfo(string timeName)
        {
            return GameGod.Instance.TimeManager.GetTimerInfo(timeName);
        }
        /// <summary>
        /// 移除定时器监听
        /// </summary>
        public virtual void RemoveTimer(string timeName, TimerInfo timerInfo = null)
        {
            GameGod.Instance.TimeManager.RemoveTimer(timeName);
        }
        #endregion

        #region Fsm
        public Fsm<T> CreateFsm<T>(T owner, FsmState<T>[] states) where T : class
        {
            var fsm = GameGod.Instance.FsmManager.CreateFsm<T>(owner, states);
            return fsm;
        }
        public void RelaseFsm(int fsmId)
        {
            GameGod.Instance.FsmManager.RelaseFsm(fsmId);
        }
        #endregion

        #region Net
        public virtual void HttpClearHeader()
        {
            GameGod.Instance.HttpManager.ClearHeader();
        }
        public virtual void HttpAddHeader(string key, string value)
        {
            GameGod.Instance.HttpManager.AddHeader(key, value);
        }
        public virtual HttpRoutine HttpGet(string url, Action<string> callBack = null)
        {
            return GameGod.Instance.HttpManager.Get(url, callBack);
        }
        public virtual HttpRoutine HttpPost(string url, string json = null, Action<string> callBack = null)
        {
            return GameGod.Instance.HttpManager.Post(url, json, callBack);
        }

        public virtual void SocketClearHeader()
        {
            GameGod.Instance.SocketManager.ClearHeader();
        }
        public virtual void SocketAddHeader(string key, string value)
        {
            GameGod.Instance.SocketManager.AddHeader(key, value);
        }
        public virtual void SocketConnect(string wsUrl, Action callBack = null)
        {
            GameGod.Instance.SocketManager.Connect(wsUrl,callBack);
        }
        public virtual void SocketSendMsg(string msg, Action<string> callBack = null)
        {
            GameGod.Instance.SocketManager.SendMsg(msg, callBack);
        }
        public virtual void SocketClose()
        {
            GameGod.Instance.SocketManager.CloseSocket();
        }
        #endregion

        #region Log
        public virtual void Log(E_Log logType, string title = null, string content = null)
        {
            GameGod.Instance.Log(logType, title, content);
        }
        #endregion
    }
}