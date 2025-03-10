﻿using hedCommon.extension.runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUILayout;

namespace extUnityComponents.collider
{
    public class FitCapsule
    {
        private CapsuleCollider[] _currentTargets = null;
        private DecoratorComponentsEditor _currentEditor;
        private MeshFilter[] _meshFilter;

        public void Init(CapsuleCollider[] parent, DecoratorComponentsEditor current)
        {
            _currentTargets = parent;
            _currentEditor = current;

            SetupMeshFilter();
        }

        private void SetupMeshFilter()
        {
            _meshFilter = new MeshFilter[_currentTargets.Length];
            for (int i = 0; i < _currentTargets.Length; i++)
            {
                _meshFilter[i] = _currentTargets[i].GetComponent<MeshFilter>();
            }
        }

        public bool IsNullOrEmptyOrEmptyContent()
        {
            if (_meshFilter == null)
            {
                return (true);
            }
            if (_meshFilter.Length == 0)
            {
                return (true);
            }
            for (int i = 0; i < _meshFilter.Length; i++)
            {
                if (_meshFilter[i] == null)
                {
                    return (true);
                }
            }
            return (false);
        }

        public void DisplayFitMesh()
        {
            using (HorizontalScope horizontalScope = new HorizontalScope(EditorStyles.helpBox))
            {
                GUILayout.Label("Fit collider to the mesh:");

                if (!IsNullOrEmptyOrEmptyContent())
                {
                    if (GUILayout.Button("Fit"))
                    {
                        ApplyFitMeshs();
                    }
                }
                else
                {
                    EditorGUI.BeginDisabledGroup(true);
                    {
                        GUILayout.Button("No MeshFilter");
                    }
                    EditorGUI.EndDisabledGroup();
                }
            }
        }

        private void ApplyFitMeshs()
        {
            for (int k = 0; k < _currentTargets.Length; k++)
            {
                Undo.RecordObject(_currentTargets[k], "datas Capsule Collider");
                ExtColliders.AutoSizeColliders3d(_currentTargets[k].gameObject, _currentEditor.GetTargetIndex<CapsuleCollider>(k));
            }
        }
    }
}