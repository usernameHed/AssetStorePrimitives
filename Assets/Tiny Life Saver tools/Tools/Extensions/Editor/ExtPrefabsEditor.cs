﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using hedCommon.extension.editor;
using hedCommon.extension.editor.sceneView;

public static class ExtPrefabsEditor 
{
    /// <summary>
    /// is this GameObject a prefabs ?
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static bool IsPrefab(GameObject obj)
    {
        var prefab = PrefabUtility.GetOutermostPrefabInstanceRoot(obj);
        return prefab != null && prefab == obj;
    }

    public static bool IsPrefab(GameObject obj, out GameObject prefab)
    {
        prefab = PrefabUtility.GetOutermostPrefabInstanceRoot(obj);
        return prefab != null && prefab == obj;
    }

    /// <summary>
    /// default path is: Assets/_Prefabs/Editor/
    /// call 
    /// </summary>
    /// <param name="pathPrefab"></param>
    /// <returns></returns>
    public static GameObject InstantiatePrefabsFromAssetPrefabPath(string pathPrefab, string defaultPath = "Assets/_Prefabs/_ToLoad/")
    {
        GameObject prefabsAsset = (GameObject)AssetDatabase.LoadAssetAtPath(defaultPath + pathPrefab + ".prefab", typeof(GameObject));
        GameObject instance = GameObject.Instantiate(prefabsAsset);

        instance.transform.SetParent(Selection.activeTransform);
        if (Selection.activeTransform != null)
        {
            instance.transform.localPosition = Vector3.zero;
        }
        Selection.activeGameObject = instance;

        ExtSceneView.PlaceGameObjectInFrontOfSceneView(instance);
        GameObjectUtility.EnsureUniqueNameForSibling(instance);

        return (instance);
    }

    /// <summary>
    /// default path is: Assets/_Prefabs/Editor/
    /// call 
    /// </summary>
    /// <param name="pathPrefab"></param>
    /// <returns></returns>
    public static GameObject InstantiatePrefabsWithLinkFromAssetPrefabPath(string pathPrefab, string defaultPath = "Assets/_Prefabs/_ToLoad/")
    {
        GameObject prefabsAsset = (GameObject)AssetDatabase.LoadAssetAtPath(defaultPath + pathPrefab + ".prefab", typeof(GameObject));
        GameObject instance = PrefabUtility.InstantiatePrefab(prefabsAsset) as GameObject;

        instance.transform.SetParent(Selection.activeTransform);
        if (Selection.activeTransform != null)
        {
            instance.transform.localPosition = Vector3.zero;
        }
        Selection.activeGameObject = instance;

        ExtSceneView.PlaceGameObjectInFrontOfSceneView(instance);
        GameObjectUtility.EnsureUniqueNameForSibling(instance);

        return (instance);
    }
}
