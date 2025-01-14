using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SimpleCleaner.Core
{
    public static class ConfigLoader
    {
        /// <summary>
        /// Return ONLY 1 SO
        /// </summary>
        public static AssetPathConfig LoadScriptableObjects()
        {
            List<AssetPathConfig> scriptableObjects = new List<AssetPathConfig>();

			// type : AssetPathConfig
			string[] guids = AssetDatabase.FindAssets("t:AssetPathConfig", new[] { SimpleCleaner.Util.Constants.PATH_CONFIG });
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

            if (scriptableObjects.Count <= 0)
            {
                Debug.LogError("There is no configure SO!");
                return null;
            }
            return scriptableObjects[0];
        }
    }
}