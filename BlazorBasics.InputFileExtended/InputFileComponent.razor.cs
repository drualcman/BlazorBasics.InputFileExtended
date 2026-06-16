namespace BlazorBasics.InputFileExtended;

public partial class InputFileComponent
{
    /// <summary>
    /// Expose InputFileHandler to manage the files when the component have reference. Example to show all the images.
    /// </summary>
    public InputFileHandler Files { get; private set; }

    Guid InputResetKey = Guid.NewGuid();
    /// <summary>
    /// Know the Id assigned to the input file to use from some external CSS or JAVASCRIPT when has reference name
    /// </summary>
    public string InputFileId => InputResetKey.ToString();


    #region variables
    ElementReference InputFileReference;
    string APIErrorMessages;
    string ErrorMessages;
    string SelectionInfo;
    string InputFileTypes;
    bool SuccessLoad = true;
    string LabelWrapperCss = "input-file";
    int Rows => Files.Count;
    DotNetObjectReference<InputFileComponent> SelfReference;
    bool SelectionNotifierRegistered;
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
            await RegisterSelectionNotifierAsync();
        }
    }

    /// <summary>
    /// Hooks the native `change` event (via JS) so <see cref="OnSelected"/> is raised the
    /// instant files are picked — before Blazor's own change pipeline or any byte read.
    /// </summary>
    async Task RegisterSelectionNotifierAsync()
    {
        if (SelectionNotifierRegistered || !OnSelected.HasDelegate || FileEventScriptsReference is null)
            return;

        try
        {
            SelfReference ??= DotNetObjectReference.Create(this);
            await FileEventScriptsReference.InvokeVoidAsync("RegisterSelectionNotifier", InputFileReference, SelfReference);
            SelectionNotifierRegistered = true;
        }
        catch (Exception ex)
        {
            // Best-effort early notification; the in-Change fallback still raises OnSelected.
            Console.WriteLine(ex.Message);
        }
    }

    /// <summary>
    /// Invoked from JS the moment a selection happens, carrying metadata only (no bytes read).
    /// </summary>
    /// <param name="files">Basic metadata for the selected files.</param>
    [JSInvokable]
    public async Task NotifySelected(FileBasicInfo[] files)
    {
        if (!OnSelected.HasDelegate || files is null || files.Length == 0)
            return;

        List<FileUploadContent> metadata = new(files.Length);
        long totalSize = 0;
        foreach (FileBasicInfo file in files)
        {
            metadata.Add(new FileUploadContent
            {
                Name = file.Name,
                LastModified = file.LastModifiedDateTime,
                Size = file.Size,
                ContentType = file.Type
            });
            totalSize += file.Size;
        }

        await OnSelected.InvokeAsync(new FilesUploadEventArgs
        {
            Files = metadata,
            Count = metadata.Count,
            Size = totalSize,
            Action = EventAction.Change
        });
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