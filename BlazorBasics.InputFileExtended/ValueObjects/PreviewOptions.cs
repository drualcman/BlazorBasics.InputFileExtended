namespace BlazorBasics.InputFileExtended.ValueObjects;
/// <summary>
/// Image options
/// </summary>
public class PreviewOptions
{
    /// <summary>
    /// Inicate if the file it's a image
    /// </summary>
    public bool IsImage { get; set; } = true;
    /// <summary>
    /// If IsImage = true this indicate if need to do a preview
    /// </summary>
    public bool ShowPreview { get; set; } = false;
    /// <summary>
    /// CSS class for the preview image wrapper. Default image
    /// </summary>
    public string PreviewWrapperCss { get; set; } = "image-container";
    /// <summary>
    /// CSS class for the image file preview
    /// </summary>
    public string ImagePreviewCss { get; set; } = "image";
}
