namespace BlazorBasics.InputFileExtended.Handlers;

public partial class InputFileHandler
{
    /// <summary>
    /// Set the max allowed files
    /// </summary>
    /// <param name="maxfile"></param>
    public void SetMaxFiles(int maxfile) => MaxAllowedFiles = maxfile;
    /// <summary>
    /// Set the max file size allowed
    /// </summary>
    /// <param name="maxSize"></param>
    public void SetMaxFileSize(long maxSize) => MaxAllowedSize = maxSize;

    internal IReadOnlyList<FileUploadContent> GetFiles() => UploadedFiles;
}
