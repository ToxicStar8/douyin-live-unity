/*********************************************
 * BFramework
 * 字典方法扩展类
 * 创建时间：2023/01/08 20:40:23
 *********************************************/
using LitJson;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    /// <summary>
    /// 字典扩展类
    /// </summary>
    public static class Ex_Dictionary
    {
        public static string ToJson<K, V>(this Dictionary<K,V> dic)
        {
            return JsonMapper.ToJson(dic);
        }
    }
}
