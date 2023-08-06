namespace BlazorBasics.InputFileExtended.Models;
/// <summary>
/// What happen in the event about the file
/// </summary>
public enum EventAction
{
    /// <summary>
    /// File Added to the object container
    /// </summary>
    Added,
    /// <summary>
    /// File updated from the object container
    /// </summary>
    Updated,
    /// <summary>
    /// File removed from the object container
    /// </summary>
    Removed,
    /// <summary>
    /// Object container clean. No files.
    /// </summary>
    Clean,
    /// <summary>
    /// File upload from to the post endpoint
    /// </summary>
    Upload
}
