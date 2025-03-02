﻿using hedCommon.extension.editor;
using hedCommon.extension.editor.sceneView;
using hedCommon.extension.runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace hedCommon.extension.editor
{
    public static class ExtHandle
    {
        public enum DrawOutlineType
        {
            MIDDLE,
            INSIDE,
            OUTSIDE,
        }

        public static void DoMultiHandle(Transform toMove, out bool hasChanged)
        {
            hasChanged = false;
            Tool current = Tools.current;
            switch (current)
            {
                case Tool.Move:
                    DoHandleMove(toMove, true, out hasChanged);
                    break;
                case Tool.Rotate:
                    DoHandleRotation(toMove, true, out hasChanged);
                    break;
                case Tool.Scale:
                    DoHandleScale(toMove, true, out hasChanged);
                    break;
                case Tool.Rect:
                    DoHandleRect(toMove, true, out hasChanged);
                    break;
                case Tool.Transform:
                    DoHandleTransform(toMove, true, out hasChanged);
                    break;
            }
        }
        public static void DoMultiHandle(ref Matrix4x4 toMove, out bool hasChanged)
        {
            hasChanged = false;
            Tool current = Tools.current;
            switch (current)
            {
                case Tool.Move:
                    DoHandleMove(ref toMove, out hasChanged);
                    break;
                
                case Tool.Rotate:
                    DoHandleRotation(ref toMove, out hasChanged);
                    break;

                case Tool.Scale:
                    DoHandleScale(ref toMove, out hasChanged);
                    break;
            }
        }

        public static void DoHandleMove(Transform toMove, bool record, out bool hasChanged)
        {
            hasChanged = false;
            if (record)
            {
                Undo.RecordObject(toMove.gameObject.transform, "handle Point move");
            }

            Quaternion rotation = (Tools.pivotRotation == PivotRotation.Global) ? Quaternion.identity : toMove.rotation;
            Vector3 newPosition = Handles.PositionHandle(toMove.position, rotation);

            if (newPosition != toMove.position)
            {
                hasChanged = true;
                toMove.position = newPosition;
                if (record)
                {
                    EditorUtility.SetDirty(toMove);
                }
            }
        }
        public static void DoHandleMove(ref Matrix4x4 toMove, out bool hasChanged)
        {
            hasChanged = false;

            Quaternion rotation = (Tools.pivotRotation == PivotRotation.Global) ? Quaternion.identity : toMove.ExtractRotation();
            Vector3 newPosition = Handles.PositionHandle(toMove.ExtractPosition(), rotation);

            if (newPosition != toMove.ExtractPosition())
            {
                hasChanged = true;
                toMove = toMove.SetPosition(newPosition);
            }
        }

        public static Vector3 DoHandleMove(Vector3 toMove, Quaternion currentRotation, out bool hasChanged)
        {
            hasChanged = false;

            Quaternion rotation = (Tools.pivotRotation == PivotRotation.Global) ? Quaternion.identity : currentRotation;
            Vector3 newPosition = Handles.PositionHandle(toMove, rotation);
            if (newPosition != toMove)
            {
                hasChanged = true;
                toMove = newPosition;
            }
            return (toMove);
        }

        public static Quaternion DoHandleRotation(Transform toMove, bool record, out bool hasChanged)
        {
            hasChanged = false;
            if (record)
            {
                Undo.RecordObject(toMove.gameObject.transform, "handle Point rotation");
            }

            Quaternion newRotation = Handles.RotationHandle(toMove.rotation, toMove.position);
            if (newRotation != toMove.rotation)
            {
                hasChanged = true;
                toMove.rotation = newRotation;
                if (record)
                {
                    EditorUtility.SetDirty(toMove);
                }
            }
            return (newRotation);
        }

        public static void DoHandleRotation(ref Matrix4x4 toMove, out bool hasChanged)
        {
            hasChanged = false;
            Quaternion newRotation = Handles.RotationHandle(toMove.ExtractRotation(), toMove.ExtractPosition());
            if (newRotation != toMove.ExtractRotation())
            {
                hasChanged = true;
                toMove = toMove.SetRositionOfTRS(newRotation);
            }
        }

        public static void DoHandleScale(Transform toMove, bool record, out bool hasChanged)
        {
            hasChanged = false;
            if (record)
            {
                Undo.RecordObject(toMove.gameObject.transform, "handle Point move");
            }

            Quaternion rotation = (Tools.pivotRotation == PivotRotation.Global) ? Quaternion.identity : toMove.rotation;
            Vector3 newScale = Handles.ScaleHandle(toMove.localScale, toMove.position, rotation, HandleUtility.GetHandleSize(toMove.position));
            if (newScale != toMove.localScale)
            {
                hasChanged = true;
                toMove.localScale = newScale;
                if (record)
                {
                    EditorUtility.SetDirty(toMove);
                }
            }
        }
        public static void DoHandleScale(ref Matrix4x4 toMove, out bool hasChanged)
        {
            hasChanged = false;

            Quaternion rotation = (Tools.pivotRotation == PivotRotation.Global) ? Quaternion.identity : toMove.ExtractRotation();
            Vector3 newScale = Handles.ScaleHandle(toMove.ExtractScale(), toMove.ExtractPosition(), rotation, HandleUtility.GetHandleSize(toMove.ExtractPosition()));
            if (newScale != toMove.ExtractScale())
            {
                hasChanged = true;
                toMove = toMove.SetScaleOfTRS(newScale);
            }
        }

        public static void DoHandleRect(Transform toMove, bool record, out bool hasChanged)
        {
            hasChanged = false;
            if (record)
            {
                Undo.RecordObject(toMove.gameObject.transform, "handle Point move");
            }
        }

        public static void DoHandleTransform(Transform toMove, bool record, out bool hasChanged)
        {
            hasChanged = false;
            if (record)
            {
                Undo.RecordObject(toMove.gameObject.transform, "handle Point transform");
            }

            Vector3 position = toMove.position;
            Quaternion rotation = toMove.rotation;
            float scale = toMove.localScale.Minimum();

            Handles.TransformHandle(ref position, ref rotation, ref scale);

            if (position != toMove.position || rotation != toMove.rotation || scale != toMove.localScale.Minimum())
            {
                hasChanged = true;
                toMove.position = position;
                toMove.rotation = rotation;
                toMove.localScale = new Vector3(scale, scale, scale);
                if (record)
                {
                    EditorUtility.SetDirty(toMove);
                }
            }
        }

        public static void DrawSphereArrow(Color newColor, Vector3 position, float sizeRatio = 1f)
        {
            DrawSphereArrow(newColor, position, ExtSceneView.GetSceneViewCameraTransform().rotation, sizeRatio);
        }

        public static void DrawSphereArrow(Color newColor, Vector3 position, Quaternion rotation, float sizeRatio = 1f)
        {
            Color old = Handles.color;
            Handles.color = newColor;
            Handles.CircleHandleCap(0, position, rotation, HandleUtility.GetHandleSize(position) * sizeRatio, EventType.Repaint);
            Handles.color = old;
        }
    }
}