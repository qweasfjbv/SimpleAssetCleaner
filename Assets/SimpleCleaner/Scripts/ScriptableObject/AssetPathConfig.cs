using System.Collections.Generic;
using UnityEngine;

namespace SimpleCleaner
{
    [CreateAssetMenu(menuName = "Asset Path Configuration")]
    public class AssetPathConfig : ScriptableObject
    {
        public List<string> includePaths;
        public List<string> excludePaths;
        public List<string> excludeExtention;
    }
}