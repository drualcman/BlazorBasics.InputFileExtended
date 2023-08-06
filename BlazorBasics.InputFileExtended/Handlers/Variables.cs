namespace BlazorBasics.InputFileExtended.Handlers;

public partial class InputFileHandler
{
    int MaxAllowedFiles;
    long MaxAllowedSize;

    /// <summary>
    /// All files uploaded
    /// </summary>
    internal List<FileUploadContent> UploadedFiles = new List<FileUploadContent>();

}
