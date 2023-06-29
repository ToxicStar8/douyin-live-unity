/*********************************************
 * BFramework
 * 类对象池
 * 创建时间：2023/01/08 20:40:23
 *********************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    /// <summary>
    /// 类对象池
    /// </summary>
    public class ClassObjectPool<T> where T : class, new()
    {
        /// <summary>
        /// 池队列
        /// </summary>
        public Queue<T> ClassQueue { private set; get; } = new Queue<T>();

        /// <summary>
        /// 已创建出来的对象 频繁增删 使用链表
        /// </summary>
        public LinkedList<T> ClassLinkedList { private set; get; } = new LinkedList<T>();

        public string ClassName => typeof(T).Name;
        public int Count => ClassLinkedList.Count;
        //public T this[int index] => ClassList[index];

        /// <summary>
        /// 创建对象
        /// </summary>
        public T CreateClassObj()
        {
            T t = null;
            if (ClassQueue.Count == 0)
            {
                //GameEntry.Instance.Log(E_Log.Framework, "不存在" + ClassName + "对象", "创建");
                t = new T();
            }
            else
            {
                //GameEntry.Instance.Log(E_Log.Framework, "已有" + ClassName + "对象", "取出");
                t = ClassQueue.Dequeue();
            }
            ClassLinkedList.AddLast(t);
#if UNITY_EDITOR
            //更新类数量
            GameGod.Instance.PoolManager.CreateClassObjectPool<T>();
#endif
            return t;
        }

        /// <summary>
        /// 回收对象
        /// </summary>
        public void Recycle(T obj)
        {
            //GameEntry.Instance.Log(E_Log.Framework, ClassName, "回池");
            ClassLinkedList.Remove(obj);
            ClassQueue.Enqueue(obj);
#if UNITY_EDITOR
            //更新类数量
            GameGod.Instance.PoolManager.CreateClassObjectPool<T>();
#endif
        }

        /// <summary>
        /// 回收全部对象
        /// </summary>
        public void RecycleAll()
        {
            for (var curNode = ClassLinkedList.First; curNode != null; curNode = curNode.Next)
            {
                var t =  curNode.Value;
                ClassQueue.Enqueue(t);
            }
            ClassLinkedList.Clear();
#if UNITY_EDITOR
            //更新类数量
            GameGod.Instance.PoolManager.CreateClassObjectPool<T>();
#endif
        }

        /// <summary>
        /// 销毁对象
        /// </summary>
        public void Relase(T obj)
        {
            ClassLinkedList.Remove(obj);
            obj = null;
#if UNITY_EDITOR
            //更新类数量
            GameGod.Instance.PoolManager.CreateClassObjectPool<T>();
#endif
        }

        /// <summary>
        /// 只销毁已创建出来的对象
        /// </summary>
        public void RelaseAll()
        {
            for (var curNode = ClassLinkedList.First; curNode != null; curNode = curNode.Next)
            {
                curNode.Value = null;
            }
            ClassLinkedList.Clear();
#if UNITY_EDITOR
            //更新类数量
            GameGod.Instance.PoolManager.CreateClassObjectPool<T>();
#endif
        }

        public void OnDispose()
        {
            RelaseAll();
            ClassLinkedList = null;

            for (int i = 0; i < ClassQueue.Count; i++)
            {
                var obj = ClassQueue.Dequeue();
                obj = null;
            }
            ClassQueue.Clear();
            ClassQueue = null;
        }
    }
}