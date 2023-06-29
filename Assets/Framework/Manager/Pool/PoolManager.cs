/*********************************************
 * BFramework
 * 对象池管理器
 * 创建时间：2023/01/08 20:40:23
 *********************************************/
using MainPackage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Framework
{
    /// <summary>
    /// 对象池管理器
    /// </summary>
    public class PoolManager : ManagerBase
    {
        /// <summary>
        /// 游戏对象池字典
        /// </summary>
        public Dictionary<string, GameObjectPool> GameObjectPoolDic { private set; get; }

        /// <summary>
        /// 类对象池字典
        /// </summary>
        public Dictionary<string, object> ClassObjectPoolDic { private set; get; }

#if UNITY_EDITOR
        /// <summary>
        /// 在监视面板显示的信息
        /// </summary>
        public Dictionary<string, int[]> InspectorDic = new Dictionary<string, int[]>();
#endif

        public override void OnStart()
        {
            GameObjectPoolDic = new Dictionary<string, GameObjectPool>();
            ClassObjectPoolDic = new Dictionary<string, object>();
        }

        #region 游戏对象池
        /// <summary>
        /// 创建游戏对象池
        /// </summary>
        public GameObjectPool CreateGameObjectPool(string objName)
        {
            var name = objName.Replace(".prefab", "");
            if (!GameObjectPoolDic.TryGetValue(name, out var pool))
            {
                GameGod.Instance.Log(E_Log.Framework, "不存在" + name + "池","创建");
                //初始化使用完整预制体名字
                pool = new GameObjectPool(objName);
                GameObjectPoolDic[name] = pool;
            }
            return pool;
        }

        /// <summary>
        /// 获取池中的游戏对象
        /// </summary>
        public GameObject CreateGameObject(string objName,Transform trans = null)
        {
            var pool = CreateGameObjectPool(objName);
            var go = pool.CreateObj(trans);
            return go;
        }
        /// <summary>
        /// 回收游戏对象到池中
        /// </summary>
        public void RecycleGameObject(GameObject go)
        {
            var name = go.name.Replace("(Clone)", "");
            var pool = CreateGameObjectPool(name);
            pool.Recycle(go);
        }
        /// <summary>
        /// 销毁游戏对象池
        /// </summary>
        public void DisposeGameObjectPool(string goName)
        {
            if (GameObjectPoolDic.TryGetValue(goName, out var pool))
            {
                pool.OnDispose();
                pool = null;
                GameObjectPoolDic.Remove(goName);
            }
        }
        #endregion

        #region 类对象池
        /// <summary>
        /// 创建类对象池
        /// </summary>
        public ClassObjectPool<T> CreateClassObjectPool<T>() where T : class, new()
        {
            string className = typeof(T).Name;
            if (!ClassObjectPoolDic.TryGetValue(className, out var pool))
            {
                GameGod.Instance.Log(E_Log.Framework, "不存在" + className + "池","创建");
                pool = new ClassObjectPool<T>();
                ClassObjectPoolDic[className] = pool;
            }
#if UNITY_EDITOR
            //更新面板类的数量
            var inspectorShow = pool as ClassObjectPool<T>;
            InspectorDic[className] = new int[2] { inspectorShow.ClassLinkedList.Count, inspectorShow.ClassQueue.Count };
#endif
            return pool as ClassObjectPool<T>;
        }
        /// <summary>
        /// 获取池中的类
        /// </summary>
        public T CreateClassObj<T>() where T: class, new()
        {
            var pool = CreateClassObjectPool<T>();
            var obj = pool.CreateClassObj();
            return obj;
        }
        /// <summary>
        /// 回收类到池中
        /// </summary>
        public void RecycleClassObj<T>(T obj) where T : class, new()
        {
            var pool = CreateClassObjectPool<T>();
            pool.Recycle(obj);
        }
        public void DisposeClassObjectPool<T>() where T : class,new()
        {
            string className = typeof(T).Name;
            if (ClassObjectPoolDic.TryGetValue(className, out var obj))
            {
                var pool = obj as ClassObjectPool<T>;
                pool.OnDispose();
                pool = null;
                ClassObjectPoolDic.Remove(className);

#if UNITY_EDITOR
                InspectorDic.Remove(className);
#endif
            }
        }
        #endregion

        public override void OnUpdate() { }
        public override void OnDispose()
        {
            foreach (var item in GameObjectPoolDic)
            {
                item.Value.OnDispose();
            }
            GameObjectPoolDic.Clear();
            GameObjectPoolDic = null;

            ClassObjectPoolDic.Clear();
            ClassObjectPoolDic = null;
        }
    }
}