namespace BlazorBasics.InputFileExtended.Models;

/// <summary>
/// Return file name and file stream per each file uploaded
/// </summary>
public class FileUploadEventArgs : EventArgs
{
    /// <summary>
    /// File uploaded with all the data
    /// </summary>
    public FileUploadContent File { get; set; }
    /// <summary>
    /// Index in the object
    /// </summary>
    public Guid FileId { get; set; }
    /// <summary>
    /// Action used
    /// </summary>
    public EventAction Action { get; set; }
}
