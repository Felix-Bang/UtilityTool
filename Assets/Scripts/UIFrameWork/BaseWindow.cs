// =============================================================================
// File      : WindowVR.cs
// Author    : 刘宏伟
// Create    : 2016-10-25 10:50
// Copyright : Copyright (c) 2014-2016 fsnmt.com, All rights reserved.     
// =============================================================================

using UnityEngine;
using System.Collections.Generic;

namespace FelixBangTools
{
    public abstract class BaseWindow : MonoBehaviour
    {       
        private WindowID _windowID = WindowID.WindowID_Invaild;
        private WindowType _windowType = WindowType.Fixed;
        private Transform _mTransform;
        private Dictionary<string, WindowWidget> _windowWidgets = new Dictionary<string, WindowWidget>();

        protected virtual void Awake()
        {
            _mTransform = this.gameObject.transform;
            this.FindChildWidgets(_mTransform);
            SetWindowID();
            InitWindowOnAwake();
        }

        public WindowID ID
        {
            get
            {
                if (this._windowID == WindowID.WindowID_Invaild)
                    Debug.LogError("window id is " + WindowID.WindowID_Invaild);

                return _windowID;
            }
            protected set { _windowID = value; }
        }

        public WindowType Type
        {
            get { return _windowType; }
            protected set { _windowType = value; }
        }

        protected abstract void SetWindowID();

        protected virtual void InitWindowOnAwake() { }

        protected WindowWidget GetWidget(string name)
        {
            // If allready find out, return 
            if (_windowWidgets.ContainsKey(name))
                return _windowWidgets[name];

            // Find out widget with name and add to dictionary
            Transform t = gameObject.transform.FindChild(name);
            if (t == null) return null;

            WindowWidget widget = t.gameObject.GetComponent<WindowWidget>();
            if (widget != null)
                _windowWidgets.Add(widget.gameObject.name, widget);

            return t.gameObject.GetComponent<WindowWidget>();
        }

        protected T GetWidget<T>(string name) where T : Component
        {
            // Find out widget with name and add to dictionary
            GameObject go = GameObject.Find(name);
            if (go == null) return null;

            T widget = go.GetComponent<T>();

            return widget;
        }

        private void FindChildWidgets(Transform t)
        {
            WindowWidget widget = t.gameObject.GetComponent<WindowWidget>();

            if (widget != null)
            {
                string name = t.gameObject.name;
                if (!_windowWidgets.ContainsKey(name))
                    _windowWidgets.Add(name, widget);
                else
                {
                    //Debug.LogWarning("Scene[" + this.transform.name + "]UISceneWidget[" + name + "]is exist!");
                }
            }

            for (int i = 0; i < t.childCount; ++i)
            {
                Transform child = t.GetChild(i);
                FindChildWidgets(child);
            }
        }
    }
}

