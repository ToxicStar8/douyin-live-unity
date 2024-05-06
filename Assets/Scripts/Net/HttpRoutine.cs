/*********************************************
 * BFramework
 * Http访问器
 * 创建时间：2023/01/08 20:40:23
 *********************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using LitJson;
using GameData;

namespace Framework
{
    /// <summary>
    /// Http访问器
    /// </summary>
    public class HttpRoutine
    {
        /// <summary>
        /// Http管理器
        /// </summary>
        public HttpManager HttpMgr => HttpManager.Instance;

        /// <summary>
        /// Http请求回调
        /// </summary>
        private Action<string> _callBack;

        /// <summary>
        /// Http请求回调数据
        /// </summary>
        private string _jsonData;

        /// <summary>
        /// 是否繁忙
        /// </summary>
        public bool IsBusy { private set; get; }

        /// <summary>
        /// 当前重试次数
        /// </summary>
        private int _currRetry = 0;

        /// <summary>
        /// URL
        /// </summary>
        private string _url;

        /// <summary>
        /// Post发送的数据
        /// </summary>
        private string _json;

        /// <summary>
        /// 
        /// </summary>
        private UnityWebRequest _webRequest;
        public UnityWebRequest GetWWW() => _webRequest;

        public HttpRoutine()
        {

        }

        /// <summary>
        /// 外部调用的Get
        /// </summary>
        public void Get(string url, Action<string> callBack = null)
        {
            if (IsBusy)
            {
               Debug.LogError("网络锁");
                return;
            }

            IsBusy = true;

            _url = url;
            _callBack = callBack;
            _webRequest?.Dispose();

            GetUrl();
        }

        /// <summary>
        /// 外部调用的Post
        /// </summary>
        public void Post(string url, string json = null, Action<string> callBack = null)
        {
            if (IsBusy)
            {
                 Debug.LogError("网络锁");
                return;
            }

            IsBusy = true;

            _url = url;
            _json = json;
            _callBack = callBack;
            _webRequest?.Dispose();

            PostUrl();
        }

        private void GetUrl()
        {
            Debug.Log( string.Format("Get===><color=#00ffff>{0}</color>\n\r重试===><color=#00ffff>{1}</color>", _url, _currRetry));
            _webRequest = UnityWebRequest.Get(_url);
            UnityEngine.Object.FindAnyObjectByType<UIMainMenu>().StartCoroutine(SendRequest());
        }

        private void PostUrl()
        {
            //加密
            if (!string.IsNullOrWhiteSpace(_json))
            {
                //if (GameEntry.ParamsSettings.PostIsEncrypt && _currRetry == 0)
                //{
                //    _sendDic["value"] = _json;
                //    //web加密
                //    _sendDic["deviceIdentifier"] = DeviceUtil.DeviceIdentifier;
                //    _sendDic["deviceModel"] = DeviceUtil.DeviceModel;
                //    long t = GameEntry.Data.SysDataManager.CurrServerTime;
                //    _sendDic["sign"] = EncryptUtil.Md5(string.Format("{0}:{1}", t, DeviceUtil.DeviceIdentifier));
                //    _sendDic["t"] = t;

                //    _json = _sendDic.ToJson();
                //}

                //if (!string.IsNullOrWhiteSpace(GameEntry.ParamsSettings.PostContentType))
                //    www.SetRequestHeader("Content-Type", GameEntry.ParamsSettings.PostContentType);
            }

            //这里如果使用UnityWebRequest.Post再new UploadHandlerRaw，会造成内存泄漏
            _webRequest = new UnityWebRequest(_url, "POST");
            _webRequest.downloadHandler = new DownloadHandlerBuffer();
            _webRequest.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(_json));
            _webRequest.SetRequestHeader("Content-Type", "application/json");
            Debug.Log( string.Format("Post===><color=#00ffff>{0}</color>\n\r重试===><color=#00ffff>{1}</color>", _url + _json, _currRetry));
            UnityEngine.Object.FindAnyObjectByType<UIMainMenu>().StartCoroutine(SendRequest());
        }

        private IEnumerator SendRequest()
        {
            foreach (var item in HttpMgr.HttpHeaderDic)
            {
                _webRequest.SetRequestHeader(item.Key, item.Value);
            }
            _webRequest.timeout = 5;
            yield return _webRequest.SendWebRequest();
            if (_webRequest.result == UnityWebRequest.Result.ConnectionError || _webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                //报错了 进行重试
                if (_currRetry > 0) yield return HttpMgr.WaitSeconds;
                _currRetry++;

                //如果还有重试次数 重试
                if (_currRetry <= HttpMgr.Retry)
                {
                    switch (_webRequest.method)
                    {
                        case UnityWebRequest.kHttpVerbGET:
                            GetUrl();
                            break;
                        case UnityWebRequest.kHttpVerbPOST:
                            PostUrl();
                            break;
                    }
                    yield break;
                }

                //结束
                IsBusy = false;
                _jsonData = _webRequest.error;
                //打印错误
                if (!string.IsNullOrWhiteSpace(_jsonData))
                {
                    Debug.LogError(_jsonData);
                }
            }
            else
            {
                IsBusy = false;
                _jsonData = _webRequest.downloadHandler.text;
                //打印数据
                if (!string.IsNullOrWhiteSpace(_jsonData))
                {
                    Debug.Log(string.Format("<color=#FFF11A>{{\"code\":{0},\"data\":{1}}}</color>", _webRequest.responseCode, _jsonData));
                }
            }
            //执行回调
            _callBack?.Invoke(_jsonData);
            //清理状态
            _callBack = null;
            _currRetry = 0;
            _url = null;
            _jsonData = null;
        }
    }
}