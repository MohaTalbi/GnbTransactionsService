namespace GnbTransactionsService.Utils
{
    public static class PathValidator
    {
        /// <summary>
        /// funcion to validate the file path and return the full path if valid, otherwise throw an exception
        /// </summary>
        /// <param name="contentRootPath"></param>
        /// <param name="relativePath"></param>
        /// <param name="logicalName"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        public static string GetRequiredFilePath(string contentRootPath,
                                                string? relativePath,
                                                string logicalName)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
            {
                throw new InvalidOperationException(
                    $"Configuration path for '{logicalName}' is missing.");
            }

            string fullPath = Path.Combine(contentRootPath, relativePath);

            if (!File.Exists(fullPath))
            {
                throw new FileNotFoundException(
                    $"Required file '{logicalName}' not found at: {fullPath}");
            }

            return fullPath;
        }
    }
}
