// =============================================================================
// File      : WrapContent.cs
// Author    : LHW
// Create    : 2017-00-00 00:00
// Copyright : Copyright (c) 2014-2017 fsnmt.com, All rights reserved.   
// 功能说明  :   
// =============================================================================

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WrapContentBase:MonoBehaviour
{
    public delegate void OnInitializeItem(Transform go, int realIndex);

    [SerializeField]
    private int itemWidth = 200;   //水平间距 
    [SerializeField]
    private int itemHeight = 200;  //垂直间距 
    [SerializeField]
    private int row = 3;  //行数
    [SerializeField] 
    private int column = 3;  //列数
    [SerializeField]
    public int minIndex = 0; //最小索引
    [SerializeField]
    public int maxIndex = 0; //最大索引
    [SerializeField]
    private bool cullContent = true; //是否剔除
    [SerializeField]
    private UIScrollBar scrollBar;
    [SerializeField]
    private Vector3 scrollBarPos;
    [SerializeField]
    private float scrollSpring = 10f;

    public OnInitializeItem onInitializeItem;

    protected Transform mTrans; //当前脚本的Transform
    protected UIPanel mPanel;
    protected UIScrollView mScrollView;
    protected SpringPanel mSpring;
    protected bool mHorizontal = false;
    protected bool mFirstTime = true;
    protected List<Transform> mChildren = new List<Transform>();

    private int dataCount; //数据
    private int itemCount = 9; //Item预制件总数
    private int allRows;  //实际的行数
    private List<Transform> closeItems = new List<Transform>();
    private int closeItemCount = 0;
    private bool isFirst = true;

    protected virtual void Awake()
    {
        if (!CacheScrollView()) return;

        if (mScrollView != null)
            mScrollView.GetComponent<UIPanel>().onClipMove = OnMove;

        if (scrollBar != null)
            scrollBar.gameObject.SetActive(false);

        for (int i = 0; i < row * column; i++)
        {
            Transform t = mTrans.GetChild(i);
            mChildren.Add(t);
        }

        mFirstTime = false;
    }

    protected bool CacheScrollView()
    {
        mTrans = transform;
        mPanel = mTrans.GetComponentInParent<UIPanel>();
        mSpring = mPanel.GetComponent<SpringPanel>();
        mScrollView = mPanel.GetComponent<UIScrollView>();

        if (mScrollView != null)
            mScrollView.onStoppedMoving += StopMove;
        
        if (mScrollView == null) return false;

        if (mScrollView.movement == UIScrollView.Movement.Horizontal) mHorizontal = true;
        else if (mScrollView.movement == UIScrollView.Movement.Vertical) mHorizontal = false;
        else return false;

        return true;
    }

    protected virtual void OnMove(UIPanel panel)
    {
        if (scrollBar != null)
            scrollBar.gameObject.SetActive(true);

        WrapContent();

        if (scrollBar != null)
            ScrollBarMove();
    }

    protected void ScrollBarMove()
    {
        scrollBar.transform.localPosition = scrollBarPos;
    }

    protected virtual void StopMove()
    {
        if (scrollBar != null)
            scrollBar.gameObject.SetActive(false);
    }

    protected void InstantiateItems(int data, Action<GameObject, int> setItemAction, Action emptyAction = null)
    {
        dataCount = data;
        itemCount = row * column;
        allRows = Mathf.CeilToInt(dataCount / column);

        if (!isFirst)
            RestListTrans();
        else
            isFirst = false;

        if (dataCount < itemCount)
        {
            for (int i = 0; i < itemCount; i++)
            {
                GameObject t = mTrans.GetChild(i).gameObject;

                if (i < dataCount)
                {
                    if (!t.activeSelf)
                        t.SetActive(true);

                    if (setItemAction != null)
                        setItemAction(t, i);

                    continue;
                }
                else
                {
                    if (t.gameObject.activeSelf)
                        t.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            for (int i = 0, iMax = row * column; i < iMax; i++)
            {
                GameObject t = mTrans.GetChild(i).gameObject;

                if (!t.activeSelf)
                    t.SetActive(true);

                if (setItemAction != null)
                    setItemAction(t, i);
            }

            //不满row*column 即最后一版要显示的items的个数
            int lastPageNum = dataCount % itemCount;
            closeItems.Clear();

            for (int i = 0; i < row; i++)
            {
                if (lastPageNum >= i * column && lastPageNum < (i + 1) * column)
                {
                    for (int j = lastPageNum; j < (i + 1) * column; j++)
                        closeItems.Add(mChildren[j]);

                    closeItemCount = closeItems.Count;
                    break;
                }

            }
        }

        if (dataCount <= 0)
        {
            if (emptyAction != null)
                emptyAction();
        }

        if (scrollBar != null)
        {
            InitScrollBarSize();
            scrollBar.gameObject.SetActive(false);
        }

        mScrollView.ResetPosition();

    }

    private void RestListTrans()
    {
        mSpring.enabled = false;
        mScrollView.dragEffect = UIScrollView.DragEffect.None;
        mPanel.transform.localPosition = Vector3.zero;
        mPanel.clipOffset = Vector2.zero;

        for (int i = 0; i < itemCount; i++)
        {
            Vector3 pos = new Vector3((float)i % column * itemWidth, i / column * -itemHeight, 0);
            mChildren[i].transform.localPosition = pos;
        }

        mScrollView.dragEffect = UIScrollView.DragEffect.MomentumAndSpring;
    }

    private void InitScrollBarSize()
    {
        scrollBar.barSize = mPanel.finalClipRegion.w / (allRows * itemHeight);
    }

    private void WrapContent()
    {
        if (scrollBar != null)
            SetScrollBarPos();

        if (dataCount < itemCount)
        {
            mScrollView.restrictWithinPanel = true;
            mScrollView.InvalidateBounds();
            return;
        }

        float extents = itemHeight * row * 0.5f;
        Vector3[] corners = mPanel.worldCorners;

        for (int i = 0; i < 4; ++i)
        {
            Vector3 v = corners[i];
            v = mTrans.InverseTransformPoint(v);
            corners[i] = v;
        }

        Vector3 center = Vector3.Lerp(corners[0], corners[2], 0.5f);
        bool allWithinRange = true;
        float ext2 = extents * 2f;

        float min = corners[0].y - itemHeight;
        float max = corners[2].y + itemHeight;

        for (int i = 0, imax = mChildren.Count; i < imax; ++i)
        {
            Transform t = mChildren[i];
            float distance = t.localPosition.y - center.y;

            //MoveDown
            if (distance < -extents)
            {
                Vector3 pos = t.localPosition;
                pos.y += ext2;
                distance = pos.y - center.y;
                int realRow = Mathf.RoundToInt(pos.y / itemHeight);

                if (minIndex == maxIndex || (minIndex <= realRow && realRow <= maxIndex))
                {
                    SwitchItems(true);
                    if (realRow > 0)
                    {
                        mScrollView.restrictWithinPanel = true;
                        return;
                    }
                    t.localPosition = pos;
                    SetItemValue(t);
                    UpdateItem(t, -realRow, i);
                }
                else
                    allWithinRange = false;
            }
            else if (distance > extents)//MoveUp
            {
                Vector3 pos = t.localPosition;
                pos.y -= ext2;
                distance = pos.y - center.y;
                int realRow = Mathf.RoundToInt(pos.y / itemHeight);

                if (minIndex == maxIndex || (minIndex <= realRow && realRow <= maxIndex))
                {
                    if (-realRow > allRows)
                    {
                        SwitchItems(false);
                        mScrollView.restrictWithinPanel = true;
                        return;
                    }

                    t.localPosition = pos;
                    SetItemValue(t);
                    UpdateItem(t, -realRow, i);
                }
                else
                    allWithinRange = false;

            }

            if (cullContent)
            {
                distance += mPanel.clipOffset.y - mTrans.localPosition.y;
                if (!UICamera.IsPressed(t.gameObject))
                    NGUITools.SetActive(t.gameObject, (distance > min && distance < max), false);
            }
        }
        mScrollView.restrictWithinPanel = !allWithinRange;
        mScrollView.InvalidateBounds();
    }

    protected virtual void SetItemValue(Transform t) { }

    private void SetScrollBarPos()
    {
        scrollBar.value = (mPanel.transform.localPosition.y - scrollSpring) / ((allRows * itemHeight) - mPanel.finalClipRegion.w);
    }

    private void SwitchItems(bool isActive)
    {
        if (closeItemCount < 1)
            return;

        for (int i = 0; i < closeItemCount; i++)
            closeItems[i].gameObject.SetActive(isActive);
    }

    private void UpdateItem(Transform item, int index, int num)
    {
        if (index < 0)
            return;

        int realIndex = index * column + num % column;

        if (realIndex >= dataCount)
            return;

        if (onInitializeItem != null)
            onInitializeItem(item, realIndex);
    }

    private void OnSort()
    {
        if (mTrans == null)
            mTrans = transform;

        for (int i = 0; i < mTrans.childCount; i++)
        {
            mTrans.GetChild(i).transform.localPosition = new Vector3(0, -i * itemHeight, 0);

            for (int j = 0; j < mTrans.GetChild(i).childCount; j++)
                mTrans.GetChild(i).GetChild(j).transform.localPosition = new Vector3((j - 1) * itemHeight, 0, 0);
        }
    }

    private bool IsWideScreen()
    {
#if UNITY_EDITOR
        float aspectRatio = (float)Screen.width / (float)Screen.height;
#else
        float aspectRatio = (float)Screen.currentResolution.width / (float)Screen.currentResolution.height;
#endif
        if (aspectRatio < 1.7F)
            return false;
        else
            return true;
    }
}
