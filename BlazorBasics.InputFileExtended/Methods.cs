namespace BlazorBasics.InputFileExtended;

public partial class InputFileComponent
{
    #region public
    /// <summary>
    /// Clean all the files
    /// </summary>
    public void Clean()
    {
        Files.Clean();
        SelectionInfo = string.Empty;
    }

    /// <summary>
    /// Expose method for execute Save when is inside of the form need to be validated.
    /// </summary>
    public async Task FormSave()
    {
        bool isValid = true;
        if (Parameters.ButtonOptions.OnBeforeSubmit is not null)
        {
            await Parameters.ButtonOptions.OnBeforeSubmit.Invoke(Files.GetFiles());
        }
        if (Model is null)
        {
            await SendFile();
        }
        else
        {
            isValid = Model.Validate();
            if (isValid)
            {
                await SendFile();
            }
            else
            {
                StringBuilder errors = new StringBuilder();
                foreach (string err in Model.GetValidationMessages())
                {
                    errors.Append(err);
                    errors.Append(", ");
                }
                errors.Remove(errors.Length - 2, 2);
                await OnError.InvokeAsync(new InputFileException(errors.ToString(), "Save"));
            }
        }
        if (Parameters.ButtonOptions.OnAfterSubmit is not null)
        {
            await Parameters.ButtonOptions.OnAfterSubmit.Invoke(isValid);
        }
    }
    #endregion

    #region private
    bool IsSaving = false;
    async Task Change(InputFileChangeEventArgs e)
    {
        CleanErrorMessages();
        await Files.UploadFile(e);
    }

    async Task SendFile()
    {
        IsSaving = true;
        await InvokeAsync(StateHasChanged);
        await Parameters.ButtonOptions.OnSubmit.Invoke(Files.GetFiles());
        if (Parameters.ButtonOptions.CleanOnSuccessUpload)
            Clean();
        IsSaving = false;
        await InvokeAsync(StateHasChanged);
    }

    private async Task<IJSObjectReference> GetJSObjectReference(string filename)
    {
        try
        {
            IJSObjectReference loadJavascrip = await JavaScript.InvokeAsync<IJSObjectReference>(
                "import", $"./{ContentHelper.ContentPath}/js/{filename}.js").AsTask();
            return loadJavascrip;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    private void RemoveFile(FileUploadContent file)
    {
        if (Files.Remove(file))
        {
            if (!Files.UploadedFiles.Any())
                SelectionInfo = string.Empty;
        }
        CleanErrorMessages();
    }

    private async Task Remove(FileUploadContent file)
    {
        FilesUploadEventArgs removed = new FilesUploadEventArgs
        {
            Action = EventAction.Removed,
            Files = new List<FileUploadContent>() { file },
            Count = 1,
            Size = file.Size
        };
        await OnChange.InvokeAsync(removed);
    }

    void CleanErrorMessages()
    {
        APIErrorMessages = string.Empty;
        ErrorMessages = string.Empty;
    }
    #endregion

}
