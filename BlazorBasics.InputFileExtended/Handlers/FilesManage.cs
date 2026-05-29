namespace BlazorBasics.InputFileExtended.Handlers;

public partial class InputFileHandler
{

    /// <summary>
    /// Enumerator to get all the files
    /// </summary>
    /// <returns></returns>
    public IEnumerator<FileUploadContent> GetEnumerator()
    {
        foreach (FileUploadContent item in UploadedFiles)
            yield return item;
    }

    /// <summary>
    /// Use with InputFile OnChange
    /// </summary>
    /// <param name="e">InputFileChangeEventArgs</param>
    public async Task UploadFile(InputFileChangeEventArgs e)
    {
        try
        {
            if (e.FileCount == 0)
            {
                UploadedImage = null;
                FileName = null;
                throw new InputFileException("No images found", nameof(e));
            }
            else
            {
                if (this.MaxAllowedFiles == 1)          //if only allowed 1 file always reset the dictionary
                    UploadedFiles = new List<FileUploadContent>();

                if (e.FileCount > this.MaxAllowedFiles)
                {
                    throw new InputFileException(e, this.MaxAllowedSize, this.MaxAllowedFiles, "Max file count exception.", nameof(e));
                }
                else if (this.Count > this.MaxAllowedFiles)
                {
                    throw new InputFileException(e, this.MaxAllowedSize, this.MaxAllowedFiles, "Max selected file count exception.", nameof(e));
                }
                else
                {
                    long size = 0;
                    var files = e.GetMultipleFiles(maximumFileCount: MaxAllowedFiles);
                    foreach (IBrowserFile file in files)
                    {
                        size += file.Size;
                        if (file.Size > this.MaxAllowedSize)
                            throw new InputFileException(e, this.MaxAllowedSize, this.MaxAllowedFiles, $"{file.Name}.", nameof(e));
                        if (size > this.MaxAllowedSize)
                            throw new InputFileException(e, this.MaxAllowedSize, this.MaxAllowedFiles, $"{file.Name} overflow.", nameof(e));
                    }
                    foreach (IBrowserFile file in files)
                    {
                        await using Stream stream =
                            file.OpenReadStream(maxAllowedSize: MaxAllowedSize);

                        using MemoryStream memoryStream = new MemoryStream();

                        await stream.CopyToAsync(memoryStream);

                        byte[] fileBytes = memoryStream.ToArray();

                        FileUploadContent toAdd = new()
                        {
                            Name = file.Name,
                            LastModified = file.LastModified,
                            Size = file.Size,
                            ContentType = file.ContentType,
                        };

                        toAdd.SetFileBytes(fileBytes);

                        Add(toAdd);
                    }
                }
            }
        }
        catch (InputFileException internalException)
        {
            ResetState();
            if (OnUploadError is not null)
            {
                await OnUploadError(this, internalException);
            }
        }
        catch (EndOfStreamException stream)
        {
            ResetState();
            if (OnUploadError is not null)
            {
                await OnUploadError(this, new InputFileException(e, this.MaxAllowedSize, this.MaxAllowedFiles, $"EndOfStreamException: {stream.Message}", nameof(e), stream));
            }
        }
        catch (FileLoadException load)
        {
            ResetState();
            if (OnUploadError is not null)
            {
                await OnUploadError(this, new InputFileException(e, this.MaxAllowedSize, this.MaxAllowedFiles, $"FileLoadException: {load.Message}", nameof(e), load));
            }
        }
        catch (IOException ioex)
        {
            ResetState();
            if (OnUploadError is not null)
            {
                await OnUploadError(this, new InputFileException(e, this.MaxAllowedSize, this.MaxAllowedFiles, $"IOException: ", nameof(e), ioex));
            }
        }
        catch (Exception ex)
        {
            ResetState();
            if (OnUploadError is not null)
            {
                await OnUploadError(this, new InputFileException(e, this.MaxAllowedSize, this.MaxAllowedFiles, $"Exception: {ex.Message}", nameof(e), ex));
            }
        }
    }

    private void ResetState()
    {
        // Limpiar estado en caso de error
        UploadedImage = null;
        FileName = null;
    }

    #region object CRUD
    /// <summary>
    /// Add a image
    /// </summary>
    /// <param name="image"></param>
    public void Add(FileUploadContent image)
    {
        try
        {
            if (image.Size < this.MaxAllowedSize)
            {
                if (this.MaxAllowedFiles == 1)          //if only allowed 1 file always reset the list
                    UploadedFiles = new List<FileUploadContent>();

                if (this.Count < this.MaxAllowedFiles)
                {
                    //last image added is the default image to send
                    UploadedImage = image.FileBytes;
                    FileName = image.Name;
                    UploadedFiles.Add(image);
                    if (OnUploadFile is not null)
                    {
                        OnUploadFile(this, new FileUploadEventArgs { File = image, FileId = image.FileId, Action = EventAction.Added });
                    }
                }
                else
                {
                    throw new InputFileException(image, this.MaxAllowedSize, this.MaxAllowedFiles, $"Max files exception", nameof(image));
                }
            }
            else
            {
                throw new InputFileException(image, this.MaxAllowedSize, this.MaxAllowedFiles, $"{image.Name}.", nameof(image));
            }
        }
        catch (InputFileException internalException)
        {
            if (OnUploadError is not null)
            {
                OnUploadError(this, internalException);
            }
        }
        catch (Exception ex)
        {
            if (OnUploadError is not null)
            {
                OnUploadError(this, new InputFileException(image, this.MaxAllowedSize, this.MaxAllowedFiles, $"Exception: {ex.Message}", nameof(image), ex));
            }
        }
    }

    /// <summary>
    /// Update image by index
    /// </summary>
    /// <param name="index"></param>
    /// <param name="image"></param>
    public bool Update(int index, FileUploadContent image)
    {
        bool result = false;
        try
        {
            UploadedFiles[index] = image;
            result = true;
            if (OnUploadFile is not null)
            {
                OnUploadFile(this, new FileUploadEventArgs { File = image, FileId = image.FileId, Action = EventAction.Updated });
            }
        }
        catch (IndexOutOfRangeException ix)
        {
            if (OnUploadError is not null)
            {
                OnUploadError(this, new InputFileException(image, this.MaxAllowedSize, this.MaxAllowedFiles, $"File index {index} not found", nameof(image), ix));
            }
        }
        catch (Exception ex)
        {
            if (OnUploadError is not null)
            {
                OnUploadError(this, new InputFileException(image, this.MaxAllowedSize, this.MaxAllowedFiles, $"Exception: {ex.Message}", nameof(image), ex));
            }
        }
        return result;
    }

    /// <summary>
    /// Update image by file name
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="image"></param>
    public bool Update(string fileName, FileUploadContent image)
    {
        bool result = false;
        try
        {
            FileUploadContent file = UploadedFiles.First(i => i.Name == fileName);
            if (file is null)
                throw new InputFileException(image, this.MaxAllowedSize, this.MaxAllowedFiles, $"File {fileName} not found", nameof(image));
            result = Update(UploadedFiles.IndexOf(file), image);
        }
        catch (InputFileException internalException)
        {
            if (OnUploadError is not null)
            {
                OnUploadError(this, internalException);
            }
        }
        catch (Exception ex)
        {
            result = false;
            if (OnUploadError is not null)
            {
                OnUploadError(this, new InputFileException(image, this.MaxAllowedSize, this.MaxAllowedFiles, $"Exception: {ex.Message}", nameof(image), ex));
            }
        }
        return result;
    }

    /// <summary>
    /// Update image by file FileId
    /// </summary>
    /// <param name="id"></param>
    /// <param name="image"></param>
    public bool Update(Guid id, FileUploadContent image)
    {
        bool result = false;
        try
        {
            FileUploadContent file = UploadedFiles.First(i => i.FileId == id);
            if (file is null)
                throw new InputFileException(file, this.MaxAllowedSize, this.MaxAllowedFiles, $"File {id} not found", nameof(image));
            result = Update(UploadedFiles.IndexOf(file), image);
        }
        catch (InputFileException internalException)
        {
            if (OnUploadError is not null)
            {
                OnUploadError(this, internalException);
            }
        }
        catch (Exception ex)
        {
            result = false;
            if (OnUploadError is not null)
            {
                OnUploadError(this, new InputFileException(image, this.MaxAllowedSize, this.MaxAllowedFiles, $"Exception: {ex.Message}", nameof(image), ex));
            }
        }
        return result;
    }

    /// <summary>
    /// Remove image from index
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public bool Remove(int index)
    {
        bool result = false;
        try
        {
            result = Remove(UploadedFiles[index]);
        }
        catch (InputFileException internalException)
        {
            if (OnUploadError is not null)
            {
                OnUploadError(this, internalException);
            }
        }
        catch (Exception ex)
        {
            result = false;
            if (OnUploadError is not null)
            {
                OnUploadError(this, new InputFileException($"Exception: {ex.Message}", nameof(index), ex));
            }
        }
        return result;
    }

    /// <summary>
    /// Remove image
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    public bool Remove(FileUploadContent file)
    {
        bool result = false;
        try
        {
            result = UploadedFiles.Remove(file);
        }
        catch (IndexOutOfRangeException ix)
        {
            if (OnUploadError is not null)
            {
                OnUploadError(this, new InputFileException(file, this.MaxAllowedSize, this.MaxAllowedFiles, $"File {file.Name} not found", nameof(file), ix));
            }
        }
        catch (Exception ex)
        {
            if (OnUploadError is not null)
            {
                OnUploadError(this, new InputFileException(file, this.MaxAllowedSize, this.MaxAllowedFiles, $"File {file.Name} not found. Exception: {ex.Message}", nameof(file), ex));
            }
        }
        return result;
    }

    /// <summary>
    /// Remove image from file name
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool Remove(Guid id)
    {
        bool result = false;
        FileUploadContent file = UploadedFiles.First(i => i.FileId == id);
        if (file is null)
        {
            if (OnUploadError is not null)
            {
                OnUploadError(this, new InputFileException($"File not found {id}", nameof(id)));
            }
        }
        else
            result = Remove(file);
        return result;
    }

    /// <summary>
    /// Remove image from file name
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public bool Remove(string fileName)
    {
        bool result = false;
        FileUploadContent file = UploadedFiles.First(i => i.Name == fileName);
        if (file is null)
        {
            if (OnUploadError is not null)
            {
                OnUploadError(this, new InputFileException($"File {fileName} not found", nameof(fileName)));
            }
        }
        else
            result = Remove(file);
        return result;
    }
    #endregion

    /// <summary>
    /// Remove all the files into the object
    /// </summary>
    public void Clean()
    {
        UploadedFiles = [];
        UploadedImage = null;
        FileName = string.Empty;
    }
}
