/*********************************************
 * BFramework
 * 音效生成工具
 * 创建时间：2023/02/13 17:53:36
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
    /// 音效生成
    /// </summary>
//    public class BuildAudio : Editor
//    {
//        /// <summary>
//        /// 音效磁盘路径
//        /// </summary>
//        public static string AudioPath = Application.dataPath + "/GameData/Art/Audio";

//        /// <summary>
//        /// 音效代码磁盘路径
//        /// </summary>
//        public static string AudioScriptPath = Application.dataPath + "/GameData/Scripts/Define/AudioNames.cs";

//        [MenuItem("BFramework/Build Audio")]
//        public static void BuildAudioScript()
//        {
//            string temp = @"/*********************************************
// * 自动生成代码，禁止手动修改文件
// * 脚本名：AudioNames.cs
// * 创建时间：#Time
// *********************************************/

//namespace GameData
//{
//    /// <summary>
//    /// 快捷获取音乐音效名
//    /// </summary>
//    public static class AudioNames
//    {#AudioNames
//    }
//}
//";

//            string audioNames = string.Empty;
//            var pathDir = new DirectoryInfo(AudioPath);
//            //循环目录下的全部文件
//            foreach (var item in pathDir.GetFileSystemInfos("*.*", SearchOption.AllDirectories))
//            {
//                var fileInfo = item as FileInfo;
//                if (fileInfo != null)
//                {
//                    var file = new FileInfo(fileInfo.FullName);
//                    if (file.Extension != ".meta" && file.Extension != ".spriteatlas")
//                    {
//                        //文本添加
//                        audioNames += $"\r\n        public static string {fileInfo.Name.Replace(fileInfo.Extension, "")} => \"{fileInfo.Name}\";";
//                    }
//                }
//            }
//            //导出文件 替换文本
//            var scripts = File.CreateText(AudioScriptPath);
//            temp = temp.Replace("#Time", System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
//            temp = temp.Replace("#AudioNames", audioNames);
//            scripts.Write(temp);
//            scripts.Close();
//            Debug.Log("音效代码生成完毕!");

//            //回收资源
//            System.GC.Collect();
//            //刷新编辑器
//            AssetDatabase.Refresh();
//        }
//    }
}
