// =============================================================================
// File      : UIInputNoPopup.cs
// Author    : 刘宏伟
// Create    : 2016-11-09 14:40
// Copyright : Copyright (c) 2014-2016 fsnmt.com, All rights reserved.     
// =============================================================================

using UnityEngine;
using System.Collections;

namespace FelixBangTools
{
    public class UIInputNoPopup : UIInput
    {
        private int _Index;
        public int Index
        {
            get { return _Index; }
            set { _Index = value; }
        }

        public void InsertSign(string text)
        {
            base.Insert(text);
            
            UpdateLabel();
        }

        public void DoBack()
        {
            base.DoBackspace();
            UpdateLabel();
        }

        protected override void Update()
        {
         // void tt() { 
#if UNITY_EDITOR
            if (!Application.isPlaying) return;
#endif
            if (!isSelected || mSelectTime == Time.frameCount) return;

            if (mDoInit) Init();
#if MOBILE
		// Wait for the keyboard to open. Apparently mKeyboard.active will return 'false' for a while in some cases.
	//	mWaitForKeyboard = false;
        if (mWaitForKeyboard)
		{
			if (mKeyboard != null && !mKeyboard.active) return;
			mWaitForKeyboard = false;
		}
#endif
            // Unity has issues bringing up the keyboard properly if it's in "hideInput" mode and you happen
            // to select one input in the same Update as de-selecting another.
            if (mSelectMe != -1 && mSelectMe != Time.frameCount)
            {
                mSelectMe = -1;
                mSelectionEnd = string.IsNullOrEmpty(mValue) ? 0 : mValue.Length;
                mDrawStart = 0;
                mSelectionStart = selectAllTextOnFocus ? 0 : mSelectionEnd;
                label.color = activeTextColor;
#if MOBILE
			RuntimePlatform pf = Application.platform;
			if (pf == RuntimePlatform.IPhonePlayer
				|| pf == RuntimePlatform.Android
				|| pf == RuntimePlatform.WP8Player
#if UNITY_4_3
				|| pf == RuntimePlatform.BB10Player
#else
				|| pf == RuntimePlatform.BlackBerryPlayer
				|| pf == RuntimePlatform.MetroPlayerARM
				|| pf == RuntimePlatform.MetroPlayerX64
				|| pf == RuntimePlatform.MetroPlayerX86
#endif
			)
			{
				string val;
				TouchScreenKeyboardType kt;

				if (inputShouldBeHidden)
				{
					TouchScreenKeyboard.hideInput = true;
					kt = (TouchScreenKeyboardType)((int)keyboardType);
					val = "|";
				}
				else if (inputType == InputType.Password)
				{
					TouchScreenKeyboard.hideInput = false;
					kt = (TouchScreenKeyboardType)((int)keyboardType);
					val = mValue;
					mSelectionStart = mSelectionEnd;
				}
				else
				{
					TouchScreenKeyboard.hideInput = false;
					kt = (TouchScreenKeyboardType)((int)keyboardType);
					val = mValue;
					mSelectionStart = mSelectionEnd;
				}

				mWaitForKeyboard = true;
				mKeyboard = (inputType == InputType.Password) ?
					TouchScreenKeyboard.Open(val, kt, false, false, true) :
					TouchScreenKeyboard.Open(val, kt, !inputShouldBeHidden && inputType == InputType.AutoCorrect,
						label.multiLine && !hideInput, false, false, defaultText);
#if UNITY_METRO
				mKeyboard.active = true;
#endif
			}
			else
#endif // MOBILE
                {
                    Vector2 pos = (UICamera.current != null && UICamera.current.cachedCamera != null) ?
                        UICamera.current.cachedCamera.WorldToScreenPoint(label.worldCorners[0]) :
                        label.worldCorners[0];
                    pos.y = Screen.height - pos.y;
                    Input.imeCompositionMode = IMECompositionMode.On;
                    Input.compositionCursorPos = pos;
                }

                UpdateLabel();
                if (string.IsNullOrEmpty(Input.inputString)) return;
            }
#if MOBILE
		if (mKeyboard != null)
		{
			string text = (mKeyboard.done || !mKeyboard.active) ? mCached : mKeyboard.text;
 
			if (inputShouldBeHidden)
			{
				if (text != "|")
				{
					if (!string.IsNullOrEmpty(text))
					{
						Insert(text.Substring(1));
					}
					else if (!mKeyboard.done && mKeyboard.active)
					{
						DoBackspace();
					}
					mKeyboard.text = "|";
				}
			}
			else if (mCached != text)
			{
				mCached = text;
				if (!mKeyboard.done && mKeyboard.active) value = text;
			}

			if (mKeyboard.done || !mKeyboard.active)
			{
				if (!mKeyboard.wasCanceled) Submit();
				mKeyboard = null;
				isSelected = false;
				mCached = "";
			}
		}
		else
#endif // MOBILE
            {
                string ime = Input.compositionString;

                // There seems to be an inconsistency between IME on Windows, and IME on OSX.
                // On Windows, Input.inputString is always empty while IME is active. On the OSX it is not.
                if (string.IsNullOrEmpty(ime) && !string.IsNullOrEmpty(Input.inputString))
                {
                    // Process input ignoring non-printable characters as they are not consistent.
                    // Windows has them, OSX may not. They get handled inside OnGUI() instead.
                    string s = Input.inputString;

                    for (int i = 0; i < s.Length; ++i)
                    {
                        char ch = s[i];
                        if (ch < ' ') continue;

                        // OSX inserts these characters for arrow keys
                        if (ch == '\uF700') continue;
                        if (ch == '\uF701') continue;
                        if (ch == '\uF702') continue;
                        if (ch == '\uF703') continue;

                        Insert(ch.ToString());
                    }
                }

                // Append IME composition
                if (mLastIME != ime)
                {
                    mSelectionEnd = string.IsNullOrEmpty(ime) ? mSelectionStart : mValue.Length + ime.Length;
                    mLastIME = ime;
                    UpdateLabel();
                    ExecuteOnChange();
                }
            }

            // Blink the caret
            if (mCaret != null && mNextBlink < RealTime.time)
            {
                mNextBlink = RealTime.time + 0.5f;
                mCaret.enabled = !mCaret.enabled;
            }

            // If the label's final alpha changes, we need to update the drawn geometry,
            // or the highlight widgets (which have their geometry set manually) won't update.
            if (isSelected && mLastAlpha != label.finalAlpha)
                UpdateLabel();

            // Cache the camera
            //if (mCam == null) mCam = UICamera.FindCameraForLayer(gameObject.layer);

            //// Having this in OnGUI causes issues because Input.inputString gets updated *after* OnGUI, apparently...
            //if (mCam != null)
            //{
            //    bool newLine = false;

            //    if (label.multiLine)
            //    {
            //        bool ctrl = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
            //        if (onReturnKey == OnReturnKey.Submit) newLine = ctrl;
            //        else newLine = !ctrl;
            //    }

            //    if (UICamera.GetKeyDown(mCam.submitKey0))
            //    {
            //        if (newLine)
            //        {
            //            Insert("\n");
            //        }
            //        else
            //        {
            //            if (UICamera.controller.current != null)
            //                UICamera.controller.clickNotification = UICamera.ClickNotification.None;
            //            UICamera.currentKey = mCam.submitKey0;
            //            Submit();
            //        }
            //    }

            //    if (UICamera.GetKeyDown(mCam.submitKey1))
            //    {
            //        if (newLine)
            //        {
            //            Insert("\n");
            //        }
            //        else
            //        {
            //            if (UICamera.controller.current != null)
            //                UICamera.controller.clickNotification = UICamera.ClickNotification.None;
            //            UICamera.currentKey = mCam.submitKey1;
            //            Submit();
            //        }
            //    }

            //    if (!mCam.useKeyboard && UICamera.GetKeyUp(KeyCode.Tab))
            //        OnKey(KeyCode.Tab);
            //}
        }

    }
}