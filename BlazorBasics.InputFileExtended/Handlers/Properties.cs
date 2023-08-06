namespace BlazorBasics.InputFileExtended.Handlers;

public partial class InputFileHandler
{
    /// <summary>
    /// Define the indexer to allow client code to use [] notation to access directly to the file. 
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public FileUploadContent this[int index] => UploadedFiles[index];

    /// <summary>
    /// Define the indexer to allow client code to use [] notation to access directly to the file by file name. 
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public FileUploadContent this[string fileName] => UploadedFiles.First(n => n.Name == fileName);

    /// <summary>
    /// Return first image from the dictionary
    /// </summary>
    public FileUploadContent First
    {
        get
        {
            int c = UploadedFiles.Count;
            if(c > 0)
            {
                return UploadedFiles[0];
            }
            else
            {
                if(OnUploadError is not null)
                {
                    OnUploadError(this, new InputFileException("No images found", "First"));
                }
                return null;
            }
        }
    }

    /// <summary>
    /// Return last image from the dictionary
    /// </summary>
    public FileUploadContent Last
    {
        get
        {
            int c = UploadedFiles.Count;
            if(c > 0)
            {
                return UploadedFiles[c - 1];
            }
            else
            {
                if(OnUploadError is not null)
                {
                    OnUploadError(this, new InputFileException("No images found", "Last"));
                }
                return null;
            }
        }
    }

    /// <summary>
    /// Return how many images have stored
    /// </summary>
    public int Count => UploadedFiles.Count;

    /// <summary>
    /// Return total file size uploaded
    /// </summary>
    public long Size => UploadedFiles.Sum(s => s.Size);

}
