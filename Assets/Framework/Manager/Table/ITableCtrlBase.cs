/*********************************************
 * BFramework
 * 表控制器接口
 * 创建时间：2023/01/08 20:40:23
 *********************************************/
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace Framework
{
    /// <summary>
    /// 表控制器接口
    /// </summary>
    public interface ITableCtrlBase     //字典中的值类型不能使用不带参数的泛型方法 所以这里使用接口
    {
        public abstract int GetCreateStatus();
        public abstract void OnInitData();
    }
}