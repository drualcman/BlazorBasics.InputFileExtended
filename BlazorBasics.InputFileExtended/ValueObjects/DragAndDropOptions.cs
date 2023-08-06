namespace BlazorBasics.InputFileExtended.ValueObjects;
/// <summary>
/// configuration for the drag and drop options
/// </summary>
public class DragAndDropOptions
{
    /// <summary>
    /// Enable is can drop files
    /// </summary>
    public bool CanDropFiles { get; set; } = false;

    /// <summary>
    /// Css when drop a file
    /// </summary>
    public string DropZoneCss { get; set; } = "drop-zone";

    /// <summary>
    /// Css when drop a file
    /// </summary>
    public string DroppingCss { get; set; } = "drop-zone-drag";
}
