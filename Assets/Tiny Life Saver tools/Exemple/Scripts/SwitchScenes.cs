using hedCommon.sceneWorkflow;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hedCommon.exemple
{
    public class SwitchScenes : MonoBehaviour
    {
        [SerializeField]
        private GlobalScenesListerAsset _globalScenesListerAsset;

        /// <summary>
        /// open a group of scene
        /// </summary>
        /// <param name="index"></param>
        public void OpenGroupOfSceneByIndex(int index)
        {
            _globalScenesListerAsset.LoadScenesByIndex(index, OnComplete, hardReload: false);
        }

        public void OnComplete(SceneAssetLister sceneAssetLister)
        {
            Debug.Log("Group " + sceneAssetLister.NameList + " has loaded !");
        }
    }
}