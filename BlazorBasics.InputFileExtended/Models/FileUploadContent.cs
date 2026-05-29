namespace BlazorBasics.InputFileExtended.Models;

/// <summary>
/// Manage the file upload
/// </summary>
public class FileUploadContent : IDisposable
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
    /// Set a identifier for the file in the object
    /// </summary>
    public Guid FileId { get; private set; } = Guid.NewGuid();
    /// <summary>
    /// Image data URL ready to be used as an img src attribute
    /// </summary>
    public string ImageDataUrl { get; private set; }
    /// <summary>
    /// Get the file bytes
    /// </summary>
    public byte[] FileBytes { get; private set; }

    internal void SetFileBytes(byte[] bytes)
    {
        if (bytes is not null && bytes.Length > 0)
        {
            string mimeType = !string.IsNullOrEmpty(ContentType) && ContentType.StartsWith("image/")
                ? ContentType
                : GetMimeTypeFromBytes(bytes); // Fallback por si ContentType no es confiable

            ImageDataUrl = $"data:{mimeType};base64,{Convert.ToBase64String(bytes)}";
            FileBytes = bytes;
        }
    }

    private string GetMimeTypeFromBytes(byte[] bytes)
    {
        if (bytes.Length > 4)
        {
            // JPEG: FF D8
            if (bytes[0] == 0xFF && bytes[1] == 0xD8)
                return "image/jpeg";

            // PNG: 89 50 4E 47
            if (bytes[0] == 0x89 && bytes[1] == 0x50 && bytes[2] == 0x4E && bytes[3] == 0x47)
                return "image/png";

            // GIF: GIF87a o GIF89a
            if (bytes[0] == 0x47 && bytes[1] == 0x49 && bytes[2] == 0x46)
                return "image/gif";

            // BMP: BM
            if (bytes[0] == 0x42 && bytes[1] == 0x4D)
                return "image/bmp";

            // WEBP: RIFF ???? WEBP
            if (bytes[0] == 0x52 && bytes[1] == 0x49 && bytes[2] == 0x46 && bytes[3] == 0x46)
                return "image/webp";
        }
        return "image/jpeg";
    }

    /// <summary>
    /// Clears the stored file byte array to release memory and remove the reference to its contents.
    /// </summary>
    /// <remarks>Only clears the FileBytes field; does not release unmanaged resources or suppress
    /// finalization. Safe to call multiple times.</remarks>
    public void Dispose()
    {
        FileBytes = [];
    }
}