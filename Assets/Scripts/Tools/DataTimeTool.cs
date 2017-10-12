// =============================================================================
// File      : DataTimeTool.cs
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
    public class DataTimeTool
    {
        /// <summary>
        /// 时间戳转换为普通时间，如：2010-1-1 12:00:00。
        /// </summary>
        /// <param name="timeStamp">Unix时间戳格式。</param>
        /// <returns>返回普通时间格式。</returns>
        public static string ConvertToUniversalTime(string timeStamp)
        {
            DateTime dtResult = ConvertToDateTime(timeStamp);
            return dtResult.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 时间戳转换为DateTime，如：12/12/2014 4:58:28 PM
        /// </summary>
        /// <param name="timeStamp">Unix时间戳格式。</param>
        /// <returns>返回C#格式时间。</returns>
        public static DateTime ConvertToDateTime(string timeStamp)
        {
            if (string.IsNullOrEmpty(timeStamp))
                return default(DateTime);

            long time;

            if (timeStamp.Length < 18)
                time = long.Parse(timeStamp + "0000000");
            else
                time = long.Parse(timeStamp);

            TimeSpan toNow = new TimeSpan(time);
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return startTime.Add(toNow);
        }

        /// <summary>
        /// DateTime时间格式转换为Unix时间戳格式。
        /// </summary>
        /// <param name="time">DateTime时间格式。</param>
        /// <returns>Unix时间戳格式。</returns>
        public static int ConvertToTimeStamp(DateTime time)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }
    }
}

