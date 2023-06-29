/*********************************************
 * BFramework
 * 游戏对象池
 * 创建时间：2023/01/08 20:40:23
 *********************************************/
using MainPackage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class GameObjectPool
    {
        /// <summary>
        /// 池队列
        /// </summary>
        public Queue<GameObject> ObjQueue { private set; get; } = new Queue<GameObject>();

        /// <summary>
        /// 已创建出来的对象
        /// </summary>
        public LinkedList<GameObject> ObjLinkedList { private set; get; } = new LinkedList<GameObject>();

        public string ObjName { private set; get; }
        public int Count => ObjLinkedList.Count;
        //public GameObject this[int index] => ObjList[index];

        public GameObjectPool(string objName)
        {
            ObjName = objName;
        }

        /// <summary>
        /// 创建对象
        /// </summary>
        public GameObject CreateObj(Transform trans = null)
        {
            GameObject go = null;
            if (ObjQueue.Count == 0)
            {
                //GameEntry.Instance.Log(E_Log.Framework, "不存在" + ObjName + "对象","创建");
                var obj = GameGod.Instance.LoadManager.LoadSync<GameObject>(ObjName);
                go = Object.Instantiate(obj, trans);
            }
            else
            {
                //GameEntry.Instance.Log(E_Log.Framework, "已有" + ObjName + "对象", "取出");
                go = ObjQueue.Dequeue();
                go.SetParent(trans);
            }
            //go.SetScale(Vector3.one);
            go.SetLocalPos(Vector3.zero);
            go.SetLocalRotation(Quaternion.identity);
            ObjLinkedList.AddLast(go);
            return go;
        }

        /// <summary>
        /// 回收对象
        /// </summary>
        public void Recycle(GameObject go)
        {
            //GameEntry.Instance.Log(E_Log.Framework, ObjName,"回池");
            go.SetParent(GameEntry.Instance.ObjPool);
            ObjLinkedList.Remove(go);
            ObjQueue.Enqueue(go);
        }

        /// <summary>
        /// 回收全部对象
        /// </summary>
        public void RecycleAll()
        {
            for (var curNode = ObjLinkedList.First; curNode != null; curNode = curNode.Next)
            {
                var go = curNode.Value;
                go.SetParent(GameEntry.Instance.ObjPool);
                ObjQueue.Enqueue(go);
            }
            ObjLinkedList.Clear();
        }

        /// <summary>
        /// 销毁对象
        /// </summary>
        public void Relase(GameObject go)
        {
            ObjLinkedList.Remove(go);
            go.Destroy();
        }

        /// <summary>
        /// 销毁全部对象
        /// </summary>
        public void RelaseAll()
        {
            for (var curNode = ObjLinkedList.First; curNode != null; curNode = curNode.Next)
            {
                Relase(curNode.Value);
            }
            ObjLinkedList.Clear();
        }

        public void OnDispose()
        {
            RelaseAll();
            ObjLinkedList = null;

            for (int i = 0; i < ObjQueue.Count; i++)
            {
                var go = ObjQueue.Dequeue();
                go.Destroy();
            }
            ObjQueue.Clear();
            ObjQueue = null;

            //只在关闭池的时候卸载一次
            GameGod.Instance.LoadManager.UnloadAsset(ObjName);
        }
    }
}