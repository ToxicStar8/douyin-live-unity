/*********************************************
 * BFramework
 * 2D碰撞器扩展类
 * 创建时间：2023/04/25 16:45:24
 *********************************************/
using UnityEngine;

namespace Framework
{
    /// <summary>
    /// 文本扩展类
    /// </summary>
    public static class Ex_Collider2D
    {
        /// <summary>
        /// 获得区域内随机一个点
        /// </summary>
        public static Vector2 GetRandomPos(this Collider2D cr)
        {
            var x = cr.bounds.size.x * 0.5f;
            var y = cr.bounds.size.y * 0.5f;
            var rdX = UnityEngine.Random.Range(-x, x);
            var rdY = UnityEngine.Random.Range(-y, y);
            var rdPos = new Vector2(rdX, rdY);
            return rdPos;
        }
    }
}
