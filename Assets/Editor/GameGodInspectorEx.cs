/*********************************************
 * BFramework
 * GameGod检查器扩展
 * 创建时间：2023/04/25 11:52:36
 *********************************************/
using MainPackage;
using UnityEditor;
using UnityEngine;

namespace Framework
{
    /// <summary>
    /// GameGod检查器扩展
    /// </summary>
    [CustomEditor(typeof(GameGod))]
    public class GameGodInspectorEx : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.Space(10);
            if (!Application.isPlaying || GameGod.Instance == null || GameGod.Instance.PoolManager == null)
            {
                GUI.contentColor = Color.red;
                GUILayout.Label("仅在运行下显示池对象概况");
                return;
            }



            GUI.contentColor = Color.cyan;
            GUILayout.BeginHorizontal("box", GUILayout.Width(400));
            GUILayout.Label("类对象池" , GUILayout.Width(200));
            GUILayout.Label("创建数量", GUILayout.Width(100));
            GUILayout.Label("池中数量", GUILayout.Width(100));
            GUILayout.EndHorizontal();
            //类对象池
            GUI.contentColor = Color.white;
            foreach (var item in GameGod.Instance.PoolManager.InspectorDic)
            {
                //key=name value=type
                GUILayout.BeginHorizontal("box", GUILayout.Width(400));
                GUILayout.Label(item.Key, GUILayout.Width(200));
                GUILayout.Label(item.Value[0].ToString(), GUILayout.Width(100));
                GUILayout.Label(item.Value[1].ToString(), GUILayout.Width(100));
                GUILayout.EndHorizontal();
            }

            GUI.contentColor = Color.cyan;
            GUILayout.BeginHorizontal("box", GUILayout.Width(400));
            GUILayout.Label("游戏对象池", GUILayout.Width(200));
            GUILayout.Label("创建数量", GUILayout.Width(100));
            GUILayout.Label("池中数量", GUILayout.Width(100));
            GUILayout.EndHorizontal();
            //游戏对象池
            GUI.contentColor = Color.white;
            foreach (var item in GameGod.Instance.PoolManager.GameObjectPoolDic)
            {
                GUILayout.BeginHorizontal("box", GUILayout.Width(400));
                GUILayout.Label(item.Key, GUILayout.Width(200));
                GUILayout.Label(item.Value.ObjLinkedList.Count.ToString(), GUILayout.Width(100));
                GUILayout.Label(item.Value.ObjQueue.Count.ToString(), GUILayout.Width(100));
                GUILayout.EndHorizontal();
            }



            GUILayout.Space(10);
            GUI.contentColor = Color.cyan;
            GUILayout.BeginHorizontal("box", GUILayout.Width(400));
            GUILayout.Label("使用中的计时器名", GUILayout.Width(400));
            GUILayout.EndHorizontal();
            //计时器字典
            GUI.contentColor = Color.white;
            foreach (var item in GameGod.Instance.TimeManager.TimerInfoDic)
            {
                GUILayout.BeginHorizontal("box", GUILayout.Width(400));
                GUILayout.Label(item.Key, GUILayout.Width(400));
                GUILayout.EndHorizontal();
            }

            //实时重绘
            Repaint();
        }
    }
}
