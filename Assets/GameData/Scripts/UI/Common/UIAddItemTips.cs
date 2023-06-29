/*********************************************
 * 
 * 脚本名：UIAddItemTips.cs
 * 创建时间：2023/01/30 15:16:49
 *********************************************/
using DG.Tweening;
using Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameData
{
    public partial class UIAddItemTips : GameUIBase
    {
        private class AddItemTipsData
        {
            public int ItemId;
            public int ItemCount;
            public RectTransform Parent;
            //public TableProp TbProp;
        }

        private Queue<AddItemTipsData> _tipsQueue;

        public override void OnInit()
        {
            _tipsQueue = new Queue<AddItemTipsData>();
        }

        public override void OnShow(params object[] args)
        {
            var itemId = (int)args[0];
            var itemCount = (int)args[1];
            var parent = args[2] as RectTransform;
            _tipsQueue.Enqueue(new AddItemTipsData()
            {
                ItemId = itemId,
                ItemCount  = itemCount,
                Parent = parent,
                //TbProp = GetTableCtrl<TablePropCtrl>().GetDataById(itemId),
            });

            ShowTips();
        }

        private void ShowTips()
        {
            if (_tipsQueue.Count == 0)
            {
                HideSelf();
                return;
            }

            var unit = TipsUnitPool.CreateUnit(rectTransform);
            var tipsData = _tipsQueue.Dequeue();
            //显示
            unit.FnShow(/*tipsData.TbProp,*/ tipsData.ItemCount);
            unit.rectTransform.SetParent(tipsData.Parent);
            unit.rectTransform.DOKill();
            unit.rectTransform.localPosition = Vector3.zero;
            unit.rectTransform.DOLocalMoveY(50, 0.3f).onComplete = () =>
            {
                TipsUnitPool.Recycle(unit);
            };
        }

        public override void OnBeforDestroy()
        {
            
        }
    }
}
