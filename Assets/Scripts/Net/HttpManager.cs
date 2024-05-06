/*********************************************
 * BFramework
 * Http管理器
 * 创建时间：2023/01/08 20:40:23
 *********************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    /// <summary>
    /// Http管理器
    /// </summary>
    public class HttpManager : InstanceBase<HttpManager>
    {
        /// <summary>
        /// 连接失败后重试次数
        /// </summary>
        public int Retry { get; private set; }

        /// <summary>
        /// 连接失败后重试间隔（秒）
        /// </summary>
        public int RetryInterval { get; private set; }

        public WaitForSeconds WaitSeconds { get; private set; } = new WaitForSeconds(0);

        //Token 适配不同的后端要求 例：Token、Authorization等
        public Dictionary<string,string> HttpHeaderDic { get; private set; } = new Dictionary<string, string>();

        /// <summary>
        /// 清空浏览器请求标头
        /// </summary>
        public void ClearHeader()
        {
            HttpHeaderDic.Clear();
            Debug.Log("清空浏览器标头");
        }

        /// <summary>
        /// 添加浏览器请求标头
        /// </summary>
        public void AddHeader(string key,string value)
        {
            HttpHeaderDic.Add(key, value);
            Debug.Log("添加浏览器标头" + key + "===>"+ value);
        }

        public HttpRoutine Get(string url, Action<string> callBack = null)
        {
            var routine = new HttpRoutine();
            routine.Get(url, callBack);
            return routine;
        }

        public HttpRoutine Post(string url, string json = null, Action<string> callBack = null)
        {
            var routine = new HttpRoutine();
            routine.Post(url, json, callBack);
            return routine;
        }
    }
}
