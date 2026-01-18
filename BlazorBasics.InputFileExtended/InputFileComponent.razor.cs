namespace BlazorBasics.InputFileExtended;

public partial class InputFileComponent
{
    /// <summary>
    /// Expose InputFileHandler to manage the files when the component have reference. Example to show all the images.
    /// </summary>
    public InputFileHandler Files { get; private set; }

    /// <summary>
    /// Know the Id assigned to the input file to use from some external CSS or JAVASCRIPT when has reference name
    /// </summary>
    public readonly string InputFileId = Guid.NewGuid().ToString();


    #region variables 
    ElementReference InputFileReference;
    string APIErrorMessages;
    string ErrorMessages;
    string SelectionInfo;
    string InputFileTypes;
    bool SuccessLoad = true;
    string LabelWrapperCss = "input-file";
    int Rows => Files.Count;
    #endregion


    /// <summary>
    /// Initialize component file manager
    /// </summary>
    protected override void OnInitialized()
    {
        Files = new InputFileHandler();
        Files.OnUploaded += Files_OnUploaded;
        Files.OnUploadFile += Files_OnUploadFile;
        Files.OnUploadError += Files_OnUploadError;
        Files.OnAPIError += Files_OnAPIError;
        SelectionInfo = string.Empty;
        GlobalEvents.ItemDeleted += RemoveFile;
    }

    /// <summary>
    /// Load javascripts after objects are on the page
    /// </summary>
    /// <param name="firstRender"></param>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await LoadFileEventsScript();
            await LoadPasteScript();
            await LoadDragAdnDropScripts();
        }
    }

    async Task OnClick()
    {
        if (Parameters.OnShouldCancelClick is not null)
        {
            bool cancel = await Parameters.OnShouldCancelClick.Invoke();
            await FileEventScriptsReference.InvokeVoidAsync("PreventDefault", InputFileId, cancel);
        }
    }
}