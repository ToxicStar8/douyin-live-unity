/*********************************************
 * BFramework
 * 资产名字生成工具
 * 创建时间：2023/04/21 17:44:36
 *********************************************/
using System.Globalization;
using System.IO;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;

namespace Framework
{
    /// <summary>
    /// 资产名字生成
    /// </summary>
    public class BuildAssetName : Editor
    {
        /// <summary>
        /// ABConfig
        /// </summary>
        private static ABConfig _abConfig => AssetDatabase.LoadAssetAtPath<ABConfig>(ConstDefine.ABConfigPath);

        /// <summary>
        /// 资产名字代码磁盘路径
        /// </summary>
        public static string AssetNamesScriptPath = Application.dataPath + "/GameData/Scripts/Define/AssetName.cs";

        [MenuItem("BFramework/Build Asset Name")]
        public static void BuildAssetNamesScript()
        {
            string temp = @"/*********************************************
 * 自动生成代码，禁止手动修改文件
 * 脚本名：AssetName.cs
 * 创建时间：#Time
 *********************************************/

namespace GameData
{
    /// <summary>
    /// 快捷获取资产名
    /// </summary>
    public static class AssetName
    {#AssetName
    }
}
";

            string assetNames = string.Empty;
            var needSearchPathList = _abConfig.RootABList;
            //循环全部需要打包的地址
            for (int i = 0,count = needSearchPathList.Count; i < count; i++)
            {
                var pathDir = new DirectoryInfo(needSearchPathList[i]);
                //配表文本无需调用、Json文件主工程打包用、字体文件看情况
                //不生成
                if (pathDir.Name == "Table" || pathDir.Name == "JsonInformation" || pathDir.Name == "Font")
                {
                    continue;
                }
                //循环目录下的全部文件
                foreach (var item in pathDir.GetFileSystemInfos("*.*", SearchOption.AllDirectories))
                {
                    var fileInfo = item as FileInfo;
                    if (fileInfo != null)
                    {
                        var file = new FileInfo(fileInfo.FullName);
                        //不是meta和图集文件
                        if (file.Extension != ".meta" && file.Extension != ".spriteatlas")
                        {
                            var prefix = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(fileInfo.Extension.Replace(".", ""));
                            var name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(fileInfo.Name.Replace(fileInfo.Extension, ""));
                            //文本添加
                            assetNames += $"\r\n        public static string {prefix}_{name} => \"{fileInfo.Name}\";";
                        }
                    }
                }
            }

            //导出文件 替换文本
            var scripts = File.CreateText(AssetNamesScriptPath);
            temp = temp.Replace("#Time", System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
            temp = temp.Replace("#AssetName", assetNames);
            scripts.Write(temp);
            scripts.Close();
            Debug.Log("资产名字代码生成完毕!");

            //回收资源
            System.GC.Collect();
            //刷新编辑器
            AssetDatabase.Refresh();
        }
    }
}
