// =============================================================================
// File      : Test.cs
// Author    : 
// Create    : 2017-00-00 00:00
// Copyright : Copyright (c) 2014-2017 fsnmt.com, All rights reserved.   
// 功能说明  :   
// =============================================================================

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FelixBangTools;

public class Test : MonoBehaviour 
{
    void Start () 
	{
        Debug.Log(DateTime.Now);
        int stamp = DataTimeTool.ConvertToTimeStamp(DateTime.Now);
        Debug.Log(stamp);
        DateTime dateTime = DataTimeTool.ConvertToDateTime(stamp.ToString());
        Debug.Log(dateTime);
        string showTime = DataTimeTool.ConvertToUniversalTime(stamp.ToString());
        Debug.Log(showTime);
    }
}
