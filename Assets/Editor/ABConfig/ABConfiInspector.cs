/*********************************************
 * BFramework
 * AB包菜单列表扩展
 * 创建时间：2022/12/28 16:33:23
 *********************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Framework
{
    [CustomEditor(typeof(ABConfig))]
    public class ABConfiInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            ABConfig abconfig = target as ABConfig;
            if (GUILayout.Button("生成真正的AB包列表"))
            {
                abconfig.SetABNameAndPath();
            }
        }
    }
}