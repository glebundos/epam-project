using FileCabinetApp.CommandHandlers;

namespace FileCabinetApp
{
    /// <summary>
    /// Class for memoizing results of commands requests.
    /// </summary>
    public static class Memoizer
    {
        private static Dictionary<AppCommandRequest, object> memoizedResults = new Dictionary<AppCommandRequest, object>();

        /// <summary>
        /// Gets the last result of given request.
        /// </summary>
        /// <param name="request"> - command request.</param>
        /// <returns>Memoized result of given request.</returns>
        public static object? Remember(AppCommandRequest request)
        {
            if (memoizedResults.ContainsKey(request))
            {
                return memoizedResults[request];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Save result of new given request.
        /// </summary>
        /// <param name="request"> - command request.</param>
        /// <param name="result"> - result of command execution.</param>
        public static void Memoize(AppCommandRequest request, object result)
        {
            if (memoizedResults.ContainsKey(request))
            {
                memoizedResults[request] = result;
            }
            else
            {
                memoizedResults.Add(request, result);
            }
        }

        /// <summary>
        /// Clears all memoized results.
        /// </summary>
        public static void Clear()
        {
            memoizedResults = new Dictionary<AppCommandRequest, object>();
        }
    }
}
