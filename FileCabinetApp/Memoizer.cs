using FileCabinetApp.CommandHandlers;

namespace FileCabinetApp
{
    public static class Memoizer
    {
        private static Dictionary<AppCommandRequest, object> memoizedResults = new Dictionary<AppCommandRequest, object>();

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

        public static void Clear()
        {
            memoizedResults = new Dictionary<AppCommandRequest, object>();
        }
    }
}
