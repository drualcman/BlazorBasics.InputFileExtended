namespace BlazorBasics.InputFileExtended.Handlers;

/// <summary>
/// Manage upload files
/// </summary>
public partial class InputFileHandler
{
    /// <summary>
    /// Can upload files
    /// </summary>
    /// <param name="maxFiles">Maximum files allowed to upload</param>
    /// <param name="maxSize">Maximum file size to upload</param>
    public InputFileHandler(int maxFiles = 5, long maxSize = 512000)
    {
        MaxAllowedFiles = maxFiles;
        MaxAllowedSize = maxSize;
    }
}
