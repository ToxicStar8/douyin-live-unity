/*********************************************
 * BFramework
 * 数据基类
 * 创建时间：2023/01/08 20:40:23
 *********************************************/
using Framework;

namespace GameData
{
    /// <summary>
    /// 数据基类
    /// </summary>
    public abstract class DataBase : GameBase
    {
        public DataBase() { }
        public abstract void OnInit();
        public abstract void OnDispose();
    }
}
