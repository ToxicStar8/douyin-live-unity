/*********************************************
 * 自动生成代码，禁止手动修改文件
 * 脚本名：UIMainMenu.Design.cs
 * 修改时间：2023/06/28 14:28:54
 *********************************************/

using Framework;
using UnityEngine;
using UnityEngine.UI;

namespace GameData
{
    public partial class UIMainMenu
    {
        /// <summary>
        /// 
        /// </summary>
        public UnityEngine.UI.LoopScrollRect Sv_Msg;

        /// <summary>
        /// 
        /// </summary>
        public UnityEngine.RectTransform MsgUnit;

        /// <summary>
        /// 
        /// </summary>
        public UnitPool<UIMainMenu_MsgUnit> MsgUnitPool;

        /// <summary>
        /// 
        /// </summary>
        public Framework.ButtonEx Btn_Start;

        /// <summary>
        /// 
        /// </summary>
        public Framework.TextEx Txt_Url;

        /// <summary>
        /// 
        /// </summary>
        public UnityEngine.UI.InputField Input_Room;

        public override void OnCreate()
        {
            rectTransform = gameObject.GetComponent<RectTransform>();
            Sv_Msg = rectTransform.Find("Sv_Msg").GetComponent<UnityEngine.UI.LoopScrollRect>();
			MsgUnit = rectTransform.Find("Sv_Msg/Viewport/Content/MsgUnit").GetComponent<UnityEngine.RectTransform>();
			MsgUnitPool = new UnitPool<UIMainMenu_MsgUnit>(this,MsgUnit.gameObject);
			Btn_Start = rectTransform.Find("Btn_Start").GetComponent<Framework.ButtonEx>();
			Txt_Url = rectTransform.Find("Txt_Url").GetComponent<Framework.TextEx>();
			Input_Room = rectTransform.Find("Input_Room").GetComponent<UnityEngine.UI.InputField>();
			
        }
    }
}
