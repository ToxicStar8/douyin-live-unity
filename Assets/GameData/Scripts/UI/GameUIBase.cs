/*********************************************
 * BFramework
 * UI通用方法存放基类
 * 创建时间：2023/01/08 20:40:23
 *********************************************/
using Framework;
using GameData;
using MainPackage;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameData
{
    /// <summary>
    /// UI通用方法存放基类
    /// </summary>
    public abstract class GameUIBase : UIBase
    {
        #region Event
        public void AddEventListener(UIEvent eventNo, Action<object[]> callback)
        {
            AddEventListener((ushort)eventNo, callback);
        }
        public void SendEven(UIEvent eventNo, params object[] args)
        {
            SendEven((ushort)eventNo, args);
        }
        #endregion

        #region UI操作
        /// <summary>
        /// 关闭自己
        /// </summary>
        public void CloseSelf()
        {
            CloseUI(uiName);
        }

        /// <summary>
        /// 隐藏自己
        /// </summary>
        public void HideSelf()
        {
            HideUI(uiName);
        }
        #endregion

        /// <summary>
        /// 显示新增道具
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="itemCount"></param>
        /// <param name="parent"></param>
        public void ShowAddItemTips(int itemId, int itemCount,RectTransform parent)
        {
            GameGod.Instance.UIManager.OpenUI<UIAddItemTips>(E_UILevel.Tips, itemId, itemCount , parent);
        }

        /// <summary>
        /// 显示确认框
        /// </summary>
        /// <param name="tips"></param>
        public void ShowDialog(UIDialogData dialogData)
        {
            GameGod.Instance.UIManager.OpenUI<UIDialog>(E_UILevel.Pop, dialogData);
        }

        /// <summary>
        /// 显示提示
        /// </summary>
        /// <param name="tips"></param>
        public void ShowTips(string tips,int itemId = 0)
        {
            GameGod.Instance.UIManager.OpenUI<UITips>(E_UILevel.Tips, tips, itemId);
        }
    }
}