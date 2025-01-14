using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SimpleCleaner.Core
{
	public static class PathFilter
	{
		/// <summary>
		/// Get paths and
		///  filter according to Include/Exclude paths
		/// </summary>
		public static void FilterPaths(ref string[] paths)
		{
			AssetPathConfig pathConfig = ConfigLoader.LoadScriptableObjects();

			// Filter - Include paths
			List<string> filteredPaths = paths
				.Where(path => pathConfig.includePaths.Any(include => path.StartsWith(include, System.StringComparison.OrdinalIgnoreCase)))
				.ToList();

			// Filter - Exclude paths
			filteredPaths.RemoveAll(path => pathConfig.excludePaths.Any(exclude => path.StartsWith(exclude, System.StringComparison.OrdinalIgnoreCase)));

			paths = filteredPaths.ToArray();
			return;
		}

	}
}