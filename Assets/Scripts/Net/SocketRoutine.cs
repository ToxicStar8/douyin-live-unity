/*********************************************
 * BFramework
 * Socket访问器
 * 创建时间：2023/01/08 20:40:23
 *********************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using LitJson;
using UnityWebSocket;
using System.IO;
using System.IO.Compression;
using Douyin;
using Google.Protobuf;

namespace Framework
{
    /// <summary>
    /// Socket访问器
    /// </summary>
    public class SocketRoutine
    {
        /// <summary>
        /// Socket管理器
        /// </summary>
        public SocketManager SoctetMgr => SocketManager.Instance;

        /// <summary>
        /// 客户端Socket
        /// </summary>
        public WebSocket Socket;

        /// <summary>
        /// 拦截的消息队列
        /// </summary>
        private Queue<SocketEvent> _eventQueue;

        private string _url;       //当前需要链接的地址

        /// <summary>
        /// 开启回调
        /// </summary>
        private Action OpenCallback;

        public void OnInit(string url)
        {
            _url = url;

            Socket = new WebSocket(_url);
            _eventQueue = new Queue<SocketEvent>();

            Socket.OnMessage += (sender, e) =>
            {
                lock (_eventQueue)
                {
                    _eventQueue.Enqueue(new SocketEvent(2, sender, e.Data, e.RawData));
                }
            };
            Socket.OnOpen += (sender, e) =>
            {
                lock (_eventQueue)
                {
                    _eventQueue.Enqueue(new SocketEvent(4, sender));
                }
            };
            Socket.OnError += (sender, e) =>
            {
                lock (_eventQueue)
                {
                    _eventQueue.Enqueue(new SocketEvent(1, sender, e.Message));
                }
            };
            Socket.OnClose += (sender, e) =>
            {
                lock (_eventQueue)
                {
                    var evt = new SocketEvent(3, sender);
                    evt.code = e.Code;
                    evt.reason = e.Reason;
                    _eventQueue.Enqueue(evt);
                }
            };
        }

        /// <summary>
        /// 连接
        /// </summary>
        public void Connect(Action callback = null, Dictionary<string, string> headerDic = null)
        {
            OpenCallback = callback;
            Socket.ConnectAsync(headerDic);
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        public void SendMsg(string msg)
        {
            Debug.Log( "WebSocket 发送消息===>"+ msg);
            Socket.SendAsync(msg);
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        public void SendMsg(byte[] bytes)
        {
            Debug.Log("WebSocket 发送消息===>"+ bytes.ToString());
            Socket.SendAsync(bytes);
        }

        /// <summary>
        /// 获取SocketClient
        /// </summary>
        public System.Net.WebSockets.ClientWebSocket GetSocket()
        {
            return Socket.GetSocket();
        }

        /// <summary>
        /// 关闭Socket
        /// </summary>
        /// <param name="msg"></param>
        public void CloseSocket()
        {
            Socket.CloseAsync();
        }

        public void OnUpdate()
        {
            lock (_eventQueue)
            {
                while (_eventQueue.TryDequeue(out SocketEvent evt))
                {
                    RcvSocketMs(evt);
                }
            }
        }

        /// <summary>
        /// 接收解析WebSocket消息
        /// </summary>
        /// <param name="evt"></param>
        private void RcvSocketMs(SocketEvent evt)
        {
            switch (evt.type)
            {
                case 1:         // 错误
                    Debug.LogError("WebSocket 错误===>"+ evt.msg);
                    break;
                case 2:         // 消息
                    Debug.Log("WebSocket 接收消息===>"+ evt.msg);
                    //分发消息
                    DispatchMsg(evt);
                    break;
                case 3:         // WS 关闭
                    Debug.Log("WebSocket 主动关闭");
                    break;
                case 4:         // WS 打开
                    Debug.Log("WebSocket 已连接");
                    OpenCallback?.Invoke();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 派送消息
        /// </summary>
        /// <param name="msg"></param>
        private void DispatchMsg(SocketEvent evt)
        {
            //1、拆收到消息
            var wssPackage = PushFrame.Parser.ParseFrom(evt.bytes);
            //2、反序列化pb后 数据需要GZip解压
            var unGzip = GZipDecompress(wssPackage.Payload.ToByteArray());
            var payloadPackage = Response.Parser.ParseFrom(unGzip);
            //3、拆Gzip解压后 如果需要Ack就发送Ack
            if (payloadPackage.NeedAck)
            {
                //发送ack不能ToString再GetBytes，必须直接发送byte[]
                var obj = new PushFrame();
                obj.PayloadType = "ack";
                obj.Payload = ByteString.CopyFromUtf8(payloadPackage.InternalExt);
                obj.LogId = wssPackage.LogId;
                SendMsg(obj.ToByteArray());
                EventManager.Instance.SendEven(10000, new Message() { Method = "发送ack心跳" });
            }
            //4、消息解析输出
            foreach (var msg in payloadPackage.MessagesList)
            {
                EventManager.Instance.SendEven(10000, msg);
            }

            static byte[] GZipDecompress(byte[] bytes)
            {
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    using (GZipStream zs = new GZipStream(ms, CompressionMode.Decompress))
                    {
                        byte[] buffer = new byte[512];
                        MemoryStream buf = new MemoryStream();
                        for (int offset; (offset = zs.Read(buffer, 0, 512)) > 0;)
                            buf.Write(buffer, 0, offset);
                        return buf.ToArray();
                    }
                }
            }
        }
    }

    /// <summary>
    /// Socket事件类
    /// </summary>
    public class SocketEvent
    {
        public int type;    //握手次数 1掉线 2消息 3主动关闭 4连接
        public object ws;
        public string msg;
        //错误码和原因
        public ushort code;
        public string reason;
        public byte[] bytes;

        public SocketEvent(int type, object ws, string msg = null, byte[] bytes = null)
        {
            this.type = type;
            this.ws = ws;
            this.msg = msg;
            this.bytes = bytes;
        }
    }
}