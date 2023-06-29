/*********************************************
 * BFramework
 * 数据管理器
 * 创建时间：2023/01/08 20:40:23
 *********************************************/
using GameData;

namespace Framework
{
    /// <summary>
    /// 数据管理器
    /// </summary>
    public partial class DataManager
    {
        //随用随取
        //这种方式获取避免新模块没有初始化
        //public PlayData PlayData
        //{
        //    get
        //    {
        //        if (Data.PlayData == null)
        //        {
        //            Data.PlayData = new PlayData();
        //            Data.PlayData.OnInit();
        //        }
        //        return Data.PlayData;
        //    }
        //}
    }
}
