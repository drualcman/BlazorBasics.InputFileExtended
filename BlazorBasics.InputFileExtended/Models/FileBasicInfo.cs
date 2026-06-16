namespace BlazorBasics.InputFileExtended.Models;

/// <summary>
/// Modelo para información básica del archivo (solo metadatos)
/// </summary>
public class FileBasicInfo
{
    /// <summary>
    /// The name of the file as specified by the browser.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The size of the file in bytes as specified by the browser.
    /// </summary>
    public long Size { get; set; }

    /// <summary>
    /// The MIME type of the file as specified by the browser.
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// The last modified date
    /// </summary>
    public long LastModified { get; set; }

    /// <summary>
    /// Convert LastModified to DateTimeOffset
    /// </summary>
    public DateTimeOffset LastModifiedDateTime
    {
        get => DateTimeOffset.FromUnixTimeMilliseconds(LastModified);
    }
}
