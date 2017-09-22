// =============================================================================
// File      : SingleWrapContentA.cs
// Author    : 
// Create    : 2017-00-00 00:00
// Copyright : Copyright (c) 2014-2017 fsnmt.com, All rights reserved.   
// 功能说明  :   
// =============================================================================

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SingleWrapContent : WrapContentBase
{
    protected override void Awake()
    {
        base.Awake();
    }

    public void InitializeProduct(/*List<GoodsCommonItemModel> goodList, */Action callback)
    {
        //int data = goodList.Count;
        //InstantiateItems(data,(item,index)=> 
        //{
        //    if (item.GetComponent<ProductItem>().Value)
        //        item.GetComponent<ProductItem>().Value = false;
        //    item.GetComponent<ProductItem>().SetProtuct(goodList[index], index);

        //},callback);
    }

    protected override void SetItemValue(Transform t)
    {
        base.SetItemValue(t);
        //if (t.GetComponent<ProductItem>() != null)
        //{
        //    if (t.GetComponent<ProductItem>().Value)
        //        t.GetComponent<ProductItem>().Value = false;
        //}
    }

}
