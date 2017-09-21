// =============================================================================
// File      : CreatAndDestory.cs
// Author    : Felix-Bang
// Create    : 2017-09-21 15:00
// Copyright : Copyright (c) 2014-2017 fsnmt.com, All rights reserved.   
// 功能说明  :  Creat & Destory GameObject In Unity 
// =============================================================================

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FelixBangTools
{
    public class CreatAndDestory : MonoBehaviour
    {
        /// <summary>
        /// CreatItem
        /// </summary>
        /// <param name="parent">Parent</param>
        /// <param name="prefab">Prefab</param>
        /// <returns></returns>
        public static GameObject CreatItem(GameObject parent, GameObject prefab)
        {
            GameObject go = Instantiate(prefab) as GameObject;
            go.name = prefab.name;
            if (go != null)
            {
                Transform t = go.transform;
                t.parent = parent.transform;
                t.localPosition = Vector3.zero;     //localPosition
                t.localRotation = Quaternion.identity;
                t.localScale = Vector3.one;
                go.layer = parent.layer;
            }

            return go;
        }

        public static GameObject CreatItemInWorld(GameObject parent, GameObject prefab, Vector3 pos)
        {
            GameObject go = Instantiate(prefab) as GameObject;
            go.name = prefab.name;

            if (go != null)
            {
                Transform t = go.transform;
                t.parent = parent.transform;
                t.position = pos;   //Position
                t.localRotation = Quaternion.identity;
                t.localScale = Vector3.one;
                go.layer = parent.layer;
            }

            return go;
        }

        public static GameObject CreatItem(GameObject parent, GameObject prefab, Vector3 pos, Vector3 scale)
        {
            GameObject go = Instantiate(prefab) as GameObject;
            go.name = prefab.name;

            if (go != null)
            {
                Transform t = go.transform;
                t.parent = parent.transform;
                t.localPosition = pos;  //localPosition
                t.localRotation = Quaternion.identity;
                t.localScale = scale;
                go.layer = parent.layer;
            }

            return go;
        }

        /// <summary>
        /// InstantiatePrefabList（预制件列表实例化）
        /// </summary>
        /// <param name="dataCount"></param>
        /// <param name="parentTrans"></param>
        /// <param name="prefab"></param>
        /// <param name="initializePrefab">初始化预制件，用于设置预制件内容</param>
        /// <param name="complete"></param>
        public static void InstantiatePrefabList(int dataCount, Transform parentTrans, GameObject prefab,
            Action<GameObject, int> initializePrefab, Action complete = null)
        {
            int childCount = parentTrans.childCount;

            if (!parentTrans.gameObject.activeSelf)
                parentTrans.gameObject.SetActive(true);

            if (childCount <= dataCount)
            {
                for (int i = 0; i < dataCount; i++)
                {
                    if (i < childCount)
                    {
                        GameObject obj = parentTrans.GetChild(i).gameObject;

                        if (!obj.activeSelf)
                            obj.SetActive(true);

                        if (initializePrefab != null)
                            initializePrefab(obj, i);
                    }
                    else
                    {
                        GameObject obj = CreatItem(parentTrans.gameObject, prefab);

                        if (initializePrefab != null)
                            initializePrefab(obj, i);
                    }
                }
            }
            else
            {
                for (int i = 0; i < childCount; i++)
                {
                    GameObject obj = parentTrans.GetChild(i).gameObject;

                    if (i < dataCount)
                    {
                        if (!obj.activeSelf)
                            obj.SetActive(true);

                        if (initializePrefab != null)
                            initializePrefab(obj, i);
                    }
                    else
                    {
                        if (obj.activeSelf)
                            obj.SetActive(false);
                    }
                }
            }

            if (complete != null)
                complete();
        }


        /// <summary> 将一个物体放在另一个物体下，确立父子关系 </summary>  
        /// <param name="target">父物体</param>
        /// <param name="child">子物体</param>
        public static void AddChildToTarget(Transform target, Transform child)
        {
            child.parent = target;
            child.localScale = Vector3.one;
            child.localPosition = Vector3.zero;
            child.localEulerAngles = Vector3.zero;

            ChangeChildLayer(child, target.gameObject.layer);
        }

        public static void ChangeChildLayer(Transform t, int layer)
        {
            t.gameObject.layer = layer;
            for (int i = 0; i < t.childCount; ++i)
            {
                Transform child = t.GetChild(i);
                child.gameObject.layer = layer;
                ChangeChildLayer(child, layer);
            }
        }

        /// <summary> Destory </summary>
        public static void SaveDestory(Transform target)
        {
            if (target != null)
                SaveDestory(target.gameObject);
        }

        public static void SaveDestory(GameObject target)
        {
            if (target != null)
                UnityEngine.Object.Destroy(target);

            Resources.UnloadUnusedAssets();
            GC.Collect();
        }
    }
}
