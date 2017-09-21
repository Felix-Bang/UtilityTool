// =============================================================================
// File      : FindTool.cs
// Author    : Felix-Bang
// Create    : 2017-09-21 14:00
// Copyright : Copyright (c) 2014-2017 fsnmt.com, All rights reserved.   
// 功能说明  :  Find the gameobject or components which you need in Unity.
// =============================================================================

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FelixBangTools
{
    public class FindTool : MonoBehaviour
    {
        /* 一、相关方法
         *     GameObject.Find
         *     Transform.Find
         *     GameObject.FindWithTag 
         *     GameObject.FindGameObjectsWithTag 
         *     Resources.FindObjectsOfTypeAll
         * 二、GameObject.Find()
         *     1. 无法查找隐藏对象，也可以支持路径查找.
         *          如果路径查找中的任何一个父节点active==false，这个对象都将查找不到。
         *       即!obj.activeInHierarchy时，找不到obj对象。
         *     2. 使用效率低下
         * 三、Transform.Find()
         *     1. 可以查找隐藏对象
         *     2. 支持路径查找
         *          如果要去找孙结点，可以加”/“符号，如:
         *                  Transform childForm = uiNode.transform.FindChild("Hero/Update/Icon");
         *     3. 此外，还有一个 Transform.FindChind() 和这个没区别，但一般好像都用Transform.Find()
         *     4. 查找子结点，可以用  transform.GetChild(i) 
         */


        /// <summary>
        /// FindChild
        /// </summary>
        /// <param name="trans">Parent's Transform </param>
        /// <param name="childName">Child Name</param>
        /// <returns></returns>
        public static GameObject FindChild(Transform trans, string childName)
        {
            Transform child = trans.FindChild(childName);

            if (child != null)
                return child.gameObject;

            GameObject go = null;

            for (int i = 0, iMax = trans.childCount; i < iMax; i++)
            {
                child = trans.GetChild(i);
                go = FindChild(child, childName);

                if (go != null)
                    return go;
            }

            return null;
        }

        /// <summary>查找某类型自物体</summary>
        public static T FindChild<T>(Transform trans, string childName) where T : Component
        {
            GameObject go = FindChild(trans, childName);
            if (go == null)
                return null;
            return go.GetComponent<T>();
        }

        /* 得到Component的方式
         * 1）找到一个符合条件的之后就返回找到的这个并且不再寻找
         *      gameObject.GetComponent<T>() 
         *      gameObject.GetComponentInChildren
         *      gameObject.GetComponentInParent
         * 2）会找出所有的符合条件的，并做成一个数组返回
         *      gameObject.GetComponents
         *      gameObject.GetComponentsInChildren
         *      gameObject.GetComponentsInParent
         *      
         * 3）增加组件
         *      gameObject.AddComponent<T>();    
         */
    }
}

