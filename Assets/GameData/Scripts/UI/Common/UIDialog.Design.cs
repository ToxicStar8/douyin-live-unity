/*********************************************
 * 自动生成代码，禁止手动修改文件
 * 脚本名：UIDialog.Design.cs
 * 修改时间：2023/01/28 14:04:17
 *********************************************/

using Framework;
using UnityEngine;
using UnityEngine.UI;

namespace GameData
{
    public partial class UIDialog
    {
        /// <summary>
        /// 
        /// </summary>
        public Framework.ButtonEx Btn_Mask;

        /// <summary>
        /// 
        /// </summary>
        public Framework.ImageEx Img_Bg;

        /// <summary>
        /// 
        /// </summary>
        public Framework.TextEx Txt_Content;

        /// <summary>
        /// 
        /// </summary>
        public Framework.TextEx Txt_Title;

        /// <summary>
        /// 
        /// </summary>
        public UnityEngine.RectTransform Btns;

        /// <summary>
        /// 
        /// </summary>
        public Framework.ButtonEx Btn_Cancel;

        /// <summary>
        /// 
        /// </summary>
        public Framework.TextEx Txt_Cancel;

        /// <summary>
        /// 
        /// </summary>
        public Framework.ButtonEx Btn_Confirm;

        /// <summary>
        /// 
        /// </summary>
        public Framework.TextEx Txt_Confirm;

        public override void OnCreate()
        {
            rectTransform = gameObject.GetComponent<RectTransform>();
            Btn_Mask = rectTransform.Find("Btn_Mask").GetComponent<Framework.ButtonEx>();
			Img_Bg = rectTransform.Find("Img_Bg").GetComponent<Framework.ImageEx>();
			Txt_Content = rectTransform.Find("Txt_Content").GetComponent<Framework.TextEx>();
			Txt_Title = rectTransform.Find("Txt_Content/Txt_Title").GetComponent<Framework.TextEx>();
			Btns = rectTransform.Find("Txt_Content/Btns").GetComponent<UnityEngine.RectTransform>();
			Btn_Cancel = rectTransform.Find("Txt_Content/Btns/Btn_Cancel").GetComponent<Framework.ButtonEx>();
			Txt_Cancel = rectTransform.Find("Txt_Content/Btns/Btn_Cancel/Txt_Cancel").GetComponent<Framework.TextEx>();
			Btn_Confirm = rectTransform.Find("Txt_Content/Btns/Btn_Confirm").GetComponent<Framework.ButtonEx>();
			Txt_Confirm = rectTransform.Find("Txt_Content/Btns/Btn_Confirm/Txt_Confirm").GetComponent<Framework.TextEx>();
			
        }
    }
}
