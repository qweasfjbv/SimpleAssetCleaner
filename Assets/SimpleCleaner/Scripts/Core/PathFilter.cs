using System.Collections.Generic;
using System.Linq;

namespace SimpleCleaner.Core
{
	public static class PathFilter
	{
		/// <summary>
		/// Get paths and
		///  filter according to Include/Exclude paths and extentions.
		/// </summary>
		public static void FilterPaths(ref string[] paths)
		{
			/** Filter Paths  **/

			AssetPathConfig pathConfig = ConfigLoader.LoadAssetPathSO();

			// Filter - Include paths
			List<string> filteredPaths = paths
				.Where(path => pathConfig.includePaths.Any(include => path.StartsWith(include, System.StringComparison.OrdinalIgnoreCase)))
				.ToList();

			// Filter - Exclude paths
			filteredPaths.RemoveAll(path => pathConfig.excludePaths.Any(exclude => path.StartsWith(exclude, System.StringComparison.OrdinalIgnoreCase)));

			/** Filter Extentions **/

			// Filter - Exclude Extentions
			filteredPaths.RemoveAll(path => pathConfig.excludeExtention.Any(exclude => path.EndsWith(exclude, System.StringComparison.OrdinalIgnoreCase)));

			paths = filteredPaths.ToArray();
			return;
		}
	}
}