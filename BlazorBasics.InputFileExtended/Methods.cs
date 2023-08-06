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
        FileBytes = null;
        SelectionInfo = string.Empty;
    }

    /// <summary>
    /// Expose method for execute Save when is inside of the form need to be validated.
    /// </summary>
    public async Task FormSave()
    {
        if(Model is null)
        {
            await SendFile();
        }
        else if(Model.Validate())
        {
            await SendFile();
        }
        else
        {
            StringBuilder errors = new StringBuilder();
            foreach(string err in Model.GetValidationMessages())
            {
                errors.Append(err);
                errors.Append(", ");
            }
            errors.Remove(errors.Length - 2, 2);
            await OnError.InvokeAsync(new InputFileException(errors.ToString(), "Save"));
        }
    }
    #endregion

    #region private
    async Task Change(InputFileChangeEventArgs e)
    {
        ErrorMessages = string.Empty;
        await Files.UploadFile(e);
    }

    async Task SendFile()
    {
        if(Parameters.ButtonOptions.OnSubmit.Invoke(Files.GetFiles()))
        {
           if(Parameters.ButtonOptions.CleanOnSuccessUpload)
                Clean();
        }
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
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }
    #endregion

}
