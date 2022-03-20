using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Editor.OdinExtensions
{
    public class ScriptableObjectCreator : OdinMenuEditorWindow
    {
        private readonly HashSet<Type> _scriptableObjectTypes =
            new(AssemblyUtilities
                .GetTypes(AssemblyTypeFlags.CustomTypes)
                .Where(t =>
                    t.IsClass &&
                    typeof(ScriptableObject).IsAssignableFrom(t) &&
                    !typeof(EditorWindow).IsAssignableFrom(t) &&
                    !typeof(UnityEditor.Editor).IsAssignableFrom(t)));

        [MenuItem("Assets/Create Scriptable Object", priority = -1000)]
        private static void ShowDialog()
        {
            var path = "Assets";
            var obj = Selection.activeObject;
            if (obj && AssetDatabase.Contains(obj))
            {
                path = AssetDatabase.GetAssetPath(obj);
                if (!Directory.Exists(path))
                {
                    path = Path.GetDirectoryName(path);
                }
            }

            var window = CreateInstance<ScriptableObjectCreator>();
            window.ShowUtility();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 500);
            window.titleContent = new GUIContent(path);
            window._targetFolder = path.Trim('/');
        }

        private ScriptableObject _previewObject;
        private string _targetFolder;
        private Vector2 _scroll;

        private Type SelectedType
        {
            get
            {
                var m = MenuTree.Selection.LastOrDefault();
                return m?.Value as Type;
            }
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            MenuWidth = 270;
            WindowPadding = Vector4.zero;

            OdinMenuTree tree = new OdinMenuTree(false);
            tree.Config.DrawSearchToolbar = true;
            tree.DefaultMenuStyle = OdinMenuStyle.TreeViewStyle;
            tree.AddRange(_scriptableObjectTypes.Where(x => !x.IsAbstract), GetMenuPathForType).AddThumbnailIcons();
            tree.SortMenuItemsByName();
            tree.Selection.SelectionConfirmed += x => CreateAsset();
            tree.Selection.SelectionChanged += e =>
            {
                if (_previewObject && !AssetDatabase.Contains(_previewObject))
                {
                    DestroyImmediate(_previewObject);
                }

                if (e != SelectionChangedType.ItemAdded)
                {
                    return;
                }

                var t = SelectedType;
                if (t != null && !t.IsAbstract)
                {
                    _previewObject = CreateInstance(t);
                }
            };

            return tree;
        }

        private string GetMenuPathForType(Type t)
        {
            if (t != null && _scriptableObjectTypes.Contains(t))
            {
                var menuName = t.Name.Split('`').First().SplitPascalCase();
                return GetMenuPathForType(t.BaseType) + "/" + menuName;
            }

            return "";
        }

        protected override IEnumerable<object> GetTargets()
        {
            yield return _previewObject;
        }

        protected override void DrawEditor(int index)
        {
            _scroll = GUILayout.BeginScrollView(_scroll);
            {
                base.DrawEditor(index);
            }
            GUILayout.EndScrollView();

            if (!_previewObject) return;
            GUILayout.FlexibleSpace();
            SirenixEditorGUI.HorizontalLineSeparator(1);
            if (GUILayout.Button("Create Asset", GUILayoutOptions.Height(30)))
            {
                CreateAsset();
            }
        }

        private void CreateAsset()
        {
            if (!_previewObject) return;
            var dest = _targetFolder + "/new " + MenuTree.Selection.First().Name.ToLower() + ".asset";
            dest = AssetDatabase.GenerateUniqueAssetPath(dest);
            AssetDatabase.CreateAsset(_previewObject, dest);
            AssetDatabase.Refresh();
            Selection.activeObject = _previewObject;
            EditorApplication.delayCall += Close;
        }
    }
}