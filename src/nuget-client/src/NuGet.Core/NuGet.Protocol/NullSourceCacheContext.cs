namespace NuGet.Protocol.Core.Types
{
    public class NullSourceCacheContext : SourceCacheContext
    {
        private static SourceCacheContext _instance;

        public static SourceCacheContext Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new NullSourceCacheContext();
                    _instance.DirectDownload = true;
                }

                return _instance;
            }
        }

        public override string GeneratedTempFolder
        {
            get
            {
                return string.Empty;
            }
        }

        public override SourceCacheContext WithRefreshCacheTrue() { return _instance; }

        public override SourceCacheContext Clone() { return _instance; }
    }
}
