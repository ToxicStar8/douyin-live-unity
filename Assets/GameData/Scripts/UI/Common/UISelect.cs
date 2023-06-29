/*********************************************
 * 
 * 脚本名：UISelect.cs
 * 创建时间：2023/01/06 13:57:39
 *********************************************/
using Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameData
{
    public class UISelectData
    {
        public string Title;
        public string Txt_A;
        public string Txt_B;
        public string Txt_C;
        public string Txt_D;
        public Action ACallback;
        public Action BCallback;
        public Action CCallback;
        public Action DCallback;
    }

    public partial class UISelect : GameUIBase
    {
        private UISelectData _data;

        public override void OnInit()
        {
            Btn_Mask.AddListener(OnClick_Btn_Mask);
            Btn_A.AddListener(OnClick_Btn_A);
            Btn_B.AddListener(OnClick_Btn_B);
            Btn_C.AddListener(OnClick_Btn_C);
            Btn_D.AddListener(OnClick_Btn_D);
        }

        public override void OnShow(params object[] args)
        {
            _data = args[0] as UISelectData;
            Btn_A.SetActive(!string.IsNullOrWhiteSpace(_data.Txt_A));
            Btn_B.SetActive(!string.IsNullOrWhiteSpace(_data.Txt_B));
            Btn_C.SetActive(!string.IsNullOrWhiteSpace(_data.Txt_C));
            Btn_D.SetActive(!string.IsNullOrWhiteSpace(_data.Txt_D));
            Txt_Title.text = _data.Title;
            Txt_A.text = _data.Txt_A;
            Txt_B.text = _data.Txt_B;
            Txt_C.text = _data.Txt_C;
            Txt_D.text = _data.Txt_D;
        }

        private void OnClick_Btn_Mask()
        {
            CloseSelf();
        }

        private void OnClick_Btn_A()
        {
            _data.ACallback?.Invoke();
            CloseSelf();
        }

        private void OnClick_Btn_B()
        {
            _data.BCallback?.Invoke();
            CloseSelf();
        }

        private void OnClick_Btn_C()
        {
            _data.CCallback?.Invoke();
            CloseSelf();
        }

        private void OnClick_Btn_D()
        {
            _data.DCallback?.Invoke();
            CloseSelf();
        }

        public override void OnBeforDestroy()
        {
            
        }
    }
}
