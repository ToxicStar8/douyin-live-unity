/*********************************************
 * BFramework
 * UI代码生成
 * 创建时间：2022/12/25 20:40:23
 *********************************************/
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Framework
{
    /// <summary>
    /// UI代码生成
    /// </summary>
    public class GenerateUIScript : Editor
    {
        [MenuItem("GameObject/生成UI代码", false, 10000)]
        public static void GenerateUI()
        {
            var go = Selection.activeGameObject;
            if (go == null || !(go.name.StartsWith("UI") || go.name.EndsWith("Unit")))
            {
                Debug.LogError("未选中UI/Unit！");
                return;
            }

            //寻找到当前obj的路径
            var directoryInfo = new DirectoryInfo(Application.dataPath);
            string prefabName = go.name + ".prefab";
            string path = string.Empty;
            foreach (var item in directoryInfo.GetFiles("*.*", SearchOption.AllDirectories))
            {
                if (item.Name == prefabName)
                {
                    //预制体路径
                    path = item.FullName;
                    if (go.name.StartsWith("UI"))
                    {
                        //替换成脚本路径
                        path = path.Replace("GameData\\Prefabs", "GameData\\Scripts").Substring(0, path.Length - prefabName.Length);
                    }
                    else
                    {
                        path = path.Replace("GameData\\Prefabs", "GameData\\Scripts").Substring(0, path.Length - prefabName.Length - 6);
                    }
                    break;
                }
            }

            if (string.IsNullOrEmpty(path))
            {
                Debug.LogError("UI内的Unit请直接使用UI生成代码");
                return;
            }
            //创建目录
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (go.name.StartsWith("UI"))
            {
                CreateUIScript(go, path);
                CreateUIDesignScript(go, path);
            }
            else if (go.name.EndsWith("Unit"))
            {
                //没有父物体就直接用父文件夹名
                var parentName = new DirectoryInfo(path).Name;
                CreateUnitScript(parentName, go, path);
                CreateUnitDesignScript(parentName, go, path);
            }

            Debug.Log(go.name + "生成完毕！");
            //回收资源
            System.GC.Collect();
            //刷新编辑器
            AssetDatabase.Refresh();
        }

        private static void CreateUIScript(GameObject go, string path)
        {
            string temp = @"/*********************************************
 * 
 * 脚本名：#UIName.cs
 * 创建时间：#Time
 *********************************************/
using Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameData
{
    public partial class #UIName : GameUIBase
    {
        public override void OnInit()
        {#ButtonBindFunc
        }

        public override void OnShow(params object[] args)
        {
            
        }
#ButtonOnClick
        public override void OnBeforDestroy()
        {
            
        }
    }
}
";
            if (!File.Exists(path + go.name + ".cs"))
            {
                //按钮绑定方法
                var uiBindtList = go.GetComponentsInChildren<UIBind>().ToList();
                string btnBindFunc = string.Empty;
                string btnOnClickStr = string.Empty;
                for (int i = 0; i < uiBindtList.Count; i++)
                {
                    var uiBind = uiBindtList[i];
                    //空就跳过
                    if(uiBind == null)
                    {
                        continue;
                    }
                    //如果有Unit 就把Unit下面的绑定组件全部置空
                    if (uiBind.Type != BindType.Component)
                    {
                        var unitBindtArr = uiBind.GetComponentsInChildren<UIBind>();
                        foreach (var unitComponent in unitBindtArr)
                        {
                            if (uiBind.name == unitComponent.name)
                            {
                                continue;
                            }

                            var index = uiBindtList.FindIndex(x => x != null && x.GetHashCode() == unitComponent.GetHashCode());
                            uiBindtList[index] = null;
                        }
                    }
                    //如果是按钮 就直接追加方法
                    if (uiBind.GetComponent<Button>() != null)
                    {
                        string funcName = "OnClick_" + uiBind.name;
                        btnBindFunc += "\r\n            " + uiBind.name + ".AddListener(" + funcName + ");";
                        btnOnClickStr += "\r\n        private void " + funcName + "()\r\n        {\r\n            Log(E_Log.Log,\"点击了" + uiBind.name + "\");\r\n        }\r\n";
                        continue;
                    }
                }
                //如果为空 加个回车 美观
                btnBindFunc = string.IsNullOrWhiteSpace(btnBindFunc) ? "\r\n" : btnBindFunc;

                //导出文件 替换文本
                //var scripts = File.CreateText(path + go.name + ".cs");
                temp = temp.Replace("#UIName", go.name);
                temp = temp.Replace("#Time", System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                temp = temp.Replace("#ButtonBindFunc", btnBindFunc);
                temp = temp.Replace("#ButtonOnClick", btnOnClickStr);
                //scripts.Write(temp);
                //scripts.Close();
                File.WriteAllText(path + go.name + ".cs", temp, Encoding.UTF8);
            }
        }

        /// <summary>
        /// 生成UI绑定关系 每次覆盖
        /// </summary>
        /// <param name="go"></param>
        /// <param name="path"></param>
        private static void CreateUIDesignScript(GameObject go, string path)
        {
            string temp = @"/*********************************************
 * 自动生成代码，禁止手动修改文件
 * 脚本名：#UIName.Design.cs
 * 修改时间：#Time
 *********************************************/

using Framework;
using UnityEngine;
using UnityEngine.UI;

namespace GameData
{
    public partial class #UIName
    {#Component
        public override void OnCreate()
        {
            rectTransform = gameObject.GetComponent<RectTransform>();
            #Find
        }
    }
}
";
            string newComponentStr;
            string newFindStr;
            //生成绑定关系
            GenerateBind(0, go, path, out newComponentStr, out newFindStr);

            //创建对象
            //var scripts = File.CreateText(path + go.name + ".Design.cs");
            //文本替换
            temp = temp.Replace("#UIName", go.name);
            temp = temp.Replace("#Time", System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
            temp = temp.Replace("#Component", newComponentStr);
            temp = temp.Replace("#Find", newFindStr);
            //scripts.Write(temp);
            //scripts.Close();
            File.WriteAllText(path + go.name + ".Design.cs", temp, Encoding.UTF8);
        }

        /// <summary>
        /// 生成绑定关系
        /// </summary>
        /// <param name="startIndex">UI=0 Unit=1 因为UIBind会获取自己 如果Unit也是0会出问题</param>
        /// <param name="go">目标对象</param>
        /// <param name="path">脚本路径</param>
        /// <param name="componentStr">组件文本模板</param>
        /// <param name="FindStr">寻找文本模板</param>
        /// <param name="newComponentStr">拿去替换的组件文本</param>
        /// <param name="newFindStr">拿去替换的寻找文本</param>
        private static void GenerateBind(int startIndex, GameObject go, string path, out string newComponentStr, out string newFindStr)
        {
            string componentStr = @"
        /// <summary>
        /// #Content
        /// </summary>
        public #Component #Name;
";
            string FindStr = "#Name = rectTransform.Find(\"#Path\").GetComponent<#Component>();";

            string PoolStr = "#PoolName = new UnitPool<#UnitName>(this,#Obj);";

            var componentList = go.GetComponentsInChildren<UIBind>(true).ToList();
            newComponentStr = string.Empty;
            newFindStr = string.Empty;
            for (int i = startIndex; i < componentList.Count; i++)
            {
                var uiBind = componentList[i];
                //Unit下置空的可能性
                if (uiBind == null)
                {
                    continue;
                }
                //只生成组件
                if (uiBind.Type != BindType.Component)
                {
                    if (!uiBind.name.EndsWith("Unit"))
                    {
                        Debug.LogError(uiBind.name + "必须是以Unit结尾才允许设置类型为Unit！");
                    }
                    else
                    {
                        //生成Unit代码
                        CreateUnitDesignScript(go.name,uiBind.gameObject, path);
                        CreateUnitScript(go.name, uiBind.gameObject, path);
                    }
                    //把这个组件树下的全部组件删除不生成
                    foreach (var unitComponent in uiBind.GetComponentsInChildren<UIBind>())
                    {
                        if (uiBind.name == unitComponent.name)
                        {
                            continue;
                        }

                        var findIndex = componentList.FindIndex(x => x != null && x.GetHashCode() == unitComponent.GetHashCode());
                        componentList[findIndex] = null;
                    }
                }
                //文本存储
                if (uiBind.BindComponent != null)
                {
                    //声明变量
                    var tempComponentStr = componentStr.Replace("#Name", uiBind.BindComponent.name);
                    tempComponentStr = tempComponentStr.Replace("#Component", uiBind.BindComponent.GetType().ToString());
                    tempComponentStr = tempComponentStr.Replace("#Content", uiBind.Content);
                    newComponentStr += tempComponentStr;

                    //寻找对象
                    var tempFindStr = FindStr.Replace("#Component", uiBind.BindComponent.GetType().ToString());
                    tempFindStr = tempFindStr.Replace("#Name", uiBind.BindComponent.name);
                    tempFindStr = tempFindStr.Replace("#Path", FindComponentInPrefabPath(go.name, uiBind.gameObject));
                    tempFindStr += "\r\n\t\t\t";
                    newFindStr += tempFindStr;

                    //Unit类型
                    if (uiBind.Type != BindType.Component)
                    {
                        var poolName = uiBind.BindComponent.name + "Pool";
                        var className = go.name + "_" + uiBind.BindComponent.name;

                        //追加一个Pool
                        var tempComponentPoolStr = componentStr.Replace("#Name", poolName);
                        tempComponentPoolStr = tempComponentPoolStr.Replace("#Component", "UnitPool<" + className + ">");
                        tempComponentPoolStr = tempComponentPoolStr.Replace("#Content", "");
                        newComponentStr += tempComponentPoolStr;

                        var tempFindPoolStr = PoolStr.Replace("#PoolName", poolName);
                        tempFindPoolStr = tempFindPoolStr.Replace("#UnitName", className);
                        tempFindPoolStr = tempFindPoolStr.Replace("#Obj", uiBind.gameObject.name + ".gameObject");
                        tempFindPoolStr += "\r\n\t\t\t";
                        newFindStr += tempFindPoolStr;
                    }
                }
                else
                {
                    Debug.LogError(uiBind.name + "没有绑定组件");
                }
            }
        }

        private static void CreateUnitScript(string uiName, GameObject go, string path)
        {
            string temp = @"/*********************************************
 * 
 * 脚本名：#UnitName.cs
 * 创建时间：#Time
 *********************************************/
using Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameData
{
    public partial class #UnitName : UnitBase
    {
        public override void OnInit()
        {
            
        }

        public void FnShow()
        {
            
        }
    }
}
";

            var unitPath = path + "/Unit";
            //创建目录
            if (!Directory.Exists(unitPath))
            {
                Directory.CreateDirectory(unitPath);
            }

            var writePath = unitPath + "/" + uiName + "_" + go.name + ".cs";
            //已经生成过了就不覆盖了
            if (!File.Exists(writePath))
            {
                //导出文件 替换文本
                //var scripts = File.CreateText(unitPath + "/" + go.name + ".cs");
                temp = temp.Replace("#UnitName", uiName + "_" + go.name);
                temp = temp.Replace("#Time", System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                //scripts.Write(temp);
                //scripts.Close();
                File.WriteAllText(writePath, temp, Encoding.UTF8);
            }
        }

        /// <summary>
        /// 生成Unit绑定关系 每次覆盖
        /// </summary>
        private static void CreateUnitDesignScript(string uiName,GameObject go, string path)
        {
            string temp = @"/*********************************************
 * 自动生成代码，禁止手动修改文件
 * 脚本名：#UnitName.cs
 * 修改时间：#Time
 *********************************************/

using Framework;
using UnityEngine;
using UnityEngine.UI;

namespace GameData
{
    public partial class #UnitName
    {#Component
        public override void OnCreate()
        {
            rectTransform = gameObject.GetComponent<RectTransform>();
            #Find
        }
    }
}
";

            string newComponentStr;
            string newFindStr;
            //生成绑定关系
            GenerateBind(1, go, path, out newComponentStr, out newFindStr);

            var unitPath = path + "/Unit";
            //创建目录
            if (!Directory.Exists(unitPath))
            {
                Directory.CreateDirectory(unitPath);
            }

            temp = temp.Replace("#UnitName", uiName + "_" + go.name);
            temp = temp.Replace("#Time", System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
            temp = temp.Replace("#Component", newComponentStr);
            temp = temp.Replace("#Find", newFindStr);
            //创建对象
            //var scripts = File.CreateText(unitPath + "/" + go.name + ".Design.cs");
            //文本替换
            //scripts.Write(temp);
            //scripts.Close();
            File.WriteAllText(unitPath + "/" + uiName + "_" + go.name + ".Design.cs", temp, Encoding.UTF8);
        }

        /// <summary>
        /// 循环父物体拿到组件路径
        /// </summary>
        private static string FindComponentInPrefabPath(string scriptName, GameObject go)
        {
            //拿到父物体 修改路径 设置跳出值
            var parentGo = go.transform.parent;
            string path = parentGo.name + "/" + go.name;
            bool isParent = true;

            while (isParent)
            {
                //如果父物体名字等于传进来的对象名 就退出
                if (parentGo.name == scriptName)
                {
                    isParent = false;
                    break;
                }

                //循环添加路径
                parentGo = parentGo.transform.parent;
                path = parentGo.name + "/" + path;
            }

            //删除最上级物体名
            path = path.Substring(parentGo.name.Length + 1);

            return path;
        }
    }
}
