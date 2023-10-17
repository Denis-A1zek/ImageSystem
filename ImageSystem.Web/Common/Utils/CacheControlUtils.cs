namespace ImageSystem.Web.Common.Utils;

public static class CacheControlUtils
{
    public static string MaxAge => (60 * 60 * 24 * 7).ToString();
}
