// =============================================================================
// File      : ResolutionTool.cs
// Author    : 
// Create    : 2017-00-00 00:00
// Copyright : Copyright (c) 2014-2017 fsnmt.com, All rights reserved.   
// 功能说明  :   
// =============================================================================

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FelixBangTools
{
    public class ResolutionTool
    {
        /// <summary>
        /// 提供给PC版调用。返回 true 表示16：9，返回 false 表示4：3。
        /// </summary>
        public static bool IsWideScreen
        {
            get
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

        /// <summary>
        /// 提供给iOS和Android版调用。返回true表示9:16的竖版，返回false表示4：3版。
        /// </summary>
        public static bool IsPortrait
        {
            get
            {
#if UNITY_EDITOR
                float aspectRatio = (float)Screen.height / (float)Screen.width;
#else
                float aspectRatio = (float)Screen.currentResolution.height / (float)Screen.currentResolution.width;
#endif
                // 如果屏幕比例为4:3，强制把屏幕横屏。
                if (aspectRatio > 1.2f && aspectRatio < 1.5f)
                {
                    Screen.orientation = ScreenOrientation.Landscape;
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public static int ScreenWidth
        {
            get
            {
#if UNITY_EDITOR
                return Screen.width;
#else
                return Screen.currentResolution.width;
#endif
            }
        }

        public static int ScreenHeight
        {
            get
            {
#if UNITY_EDITOR
                return Screen.height;
#else
                return Screen.currentResolution.height;
#endif
            }
        }

        public static void SetResolution(bool isFullscreen = false)
        {
            //if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.OSXPlayer)
            {
                if (isFullscreen)
                {
                    // Fullscreen.
                    Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
                }
                else
                {
                    // 120 算作桌面任务栏高度。
                    int height = Screen.currentResolution.height - 120;
                    int width = 0;

                    // Max window (16:9 or 4:3).
                    if (IsWideScreen)
                        width = (height * 16) / 9;
                    else
                        width = (height * 4) / 3;

                    Screen.SetResolution(width, height, false);
                }
            }
        }

    }
}

