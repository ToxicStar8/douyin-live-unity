/*********************************************
 * 
 * 脚本名：UIAddItemTips_TipsUnit.cs
 * 创建时间：2023/03/14 13:57:54
 *********************************************/
using Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameData
{
    public partial class UIAddItemTips_TipsUnit : UnitBase
    {
        public override void OnInit()
        {
            
        }

        public void FnShow(/*TableProp tbProp,*/ int itemCount)
        {
            //Txt_Tips.text = tbProp.Name + "+" + itemCount.ToString();
            Txt_Tips.text = "看配表" + "+" + itemCount.ToString();
        }
    }
}
