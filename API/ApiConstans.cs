namespace InstaBot.API
{
    public static class ApiConstans
    {
        public static readonly int MIN_LIKES_COUNT = 500;
        public static readonly int DELAY_TIME = 500;
        public static readonly int DELAY_TIME_MIN = 600;
        public static readonly int DELAY_TIME_MAX = 1100;
        public static readonly string UNKNOWN = "unknown";
        public static readonly string PHOTO_EXTENSION = "jpg";
        public static readonly int MAX_REQUEST_COUNT = 500;
        public static readonly string DOWNLOADS_FILE_NAME = "Downloads";
        public static readonly string FILE_EXTENSION = ".txt";

        public const int PHOTO_SEARCH_DEFAULT_PAGE = 1;
        public const int PHOTO_SEARCH_DEFAULT_PER_PAGE = 30;
        public const string PHOTO_EXTENSION_SEARCH_PATTERN = "*.jpg";
    }
}
