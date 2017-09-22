// =============================================================================
// File      : MultipleWrapContent.cs
// Author    : 
// Create    : 2017-00-00 00:00
// Copyright : Copyright (c) 2014-2017 fsnmt.com, All rights reserved.   
// 功能说明  :   
// =============================================================================

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


    public class MultipleWrapContent : WrapContentBase
    {
        [SerializeField]
        private GameObject backToTop;
        [SerializeField]
        private UnitType dataType;

        private Vector3 originalPos;
        private bool isRefreshing;

        protected override void Awake()
        {
            base.Awake();
            if (backToTop != null)
            {
                UIEventListener.Get(backToTop).onClick = this.OnBackToTop;
                backToTop.SetActive(false);
            }
        }

        protected override void OnMove(UIPanel panel)
        {
            base.OnMove(panel);

            if (dataType == UnitType.Scene)
            {
                //判断滑动停止前赋图
                float interval = (originalPos.y - panel.cachedTransform.localPosition.y) > 0 ? (originalPos.y - panel.cachedTransform.localPosition.y) : (-(originalPos.y - panel.cachedTransform.localPosition.y));

                if (interval < 10)
                {
                    if (!isRefreshing)
                    {
                        //WindowHome.Instance.ShowScenesTexture();
                        isRefreshing = true;
                    }
                }
                else
                {
                    if (isRefreshing)
                        isRefreshing = false;
                }
                originalPos = panel.cachedTransform.localPosition;
            }

            if (backToTop != null)
                SwitchBackToTopBtn();
        }

        private void SwitchBackToTopBtn()
        {
            if (mPanel.transform.localPosition.y < 540)
                backToTop.SetActive(false);
            else if (mPanel.transform.localPosition.y > 1080)
                backToTop.SetActive(true);
        }

        private void OnBackToTop(GameObject go)
        {
            //switch (dataType)
            //{
            //    case UnitType.Scene:
            //        InitializeScenes();
            //        break;
            //    case UnitType.Product:
            //        InitializeProducts(WindowProductCenter.Instance.CurGoodList, () => PopupsManager.ShowToastShortTime("没有找到符合该条件的产品，请输入其它的字符！"));
            //        break;
            //    case UnitType.Panorama:
            //        InitializePanoramas(WindowPanoramas.Instance.CurPanoramaList);
            //        break;
            //}
        }

        protected override void StopMove()
        {
            base.StopMove();
            if (isRefreshing)
                isRefreshing = false;
        }

        //public void InitializeScenes()
        //{
        //    if (WindowHome.Instance.CurrentSceneCategory == SceneCategory.Universal)
        //        InitializeRooms(WindowHome.Instance.CurRooms);
        //    else if (WindowHome.Instance.CurrentSceneCategory == SceneCategory.HouseType)
        //        InitializeHouseType(WindowHome.Instance.CurHouses);
        //    else
        //        Debug.Log("楼盘");

        //}

        //public void InitializeRooms(List<RoomSceneItemModel> rooms)
        //{
        //    int data = rooms.Count;
        //    InstantiateItems(data, (item, index) =>
        //    {
        //        item.GetComponent<SceneItem>().InitRoomScene(rooms[index], true);

        //        if (item.GetComponent<SceneItem>().Value)
        //            item.GetComponent<SceneItem>().Value = false;
        //    }, null);
        //}

        //public void InitializeHouseType(List<HouseSceneItemModel> houses)
        //{
        //    int data = houses.Count;
        //    InstantiateItems(data, (item, index) =>
        //    {

        //        item.GetComponent<SceneItem>().InitHouseTypeScene(houses[index], true);

        //        if (item.GetComponent<SceneItem>().Value)
        //            item.GetComponent<SceneItem>().Value = false;
        //    }, null);
        //}

        //public void InitializeProducts(List<GoodsCommonItemModel> products, Action callback = null)
        //{
        //    int data = products.Count;
        //    InstantiateItems(data, (item, index) => item.GetComponent<ProductPrefab>().Produt = products[index], callback);
        //}

        //public void InitializePanoramas(List<PanoramaItemModel> panoramas, Action callback = null)
        //{
        //    int data = panoramas.Count;
        //    InstantiateItems(data, (item, index) =>
        //    {
        //        item.GetComponent<PanoramaItem>().Panorama = panoramas[index];
        //        if (item.GetComponent<PanoramaItem>().Value)
        //            item.GetComponent<PanoramaItem>().Value = false;
        //    }, callback);
        //}

        //public void InitializeHotTargets(List<SoftOutfitItemModel> hotTargets, Action callback = null)
        //{
        //    int data = hotTargets.Count;
        //    InstantiateItems(data, (item, index) =>
        //    {
        //        item.GetComponent<HotTargetItem>().SoftOutfit = hotTargets[index];
        //        if (item.GetComponent<HotTargetItem>().Value)
        //            item.GetComponent<HotTargetItem>().Value = false;

        //        if (WindowEditHot.Instance.CurrentHot.SoftOutfitInfo.hotspot_soft_outfit_id == hotTargets[index].id)
        //        {
        //            item.GetComponent<HotTargetItem>().Value = true;
        //            WindowEditHot.Instance.CurrentTarget = item.GetComponent<HotTargetItem>();
        //        }
        //    }, callback);
        //}

        //public void InitializeNavTargets(List<RoomSceneItemModel> navTargets, Action callback = null)
        //{
        //    int data = navTargets.Count;
        //    InstantiateItems(data, (item, index) =>
        //    {
        //        item.GetComponent<NavTargetItem>().NavTarget = navTargets[index];
        //        if (item.GetComponent<NavTargetItem>().Value)
        //            item.GetComponent<NavTargetItem>().Value = false;

        //        if (WindowEditNav.Instance.CurrentNav.NavInfo.hotspot_room_scene_id == navTargets[index].id)
        //        {
        //            item.GetComponent<NavTargetItem>().Value = true;
        //            WindowEditNav.Instance.CurrentTarget = item.GetComponent<NavTargetItem>();
        //        }
        //    }, callback);
        //}

        //public void InitializeNews(List<NewsItemModel> news, Action callback = null)
        //{
        //    int data = news.Count;
        //    InstantiateItems(data, (item, index) => item.GetComponent<NewsItem>().News = news[index], callback);
        //}



        protected override void SetItemValue(Transform t)
        {
            base.SetItemValue(t);

            //if (t.GetComponent<SceneItem>() != null)
            //{
            //    if (t.GetComponent<SceneItem>().Value)
            //        t.GetComponent<SceneItem>().Value = false;
            //}

            //if (t.GetComponent<PanoramaItem>() != null)
            //{
            //    if (t.GetComponent<PanoramaItem>().Value)
            //        t.GetComponent<PanoramaItem>().Value = false;
            //}
        }
    }

    public enum UnitType
    {
        Scene,
        Product,
        Panorama,
        HotTarget,
        NavTarget,
        News
    }

