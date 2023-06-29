/*********************************************
 * 
 * 脚本名：UITips_TipsUnit.cs
 * 创建时间：2023/03/14 13:57:39
 *********************************************/
using Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameData
{
    public partial class UITips_TipsUnit : UnitBase
    {
        public override void OnInit()
        {
            
        }

        public void FnShow(/*TableProp tbProp,*/ string tips)
        {
            //Img_Item.SetActive(itemId != 0);
            Img_Item.SetActive(false);
            Txt_Tips.text = tips;
        }
    }
}
