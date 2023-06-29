/*********************************************
 * BFramework
 * 数据管理器
 * 创建时间：2023/01/08 20:40:23
 *********************************************/
using GameData;
using LitJson;
using MainPackage;
using System.IO;
using UnityEngine;

namespace Framework
{
    /// <summary>
    /// 数据管理器
    /// </summary>
    public partial class DataManager : ManagerBase
    {
        /// <summary>
        /// 最后存档的时间（Time.time）
        /// </summary>
        public float LastSaveTime;

        /// <summary>
        /// 数据列表
        /// </summary>
        public DataList Data { private set; get; }

        public bool IsNullData => Data == null;

        /// <summary>
        /// 存档的路径
        /// </summary>
        private string _archivalPath;

        public override void OnStart()
        {
            InitData();
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        public void InitData()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            //WebGL不允许使用IO类函数
            var jsonData = PlayerPrefs.GetString(ConstDefine.Archival, null);
            if(jsonData != null)
            {
                try
                {
                    Data = JsonMapper.ToObject<DataList>(jsonData);
                }
                catch
                {
                    GameEntry.Instance.Log(E_Log.Error, "游戏存档损坏，请检查");
                }
            }
#else
            //存档路径
            _archivalPath = Application.persistentDataPath + "/Archival.Json";
            //简单的存档模式：
            var fileInfo = new FileInfo(_archivalPath);
            //如果有存档 直接加载存档
            if (fileInfo.Exists)
            {
                using (var text = fileInfo.OpenText())
                {
                    var jsonData = text.ReadToEnd();
                    if (!string.IsNullOrWhiteSpace(jsonData))
                    {
                        try
                        {
                            Data = JsonMapper.ToObject<DataList>(jsonData);
                        }
                        catch
                        {
                            GameGod.Instance.Log(E_Log.Error, "游戏存档损坏，请检查");
                        }
                    }
                }
            }
#endif
        }

        /// <summary>
        /// 新建存档
        /// </summary>
        public DataList GetNewData()
        {
            //新存档需要将旧数据绑定的定时器之类的先取消
            Data?.OnDispose();
            //创建新存档
            LastSaveTime = Time.time;
            Data = new DataList();
            SaveDataToFile();
            return Data;
        }

        /// <summary>
        /// 存档
        /// </summary>
        public void SaveData()
        {
            //游玩时长
            Data.GamingTime += Time.time - LastSaveTime;
            //更新记录开始的时间
            LastSaveTime = Time.time;
            SaveDataToFile();
        }

        /// <summary>
        /// 存档至文件
        /// </summary>
        private void SaveDataToFile()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            //WebGL不允许使用IO类函数
            PlayerPrefs.SetString(ConstDefine.Archival, JsonMapper.ToJson(Data));
#else
            File.WriteAllText(_archivalPath, JsonMapper.ToJson(Data));
#endif
        }

        public override void OnUpdate() { }
        /// <summary>
        /// 退出方法
        /// </summary>
        public override void OnDispose()
        {
            //SaveData();
            Data?.OnDispose();
        }
    }
}
