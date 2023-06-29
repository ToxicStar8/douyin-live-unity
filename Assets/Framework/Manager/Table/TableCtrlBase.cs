/*********************************************
 * BFramework
 * 表控制器基类
 * 创建时间：2023/01/08 20:40:23
 *********************************************/
using MainPackage;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace Framework
{
    /// <summary>
    /// 表控制器基类
    /// </summary>
    public abstract class TableCtrlBase<T> : ITableCtrlBase where T : TableBase, new()        // 先接口 再限定T，如果先限定T再写接口会判定这是T的接口
    {
        /// <summary>
        /// 表名
        /// </summary>
        public abstract string TableName { get; }

        /// <summary>
        /// 初始化数据状态 0=未初始化 1=初始化中 2=初始化完毕
        /// </summary>
        private int _initDataStatus = 0;

        /// <summary>
        /// 数据列表
        /// </summary>
        public List<T> DataList { private set; get; } = new List<T>();
        public int Count => DataList.Count;
        public T this[int index] => GetDataByIndex(index);

        /// <summary>
        /// 安全的通过索引获得数据
        /// </summary>
        public T GetDataByIndex(int index)
        {
            if (index > DataList.Count - 1)
            {
                GameGod.Instance.Log(E_Log.Error, "超出数据上限 索引", index.ToString());
                return null;
            }
            return DataList[index];
        }

        /// <summary>
        /// 通过Id获得数据
        /// </summary>
        public T GetDataById(int id)
        {
            for (int i = 0, count = DataList.Count; i < count; i++)
            {
                var table = DataList[i];
                if (table.Id == id)
                {
                    return table;
                }
            }
            GameGod.Instance.Log(E_Log.Error, "没有找到表数据 id", id.ToString());
            return null;
        }

        public int GetCreateStatus() => _initDataStatus;

        public void OnInitData()
        {
            //表格的AB包不需要卸载
            var textAsset = GameGod.Instance.LoadManager.LoadSync<TextAsset>(TableName);
            var allArr = textAsset.text.Split("\r\n", System.StringSplitOptions.RemoveEmptyEntries);  //全部的文本
            var groupArr = allArr[1].Split('^');   //变量名的组
            //从第四行开始出数据
            for (int i = 3, length = allArr.Length; i < length; i++)
            {
                T table = new T();
                table.OnInit(groupArr, allArr[i]);
                DataList.Add(table);
            }

            _initDataStatus = 2;
        }
    }
}