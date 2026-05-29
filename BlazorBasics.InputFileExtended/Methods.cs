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
            await SendFiles();
        }
        else
        {
            isValid = Model.Validate();
            if (isValid)
            {
                await SendFiles();
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
                await OnError.InvokeAsync(new InputFileException(errors.ToString(), nameof(FormSave)));
                InputResetKey = Guid.NewGuid();
                await InvokeAsync(StateHasChanged);
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
        if (OnError.HasDelegate && e.FileCount > Parameters.MaxUploatedFiles)
        {
            await OnError.InvokeAsync(new InputFileException(e, Parameters.MaxUploatedFiles, Parameters.MaxUploatedFiles, "Max selected file count exception.", nameof(e)));
            InputResetKey = Guid.NewGuid();
        }
        else
        {
            if (Parameters.EnableStreaming)
            {
                var filesJson = await FileEventScriptsReference.InvokeAsync<JsonElement>("GetFileDetails", InputFileId);
                FileBasicInfo[] files = DeserializeFiles(filesJson);
                if (files is not null && files.Length > 0)
                {
                    List<FileUploadContent> uploadedFiles = new();
                    long size = 0;
                    foreach (var file in files)
                    {
                        var toAdd = new FileUploadContent()
                        {
                            Name = file.Name,
                            LastModified = file.LastModifiedDateTime,
                            Size = file.Size,
                            ContentType = file.Type
                        };
                        uploadedFiles.Add(toAdd);
                        size += file.Size;
                        Files.Add(toAdd);
                    }
                    SelectionInfo = $"Files: {Files.Count}, Total size: {size}";
                    if (OnChange.HasDelegate)
                        await OnChange.InvokeAsync(new FilesUploadEventArgs { Files = uploadedFiles, Count = files.Length, Size = size, Action = EventAction.Added });
                }
            }
            else
                await Files.UploadFile(e);
        }
    }

    private FileBasicInfo[] DeserializeFiles(JsonElement jsonElement)
    {
        FileBasicInfo[] result = Array.Empty<FileBasicInfo>();

        if (jsonElement.ValueKind == JsonValueKind.Array)
        {
            string json = jsonElement.GetRawText();

            FileBasicInfo[]? deserialized =
                JsonSerializer.Deserialize<FileBasicInfo[]>(
                    json,
                    SERIALIZE_OPTIONS);

            if (deserialized != null)
            {
                result = deserialized;
            }
        }

        return result;
    }

    async Task SendFile(FileUploadEventArgs file)
    {
        IsSaving = true;
        await Task.Delay(1);
        if (Parameters.ButtonOptions.OnSubmit is not null)
            await Parameters.ButtonOptions.OnSubmit.Invoke([file.File]);
        IsSaving = false;
        await InvokeAsync(StateHasChanged);
    }

    async Task SendFiles()
    {
        IsSaving = true;
        await Task.Delay(1);
        if (Parameters.ButtonOptions.OnSubmit is not null)
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
