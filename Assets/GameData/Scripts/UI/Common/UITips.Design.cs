/*********************************************
 * 自动生成代码，禁止手动修改文件
 * 脚本名：UITips.Design.cs
 * 修改时间：2023/03/14 13:57:39
 *********************************************/

using Framework;
using UnityEngine;
using UnityEngine.UI;

namespace GameData
{
    public partial class UITips
    {
        /// <summary>
        /// 
        /// </summary>
        public Framework.ImageEx TipsUnit;

        /// <summary>
        /// 
        /// </summary>
        public UnitPool<UITips_TipsUnit> TipsUnitPool;

        public override void OnCreate()
        {
            rectTransform = gameObject.GetComponent<RectTransform>();
            TipsUnit = rectTransform.Find("TipsUnit").GetComponent<Framework.ImageEx>();
			TipsUnitPool = new UnitPool<UITips_TipsUnit>(this,TipsUnit.gameObject);
			
        }
    }
}
