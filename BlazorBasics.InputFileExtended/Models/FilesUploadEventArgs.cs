namespace BlazorBasics.InputFileExtended.Models;

/// <summary>
/// Return all files uploaded
/// </summary>
public class FilesUploadEventArgs : EventArgs
{
    /// <summary>
    /// Files uploaded
    /// </summary>
    public List<FileUploadContent> Files { get; set; }
    /// <summary>
    /// Total size of all the files uploated
    /// </summary>
    public long Size { get; set; }
    /// <summary>
    /// Number of the files uploated
    /// </summary>
    public int Count { get; set; }
    /// <summary>
    /// Action used
    /// </summary>
    public EventAction Action { get; set; }
}
