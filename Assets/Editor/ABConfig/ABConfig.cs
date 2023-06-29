/*********************************************
 * BFramework
 * AB包菜单列表
 * 创建时间：2022/12/28 16:33:23
 *********************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Framework
{
    // 创建资源AB配置菜单
    [CreateAssetMenu(fileName = "ABConfig", menuName = "CreateABConfig", order = 999)]
    public class ABConfig : ScriptableObject
    {
        /// <summary>
        /// 需要打包的文件夹路径，会遍历这个文件夹下的所有文件和文件夹，
        /// 该目录下的所有文件名和文件夹名不能重复，必须保证名字的唯一性。
        /// 直接定位到父目录，用子文件夹名当ab包名，如果根目录有文件用父目录单独打包。
        /// </summary>
        [Header("AB包配置根目录（根目录下所有文件夹全部分开打包）")]
        public List<string> RootABList = new List<string>();

        [Header("真正的AB包打包列表(自动生成)")]
        public List<ABData> TrueABList = new List<ABData>();

        [System.Serializable]
        public class ABData
        {
            public string path;
            public string abName;               // ab包名
            //public bool must;
        }

        /// <summary>
        /// 设置打包名和路径
        /// </summary>
        public List<ABData> SetABNameAndPath()
        {
            //所有AB包的路径 AB包路径,AB包名
            var allABPathList = new List<KeyValuePair<string, string>>();
            //需要删除Assets目录之前的Length
            int needDeleteLength = Application.dataPath.Length - "Assets".Length;
            //遍历输入的目录
            for (int i = 0; i < RootABList.Count; i++)
            {
                var rootHasFile = false;
                var path = RootABList[i];
                var pathDir = new DirectoryInfo(path);
                //循环目录下的文件和子目录 但是不包含子目录下的文件和子目录
                foreach (var item in pathDir.GetFileSystemInfos("*.*", SearchOption.TopDirectoryOnly))
                {
                    var childDir = item as DirectoryInfo;
                    //空值就代表它是文件
                    if (childDir == null)
                    {
                        //场景需要单独打包
                        if (item.Extension == ".unity")
                        {
                            allABPathList.Add(new KeyValuePair<string, string>(item.FullName.Substring(needDeleteLength).Replace("\\", "/"), item.Name.Replace(".unity","")));
                        }
                        //跳过meta文件
                        else if (item.Extension != ".meta")
                        {
                            //根目录下也有文件 说明当前路径也需要单独打个包，注意 不要包含其它子文件夹下的文件
                            rootHasFile = true;
                        }
                    }
                    else    //文件夹
                    {
                        allABPathList.Add(new KeyValuePair<string, string>(item.FullName.Substring(needDeleteLength).Replace("\\", "/"), pathDir.Name + childDir.Name));
                    }
                }

                // 只有在根目录下有文件的时候才会打包根目录
                if (rootHasFile)
                {
                    allABPathList.Add(new KeyValuePair<string, string>(path, pathDir.Name));
                }
            }

            TrueABList.Clear();
            for (int i = 0; i < allABPathList.Count; i++)
            {
                var kv = allABPathList[i];
                var data = new ABData();
                data.path = kv.Key;
                data.abName = kv.Value.ToLower();
                TrueABList.Add(data);
            }

            UnityEditor.EditorUtility.SetDirty(this);
            return TrueABList;
        }
    }
}