/*********************************************
 * 自动生成代码，禁止手动修改文件
 * 脚本名：UIAddItemTips.Design.cs
 * 修改时间：2023/03/14 13:57:54
 *********************************************/

using Framework;
using UnityEngine;
using UnityEngine.UI;

namespace GameData
{
    public partial class UIAddItemTips
    {
        /// <summary>
        /// 
        /// </summary>
        public UnityEngine.RectTransform TipsUnit;

        /// <summary>
        /// 
        /// </summary>
        public UnitPool<UIAddItemTips_TipsUnit> TipsUnitPool;

        /// <summary>
        /// 
        /// </summary>
        public Framework.ImageEx Img_Item;

        public override void OnCreate()
        {
            rectTransform = gameObject.GetComponent<RectTransform>();
            TipsUnit = rectTransform.Find("TipsUnit").GetComponent<UnityEngine.RectTransform>();
			TipsUnitPool = new UnitPool<UIAddItemTips_TipsUnit>(this,TipsUnit.gameObject);
			Img_Item = rectTransform.Find("TipsUnit/Txt_Tips/Img_Item").GetComponent<Framework.ImageEx>();
			
        }
    }
}
