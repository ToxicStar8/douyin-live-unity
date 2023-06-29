/*********************************************
 * BFramework
 * 快捷打开工具
 * 创建时间：2022/12/28 15:52:23
 *********************************************/
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using UnityEditor;
using UnityEngine;


namespace Framework
{
    /// <summary>
    /// 打开文件/文件夹工具
    /// </summary>
    public class OpenFileTools
    {
        /// <summary>
        /// 打开文件/文件夹方法
        /// </summary>
        /// <param name="path">文件路径</param>
        public static void OpenFile(string path)
        {
            Thread newThread = new Thread(new ParameterizedThreadStart(CmdOpenDirectory));
            newThread.Start(path);
        }

        /// <summary>
        /// 真正打开文件/文件夹的方法
        /// </summary>
        private static void CmdOpenDirectory(object obj)
        {
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.Arguments = "/c start " + obj.ToString();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();

            p.WaitForExit();
            p.Close();
        }

        [MenuItem("GameObject/打开UI代码", false, 10001)]
        public static void OpenUIScript()
        {
            //选择的对象
            var go = Selection.activeGameObject;
            //脚本路径
            var uiRootPath = Application.dataPath + "/GameData/Scripts/UI/";
            //文件夹信息
            var dirInfo = new DirectoryInfo(uiRootPath);

            string targetName = string.Empty;
            //UI打开方式
            if (go.name.StartsWith("UI"))
            {
                //直接就是脚本名
                targetName = go.name.Replace("(Clone)", "") + ".cs";
            }

            //Unit打开方式
            if (go.name.EndsWith("Unit"))
            {
                var goName = go.name.Replace("(Clone)", "") + ".cs";
                //判断是否为独立Unit
                var parentGo = go.transform.parent;
                if (parentGo != null && !parentGo.name.StartsWith("Canvas"))
                {
                    while (!parentGo.name.StartsWith("UI"))
                    {
                        parentGo = parentGo.parent;
                    }
                    //UI名+下划线+Unit名
                    targetName = parentGo.name + "_" + goName;
                }
                else
                {
                    //默认名+下划线+Unit名
                    targetName = "Main_" + goName;
                }
            }

            //遍历脚本文件
            foreach (var file in dirInfo.GetFileSystemInfos("*.cs", SearchOption.AllDirectories))
            {
                //找到了直接打开
                if (file.Name.Equals(targetName))
                {
                    OpenFile(file.FullName);
                    UnityEngine.Debug.Log("打开UI代码 ===> " + targetName);
                    return;
                }
            }
            UnityEngine.Debug.LogError("未找到" + go.name + "相关代码");
        }
    }
}