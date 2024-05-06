/*********************************************
 * BFramework
 * 单例类基类
 * 创建时间：2023/07/07/ 16:01:23
 *********************************************/

namespace Framework
{
    /// <summary>
    /// 单例类基类
    /// </summary>
    public abstract class InstanceBase<T> where T : class, new()
    {
        private static T _instance;
        //单例
        public static T Instance { 
            get
            {
                if(_instance == null)
                {
                    _instance = new T();
                }
                return _instance;
            }
        }
    }
}