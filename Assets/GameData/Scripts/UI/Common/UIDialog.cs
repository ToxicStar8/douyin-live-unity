/*********************************************
 * 
 * 脚本名：UIDialog.cs
 * 创建时间：2023/01/06 13:57:39
 *********************************************/
using Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameData
{
    public class UIDialogData
    {
        public string Title;
        public string Content;
        public Action ConfirmCallback;
        public Action CancelCallback;
        public string ConfirmStr = "确定";
        public string CancelStr = "取消";
        public bool IsShowConfirm = true;
        public bool IsShowCancel = true;
    }

    public partial class UIDialog : GameUIBase
    {
        private UIDialogData _uiDialogData;

        public override void OnInit()
        {
            Btn_Mask.AddListener(CloseSelf);
            Btn_Confirm.AddListener(OnClick_Btn_Confirm);
            Btn_Cancel.AddListener(OnClick_Btn_Cancel);
        }

        public override void OnShow(params object[] args)
        {
            _uiDialogData = args[0] as UIDialogData;
            Txt_Title.text = _uiDialogData.Title;
            Txt_Content.text = _uiDialogData.Content;
            Txt_Confirm.text = _uiDialogData.ConfirmStr;
            Txt_Cancel.text = _uiDialogData.CancelStr;
            Btn_Confirm.gameObject.SetActive(_uiDialogData.IsShowConfirm);
            Btn_Cancel.gameObject.SetActive(_uiDialogData.IsShowCancel);
            var sizeFitter = Txt_Content.GetComponent<UnityEngine.UI.ContentSizeFitter>();
            //强制刷新高度
            sizeFitter.SetLayoutVertical();     
            //更新背景高度
            Img_Bg.rectTransform.sizeDelta = new Vector2(670, Txt_Content.rectTransform.sizeDelta.y + 250);
        }

        private void OnClick_Btn_Confirm()
        {
            _uiDialogData.ConfirmCallback?.Invoke();
            CloseSelf();
        }

        private void OnClick_Btn_Cancel()
        {
            _uiDialogData.CancelCallback?.Invoke();
            CloseSelf();
        }

        public override void OnBeforDestroy()
        {
            
        }
    }
}
