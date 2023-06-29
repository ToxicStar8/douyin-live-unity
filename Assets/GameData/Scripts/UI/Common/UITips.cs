/*********************************************
 * 
 * 脚本名：UITips.cs
 * 创建时间：2023/01/06 13:57:39
 *********************************************/
using DG.Tweening;
using Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameData
{
    public partial class UITips : GameUIBase
    {
        private class TipsData
        {
            public string Tips;
            public int ItemId;
            //public TableProp TbProp;
        }

        private Queue<TipsData> _tipsQueue;
        private bool _isShowing = false;

        public override void OnInit()
        {
            _tipsQueue = new Queue<TipsData>();
        }

        public override void OnShow(params object[] args)
        {
            var tips = args[0] as string;
            var itemId = (int)args[1];
            _tipsQueue.Enqueue(new TipsData()
            {
                Tips = tips,
                ItemId = itemId,
                //TbProp = itemId == 0 ? null : GetTableCtrl<TablePropCtrl>().GetDataById(itemId),
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

            if (_isShowing)
            {
                return;
            }
            _isShowing = true;

            var unit = TipsUnitPool.CreateUnit(rectTransform);
            var tipsData = _tipsQueue.Dequeue();
            //字符串拼接
            //if (tipsData.TbProp != null)
            //{
            //    tipsData.Tips = tipsData.TbProp.Name + tipsData.Tips;
            //}
            //显示
            unit.FnShow(/*tipsData.TbProp,*/ tipsData.Tips);
            unit.rectTransform.DOKill();
            unit.rectTransform.localPosition = Vector3.zero;
            unit.rectTransform.DOLocalMoveY(200, 0.3f).onComplete = () =>
            {
                _isShowing = false;
                TipsUnitPool.Recycle(unit);
                ShowTips();
            };
        }

        public override void OnBeforDestroy() { }
    }
}
