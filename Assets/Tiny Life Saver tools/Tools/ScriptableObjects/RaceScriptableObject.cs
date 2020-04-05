using hedCommon.extension.runtime;
using System.IO;
#if UNITY_EDITOR
    using UnityEditor;
#endif
using UnityEngine;

namespace hedCommon.scriptableObject
{
    public abstract class RaceScriptableObject : ScriptableObject
    {
        [SerializeField, HideInInspector]
        protected bool _isActive = true;
        public void SetActiveSelf(bool active)
        {
            _isActive = active;
        }

        [HideInInspector]
        public string FolderParentName = "";
        [HideInInspector]
        public string FolderParentPath = "";
        [HideInInspector]
        public string CalculatedPath = "";

        public virtual bool IsActiveSelf()
        {
            return (_isActive);
        }
        public virtual bool IsActiveInBuild()
        {
            return (_isActive);
        }

        public abstract string GetModifiableKeyAsset();
        public abstract string GetExplicitModifiableKeyAsset();

#if UNITY_EDITOR
        public void CalculatePaths()
        {
            CalculatedPath = AssetDatabase.GetAssetPath(this);      //get Assets/__RACES/Tracks/Monaco/Monaco.asset
            FolderParentPath = Path.GetDirectoryName(CalculatedPath);   //get Assets\__RACES\Tracks\Monaco
            FolderParentPath = ReformatPathForUnity(FolderParentPath);  //get Assets/__RACES/Tracks/Monaco
            FolderParentName = Path.GetFileName(FolderParentPath);            //get Monaco
        }
#endif
        /// <summary>
        /// change a path from
        /// Assets\path\of\file
        /// to
        /// Assets/path/of/file
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static string ReformatPathForUnity(string path, char characterReplacer = '-')
        {
            string formattedPath = path.Replace('\\', '/');
            formattedPath = formattedPath.Replace('|', characterReplacer);
            return (formattedPath);
        }
    }
}