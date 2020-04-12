using JetBrains.Annotations;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace hedCommon.extension.editor.editorWindow
{
    /// <summary>
    ///     Base class for an <see cref="EditorWindow" /> that uses USS and UXML.
    /// </summary>
    public abstract class EditorWindowUXML : EditorWindow
    {
        #region Private

        private static readonly PropertyInfo IsDockedProperty =
            GetProperty(typeof(EditorWindow), "docked", BindingFlags.Instance | BindingFlags.NonPublic);

        private static readonly Vector2 SizeToContentMinDefault = new Vector2(100.0f, 100.0f);

        private Vector2 SizeToContentMin
        {
            get => GetVector2(SizeToContentMinDefault);
            set => SetVector2(Vector2.Min(value, SizeToContentMinDefault));
        }

        private static readonly Vector2 SizeToContentMaxDefault = new Vector2(9999.0f, 9999.0f);

        private Vector2 SizeToContentMax
        {
            get => GetVector2(SizeToContentMaxDefault);
            set => SetVector2(Vector2.Max(value, value));
        }

        private static PropertyInfo GetProperty(Type type, string name, BindingFlags bindingFlags)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));

            var property = type.GetProperty(name, bindingFlags);

            if (property == null)
                throw new ArgumentNullException(nameof(property));

            return property;
        }

        private string GetPropertyName([CallerMemberName] string propertyName = null)
        {
            return $"{GetType().Name}.{propertyName}";
        }

        private void SizeToContentUpdate()
        {
            var root = SizeToContentRoot;
            if (root == null)
            {
                Debug.LogError($"Cannot size to content, {nameof(SizeToContentRoot)} is null.");
                return;
            }

            const Position absolute = Position.Absolute;

            if (!SizeToContent)
            {
                minSize = SizeToContentMin;
                maxSize = SizeToContentMax;
                return;
            }

            if (root.resolvedStyle.position != absolute)
            {
                Debug.LogError($"Cannot size to content, '{root.name}' position must be '{absolute}'.");
                return;
            }

            SizeToContentMin = minSize;
            SizeToContentMax = maxSize;
            var size = new Vector2(root.resolvedStyle.width, root.resolvedStyle.height - (IsDocked ? 0.0f : 7.0f));
            minSize = size;
            maxSize = size;
        }

        #endregion

        #region Protected

        /// <summary>
        ///     Gets if this instance is docked.
        /// </summary>
        [JetBrains.Annotations.PublicAPI]
        protected bool IsDocked => (bool)IsDockedProperty.GetValue(this);

        /// <summary>
        ///     Gets or sets whether this instance resizes itself to fit its content.
        /// </summary>
        [PublicAPI]
        protected bool SizeToContent
        {
            get => GetBool(false);
            set
            {
                if (Equals(value, SizeToContent))
                    return;

                SetBool(value);
                SizeToContentUpdate();
            }
        }

        /// <summary>
        ///     Gets the root visual element this instance should resize itself to (see Remarks).
        /// </summary>
        /// <remarks>
        ///     Element <see cref="IResolvedStyle.position" /> must be <see cref="Position.Absolute" /> for resize to be enabled.
        /// </remarks>
        [CanBeNull]
        protected virtual VisualElement SizeToContentRoot { get; } = null;

        protected virtual void OnEnable()
        {
            var root = rootVisualElement;

            var script = MonoScript.FromScriptableObject(this);
            if (script == null)
                throw new ArgumentNullException(nameof(script));

            var path = AssetDatabase.GetAssetPath(script);
            path = "Assets/Tiny Life Saver tools/Tools/Extensions/Editor/EditorWindow/DecoratorResizableEditorWindow";
            var pathCss = Path.ChangeExtension(path, "uss");
            var pathXml = Path.ChangeExtension(path, "uxml");

            Debug.Log(pathXml);
            if (File.Exists(pathCss) == false)
                throw new FileNotFoundException("Couldn't find associated style sheet.", pathCss);

            if (File.Exists(pathXml) == false)
                throw new FileNotFoundException("Couldn't find associated visual tree asset.", pathXml);

            var css = AssetDatabase.LoadAssetAtPath<StyleSheet>(pathCss);
            var xml = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(pathXml);

            xml.CloneTree(root);

            root.styleSheets.Add(css);

            root.RegisterCallback<GeometryChangedEvent>(_ => SizeToContentUpdate());
        }

        protected virtual void OnDisable()
        {

        }

        [PublicAPI]
        protected bool GetBool(bool @default, [CallerMemberName] string propertyName = null)
        {
            return EditorPrefs.GetBool(GetPropertyName(propertyName), @default);
        }

        [PublicAPI]
        protected void SetBool(bool value, [CallerMemberName] string propertyName = null)
        {
            EditorPrefs.SetBool(GetPropertyName(propertyName), value);
        }

        [PublicAPI]
        protected Vector2 GetVector2(Vector2 @default, [CallerMemberName] string propertyName = null)
        {
            var n = GetPropertyName(propertyName);
            var x = EditorPrefs.GetFloat($"{n}.{nameof(@default.x)}", @default.x);
            var y = EditorPrefs.GetFloat($"{n}.{nameof(@default.y)}", @default.y);
            var v = new Vector2(x, y);
            return v;
        }

        [PublicAPI]
        protected void SetVector2(Vector2 value, [CallerMemberName] string propertyName = null)
        {
            var n = GetPropertyName(propertyName);
            EditorPrefs.SetFloat($"{n}.{nameof(value.x)}", value.x);
            EditorPrefs.SetFloat($"{n}.{nameof(value.y)}", value.y);
        }

        #endregion
        //end class
    }
}