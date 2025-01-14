using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SimpleCleaner.Editor
{
	public class PathConfigEditor : EditorWindow
    {
		private List<string> includePaths = new List<string>();
		private List<string> excludePaths = new List<string>();
		
		private AssetPathConfig assetConfig;

		private Vector2 includeScrollPos;
		private Vector2 excludeScrollPos;

		private GUIStyle borderStyle;

		[MenuItem("Tools/Simple Asset Cleaner/Path Config Editor")]
		public static void ShowWindow()
		{
			var window = GetWindow<PathConfigEditor>("Path Config Setter");
			window.maxSize = new Vector2(1200, 600);
			window.minSize = new Vector2(600, 600);
		}

		private void OnEnable()
		{
			assetConfig = SimpleCleaner.Core.ConfigLoader.LoadScriptableObjects();

			includePaths.Clear();
			excludePaths.Clear();

			includePaths.AddRange(assetConfig.includePaths);
			excludePaths.AddRange(assetConfig.excludePaths);
		}

		private string tmpString;
		private void OnGUI()
		{

			borderStyle = new GUIStyle(GUI.skin.box)
			{
				margin = new RectOffset(20, 20, 0, 20), // 외부 여백
				padding = new RectOffset(5, 5, 5, 5),   // 내부 여백
			};


			GUIStyle labelStyle = new GUIStyle(EditorStyles.boldLabel);
			labelStyle.alignment = TextAnchor.MiddleCenter;

			EditorGUILayout.Space(20);
			EditorGUILayout.LabelField("Cleaner Path Config", SimpleCleaner.Util.EditorUtil.GetH1LabelStyle());

			EditorGUILayout.Space(20);
			SimpleCleaner.Util.EditorUtil.GuiLine(3);
			EditorGUILayout.Space(20);

			// Include Paths Section
			EditorGUILayout.BeginHorizontal();

			EditorGUIUtility.labelWidth = 15f;
			EditorGUILayout.LabelField("Include Paths", labelStyle);

			EditorGUILayout.BeginVertical(borderStyle);
			labelStyle.alignment = TextAnchor.MiddleLeft;
			includeScrollPos = EditorGUILayout.BeginScrollView(includeScrollPos, GUILayout.Height(200));
			for (int i = 0; i < includePaths.Count; i++)
			{
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField(includePaths[i], labelStyle);
				if (GUILayout.Button("X", GUILayout.Width(20)))
				{
					includePaths.RemoveAt(i);
					i--; // Adjust index after removal
					SaveSettingPaths();
				}
				EditorGUILayout.EndHorizontal();
			}
			EditorGUILayout.EndScrollView();
			EditorGUILayout.EndVertical();

			if (GUILayout.Button("Add Include path"))
			{
				tmpString = EditorUtility.OpenFolderPanel("Include path", SimpleCleaner.Util.Constants.PATH_BASIC, "");
				if (tmpString != "")
				{
					tmpString = SimpleCleaner.Util.StringUtils.PreprocessPath(tmpString);
					if (!includePaths.Contains(tmpString))
						includePaths.Add(tmpString);

					SaveSettingPaths();
				}

			}

			EditorGUILayout.EndHorizontal();

			EditorGUILayout.Space(20);
			SimpleCleaner.Util.EditorUtil.GuiLine(3);
			EditorGUILayout.Space(20);

			labelStyle.alignment = TextAnchor.MiddleCenter;
			// Exclude Paths Section
			EditorGUILayout.BeginHorizontal();

			EditorGUIUtility.labelWidth = 15f;
			EditorGUILayout.LabelField("Exclude Paths", labelStyle);

			EditorGUILayout.BeginVertical(borderStyle);
			labelStyle.alignment = TextAnchor.MiddleLeft;
			excludeScrollPos = EditorGUILayout.BeginScrollView(excludeScrollPos, GUILayout.Height(200));
			for (int i = 0; i < excludePaths.Count; i++)
			{
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField(excludePaths[i], labelStyle);
				if (GUILayout.Button("X", GUILayout.Width(20)))
				{
					excludePaths.RemoveAt(i);
					i--;
					SaveSettingPaths();
				}
				EditorGUILayout.EndHorizontal();
			}
			EditorGUILayout.EndScrollView();
			EditorGUILayout.EndVertical();

			if (GUILayout.Button("Add Exclude path"))
			{
				tmpString = EditorUtility.OpenFolderPanel("Exclude path", "", "");
				if (tmpString == "") tmpString = SimpleCleaner.Util.Constants.PATH_BASIC;
				tmpString = SimpleCleaner.Util.StringUtils.PreprocessPath(tmpString);

				if (!excludePaths.Contains(tmpString))
					excludePaths.Add(tmpString);

				SaveSettingPaths();
			}

			EditorGUILayout.EndHorizontal();
		}

		private void SaveSettingPaths()
		{
			if (assetConfig == null)
			{
				Debug.LogError("There is NO Config SO!");
				return;
			}

			assetConfig.includePaths = includePaths;
			assetConfig.excludePaths = excludePaths;

			EditorUtility.SetDirty(assetConfig);
			AssetDatabase.SaveAssets();
		}
	}
}