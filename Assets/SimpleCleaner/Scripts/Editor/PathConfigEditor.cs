using UnityEngine;
using UnityEditor;
using SimpleCleaner.Core;
using System.Collections.Generic;

namespace SimpleCleaner.Editor
{
    public class PathConfigEditor : EditorWindow
    {
		private List<string> includePaths = new List<string>();
		private List<string> excludePaths = new List<string>();
		
		private List<AssetPathConfig> assetConfigs;

		private Vector2 includeScrollPos;
		private Vector2 excludeScrollPos;

		[MenuItem("Window/SimpleCleaner/Path Config Setter")]
		public static void ShowWindow()
		{
			var window = GetWindow<PathConfigEditor>("Path Config Setter");
			window.maxSize = new Vector2(600, 600);
			window.minSize = window.maxSize;
		}

		private void OnEnable()
		{
			assetConfigs = ConfigLoader.LoadScriptableObjects();

			foreach (var assetConfig in assetConfigs)
			{
				includePaths.AddRange(assetConfig.includePaths);
				excludePaths.AddRange(assetConfig.excludePaths);
			}
		}

		private void OnGUI()
		{
			EditorGUILayout.LabelField("Environment Variable Editor", EditorStyles.boldLabel);
			EditorGUILayout.Space();


			// Include Paths Section
			EditorGUILayout.BeginHorizontal(GUILayout.Height(position.height / 2 - 10));
			EditorGUILayout.LabelField("Include Paths", EditorStyles.boldLabel);

			includeScrollPos = EditorGUILayout.BeginScrollView(includeScrollPos, GUILayout.Height(300));
			for (int i = 0; i < includePaths.Count; i++)
			{
				EditorGUILayout.BeginHorizontal();
				includePaths[i] = EditorGUILayout.TextField(includePaths[i]);

				if (GUILayout.Button("X", GUILayout.Width(20)))
				{
					includePaths.RemoveAt(i);
					i--; // Adjust index after removal
				}
				EditorGUILayout.EndHorizontal();
			}
			EditorGUILayout.EndScrollView();

			if (GUILayout.Button("Add Include Path"))
			{
				includePaths.Add(string.Empty);
			}
			EditorGUILayout.EndHorizontal();

			// Exclude Paths Section
			EditorGUILayout.BeginHorizontal(GUILayout.Height(position.height/ 2 - 10));
			EditorGUILayout.LabelField("Exclude Paths", EditorStyles.boldLabel);

			excludeScrollPos = EditorGUILayout.BeginScrollView(excludeScrollPos, GUILayout.Height(300));
			for (int i = 0; i < excludePaths.Count; i++)
			{
				EditorGUILayout.BeginHorizontal();
				excludePaths[i] = EditorGUILayout.TextField(excludePaths[i]);

				if (GUILayout.Button("X", GUILayout.Width(20)))
				{
					excludePaths.RemoveAt(i);
					i--; // Adjust index after removal
				}
				EditorGUILayout.EndHorizontal();
			}
			EditorGUILayout.EndScrollView();

			if (GUILayout.Button("Add Exclude Path"))
			{
				excludePaths.Add(string.Empty);
			}
			EditorGUILayout.EndHorizontal();


			EditorGUILayout.Space();

			if (GUILayout.Button("Save"))
			{
				SaveEnvironmentVariables();
			}
		}

		private void SaveEnvironmentVariables()
		{
			Debug.Log("Include Paths:");
			foreach (var path in includePaths)
			{
				Debug.Log(path);
			}

			Debug.Log("Exclude Paths:");
			foreach (var path in excludePaths)
			{
				Debug.Log(path);
			}
		}
	}
}