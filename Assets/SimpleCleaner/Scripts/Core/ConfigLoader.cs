using SimpleCleaner.Util;
using System.Collections.Generic;
using UnityEditor;

namespace SimpleCleaner.Core
{
    public static class ConfigLoader
    {
        public static List<AssetPathConfig> LoadScriptableObjects()
        {
            List<AssetPathConfig> scriptableObjects = new List<AssetPathConfig>();

			// type : AssetPathConfig
			string[] guids = AssetDatabase.FindAssets("t:AssetPathConfig", new[] { Constants.PATH_CONFIG });
            scriptableObjects.Clear();

            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                AssetPathConfig asset = AssetDatabase.LoadAssetAtPath<AssetPathConfig>(assetPath);
                if (asset != null)
                {
                    scriptableObjects.Add(asset);
                }
            }

            return scriptableObjects;
        }

    }
}