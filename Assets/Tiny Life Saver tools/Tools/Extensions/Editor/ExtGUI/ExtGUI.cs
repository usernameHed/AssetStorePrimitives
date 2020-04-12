using hedCommon.extension.runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUILayout;

namespace hedCommon.extension.editor
{
    /// <summary>
    /// all of this function must be called in OnSceneGUI
    /// </summary>
    public static class ExtGUI
    {
        public static bool DrawDisplayDialog(
                string mainTitle = "main title",
                string content = "what's the dialog box say",
                string accept = "Yes",
                string no = "Get me out of here",
                bool replaceMousePositionAtTheEnd = true)
        {
            if (!EditorUtility.DisplayDialog(mainTitle, content, accept, no))
            {
                return (false);
            }
            return (true);
        }

        //end class
    }
    //end nameSpace
}