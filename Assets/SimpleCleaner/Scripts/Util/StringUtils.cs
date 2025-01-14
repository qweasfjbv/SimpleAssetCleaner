
namespace SimpleCleaner.Util
{
	public static class StringUtils
	{
		public static string PreprocessPath(string absolutePath)
		{
			// start with "Assets/"
			string relativePath = "";
			string[] folders = absolutePath.Split('/');
			bool isInAssets = false;

			foreach (var folder in folders)
			{
				if (folder == "Assets" || folder == "assets")
				{
					isInAssets = true;
				}

				if (isInAssets)
				{
					relativePath += folder + "/";
				}
			}

			if (relativePath == "")
			{
				return PreprocessPath(SimpleCleaner.Util.Constants.PATH_BASIC);
			}

			return relativePath;
		}
	}
}