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

        /// <summary>
        /// Create an horizontal line
        /// </summary>
        /// <param name="color"></param>
        /// <param name="thickness">2 pixels</param>
        /// <param name="paddingTop">1 = 10 pixels</param>
        /// <param name="paddingBottom">1 = 10 pixels</param>
        /// <param name="paddingLeft">percent of the width clamped 0-1</param>
        /// <param name="paddingRight">percent of the width clamped 0-1</param>
        public static void HorizontalLineThickness(Color color,
            int thickness = 2,
            float paddingTop = 1,
            float paddingBottom = 1,
            float paddingLeft = 0.1f,
            float paddingRight = 0.1f,
            float autoWidth = -1)
        {
            paddingTop *= 10;
            paddingBottom *= 10;

            Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(paddingBottom + thickness));
            float width = r.width;
            if (autoWidth > 0)
            {
                width = autoWidth;
            }

            paddingLeft = ExtMathf.SetBetween(paddingLeft, 0, 1);
            paddingRight = ExtMathf.SetBetween(paddingRight, 0, 1);

            paddingLeft = paddingLeft * width / 1f;
            paddingRight = paddingRight * width / 1f;



            r.height = thickness;
            r.y += paddingTop / 2;
            r.x -= 2 - paddingLeft;
            r.width += 6 - paddingLeft - paddingRight;
            EditorGUI.DrawRect(r, color);
        }

        //end class
    }
    //end nameSpace
}