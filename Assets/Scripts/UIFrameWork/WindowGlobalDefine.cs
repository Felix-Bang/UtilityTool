// =============================================================================
// File      : UIGlobalDefine.cs
// Author    : 刘宏伟
// Create    : 2016-10-25 09:56
// Copyright : Copyright (c) 2014-2016 fsnmt.com, All rights reserved.     
// =============================================================================

using UnityEngine;
using System.Collections.Generic;

namespace FelixBangTools
{
    public enum SceneID
    {  
        Loading,
        WideScreen,
        SquareScreen,
        Background
    }

    public enum WindowType
    {
        Fixed,
        Normal,    // 可推出界面(WindowLogin,UIRank等)
        PopUp,     // 模式窗口(UIMessageBox, yourPopWindow , yourTipsWindow ......)
    }

    public enum WindowID
    {
        WindowID_Invaild = 0,
        WindowID_Login,      
    }

    public enum ItemID
    {
        ItemID_Filtrate,     
    }
    
    public class WindowResourceDefine
    {
        // Define the UIWindow prefab paths
        // all window prefab placed in Resources folder
        // maybe your window assetbundle path
        public static Dictionary<int, string> windowPath = new Dictionary<int, string>()
        {
            { (int)WindowID.WindowID_Login,         "Window_Login" },
        };

        public static string WindowDir
        {
            get
            {
                if (ResolutionTool.IsWideScreen)
                    return "Windows/16-9/";
                else
                    return "Windows/4-3/";
            }
        }

        public static Dictionary<ItemID, string> itemPath = new Dictionary<ItemID, string>()
        {           
            { ItemID.ItemID_Filtrate,                  "FiltrateButton" },  
        };

        public static string ItemDir
        {
            get{ return "Item/";}
        }
    }
}