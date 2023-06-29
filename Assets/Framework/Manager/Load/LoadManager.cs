/*********************************************
 * BFramework
 * 加载管理器
 * 创建时间：2023/01/08 20:40:23
 *********************************************/
using LitJson;
using MainPackage;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.U2D;
using UnityEngine.UI;

namespace Framework
{
    public class LoadManager : ManagerBase
    {
#if UNITY_EDITOR
        /// <summary>
        /// 所有对象在Asset下对应的路径字典
        /// </summary>
        private Dictionary<string, string> _allObjDirectoryDic;

        //private static ABConfig _abConfig => AssetDatabase.LoadAssetAtPath<ABConfig>(ConstDefine.ABConfigPath);
#endif

        /// <summary>
        /// 已经加载的AB包字典
        /// </summary>
        private Dictionary<string, LoadABInfo> _loadedABPackageDic;

        /// <summary>
        /// 已经加载的资源字典
        /// </summary>
        private Dictionary<string, Object> _objLoadDic;

        public override void OnStart()
        {
            _loadedABPackageDic = new Dictionary<string, LoadABInfo>();
            _objLoadDic = new Dictionary<string, Object>();

#if UNITY_EDITOR
            if (GameEntry.Instance.IsEditorMode)
            {
                _allObjDirectoryDic = new Dictionary<string, string>();
                var directoryInfo = new DirectoryInfo(Application.dataPath + "/GameData/");
                var directoryArr = directoryInfo.GetFiles("*.*", SearchOption.AllDirectories);
                var removeLength = Application.dataPath.Length - 6;
                for (int i = 0, length = directoryArr.Length; i < length; i++)
                {
                    var fileInfo = directoryArr[i];
                    if (fileInfo.Extension != ".meta" && fileInfo.Extension != ".cs")
                    {
                        var path = fileInfo.FullName.Substring(removeLength);
                        if (_allObjDirectoryDic.ContainsKey(fileInfo.Name))
                        {
                            GameGod.Instance.Log(E_Log.Error, fileInfo.Name, "名字重复");
                            continue;
                        }
                        _allObjDirectoryDic.Add(fileInfo.Name, path);
                    }
                }
            }
#endif
        }

        /// <summary>
        /// 同步加载资源 带后缀
        /// </summary>
        public T LoadSync<T>(string objName) where T : Object
        {
            var obj = LoadSync(objName);
            return obj as T;
        }

        /// <summary>
        /// 同步加载资源
        /// </summary>
        public Object LoadSync(string objName)
        {
            if (!_objLoadDic.TryGetValue(objName, out var obj))
            {
                if (GameEntry.Instance.IsEditorMode && !GameEntry.Instance.IsRunABPackage)
                {
#if UNITY_EDITOR
                    if (!_allObjDirectoryDic.TryGetValue(objName, out var path))
                    {
                        GameGod.Instance.Log(E_Log.Error, "找不到资源" ,objName);
                        return null;
                    }

                    obj = AssetDatabase.LoadAssetAtPath<Object>(path);
#endif
                }
                else
                {
                    //获得AB包名
                    if (!GameGod.Instance.ABManager.ABInfo.ABFileDic.TryGetValue(objName, out var abName))
                    {
                        GameGod.Instance.Log(E_Log.Error, "找不到资源",objName);
                        return null;
                    }

                    /* 卸载加载解决方案：
                     * 一、(主流)引用计数 
                     * 加载：当引用为0时进行加载并且计数为1，当引用大于0时直接用并且计数+1
                     * 卸载：每次关闭UI、关闭对象时，当引用为0时即卸载不置空，当引用大于0时不卸载
                     * 问题：写代码时需要注意某些先后顺序
                     * 
                     * 二、(偏流)引用计数·改·定时卸载
                     * 加载：存在不未空的AB包时计数+1，空即未加载 进行加载并且计数为1
                     * 卸载：Update定时查询，如果引用计数为0即卸载并置空
                     * 问题：到了定时那一帧卸载，又马上被加载
                     */
                    //寻找到AB包的依赖信息
                    var abRelyOnInfo = GameGod.Instance.ABManager.ABInfo.ABRelyInfoList.Find(x => x.ABName == abName);
                    for (int i = 0, count = abRelyOnInfo.ABRelyOnNameList.Count; i < count; i++)
                    {
                        var relyName = abRelyOnInfo.ABRelyOnNameList[i];
                        //加载依赖
                        LoadAssetBundle(relyName);
                    }
                    //正式加载当前对象使用的包
                    var abPackage = LoadAssetBundle(abRelyOnInfo.ABName);
                    if(objName.EndsWith(".unity"))
                    {
                        //场景需要单独加载 todo

                    }
                    else
                    {
                        //加载对象
                        obj = abPackage.LoadAsset<Object>(objName);
                    }
                }
                _objLoadDic.Add(objName, obj);
            }
            return obj;
        }

        /// <summary>
        /// 加载图片
        /// </summary>
        public Sprite GetSprite(string atlasName,string spriteName)
        {
            Sprite sp = null;
            SpriteAtlas atlas = LoadSync<SpriteAtlas>(atlasName);
            if (atlas != null)
            {
                sp = atlas.GetSprite(spriteName);
            }
            return sp;
        }

        /// <summary>
        /// 异步加载资源
        /// </summary>
        public Object LoadASync(string objName)
        {
            return null;
        }

        /// <summary>
        /// 加载AB包
        /// </summary>
        private AssetBundle LoadAssetBundle(string abName)
        {
            if (!_loadedABPackageDic.TryGetValue(abName, out var abInfo))
            {
                //没有找到就加载并且引用计数为1
                abInfo = new LoadABInfo()
                {
                    AssetBundle = AssetBundle.LoadFromFile(GameEntry.Instance.DowloadManager.SavePath + abName),
                    Times = 1,
                };
                _loadedABPackageDic[abName] = abInfo;
            }
            else
            {
                //如果引用大于0 即没有卸载资源 直接用
                if(abInfo.Times > 0)
                {
                    abInfo.Times++;
                }
                else
                {
                    //引用为0 资源已卸载 重新加载
                    abInfo.AssetBundle = AssetBundle.LoadFromFile(GameEntry.Instance.DowloadManager.SavePath + abName);
                    abInfo.Times = 1;
                }
            }
            return abInfo.AssetBundle;
        }

        /// <summary>
        /// 卸载资源
        /// </summary>
        public void UnloadAsset(string objName)
        {
            if(_objLoadDic.TryGetValue(objName,out var obj))
            {
                //卸载Asset
                obj = null;
                _objLoadDic.Remove(objName);
                if (!GameEntry.Instance.IsEditorMode || GameEntry.Instance.IsRunABPackage)
                {
                    //卸载AB包
                    if (GameGod.Instance.ABManager.ABInfo.ABFileDic.TryGetValue(objName, out var abName))
                    {
                        //先卸载依赖的AB包
                        var abRelyOnInfo = GameGod.Instance.ABManager.ABInfo.ABRelyInfoList.Find(x => x.ABName == abName);
                        for (int i = 0, count = abRelyOnInfo.ABRelyOnNameList.Count; i < count; i++)
                        {
                            var relyName = abRelyOnInfo.ABRelyOnNameList[i];
                            TryUnloadAssetBundle(relyName);
                        }
                        //正式卸载当前对象使用的包
                        TryUnloadAssetBundle(abRelyOnInfo.ABName);
                    }
                }
            }
        }

        /// <summary>
        /// 尝试卸载AB包
        /// </summary>
        /// <param name="abInfo"></param>
        private void TryUnloadAssetBundle(string abName)
        {
            var abInfo = _loadedABPackageDic[abName];
            abInfo.Times--;
            GameGod.Instance.Log(E_Log.Framework, abName + "的计数",abInfo.Times.ToString());
            //如果引用等于0 直接卸载
            if (abInfo.Times == 0)
            {
                abInfo.AssetBundle.Unload(true);
            }
        }

        public override void OnUpdate() { }
        public override void OnDispose()
        {
#if UNITY_EDITOR
            if (GameEntry.Instance.IsEditorMode)
            {
                _allObjDirectoryDic.Clear();
                _allObjDirectoryDic = null;
            }
#endif
            _objLoadDic.Clear();
            _objLoadDic = null;
        }

        /// <summary>
        /// AB包的引用计数
        /// </summary>
        public class LoadABInfo
        {
            /// <summary>
            /// AB包文件
            /// </summary>
            public AssetBundle AssetBundle;
            /// <summary>
            /// 引用计数
            /// </summary>
            public uint Times;
        }
    }
}
