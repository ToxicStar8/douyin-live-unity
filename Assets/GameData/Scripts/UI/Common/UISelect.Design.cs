/*********************************************
 * 自动生成代码，禁止手动修改文件
 * 脚本名：UISelect.Design.cs
 * 修改时间：2023/03/14 11:13:29
 *********************************************/

using Framework;
using UnityEngine;
using UnityEngine.UI;

namespace GameData
{
    public partial class UISelect
    {
        /// <summary>
        /// 
        /// </summary>
        public Framework.ButtonEx Btn_Mask;

        /// <summary>
        /// 
        /// </summary>
        public Framework.ImageEx Bg;

        /// <summary>
        /// 
        /// </summary>
        public Framework.TextEx Txt_Title;

        /// <summary>
        /// 
        /// </summary>
        public Framework.ButtonEx Btn_A;

        /// <summary>
        /// 
        /// </summary>
        public Framework.TextEx Txt_A;

        /// <summary>
        /// 
        /// </summary>
        public Framework.ButtonEx Btn_B;

        /// <summary>
        /// 
        /// </summary>
        public Framework.TextEx Txt_B;

        /// <summary>
        /// 
        /// </summary>
        public Framework.ButtonEx Btn_C;

        /// <summary>
        /// 
        /// </summary>
        public Framework.TextEx Txt_C;

        /// <summary>
        /// 
        /// </summary>
        public Framework.ButtonEx Btn_D;

        /// <summary>
        /// 
        /// </summary>
        public Framework.TextEx Txt_D;

        public override void OnCreate()
        {
            rectTransform = gameObject.GetComponent<RectTransform>();
            Btn_Mask = rectTransform.Find("Btn_Mask").GetComponent<Framework.ButtonEx>();
			Bg = rectTransform.Find("Bg").GetComponent<Framework.ImageEx>();
			Txt_Title = rectTransform.Find("Txt_Title").GetComponent<Framework.TextEx>();
			Btn_A = rectTransform.Find("Rt_Group/Btn_A").GetComponent<Framework.ButtonEx>();
			Txt_A = rectTransform.Find("Rt_Group/Btn_A/Txt_A").GetComponent<Framework.TextEx>();
			Btn_B = rectTransform.Find("Rt_Group/Btn_B").GetComponent<Framework.ButtonEx>();
			Txt_B = rectTransform.Find("Rt_Group/Btn_B/Txt_B").GetComponent<Framework.TextEx>();
			Btn_C = rectTransform.Find("Rt_Group/Btn_C").GetComponent<Framework.ButtonEx>();
			Txt_C = rectTransform.Find("Rt_Group/Btn_C/Txt_C").GetComponent<Framework.TextEx>();
			Btn_D = rectTransform.Find("Rt_Group/Btn_D").GetComponent<Framework.ButtonEx>();
			Txt_D = rectTransform.Find("Rt_Group/Btn_D/Txt_D").GetComponent<Framework.TextEx>();
			
        }
    }
}
