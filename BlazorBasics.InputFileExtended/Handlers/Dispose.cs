namespace BlazorBasics.InputFileExtended.Handlers;

public partial class InputFileHandler : IDisposable
{

    private bool disposedValue;
    /// <summary>
    /// Overwrite the dispose to clean the object
    /// </summary>
    /// <param name="disposing"></param>
    protected virtual void Dispose(bool disposing)
    {
        if(!disposedValue)
        {
            if(disposing)
            {
                UploadedImage?.Dispose();
                int c = UploadedFiles.Count;
                for(int i = 0; i < c; i++)
                {
                    UploadedFiles[i]?.FileStreamContent?.Dispose();
                }
            }

            disposedValue = true;
        }
    }

    /// <summary>
    /// Dispose action
    /// </summary>
    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

}
