namespace BlazorBasics.InputFileExtended.Models;


/// <summary>
/// Manage the file upload
/// </summary>
public class FileUploadContent
{
    /// <summary>
    /// The name of the file as specified by the browser.
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// The last modified date as specified by the browser.
    /// </summary>
    public DateTimeOffset LastModified { get; set; }
    /// <summary>
    /// The size of the file in bytes as specified by the browser.
    /// </summary>
    public long Size { get; set; }
    /// <summary>
    /// The MIME type of the file as specified by the browser.
    /// </summary>
    public string ContentType { get; set; }
    /// <summary>
    /// File bites
    /// </summary>
    public StreamContent FileStreamContent { get; set; }
    /// <summary>
    /// Set a identifier for the file in the object
    /// </summary>
    public Guid FileId { get; private set; } = Guid.NewGuid();
    /// <summary>
    /// Get the image to show in HTML pages directly
    /// </summary>
    /// <returns></returns>
    public string ToImageHTML { get; private set; }
    /// <summary>
    /// Get the file bytes
    /// </summary>
    public byte[] FileBytes { get; private set; }
    internal void SetFileBytes(byte[] bytes)
    {
        ToImageHTML = $"data:image;base64,{Convert.ToBase64String(bytes)}";
        FileBytes = bytes;
    }
}
