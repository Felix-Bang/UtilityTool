// =============================================================================
// File      : WindowVR.cs
// Author    : 刘宏伟
// Create    : 2016-10-25 16:50
// Copyright : Copyright (c) 2014-2016 fsnmt.com, All rights reserved.     
// =============================================================================

using UnityEngine;
using System.Collections;
using System;


namespace FelixBangTools
{
    /// <summary>
    /// 界面控件基类
    /// </summary>
    public class WindowWidget : MonoBehaviour
    {
        DateTime OnClickTime;
        public float Throughtime = 0.5f;
        /// - OnHover (isOver) 悬停，悬停时传入true，移出时传入false
        public delegate void onMouseHover(WindowWidget eventObj, bool isOver);
        public onMouseHover OnMouseHover = null;
        void OnHover(bool isOver)
        {
            if (OnMouseHover != null) OnMouseHover(this, isOver);
        }
        /// - OnPress （isDown）按下时传入true，抬起时传入false
        public delegate void onMousePress(WindowWidget eventObj, bool isDown);
        public onMousePress OnMousePress = null;
        void OnPress(bool isDown)
        {
            if (OnMousePress != null) OnMousePress(this, isDown);
        }
        /// - OnSelect 相似单击，区别在于选中一次以后再选中将不再触发OnSelect
        public delegate void onMouseSelect(WindowWidget eventObj, bool selected);
        public onMouseSelect OnMouseSelect = null;
        void OnSelect(bool selected)
        {
            if (OnMouseSelect != null) OnMouseSelect(this, selected);
        }
        /// - OnClick 单击 Throughtime点击间隔时间
        public delegate void onMouseClick(WindowWidget eventObj);
        public onMouseClick OnMouseClick = null;
        void OnClick()
        {
            if (UICamera.currentTouchID==-2)
                return;

            if (Throughtime > (float)(DateTime.UtcNow - OnClickTime).TotalSeconds)
            {
                return;
            }
            OnClickTime = DateTime.UtcNow;

            if (OnMouseClick != null) OnMouseClick(this);
        }
        /// - OnDoubleClick 双击（双击间隔小于0.25秒）时触发。
        public delegate void onMouseDoubleClick(WindowWidget eventObj);
        public onMouseDoubleClick OnMouseDoubleClick = null;
        void OnDoubleClick()
        {
            if (OnMouseDoubleClick != null) OnMouseDoubleClick(this);
        }
        /// - OnDrag 按下并移动时触发，delta为传入的位移
        public delegate void onMouseDrag(WindowWidget eventObj, Vector2 delta);
        public onMouseDrag OnMouseDrag = null;
        void OnDrag(Vector2 delta)
        {
            if (OnMouseDrag != null) OnMouseDrag(this, delta);
        }
        public delegate void onMouseDrop(WindowWidget eventObj, GameObject dropObject);
        public onMouseDrop OnMouseDrop = null;
        void OnDrop(GameObject dropObject)
        {
            if (OnMouseDrop != null) OnMouseDrop(this, dropObject);
        }
        /// - OnInput (text) is sent when typing (after selecting a collider by clicking on it).
        /// - OnTooltip (show) is sent when the mouse hovers over a collider for some time without moving.
        /// - OnScroll (float delta) is sent out when the mouse scroll wheel is moved.
        /// - OnKey (KeyCode key) is sent when keyboard or controller input is used.
    }
}