/*********************************************
 * BFramework
 * 循环列表
 * 创建时间：2023/01/08 20:40:23
 *********************************************/
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Linq;
using Framework;

namespace UnityEngine.UI
{
    public class LoopScrollRect : ScrollRect
    {
        public Camera UICamera;

        public System.Action<PointerEventData> OnBeginDragCallback;
        public System.Action<PointerEventData> OnDragCallback;
        public System.Action<PointerEventData> OnEndDragCallback;
        /// <summary>
        /// 是否在拖动过程
        /// </summary>
        private bool isDraging = false;

        /// <summary>
        /// UI对象池
        /// </summary>
        public readonly Queue<RectTransform> mItemRtPool = new Queue<RectTransform>();

        /// <summary>
        /// 上下边界扩展范围
        /// </summary>
        private float BoundSize = 10;

        /// <summary>
        /// content尺寸
        /// </summary>
        private float contentMaxSize = 1000000;

        /// <summary>
        ///  初始化设置的滚动项目(对象，对应显示数据索引)
        /// </summary>
        private readonly List<KeyValuePair<RectTransform, int>> Items = new List<KeyValuePair<RectTransform, int>>();

        /// <summary>
        /// 数量
        /// </summary>
        public int Count => Items.Count;

        /// <summary>
        /// 获取滚动项
        /// </summary>
        /// <param name="itemPos">选项位置</param>
        /// <returns></returns>
        public RectTransform GetItem(int itemPos)
        {
            return Items[itemPos].Key;
        }

        /// <summary>
        /// 获取滚动项目数据索引
        /// </summary>
        public int GetItemDataIndex(int itemPos)
        {
            return Items[itemPos].Value;
        }

        /// <summary>
        /// 显示时回调(对象，对应显示数据索引)
        /// </summary>
        private Action<RectTransform, int> mShowCallback;
        private Func<RectTransform> mCreateItemFunc;
        private Tweener mContentTweener;

        /// <summary>
        /// 要滚动的数据总数据
        /// </summary>
        private int mDataCount;

        /// <summary>
        /// 显示区域大小
        /// </summary>
        private Vector2 viewSize => new Vector2(viewport.rect.width, viewport.rect.height);

        protected override void Awake()
        {
            base.Awake();
            onValueChanged.AddListener(FnOnScrollValueChanged);
        }

        // 从上到下遍历一边对象
        public void ForEach(Action<RectTransform,int> call)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                var item = Items[i];
                call(item.Key, item.Value);
            }
        }

        // 数据不足补充
        public void ItemSupplement()
        {
            FnOnScrollValueChanged();
        }

        private void FnOnScrollValueChanged(Vector2 delta = default(Vector2))
        {
            // 先处理移动
            switch (movementType)
            {
                case MovementType.Elastic:
                    if ((mContentTweener == null) && (!isDraging))
                    {
                        MovementClampedHandle();
                    }
                    break;
                case MovementType.Clamped:
                    MovementClampedHandle();
                    break;
                default:
                    break;
            }

            // 顶部回收/创建
            TopHandle();
            // 底部回收/创建
            BottonHandle();
        }

        /// <summary>
        /// 初始化滚动项
        /// </summary>
        /// <param name="dataCount">数据总数</param>
        /// <param name="createItemFunc">创建UI回调</param>
        /// <param name="showCallback">显示UI回调</param>
        public void Init(int dataCount, 
            Func<RectTransform> createItemFunc, 
            Action<RectTransform, int> showCallback, 
            bool isShowAtTop = true,
            int startIndex = 0)             // 第一个显示的位置
        {
            content.SetSizeDeltaY(contentMaxSize);           // 设置一个非常大的空间，足够一次拓展滚动区域
            content.SetAnchorPosY(contentMaxSize / 2);

            mDataCount = dataCount;
            mShowCallback = showCallback;
            mCreateItemFunc = createItemFunc;

            if (startIndex > 0)
            {
                ShowAtPosition(startIndex, viewport.position);
            }
            else
            {
                StopMovement();
                if (isShowAtTop)
                {
                    ShowAtTop();
                }
                else
                {
                    ShowAtBotton();
                }
            }
        }

        // 从第几个索引位置开始显示起（index从0开始）
        // firstAnchorPos 第一个的位置坐标
        public void ShowAtPosition(int startIndex,Vector3 firstPosition)
        {
            StopMovement();
            if (startIndex >= mDataCount)
            {
                startIndex = 0;
                Debug.Log("总数量 = " + mDataCount.ToString() + "小于等于开始索引位置 = " + startIndex.ToString() + "强制设置成0");
            }

            RecycleAll();
            float size = 0;
            for (int i = startIndex; i < mDataCount; i++)
            {
                int index = i;
                var rt = CreateItem(true);
                Items.Add(new KeyValuePair<RectTransform, int>(rt, index));
                mShowCallback(rt, index);
                size += rt.sizeDelta.y;
                TopLayout(firstPosition);
                // 显示的数量
                if (size > viewSize.y)
                {
                    break;
                }
            }
            FnOnScrollValueChanged();
        }

        // 根据第一个对象位置重新布局(数量不变),因为是按照第一个在上的原则，所以必然是顶部布局
        public void ResetAccordingToFirstItem()
        {
            StopMovement();
            if(mDataCount == 0)
            {
                return;
            }
            var first = Items[0];
            var firstPosition = first.Key.position;
            var firstIndex = first.Value;

            RecycleAll();
            float size = 0;
            for (int i = firstIndex; i < mDataCount; i++)
            {
                int index = i;
                var rt = CreateItem(true);
                Items.Add(new KeyValuePair<RectTransform, int>(rt, index));
                mShowCallback(rt, index);
                size += rt.sizeDelta.y;
                TopLayout(firstPosition);
                // 显示的数量
                if (size > viewSize.y)
                {
                    break;
                }
            }
            FnOnScrollValueChanged();
        }

        // 插入对象，滚动方只需要重新刷新一边数据即可
        public void InsertItem(int count)
        {
            mDataCount = Mathf.Max(mDataCount + count, 0);
            ResetAccordingToFirstItem();
        }

        /// <summary>
        /// 添加底部
        /// </summary>
        /// <param name="count"></param>
        /// <param name="recovery">是否从底部开始显示</param>
        public void AddAtBottom(int count, bool recovery = true, bool isShowAtButtom = true)
        {
            mDataCount += count;
            if (recovery)
            {
                if (isShowAtButtom)
                {
                    ShowAtBotton();
                }
                else
                {
                    ShowAtTop();
                }
            }
            FnOnScrollValueChanged();
            StopMovement();
        }

        /// <summary>
        /// 添加顶部
        /// </summary>
        /// <param name="count"></param>
        public void AddAtTop(int count)
        {
            mDataCount += count;

            // 给每个索引添加count
            List<KeyValuePair<RectTransform, int>> list = new List<KeyValuePair<RectTransform, int>>();
            foreach (var item in Items)
            {
                //item.Key.AddAnchorPosY(-deltaY); 
                list.Add(new KeyValuePair<RectTransform, int>(item.Key, item.Value + count));
            }
            Items.Clear();
            foreach (var item in list)
            {
                Items.Add(item);
            }
            FnOnScrollValueChanged(Vector2.zero);
            mContentTweener = null;
            content.DOKill();
            StopMovement();
        }

        /// <summary>
        /// 回收所有
        /// </summary>
        private void RecycleAll()
        {
            for (int i = 0; i < Items.Count; i++)
            {
                var rt = Items[i].Key;
                rt.SetActive(false);
                mItemRtPool.Enqueue(rt);
            }

            Items.Clear();
        }

        /// <summary>
        /// 显示在顶部
        /// </summary>
        public void ShowAtTop()
        {
            RecycleAll();
            float size = 0;
            for (int i = 0; i < mDataCount; i++)
            {
                int index = i;
                var rt = CreateItem(true);
                Items.Add(new KeyValuePair<RectTransform, int>(rt, index));
                mShowCallback(rt, index);
                size += rt.sizeDelta.y;
                TopLayout(viewport.position);
                // 显示的数量
                if (size > viewSize.y)
                {
                    break;
                }
            }
            FnOnScrollValueChanged();
        }

        // 在顶部布局
        private void TopLayout(Vector3 firstPositon)
        {
            // 创建的话，永远都是布局最后一个，如果只有一个，就放默认位置 
            if (Items.Count > 1)
            {
                RectTransform rt1 = Items[Items.Count - 2].Key;
                RectTransform rt2 = Items[Items.Count - 1].Key;
                rt2.position = rt1.position;
                rt2.AddAnchorPosY(-rt1.sizeDelta.y);
            }
            else if (Items.Count == 1)
            {
                RectTransform rt1 = Items[0].Key;
                rt1.position = firstPositon;
            }
        }

        /// <summary>
        /// 显示在底部
        /// </summary>
        Vector3[] corners = new Vector3[4];
        public void ShowAtBotton()
        {
            RecycleAll();
            float size = 0;
            for (int i = mDataCount - 1; i >= 0; i--)
            {
                int index = i;
                var rt = CreateItem(false);
                Items.Insert(0, new KeyValuePair<RectTransform, int>(rt, index));
                mShowCallback(rt, index);
                size += rt.sizeDelta.y;
                ButtonLayout();
                // 显示的数量
                if (size > viewSize.y)
                {
                    break;
                }
            }

            // 因为如果发生不够的情况下，通过该函数调整
            FnOnScrollValueChanged();
        }

        // 在底部部布局
        void ButtonLayout()
        {
            // 创建的话，永远都是布局最后一个，如果只有一个，就放默认位置 
            if (Items.Count > 1)
            {
                RectTransform rt1 = Items[1].Key;
                RectTransform rt2 = Items[0].Key;
                rt2.position = rt1.position;
                rt2.AddAnchorPosY(rt2.sizeDelta.y);
            }
            else if (Items.Count == 1)
            {
                RectTransform rt = Items[0].Key;
                rt.position = viewport.position;
                rt.AddAnchorPosY(-viewSize.y);
                rt.AddAnchorPosY(rt.sizeDelta.y);
            }
        }

        /// <summary>
        /// 顶部回收/创建
        /// </summary>
        private void TopHandle()
        {
            // 顶部回收判断
            bool hasRecycle = false;            // 是否有被回收
            while (true)
            {
                if (Items.Count <= 1) break;            // 必须保留一个

                float yViewSize = viewSize.y;
                // 判断顶部回收
                var item = Items[0];
                // 如果顶部是第一个，并且把所有的滚动项全部显示出来的情况下，不回收第一个
                if ((item.Value == 0) && (Items.Count == mDataCount))
                {
                    return;
                }

                var screenPos = RectTransformUtility.WorldToScreenPoint(UICamera, item.Key.position);
                RectTransformUtility.ScreenPointToLocalPointInRectangle(viewport, screenPos, UICamera, out Vector2 viewLocalPos);

                float bottonPos = viewLocalPos.y - item.Key.sizeDelta.y;
                if (bottonPos > BoundSize)
                {
                    Items.RemoveAt(0);
                    item.Key.SetActive(false);
                    mItemRtPool.Enqueue(item.Key);
                    hasRecycle = true;
                }
                else
                {
                    break;
                }
            }

            // 如果有被回收就不需要再创建
            if (hasRecycle) return;

            // 顶部创建
            while (true)
            {
                if (Items.Count == 0)
                {
                    if (mDataCount > 0)
                    {
                        int index = 0;          // 创建第0位置，放在view位置
                        var rt = CreateItem(true);
                        Items.Insert(0, new KeyValuePair<RectTransform, int>(rt, index));
                        mShowCallback(rt, index);
                        rt.position = viewport.position;
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }

                float yViewSize = viewSize.y;
                var item = Items[0];
                var screenPos = RectTransformUtility.WorldToScreenPoint(UICamera, item.Key.position);
                RectTransformUtility.ScreenPointToLocalPointInRectangle(viewport, screenPos, UICamera, out Vector2 viewLocalPos);

                if (viewLocalPos.y < BoundSize)
                {
                    int index = item.Value - 1;
                    if (index >= 0)
                    {
                        var rt = CreateItem(true);
                        Items.Insert(0, new KeyValuePair<RectTransform, int>(rt, index));
                        mShowCallback(rt, index);
                        rt.position = item.Key.position;
                        rt.AddAnchorPosY(rt.sizeDelta.y);
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
        }

        /// <summary>
        /// 底部回收/创建
        /// </summary>
        private void BottonHandle()
        {
            // 底部回收判断
            bool hasRecycle = false;            // 是否有被回收
            while (true)
            {
                if (Items.Count <= 1) break;            // 必须保留一个

                float yViewSize = viewSize.y;
                // 判断底部回收
                var item = Items.Last();
                var screenPos = RectTransformUtility.WorldToScreenPoint(UICamera, item.Key.position);
                RectTransformUtility.ScreenPointToLocalPointInRectangle(viewport, screenPos, UICamera, out Vector2 viewLocalPos);

                float recyclePos = -yViewSize - BoundSize;
                if (viewLocalPos.y < recyclePos)
                {
                    Items.Remove(item);
                    item.Key.SetActive(false);
                    mItemRtPool.Enqueue(item.Key);
                    hasRecycle = true;
                }
                else
                {
                    break;
                }
            }

            // 如果有被回收就不需要再创建
            if (hasRecycle) return;

            // 底部创建
            while (true)
            {
                if (Items.Count == 0) break;

                float yViewSize = viewSize.y;
                var item = Items.Last();
                var screenPos = RectTransformUtility.WorldToScreenPoint(UICamera, item.Key.position);
                RectTransformUtility.ScreenPointToLocalPointInRectangle(viewport, screenPos, UICamera, out Vector2 viewLocalPos);

                float createPos = -yViewSize - BoundSize;
                if (viewLocalPos.y - item.Key.sizeDelta.y > createPos)
                {
                    int index = item.Value + 1;
                    if (mDataCount > index)
                    {
                        var rt = CreateItem(false);
                        Items.Add(new KeyValuePair<RectTransform, int>(rt, index));
                        mShowCallback(rt, index);
                        rt.position = item.Key.position;
                        //rt.AddAnchorPosY(-Items.Last().Key.sizeDelta.y);
                        rt.AddAnchorPosY(-item.Key.sizeDelta.y);
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
        }

        /// <summary>
        /// 创建对象
        /// </summary>
        /// <returns></returns>
        private RectTransform CreateItem(bool isTop)
        {
            RectTransform rt;
            if (mItemRtPool.Count > 0)
            {
                rt = mItemRtPool.Dequeue();
                rt.SetActive(true);
            }
            else
            {
                rt = mCreateItemFunc();
            }

            if (isTop)
            {
                rt.SetAsFirstSibling();
            }
            else
            {
                rt.SetAsLastSibling();
            }
            return rt;
        }

        /// <summary>
        /// 显示内容是否能塞满整个显示区域
        /// </summary>
        /// <returns></returns>
        private bool IsFullView()
        {
            // 只有满屏显示的时候才处理
            float size = 0;
            for (int i = 0; i < Items.Count; i++)
            {
                size += Items[i].Key.sizeDelta.y;
            }
            return size >= viewSize.y;
        }

        private void MovementClampedHandle()
        {
            // 顶部夹紧
            if (Items.Count == 0) return;

            var tmpItem = Items[0];
            if (tmpItem.Value == 0)
            {
                var screenPos = RectTransformUtility.WorldToScreenPoint(UICamera, tmpItem.Key.position);
                RectTransformUtility.ScreenPointToLocalPointInRectangle(viewport, screenPos, UICamera, out Vector2 viewLocalPos);
                // 如果没有塞满全屏，只做第一个判断
                if ((viewLocalPos.y < 0) || !IsFullView())
                {
                    foreach (var item in Items)
                    {
                        item.Key.AddAnchorPosY(-viewLocalPos.y);
                    }
                    content.DOKill();
                    StopMovement();
                    return;
                }
            }

            // 底部夹紧
            tmpItem = Items.Last();
            if (tmpItem.Value == mDataCount - 1)
            {
                var screenPos = RectTransformUtility.WorldToScreenPoint(UICamera, tmpItem.Key.position);
                RectTransformUtility.ScreenPointToLocalPointInRectangle(viewport, screenPos, UICamera, out Vector2 viewLocalPos);
                float deltaY = viewLocalPos.y - tmpItem.Key.sizeDelta.y + viewSize.y;
                if (deltaY > 0)
                {
                    foreach (var item in Items)
                    {
                        item.Key.AddAnchorPosY(-deltaY);
                    }
                    content.DOKill();
                    StopMovement();
                    return;
                }
            }
        }

        private void MovementElasticHandleEnd()
        {
            if (movementType == MovementType.Elastic)
            {
                // 顶部夹紧
                if (Items.Count == 0) return;

                var tmpItem = Items[0];
                if (tmpItem.Value == 0)
                {
                    var screenPos = RectTransformUtility.WorldToScreenPoint(UICamera, tmpItem.Key.position);
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(viewport, screenPos, UICamera, out Vector2 viewLocalPos);

                    if ((viewLocalPos.y < 0) || (!IsFullView()))
                    {
                        mContentTweener = content.DOAnchorPosY(content.anchoredPosition.y - viewLocalPos.y, 0.3f).OnComplete(() => mContentTweener = null);
                        StopMovement();
                        return;
                    }
                }

                // 底部夹紧
                tmpItem = Items.Last();
                if (tmpItem.Value == mDataCount - 1)
                {
                    var screenPos = RectTransformUtility.WorldToScreenPoint(UICamera, tmpItem.Key.position);
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(viewport, screenPos, UICamera, out Vector2 viewLocalPos);
                    float deltaY = viewLocalPos.y - tmpItem.Key.sizeDelta.y + viewSize.y;
                    if (deltaY > 0)
                    {
                        mContentTweener = content.DOAnchorPosY(content.anchoredPosition.y - deltaY, 0.3f).OnComplete(() => mContentTweener = null);
                        StopMovement();
                        return;
                    }
                }
            }
        }

        private bool MovementElasticHandle()
        {
            if (movementType == MovementType.Elastic)
            {
                if (Items.Count == 0) return false;

                var tmpItem = Items[0];
                if (tmpItem.Value == 0)
                {
                    var screenPos = RectTransformUtility.WorldToScreenPoint(UICamera, tmpItem.Key.position);
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(viewport, screenPos, UICamera, out Vector2 viewLocalPos);

                    if (viewLocalPos.y < 0)
                    {
                        return true;
                    }
                }

                tmpItem = Items.Last();
                if (tmpItem.Value == mDataCount - 1)
                {
                    var screenPos = RectTransformUtility.WorldToScreenPoint(UICamera, tmpItem.Key.position);
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(viewport, screenPos, UICamera, out Vector2 viewLocalPos);
                    float deltaY = viewLocalPos.y - tmpItem.Key.sizeDelta.y + viewSize.y;
                    if (deltaY > 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 是否最后一个显示在底部
        /// </summary>
        /// <returns></returns>
        public bool IsLastShowAtButton()
        {
            if (Items.Count == 0) return false;

            var tmpItem = Items.Last();
            if (tmpItem.Value == mDataCount - 1)
            {
                var screenPos = RectTransformUtility.WorldToScreenPoint(UICamera, tmpItem.Key.position);
                RectTransformUtility.ScreenPointToLocalPointInRectangle(viewport, screenPos, UICamera, out Vector2 viewLocalPos);
                float deltaY = viewLocalPos.y - tmpItem.Key.sizeDelta.y + viewSize.y + BoundSize;
                if (deltaY >= 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 是否第一个完全显示在顶部
        /// </summary>
        /// <returns></returns>
        public bool IsFirstShowAtTop()
        {
            if (Items.Count == 0) return true;

            var tmpItem = Items[0];
            if (tmpItem.Value == 0)
            {
                var screenPos = RectTransformUtility.WorldToScreenPoint(UICamera, tmpItem.Key.position);
                RectTransformUtility.ScreenPointToLocalPointInRectangle(viewport, screenPos, UICamera, out Vector2 viewLocalPos);

                if (viewLocalPos.y <= 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 获取第一个对象在View上的局部坐标
        /// </summary>
        /// <returns></returns>
        public Vector2 GetFirstItemPosToView()
        {
            if (Count == 0) return Vector2.zero;

            var screenPos = RectTransformUtility.WorldToScreenPoint(UICamera, Items[0].Key.position);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(viewport, screenPos, UICamera, out Vector2 viewLocalPos);

            return viewLocalPos;
        }

        public override void OnDrag(PointerEventData eventData)
        {
            isDraging = true;

            if (MovementElasticHandle())
            {
                content.AddAnchorPosY(eventData.delta.y * elasticity);
                return;
            }
            base.OnDrag(eventData);
            if (movementType == MovementType.Clamped)
            {
                MovementClampedHandle();
            }
            OnDragCallback?.Invoke(eventData);
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);
            content.DOKill();
            mContentTweener = null;
            OnDragCallback?.Invoke(eventData);
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            isDraging = false;

            base.OnEndDrag(eventData);
            MovementElasticHandleEnd();
            OnEndDragCallback?.Invoke(eventData);
        }


#if UNITY_EDITOR
        [UnityEditor.MenuItem("CONTEXT/ScrollRect/替换LoopScrollRect")]
        private static void CONTEXT_ScrollRect_Instead()
        {
            GameObject selectGo = UnityEditor.Selection.activeObject as GameObject;
            ScrollRect sr = selectGo.GetComponent<ScrollRect>();
            if ((sr != null) && (sr.GetType() != typeof(LoopScrollRect)))
            {
                var content = sr.content;
                var horizontal = sr.horizontal;
                var movementType = sr.movementType;
                var inertia = sr.inertia;
                var decelerationRate = sr.decelerationRate;
                var scrollSensitivity = sr.scrollSensitivity;
                var viewport = sr.viewport;
                var horizontalScrollbar = sr.horizontalScrollbar;
                var verticalScrollbar = sr.verticalScrollbar;
                GameObject.DestroyImmediate(sr);
                LoopScrollRect xr = selectGo.AddComponent<LoopScrollRect>();
                xr.content = content;
                xr.horizontal = horizontal;
                xr.movementType = movementType;
                xr.inertia = inertia;
                xr.decelerationRate = decelerationRate;
                xr.scrollSensitivity = scrollSensitivity;
                xr.viewport = viewport;
                xr.horizontalScrollbar = horizontalScrollbar;
                xr.verticalScrollbar = verticalScrollbar;
            }
        }
#endif

    }
}