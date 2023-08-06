namespace BlazorBasics.InputFileExtended;
/// <summary>
/// How to show the selected files
/// </summary>
public partial class FileViewComponent
{
    /// <summary>
    /// File to show
    /// </summary>
    [Parameter] public FileUploadContent File { get; set; }
    /// <summary>
    /// Serup the preview
    /// </summary>
    [Parameter] public PreviewOptions Options { get; set; }  

    /// <summary>
    /// Button text
    /// </summary>
    [Parameter] public RenderFragment<FileUploadContent> ChildContent { get; set; }
    /// <summary>
    /// Fire teh evento to remove the file
    /// </summary>
    [Parameter] public EventCallback OnRemove { get; set; }

    /// <summary>
    /// Compoent params check
    /// </summary>
    protected override void OnParametersSet() 
    {
        if(Options is null)
            Options = new PreviewOptions();
    }
}
