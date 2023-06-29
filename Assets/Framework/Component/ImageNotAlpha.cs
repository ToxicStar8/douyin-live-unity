/*********************************************
 * BFramework
 * 不点击透明区域的图片类
 * 创建时间：2023/01/08 20:40:23
 *********************************************/
using UnityEngine;
using UnityEngine.UI;

namespace Framework
{
    /// <summary>
    /// 不点击透明区域的图片类
    /// </summary>
    public class ImageNotAlpha : Image
    {
        public ImageNotAlpha()
        {
            // 将图片的alpha命中最小阈值调成0.5,透明度在0.5以下的区域都不会遮挡射线
            this.alphaHitTestMinimumThreshold = 0.5f;
            // 同时,需要在图片的可写入属性中选择打开,并且该图片不能打包图集
        }
    }
}
