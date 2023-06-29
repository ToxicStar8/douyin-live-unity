/*********************************************
 * 自动生成代码，禁止手动修改文件
 * 脚本名：UIAddItemTips_TipsUnit.cs
 * 修改时间：2023/03/14 13:57:54
 *********************************************/

using Framework;
using UnityEngine;
using UnityEngine.UI;

namespace GameData
{
    public partial class UIAddItemTips_TipsUnit
    {
        /// <summary>
        /// 
        /// </summary>
        public Framework.TextEx Txt_Tips;

        /// <summary>
        /// 
        /// </summary>
        public Framework.ImageEx Img_Item;

        public override void OnCreate()
        {
            rectTransform = gameObject.GetComponent<RectTransform>();
            Txt_Tips = rectTransform.Find("Txt_Tips").GetComponent<Framework.TextEx>();
			Img_Item = rectTransform.Find("Txt_Tips/Img_Item").GetComponent<Framework.ImageEx>();
			
        }
    }
}
