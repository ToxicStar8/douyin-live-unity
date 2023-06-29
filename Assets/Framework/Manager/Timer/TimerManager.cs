/*********************************************
 * BFramework
 * 计时器管理器
 * 创建时间：2023/01/30 11:12:23
 *********************************************/
using MainPackage;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    /// <summary>
    /// 倒计时类
    /// </summary>
    public class TimerInfo
    {
        //可传入数据
        public Action Callback;             //执行回调
        public int AllCount = -1;           //执行次数 -1=直至关闭
        public float InviteTime = 1;        //执行间隔时间
        public bool IsExecImmed = false;    //是否立即执行 如果需要立即执行最好间隔时间>0.3f
        //非传入数据
        public string TimeName;             //Key
        public float OldTime = -1;          //上次执行时间
        public float NextExecTime => OldTime + InviteTime;      //下次执行时间

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="allCount">执行次数 -1=直至关闭</param>
        /// <param name="inviteTime">执行间隔时间</param>
        /// <param name="isExecImmed">是否立即执行</param>
        /// <param name="callback">执行回调</param>
        public void Init(int allCount, float inviteTime, bool isExecImmed, Action callback)
        {
            AllCount = allCount;
            InviteTime = inviteTime;
            IsExecImmed = isExecImmed;
            Callback = callback;
            OldTime = -1;
        }
    }

    /// <summary>
    /// 计时器管理器
    /// </summary>
    public class TimerManager : ManagerBase
    {
        //临时索引
        private int _tempIndex;
        //使用中的计时器
        public Dictionary<string, TimerInfo> TimerInfoDic { private set; get; }
        //等待回收的计时器
        public List<TimerInfo> WaitReycleList { private set; get; }
        //等待添加的计时器
        public List<TimerInfo> WaitAddList { private set; get; }

        public override void OnStart()
        {
            TimerInfoDic = new Dictionary<string, TimerInfo>();
            WaitReycleList = new List<TimerInfo>();
            WaitAddList = new List<TimerInfo>();
        }

        public override void OnUpdate()
        {
            var curTime = Time.time;

            //回收使用结束的计时器
            for (int i = 0, count = WaitReycleList.Count; i < count; i++)
            {
                RemoveTimer(WaitReycleList[i]);
            }
            WaitReycleList.Clear();

            //添加等待的计时器
            for (int i = 0, count = WaitAddList.Count; i < count; i++)
            {
                var timerInfo = WaitAddList[i];
                TimerInfoDic.Add(timerInfo.TimeName, timerInfo);
            }
            WaitAddList.Clear();

            //遍历计时器
            foreach (var item in TimerInfoDic)
            {
                var timerInfo = item.Value;
                //正式执行
                if (timerInfo.AllCount != 0 && curTime >= timerInfo.NextExecTime)
                {
                    //刷新时间
                    timerInfo.OldTime = curTime;
                    //执行回调
                    ExecCallback(timerInfo);
                }
            }
        }

        /// <summary>
        /// 添加一次性倒计时，执行次数永远不能为-1，即无限，否则无限循环无法跳出
        /// </summary>
        public void AddTempTimer(TimerInfo timerInfo)
        {
            if (timerInfo.AllCount == -1)
            {
                GameGod.Instance.Log(E_Log.Error, "执行次数永远不能为-1！");
                return;
            }

            AddTimer(_tempIndex.ToString(), timerInfo);
            _tempIndex++;
        }

        /// <summary>
        /// 添加倒计时
        /// </summary>
        public void AddTimer(string timeName, TimerInfo timerInfo)
        {
            if (TimerInfoDic.ContainsKey(timeName) && WaitReycleList.Find(x => x.TimeName == timeName) == null)
            {
                GameGod.Instance.Log(E_Log.Error, "定时器重复监听", timeName);
                //TimerInfoDic.Remove(timeName);
                return;
            }
            //更新名字
            timerInfo.TimeName = timeName;
            //更新时间
            timerInfo.OldTime = Time.time;
            //是否立即执行
            if (timerInfo.IsExecImmed)
            {
                //执行回调
                ExecCallback(timerInfo);
            }
            //如果执行完毕还存在 更新下次刷新时间并且添加到等待列表
            if (timerInfo.AllCount != 0)
            {
                //添加到等待列表中
                WaitAddList.Add(timerInfo);
            }
        }

        /// <summary>
        /// 获取计时器
        /// </summary>
        public TimerInfo GetTimerInfo(string timeName)
        {
            if (TimerInfoDic.TryGetValue(timeName, out var timerInfo))
            {
                return timerInfo;
            }
            GameGod.Instance.Log(E_Log.Error, "没有找到该计时器", timeName);
            return null;
        }

        /// <summary>
        /// 移除监听
        /// </summary>
        public void RemoveTimer(string timeName)
        {
            if (TimerInfoDic.TryGetValue(timeName, out var timerInfo))
            {
                timerInfo.AllCount = 0;
                WaitReycleList.Add(timerInfo);
            }
        }

        /// <summary>
        /// 移除监听
        /// </summary>
        public void RemoveTimer(TimerInfo timerInfo)
        {
            GameGod.Instance.PoolManager.RecycleClassObj<TimerInfo>(timerInfo);
            TimerInfoDic.Remove(timerInfo.TimeName);
        }

        /// <summary>
        /// 执行回调
        /// </summary>
        private void ExecCallback(TimerInfo timerInfo)
        {
            timerInfo.Callback?.Invoke();
            if (timerInfo.AllCount > 0)
            {
                timerInfo.AllCount--;
                if (timerInfo.AllCount == 0)
                {
                    //将计时器添加到等待回收的列表
                    WaitReycleList.Add(timerInfo);
                }
            }
        }

        public override void OnDispose()
        {
            TimerInfoDic.Clear();
            TimerInfoDic = null;
        }
    }
}
