/*********************************************
 * BFramework
 * 脚本模板
 * 创建时间：2023/01/08 20:40:23
 *********************************************/
using System.IO;
using UnityEngine;

/// <summary>
/// 新创建的脚本自动添加模板注释(头部注释)
/// </summary>
public class ScriptTemplate_editor : UnityEditor.AssetModificationProcessor
{
    //作者
    private const string Author = "";

    /// <summary>
    /// 资源创建时调用
    /// </summary>
    /// <param name="path">自动传入资源路径</param>
    public static void OnWillCreateAsset(string path)
    {
        //path = path.Replace(".meta", "");
        //if (!path.EndsWith(".cs")) return;
        ////注意,Application.datapath会根据使用平台不同而不同
        //string realPath = Application.dataPath.Replace("Assets", "") + path;
        //string allText = "/*********************************************\r\n"
        //               + " * \r\n"
        //               + " * 功能描述修改这行\r\n"
        //               + " * 创建时间：#Time\r\n"
        //               + " *********************************************/\r\n";
        //allText += File.ReadAllText(realPath);
        ////allText = allText.Replace("#UnityVersion#", Application.unityVersion);
        ////allText = allText.Replace("#Author#", Author);
        //allText = allText.Replace("#Time", System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
        //File.WriteAllText(realPath, allText);
    }
}