using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.IMGUI.Controls;
using SimpleCleaner.Core;
using SimpleCleaner.Util;
using JetBrains.Annotations;
using System.IO;

namespace SimpleCleaner.Editor
{
    public class CleanerEditor : EditorWindow
    {
        private TreeViewState treeViewState;
        private AssetTreeView assetTreeView;
        private List<string> unusedAssets = new List<string>();

		[MenuItem("Tools/Simple Asset Cleaner/Cleaner Editor")]
        private static void OpenWindow()
        {
            var window = GetWindow<CleanerEditor>("Simple Asset Cleaner");
            window.Show();
        }

        private void OnEnable()
        {
            if (treeViewState == null)
                treeViewState = new TreeViewState();

            assetTreeView = new AssetTreeView(treeViewState);
        }

        private void OnGUI()
        {

			EditorGUILayout.Space(20);
			EditorGUILayout.LabelField("Simple Cleaner", EditorUtil.GetH1LabelStyle());

			EditorGUILayout.Space(20);
			EditorUtil.GuiLine(3);
			EditorGUILayout.Space(20);


			if (GUILayout.Button("Find Unused Assets", GUILayout.Height(30)))
            {
                FindUnusedAssets();
            }

            if (unusedAssets.Count > 0)
            {
                EditorGUILayout.LabelField($"Found {unusedAssets.Count} unused assets:");
                Rect rect = GUILayoutUtility.GetRect(0, position.width, 0, position.height - 80);
                assetTreeView?.OnGUI(rect);

                if (GUILayout.Button("Delete Selected Assets", GUILayout.Height(30)))
                {
                    DeleteSelectedAssets();
                }
            }
            else
            {
                EditorGUILayout.LabelField("No unused assets found. Click 'Find Unused Assets' to scan.");
            }
        }

        private void FindUnusedAssets()
        {
            unusedAssets.Clear();

            string[] allAssets = AssetDatabase.GetAllAssetPaths();
            HashSet<string> referencedAssets = new HashSet<string>();

            // Find assets in "Selected Paths" only, excluding "Exceptional Paths"
            PathFilter.FilterPaths(ref allAssets);

            // Find dependencies
            foreach (var scene in EditorBuildSettings.scenes)
            {
                if (scene.enabled)
                {
                    string[] dependencies = AssetDatabase.GetDependencies(scene.path, true);
                    foreach (var dependency in dependencies)
                    {
                        referencedAssets.Add(dependency);
                    }
                }
            }

            long fileSize = 0;
            foreach (var asset in allAssets)
            {
                if (!asset.StartsWith("Assets/"))       continue;
                if (AssetDatabase.IsValidFolder(asset)) continue;

                if (!referencedAssets.Contains(asset))
                {
                    unusedAssets.Add(asset);
                    FileInfo info = new FileInfo(asset);
					fileSize += info.Length;
                }
            }

			Debug.Log($"Found {unusedAssets.Count} unused assets.");
			Debug.Log($"You can save {fileSize/1e6:f2}MB!");
			assetTreeView?.SetAssets(unusedAssets);
        }

        private void DeleteSelectedAssets()
        {
            var selectedAssets = assetTreeView.GetSelectedAssets();
            if (selectedAssets.Count == 0)
            {
                Debug.LogWarning("No assets selected for deletion.");
                return;
            }

            foreach (var asset in selectedAssets)
            {
                AssetDatabase.DeleteAsset(asset);
            }

            Debug.Log($"Deleted {selectedAssets.Count} unused assets.");
            unusedAssets.Clear();
            AssetDatabase.Refresh();
        }
    }

}