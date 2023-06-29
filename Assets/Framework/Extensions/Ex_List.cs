/*********************************************
 * BFramework
 * 列表方法扩展类
 * 创建时间：2023/01/08 20:40:23
 *********************************************/
using LitJson;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Framework
{
    /// <summary>
    /// 列表扩展类
    /// </summary>
    public static class Ex_List
    {
        public static string ToJson<T>(this List<T> list)
        {
            return JsonMapper.ToJson(list);
        }
        /// <summary>
        /// 将List乱序排放
        /// </summary>
        public static void SortByDisordered<T>(this List<T> list)
        {
            List<T> newList = list.GetRange(0, list.Count);
            list.Clear();
            while (newList.Count > 0)
            {
                list.Add(newList.RandomAndRemove());
            }
        }

        /// <summary>
        /// 随机取一个数据
        /// </summary>
        public static T Random<T>(this List<T> list)
        {
            if (list == null)
            {
                Debug.LogError("list 不能为空 null");
                return default(T);
            }
            if (list.Count > 0)
            {
                return list[UnityEngine.Random.Range(0, list.Count)];
            }
            return default(T);
        }

        /// <summary>
        /// 随机取一个数据并删除
        /// </summary>
        public static T RandomAndRemove<T>(this List<T> list)
        {
            var item = list.Random();
            list.Remove(item);
            return item;
        }

        /// <summary>
        /// 转换成 1,2,3,4……
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static string ToStringBySplit<T>(this List<T> list, char c)
        {
            StringBuilder sb = new StringBuilder("");
            for (int i = 0; i < list.Count; i++)
            {
                if (i == 0)
                {
                    sb.Append(list[i].ToString());
                }
                else
                {
                    sb.Append(c);
                    sb.Append(list[i].ToString());
                }
            }
            return sb.ToString();
        }
    }
}
