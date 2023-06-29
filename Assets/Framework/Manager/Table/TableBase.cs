/*********************************************
 * BFramework
 * 表基类
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
    /// 表基类
    /// </summary>
    public abstract class TableBase
    {
        public abstract int Id { protected set; get; }
        public int DataId => Id;
        public abstract void OnInit(string[] group, string dataStrArr);
    }
}