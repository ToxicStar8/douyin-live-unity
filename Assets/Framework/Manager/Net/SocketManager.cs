/*********************************************
 * BFramework
 * Socket管理器
 * 创建时间：2023/01/08 20:40:23
 *********************************************/
using MainPackage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using UnityEngine;

namespace Framework
{
    /// <summary>
    /// Socket管理器
    /// </summary>
    public class SocketManager : ManagerBase
    {
        private SocketRoutine _mainSocket;
        //Token 适配不同的后端要求 例：Token、Authorization等
        public Dictionary<string, string> SocketHeaderDic { get; private set; }

        public override void OnStart()
        {
            SocketHeaderDic = new Dictionary<string, string>();
        }

        /// <summary>
        /// 连接Socket
        /// </summary>
        public void Connect(string wsUrl,Action callback = null)
        {
            //关闭旧的Socket
            _mainSocket?.CloseSocket();
            //初始化新的Socket
            _mainSocket = new SocketRoutine();
            _mainSocket.OnInit(wsUrl);
            //正式连接
            _mainSocket.Connect(callback, SocketHeaderDic);
        }

        /// <summary>
        /// 清空浏览器请求标头
        /// </summary>
        public void ClearHeader()
        {
            SocketHeaderDic.Clear();
            GameGod.Instance.Log(E_Log.Framework, "清空浏览器标头");
        }

        /// <summary>
        /// 添加浏览器请求标头
        /// </summary>
        public void AddHeader(string key, string value)
        {
            SocketHeaderDic.Add(key, value);
            GameGod.Instance.Log(E_Log.Framework, "添加浏览器标头" + key, value);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        public void SendMsg(string msg, Action<string> callBack = null)
        {
            _mainSocket.SendMsg(msg);
        }

        /// <summary>
        /// 获取SocketClient
        /// </summary>
        public ClientWebSocket GetSocket()
        {
            return _mainSocket.GetSocket();
        }

        /// <summary>
        /// 关闭Socket
        /// </summary>
        public void CloseSocket()
        {
            _mainSocket.CloseSocket();
        }

        public override void OnUpdate() 
        {
            _mainSocket?.OnUpdate();
        }

        public override void OnDispose() 
        {
            _mainSocket.CloseSocket();
            _mainSocket = null;
        }
    }
}
