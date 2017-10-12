// =============================================================================
// File      : AdjustTools.cs
// Author    : Felix-Bang
// Create    : 2017-09-21 15:17
// Copyright : Copyright (c) 2014-2017 fsnmt.com, All rights reserved.   
// 功能说明  :  Adjust for NGUI 
// =============================================================================

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace FelixBangTools
{
    public class AdjustTools
    {
        /// <summary>
        /// 调节图片大小（For NGUI）
        /// </summary>
        /// <param name="standard">标准长宽</param>
        /// <param name="originTex"></param>
        /// <param name="mTex"></param>
        public static void AdjustImageSize(Vector2 standard, Texture2D originTex, UITexture mTex)
        {
            mTex.width = originTex.width;
            mTex.height = originTex.height;
            if (originTex.width > originTex.height)
            {
                
                mTex.keepAspectRatio = UITexture.AspectRatioSource.BasedOnWidth;
                mTex.width = (int)standard.x;

                if (mTex.height > standard.y)
                {
                    mTex.keepAspectRatio = UITexture.AspectRatioSource.BasedOnHeight;
                    mTex.height = (int)standard.y;
                }
            }
            else
            {
                mTex.keepAspectRatio = UITexture.AspectRatioSource.BasedOnHeight;
                mTex.height = (int)standard.y;

                if (mTex.width > standard.x)
                {
                    mTex.keepAspectRatio = UITexture.AspectRatioSource.BasedOnWidth;
                    mTex.width = (int)standard.x;
                }
            }

            mTex.mainTexture = originTex;
        }

        /// <summary>
        /// 调节文本内容，多余的内容“...”表示（For NGUI）
        /// </summary>
        /// <param name="label"></param>
        /// <param name="strContent"></param>
        public static void LabelWarp(UILabel label, string strContent, Action<bool> callback = null)
        {
            string strOut = string.Empty;
            // 当前配置下的UILabel是否能够包围Text内容
            // Wrap是NGUI中自带的方法，其中strContent表示要在UILabel中显示的内容，strOur表示处理好后返回的字符串，uiLabel.height是字符串的高度 。
            bool bWarp = label.Wrap(strContent, out strOut, label.height);
            if (strOut.Length <= 0)
                bWarp = true;

            // 如果不能，就是说Text内容不能全部显示，这个时候，我们把最后一个字符去掉，换成省略号"..."
            if (!bWarp)
            {
                strOut = strOut.Substring(0, strOut.Length - 1);
                strOut += "...";
                label.text = strOut;
            }

            if (callback != null)
                callback(bWarp);

            // 如果可以包围，就是说Text内容可以完全显示，这个时候，我们不做处理，直接显示内容。
        }

        /// <summary>
        /// 返回最大或者最小Depth界面
        /// Get the max or min depth UIPanel
        /// </summary>
        public static GameObject GetPanelDepthMaxMin(GameObject target, bool maxDepth, bool includeInactive)
        {
            List<UIPanel> lsPanels = GetPanelSorted(target, includeInactive);
            if (lsPanels != null)
            {
                if (maxDepth)
                    return lsPanels[lsPanels.Count - 1].gameObject;
                else
                    return lsPanels[0].gameObject;
            }
            return null;
        }

        private class CompareSubPanels : IComparer<UIPanel>
        {
            public int Compare(UIPanel left, UIPanel right)
            {
                return left.depth - right.depth;
            }
        }

        private static List<UIPanel> GetPanelSorted(GameObject target, bool includeInactive = false)
        {
            UIPanel[] panels = target.transform.GetComponentsInChildren<UIPanel>(includeInactive);
            if (panels.Length > 0)
            {
                List<UIPanel> lsPanels = new List<UIPanel>(panels);
                lsPanels.Sort(new CompareSubPanels());
                return lsPanels;
            }
            return null;
        }
    }
}

