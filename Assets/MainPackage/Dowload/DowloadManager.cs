///*********************************************
// * DowloadManager
// * 下载管理器
// * 创建时间：2023/06/16 17:01:23
// *********************************************/
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace MainPackage
{
    public class DowloadManager
    {
        /// <summary>
        /// 下载器
        /// </summary>
        private UnityWebRequest _request;

        /// <summary>
        /// 当前重试次数
        /// </summary>
        private int _currRetry = 0;

        /// <summary>
        /// AB包MD5列表
        /// </summary>
        public List<ABMd5Info> ABMd5InfoList { private set; get; }

        /// <summary>
        /// AB包存储路径
        /// </summary>
        public string SavePath { private set; get; }

        /// <summary>
        /// 已下载的AB包数量
        /// </summary>
        public int LoadedABTimes { private set; get; } = 0;

        /// <summary>
        /// 是否下载完毕
        /// </summary>
        public bool IsDowloadEnd { private set; get; } = false;

        /// <summary>
        /// 项目的AB包下载地址 空为StreamingAssets
        /// </summary>
        public string DownloadUrl = "";

        /// <summary>
        /// AB包MD5信息名（用于比对需要更新的AB包）
        /// </summary>
        public string ABMd5InfoName = "fileUpdateInfo.json";

        /// <summary>
        /// 开始下载
        /// </summary>
        public void StartDowload()
        {
            //编辑器模式不下载
            if (GameEntry.Instance.IsEditorMode)
            {
                SavePath = Application.dataPath.Substring(0, Application.dataPath.Length - "Assets".Length) + "AssetBundle/";
                GameEntry.Instance.WinLoading.IsInitEnd = true;
                IsDowloadEnd = true;
                return;
            }

            if(string.IsNullOrWhiteSpace(DownloadUrl))
            {
                SavePath = Application.streamingAssetsPath + "/";
                GameEntry.Instance.Log(E_Log.Framework, "存放在StreamingAssets中，无需下载");
                GameEntry.Instance.WinLoading.IsInitEnd = true;
                IsDowloadEnd = true;
                return;
            }

            SavePath = Application.persistentDataPath + "/AssetBundle/";
            _currRetry = 0;
            GameEntry.Instance.StartCoroutine(DowloadXml(DownloadUrl + ABMd5InfoName,
                () => GameEntry.Instance.StartCoroutine(DownloadRely())));
        }

        /// <summary>
        /// 下载记录ab包状态的Xml
        /// </summary>
        /// <param name="abName"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        private IEnumerator DowloadXml(string url, Action callback)
        {
            //检查资源变动
            GameEntry.Instance.Log(E_Log.Framework, "检查更新中……");
            GameEntry.Instance.WinLoading.CheckUpdate();
            //
            _request = UnityWebRequest.Get(url);
            yield return _request.SendWebRequest();
            if (_request.result == UnityWebRequest.Result.ConnectionError || _request.result == UnityWebRequest.Result.ProtocolError)
            {
                //报错了 进行重试
                if (_currRetry > 0) yield return 0;
                _currRetry++;
                //如果还有重试次数 重试
                if (_currRetry <= 3)
                {
                    GameEntry.Instance.StartCoroutine(DowloadXml(url, callback));
                    yield break;
                }
                //打印错误 退出
                if (!string.IsNullOrWhiteSpace(_request.error))
                {
                    GameEntry.Instance.Log(E_Log.Error, _request.error);
                    GameEntry.Instance.WinLoading.ShowError();
                    yield break;
                }
            }
            //将数据转化为列表
            var jsonData = _request.downloadHandler.text.ToString();
            GameEntry.Instance.Log(E_Log.Framework, jsonData);
            ABMd5InfoList = JsonMapper.ToObject<List<ABMd5Info>>(jsonData);
            callback?.Invoke();
        }

        /// <summary>
        /// 下载其余AB包
        /// </summary>
        /// <returns></returns>
        private IEnumerator DownloadRely()
        {
            //1.显示加载
            GameEntry.Instance.Log(E_Log.Framework, "加载资源中……");
            GameEntry.Instance.WinLoading.StartLoading();
            //2.循环MD5索引信息下载AB包到本地 注：不是AB依赖信息！！
            for (int i = 0, count = ABMd5InfoList.Count; i < count; i++)
            {
                var abMd5Info = ABMd5InfoList[i];
                //开始下载AB包
                bool isComplete = false;
                _currRetry = 0;
                GameEntry.Instance.StartCoroutine(DownloadABPackage(abMd5Info.ABName, () => isComplete = true));
                //等待加载成功
                yield return new WaitUntil(() => isComplete);
            }
            //3.清空状态
            GameEntry.Instance.Log(E_Log.Framework, "全部下载完毕");
            yield return null;
            GameEntry.Instance.WinLoading.IsInitEnd = true;
            IsDowloadEnd = true;
            //4.不做热重载 清除md5数据
            ABMd5InfoList.Clear();
            ABMd5InfoList = null;
        }

        /// <summary>
        /// 下载AB包方法
        /// </summary>
        /// <param name="abName">AB包名</param>
        /// <param name="callback">下载完毕回调</param>
        /// <returns></returns>
        private IEnumerator DownloadABPackage(string abName, Action callback)
        {
            //1.查看本地是否已下载
            var file = new FileInfo(SavePath + abName);
            if (file.Exists)
            {
                //比对md5和文件大小 一模一样即无变化 不需要重新下载
                var md5Info = ABMd5InfoList.Find(x => x.ABName == abName);
                if (md5Info.ABMd5 == Md5Util.GetMd5ByPath(file.FullName) && md5Info.ABSize == file.Length)
                {
                    GameEntry.Instance.Log(E_Log.Framework, "已存在名为" + abName + "的文件且文件与服务器相同", "跳过");
                    LoadedABTimes++;
                    callback?.Invoke();
                    yield break;
                }
            }
            //2.开始下载
            GameEntry.Instance.Log(E_Log.Framework, "下载AB包", abName);
            var url = DownloadUrl + abName;
            _request = UnityWebRequest.Get(url);
            yield return _request.SendWebRequest();
            //3.判断是否报错，如果报错就重试
            if (_request.result == UnityWebRequest.Result.ConnectionError || _request.result == UnityWebRequest.Result.ProtocolError)
            {
                //报错了 进行重试
                if (_currRetry > 0) yield return 0;
                _currRetry++;

                //如果还有重试次数 重试
                if (_currRetry <= 3)
                {
                    GameEntry.Instance.StartCoroutine(DownloadABPackage(abName, callback));
                    yield break;
                }

                //打印错误 退出
                if (!string.IsNullOrWhiteSpace(_request.error))
                {
                    GameEntry.Instance.Log(E_Log.Error, _request.error);
                    GameEntry.Instance.WinLoading.ShowError();
                    yield break;
                }
            }
            GameEntry.Instance.Log(E_Log.Framework, abName, "下载成功");
            //数据
            byte[] results = _request.downloadHandler.data;
            //如果没有AB包文件夹，创建
            if (!Directory.Exists(SavePath))
            {
                Directory.CreateDirectory(SavePath);
            }
            //4.存储AB包数据
            File.WriteAllBytes(SavePath + abName, results);
            //5.执行回调
            LoadedABTimes++;
            callback?.Invoke();
        }
    }
}