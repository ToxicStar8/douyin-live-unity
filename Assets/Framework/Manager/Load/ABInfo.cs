/*********************************************
 * BFramework
 * AB包信息类
 * 创建时间：2023/01/08 20:40:23
 *********************************************/
using System.Collections.Generic;

namespace Framework
{
    /// <summary>
    /// AB包信息类
    /// </summary>
    public class ABInfo
    {
        //文件名,文件所属包名
        public Dictionary<string, string> ABFileDic = new Dictionary<string, string>();
        //包名,包数据（路径,依赖列表）
        public List<ABRelyInfo> ABRelyInfoList = new List<ABRelyInfo>();

        /// <summary>
        /// AB包依赖信息类
        /// </summary>
        public class ABRelyInfo
        {
            /// <summary>
            /// 名字
            /// </summary>
            public string ABName;
            /// <summary>
            /// 路径
            /// </summary>
            //public string ABPath;
            /// <summary>
            /// 依赖的包名列表
            /// </summary>
            public List<string> ABRelyOnNameList;
        }
    }
}
