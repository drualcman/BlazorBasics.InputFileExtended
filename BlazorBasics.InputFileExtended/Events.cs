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
        if (Parameters.AllowPasteFiles)
        {

            try
            {
                FileEventScriptsReference = await GetJSObjectReference("file-dialog");
            }
            catch (Exception ex)
            {
                FileEventScriptsReference = null;
                Console.WriteLine(ex.Message);
            }
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
            await FileEventScriptsReference.DisposeAsync();
        }
        catch { }
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

    private void Files_OnUploaded(object sender, FilesUploadEventArgs e) =>
        OnChange.InvokeAsync(e);

    private async void Files_OnUploadFile(object sender, FileUploadEventArgs e)
    {
        FileBytes = e.File.FileBytes;
        if (SelectContent is null)
        {
            if (Files.Count > 0) SelectionInfo = $"{Files.Count} files";
            else SelectionInfo = string.Empty;
        }
        else
        {
            SelectionInfo = $"{Files.Count}";
        }
        await InvokeAsync(StateHasChanged);
        if (OnAddFile.HasDelegate)
            await OnAddFile.InvokeAsync(e);
        if (Parameters.ButtonOptions.AutoUpload &&
           Parameters.ButtonOptions.OnSubmit is not null)
            await SendFile();        //send the file after upload
    }

    private void Files_OnUploadError(object sender, InputFileException e)
    {
        if (OnError.HasDelegate) OnError.InvokeAsync(e);
        else ErrorMessages =
                $"{e.Message}" +
                $"{(e.ExceptionType == ExceptionType.MaxSize ? $" File size {e.FileMbBytes.ToString("N2")}Mb ({e.FileBytes} bytes) overflow maximum size is {e.MaxFileMbBytes.ToString("N2")}Mb ({e.MaxFileBytes} bytes). " : "")}" +
                $"{(e.ExceptionType == ExceptionType.MaxCount ? $" Max files selected {e.MaxFilesAllowed}. " : "")}";
    }

    private void Files_OnAPIError(object sender, InputFileException e)
    {
        APIErrorMessages = e.Message;
        InvokeAsync(StateHasChanged);
    }
}
