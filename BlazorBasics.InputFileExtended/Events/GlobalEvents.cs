namespace BlazorBasics.InputFileExtended.Events;
internal static class GlobalEvents
{
    public static event Action<FileUploadContent> ItemDeleted;

    public static void OnItemDeleted(FileUploadContent item)
    {
        ItemDeleted?.Invoke(item);
    }
}
