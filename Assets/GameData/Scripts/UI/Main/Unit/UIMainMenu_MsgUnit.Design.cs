/*********************************************
 * 自动生成代码，禁止手动修改文件
 * 脚本名：UIMainMenu_MsgUnit.cs
 * 修改时间：2023/06/28 14:28:54
 *********************************************/

using Framework;
using UnityEngine;
using UnityEngine.UI;

namespace GameData
{
    public partial class UIMainMenu_MsgUnit
    {
        /// <summary>
        /// 
        /// </summary>
        public Framework.TextEx Txt_Msg;

        public override void OnCreate()
        {
            rectTransform = gameObject.GetComponent<RectTransform>();
            Txt_Msg = rectTransform.Find("Txt_Msg").GetComponent<Framework.TextEx>();
			
        }
    }
}
