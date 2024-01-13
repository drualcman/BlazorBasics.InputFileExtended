namespace BlazorBasics.InputFileExtended;
/// <summary>
/// Share logic for FileView components
/// </summary>
public abstract class FileViewBase : ComponentBase
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
    /// Fire the event to remove the file
    /// </summary>
    [Parameter] public EventCallback OnRemove { get; set; }     
    /// <summary>
    /// Aditional HTML attributes
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object> Attributes { get; set; }  

    /// <summary>
    /// Compoent params check
    /// </summary>
    protected override void OnParametersSet() 
    {
        if(Options is null)
            Options = new PreviewOptions();        
    }
    
    /// <summary>
    /// Remove selected item and notify the parent
    /// </summary>
    /// <returns></returns>
    protected async Task Remove()
    {
        await OnRemove.InvokeAsync();
        GlobalEvents.OnItemDeleted(File);
    }
}
