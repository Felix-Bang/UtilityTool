using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace FelixBangTools
{
    public class WindowManager : Singleton<WindowManager>
    {
        protected Dictionary<int, BaseWindow> ExistWindowDic = new Dictionary<int, BaseWindow>();

        public BaseWindow GetGameWindow(WindowID id)
        {
            if (ExistWindowDic.ContainsKey((int)id))
                return ExistWindowDic[(int)id];
            else
                return null;
        }

        public void CreatWindow(WindowID id)
        {
            BaseWindow baseWindow = GetGameWindow(id);

            if (baseWindow == null)
            {
                if (WindowResourceDefine.windowPath.ContainsKey((int)id))
                {
                    string prefabPath = WindowResourceDefine.WindowDir + WindowResourceDefine.windowPath[(int)id];
                    GameObject prefab = Resources.Load<GameObject>(prefabPath);

                    if (prefab != null)
                    {
                        GameObject uiObject = (GameObject)GameObject.Instantiate(prefab);
                        NGUITools.SetActive(uiObject, true);
                        baseWindow = uiObject.GetComponent<BaseWindow>();
                        Transform targetRoot = GetTargetRoot(baseWindow.Type);
                        CreatAndDestory.AddChildToTarget(targetRoot, baseWindow.gameObject.transform);
                        ExistWindowDic[(int)id] = baseWindow;
                    }
                }
            }
            else
                baseWindow.gameObject.SetActive(true);
        }

        private Transform GetTargetRoot(WindowType type)
        {
            if (type == WindowType.Fixed)
                return null/*Global.FixedWindowRoot*/;
            else if (type == WindowType.Normal)
                return null/*Global.NormalWindowRoot*/;
            else if (type == WindowType.PopUp)
                return null/*Global.PopUpWindowRoot*/;
            else
                return null/*Global.UIRoot*/;
        }

        /// <summary>
        /// Delay to show target window
        /// </summary>
        /// <param name="delayTime"> delayTime</param>
        /// <param name="id"> WindowId</param>
        /// <param name="showData">show window data</param>
        public virtual void ShowWindowDelay(float delayTime, WindowID id)
        {
            StopAllCoroutines();
            StartCoroutine(_ShowWindowDelay(delayTime, id));
        }

        private IEnumerator _ShowWindowDelay(float delayTime, WindowID id)
        {
            yield return new WaitForSeconds(delayTime);
            CreatWindow(id);
        }

        public void HideWindow(WindowID id, Action onCompleted = null)
        {
            BaseWindow window = ExistWindowDic[(int)id];
            window.gameObject.SetActive(false);
            if (onCompleted != null)
                onCompleted();
        }

        public void CloseWindow(WindowID id, Action onCompleted = null)
        {
            BaseWindow window = ExistWindowDic[(int)id];
            //销毁
            CreatAndDestory.SaveDestory(window.gameObject);
            ExistWindowDic.Remove((int)id);

            if (onCompleted != null)
                onCompleted();
        }

        public void CloseWindow(WindowID id, float delay, Action callback = null)
        {
            StartCoroutine(DelayDeastory(id, delay, callback));
        }

        private IEnumerator DelayDeastory(WindowID id, float delay, Action callback = null)
        {
            yield return new WaitForSeconds(delay);
            CloseWindow(id, callback);
        }
    }
}

