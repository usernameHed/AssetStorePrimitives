﻿using hedCommon.extension.editor;
using hedCommon.extension.runtime;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using static UnityEditor.EditorGUILayout;

namespace hedCommon.sceneWorkflow
{
    public class CustomSceneButtons
    {
        private const int _heightText = 8;
        private const int _widthButtons = 17;
        private const int _heightButtons = 14;

        private GlobalScenesListerAsset _refGameAsset;

        public void InitTextures()
        {
            _refGameAsset = ExtFind.GetAssetByGenericType<GlobalScenesListerAsset>();
        }

        public void OnLeftToolbarGUI()
        {
            
        }

        public void DisplayScenesButton()
        {
            GUILayout.FlexibleSpace();

            if (_refGameAsset == null)
            {
                _refGameAsset = ExtFind.GetAssetByGenericType<GlobalScenesListerAsset>();
                if (_refGameAsset == null)
                {
                    if (GUILayout.Button("create a Scene Assets Reference", ExtGUIStyles.microButton, GUILayout.Width(200), GUILayout.Height(_heightButtons)))
                    {
                        _refGameAsset = ExtScriptableObject.CreateAsset<GlobalScenesListerAsset>("Assets/Global Scenes Lister Asset.asset");
                        EditorGUIUtility.PingObject(_refGameAsset);
                        Selection.activeObject = _refGameAsset;
                    }
                    GUILayout.FlexibleSpace();
                    return;
                }
            }

            

            using (VerticalScope vertical = new VerticalScope())
            {
                GUILayout.Label("Scenes", ExtGUIStyles.miniTextCentered, GUILayout.Height(_heightText));
                using (HorizontalScope horizontal = new HorizontalScope())
                {
                    if (GUILayout.Button("locate", ExtGUIStyles.microButton, GUILayout.Width(50), GUILayout.Height(_heightButtons)))
                    {
                        EditorGUIUtility.PingObject(_refGameAsset);
                        Selection.activeObject = _refGameAsset;
                    }
                    int currentLoaded = _refGameAsset.LastIndexUsed;
                    for (int i = 0; i < _refGameAsset.CountSceneToLoad; i++)
                    {
                        bool currentLoadedScene = currentLoaded == i;
                        string levelIndex = (i + 1).ToString();
                        currentLoadedScene = GUILayout.Toggle(currentLoadedScene, new GUIContent(levelIndex, "Load all scenes inside the lister " + levelIndex), ExtGUIStyles.microButton, GUILayout.Width(_widthButtons), GUILayout.Height(_heightButtons));
                        if (currentLoadedScene != (currentLoaded == i))
                        {
                            if (!Application.isPlaying)
                            {
                                if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                                {
                                    return;
                                }
                            }
                            _refGameAsset.LastIndexUsed = i;
                            _refGameAsset.LoadScenesByIndex(i, OnLoadedScenes, hardReload: true);
                            EditorGUIUtility.PingObject(_refGameAsset.GetSceneAddet(i));
                            Selection.activeObject = _refGameAsset.GetSceneAddet(i);
                            return;
                        }
                    }
                }
            }
            GUILayout.FlexibleSpace();
        }

        private void OnLoadedScenes(SceneAssetLister lister)
        {
            if (Application.isPlaying)
            {
                AbstractLinker.Instance?.InitFromPlay();
            }
            else
            {
                AbstractLinker.Instance?.InitFromEditor();
            }
        }

        //end class
    }
}