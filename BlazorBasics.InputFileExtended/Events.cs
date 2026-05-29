namespace BlazorBasics.InputFileExtended;

public partial class InputFileComponent
{
    IJSObjectReference FileEventScriptsReference;

    #region Management
    /// <summary>
    /// Add the scripts for manage the file events
    /// </summary>
    /// <returns></returns>
    async Task LoadFileEventsScript()
    {
        try
        {
            FileEventScriptsReference = await GetJSObjectReference("file-events");
        }
        catch (Exception ex)
        {
            FileEventScriptsReference = null;
            Console.WriteLine(ex.Message);
        }
    }


    /// <summary>
    /// Remove file scripts
    /// </summary>
    /// <returns></returns>
    async Task UnLoadFileEventsScript()
    {
        try
        {
            if (FileEventScriptsReference is not null)
                await FileEventScriptsReference.DisposeAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    #endregion

    /// <summary>
    /// Force to open file dialog from code
    /// </summary>
    /// <returns></returns>
    public async Task OpenFileDialog()
    {
        Console.WriteLine("OpenFileDialog");
        await FileEventScriptsReference.InvokeVoidAsync("OpenDialog", InputFileId);
    }

    private async Task Files_OnUploaded(object sender, FilesUploadEventArgs e) =>
        await OnChange.InvokeAsync(e);

    private async Task Files_OnUploadFile(object sender, FileUploadEventArgs e)
    {
        if (Files.Count > 0)
            SelectionInfo = $"{Files.Count} {(Files.Count == 1 ? "file" : "files")}";
        else
            SelectionInfo = string.Empty;

        await InvokeAsync(StateHasChanged);

        if (OnAddFile.HasDelegate)
            await OnAddFile.InvokeAsync(e);
        if (Parameters.ButtonOptions.AutoUpload &&
            Parameters.ButtonOptions.OnSubmit is not null)
            await SendFile(e);
    }

    private async Task Files_OnUploadError(object sender, InputFileException e)
    {
        if (OnError.HasDelegate)
            await OnError.InvokeAsync(e);
        else
            ErrorMessages =
                $"{e.Message}" +
                $"{(e.ExceptionType == ExceptionType.MaxSize ? $" File size {e.FileMbBytes.ToString("N2")}Mb ({e.FileBytes} bytes) overflow maximum size is {e.MaxFileMbBytes.ToString("N2")}Mb ({e.MaxFileBytes} bytes). " : "")}" +
                $"{(e.ExceptionType == ExceptionType.MaxCount ? $" Max files selected {e.MaxFilesAllowed}. " : "")}";
        InputResetKey = Guid.NewGuid();
        await InvokeAsync(StateHasChanged);
    }

    private async Task Files_OnAPIError(object sender, InputFileException e)
    {
        APIErrorMessages = e.Message;
        await InvokeAsync(StateHasChanged);
    }
}
