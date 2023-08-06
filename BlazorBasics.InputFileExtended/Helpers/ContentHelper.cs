namespace BlazorBasics.InputFileExtended.Helpers;
internal static class ContentHelper
{
    public static string ContentPath => $"_content/{typeof(ContentHelper).Assembly.GetName().Name}";
    public static string ReplaceSpaceWithPlus(this string text)
    {
        string result;
        if(string.IsNullOrEmpty(text)) result = text;
        else result = text.Replace(' ', '+');
        return result;
    }
}
