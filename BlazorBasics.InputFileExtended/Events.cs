namespace BlazorBasics.InputFileExtended;

public partial class InputFileComponent
{
    #region handlers
    private void Files_OnUploaded(object sender, FilesUploadEventArgs e) =>
        OnChange.InvokeAsync(e);

    private async void Files_OnUploadFile(object sender, FileUploadEventArgs e)
    {
        FileBytes = e.File.FileBytes;
        if(SelectContent is null)
        {
            if(Files.Count > 0) SelectionInfo = $"{Files.Count} files";
            else SelectionInfo = string.Empty;
        }
        else
        {
            SelectionInfo = $"{Files.Count}";
        }
        await InvokeAsync(StateHasChanged);
        if(OnAddFile.HasDelegate)
            await OnAddFile.InvokeAsync(e);
        if(Parameters.ButtonOptions.AutoUpload &&
           Parameters.ButtonOptions.OnSubmit is not null)
            await SendFile();        //send the file after upload
    }

    private void Files_OnUploadError(object sender, InputFileException e)
    {
        if(OnError.HasDelegate) OnError.InvokeAsync(e);
        else ErrorMessages =
                $"{e.Message}" +
                $"{(e.ExceptionType == ExceptionType.MaxSize ? $" File size {e.FileMbBytes.ToString("N2")}Mb ({e.FileBytes} bytes) overflow maximum size is {e.MaxFileMbBytes.ToString("N2")}Mb ({e.MaxFileBytes} bytes). " : "")}" +
                $"{(e.ExceptionType == ExceptionType.MaxCount ? $" Max files selected {e.MaxFilesAllowed}. " : "")}";
    }
    #endregion

}
