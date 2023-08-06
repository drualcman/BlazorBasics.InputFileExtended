namespace BlazorBasics.InputFileExtended.Exceptions
{
    /// <summary>
    /// Exeption types
    /// </summary>
    [Flags]
    public enum ExceptionType
    {
        /// <summary>
        /// Generic exception
        /// </summary>
        [Display(Name = "Generic")] Generic = 0,
        /// <summary>
        /// Max file count exception
        /// </summary>
        [Display(Name = "Maximum count")] MaxCount = 1,
        /// <summary>
        /// Max allowed size exception
        /// </summary>        
        [Display(Name = "Maximum size allowed")] MaxSize = 2
    }

    /// <summary>
    /// Return a personalize exception with all relative data
    /// </summary>
    public class InputFileException : ArgumentException
    {
        #region constructor
        /// <summary>
        ///  Initializes a new instance of the InputFileException class.
        /// </summary>
        public InputFileException() : base() { ExceptionType = ExceptionType.Generic; }

        /// <summary>
        ///     Initializes a new instance of the InputFileException class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public InputFileException(string message) : base(message) { ExceptionType = ExceptionType.Generic; }

        /// <summary>
        ///     Initializes a new instance of the InputFileException class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception. If the innerException parameter is not a null reference, the current exception is raised in a catch block that handles the inner exception.
        /// </param>
        public InputFileException(string message, Exception innerException) : base(message, innerException) { ExceptionType = ExceptionType.Generic; }

        /// <summary>
        ///     Initializes a new instance of the InputFileException class with a specified error message and the name of the parameter that causes this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="paramName">The name of the parameter that caused the current exception.</param>
        public InputFileException(string message, string paramName) : base(message, paramName) { ExceptionType = ExceptionType.Generic; }

        /// <summary>
        ///     Initializes a new instance of the InputFileException class with a specified error message, the parameter name, and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="paramName"> The name of the parameter that caused the current exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception. If the innerException parameter is not a null reference, the current exception is raised in a catch block that handles the inner exception.</param>
        public InputFileException(string message, string paramName, Exception innerException) : base(message, paramName, innerException) { ExceptionType = ExceptionType.Generic; }

        /// <summary>
        ///     Initialize a new instance of the InputFileException class with a InputFileChangeEventArgs
        /// </summary>
        /// <param name="inputFiles">Supplies information about an Microsoft.AspNetCore.Components.Forms.InputFile.OnChange event being raised.</param>
        /// <param name="maxFileBytes">How much is the maximum allowed size.</param>
        /// <param name="maxFileCount">How much is the max file count.</param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="paramName">The name of the parameter that caused the current exception.</param>
        public InputFileException(InputFileChangeEventArgs inputFiles, long maxFileBytes, int maxFileCount, string message, string paramName) :
            this(inputFiles, maxFileBytes, maxFileCount, message, paramName, null)
        { }

        /// <summary>
        ///     Initialize a new instance of the InputFileException class with a InputFileChangeEventArgs
        /// </summary>
        /// <param name="inputFiles">Supplies information about an Microsoft.AspNetCore.Components.Forms.InputFile.OnChange event being raised.</param>
        /// <param name="maxFileBytes">How much is the maximum allowed size.</param>
        /// <param name="maxFileCount">How much is the max file count.</param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="paramName">The name of the parameter that caused the current exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception. If the innerException parameter is not a null reference, the current exception is raised in a catch block that handles the inner exception.</param>
        public InputFileException(InputFileChangeEventArgs inputFiles, long maxFileBytes, int maxFileCount, string message, string paramName, Exception innerException) :
            base(message, paramName, innerException)
        {
            MaxFilesAllowed = maxFileCount;
            MaxFileBytes = maxFileBytes;
            FileBytes = 0;
            List<FileUploadContent> files = new List<FileUploadContent>();
            foreach(IBrowserFile file in inputFiles.GetMultipleFiles())
            {
                FileBytes += file.Size;
                files.Add(new FileUploadContent
                {
                    Name = file.Name,
                    LastModified = file.LastModified,
                    Size = file.Size,
                    ContentType = file.ContentType,
                    FileStreamContent = new StreamContent(file.OpenReadStream(file.Size))
                });
            }
            Files = files;
            if(FilesCount > MaxFileBytes && FileBytes > MaxFileBytes) ExceptionType = ExceptionType.MaxCount | ExceptionType.MaxSize;
            else if(FilesCount > MaxFileBytes) ExceptionType = ExceptionType.MaxCount;
            else if(FileBytes > MaxFileBytes) ExceptionType = ExceptionType.MaxSize;
            else ExceptionType = ExceptionType.Generic;
        }

        /// <summary>
        ///     Initialize a new instance of the InputFileException class with a InputFileChangeEventArgs
        /// </summary>
        /// <param name="inputFile">Supplies information about an Microsoft.AspNetCore.Components.Forms.InputFile.OnChange event being raised.</param>
        /// <param name="maxFileBytes">How much is the maximum allowed size.</param>
        /// <param name="maxFileCount">How much is the max file count.</param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="paramName">The name of the parameter that caused the current exception.</param>
        public InputFileException(FileUploadContent inputFile, long maxFileBytes, int maxFileCount, string message, string paramName) :
            this(inputFile, maxFileBytes, maxFileCount, message, paramName, null)
        { }

        /// <summary>
        ///     Initialize a new instance of the InputFileException class with a InputFileChangeEventArgs
        /// </summary>
        /// <param name="inputFile">Supplies information about an Microsoft.AspNetCore.Components.Forms.InputFile.OnChange event being raised.</param>
        /// <param name="maxFileBytes">How much is the maximum allowed size.</param>
        /// <param name="maxFileCount">How much is the max file count.</param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="paramName">The name of the parameter that caused the current exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception. If the innerException parameter is not a null reference, the current exception is raised in a catch block that handles the inner exception.</param>
        public InputFileException(FileUploadContent inputFile, long maxFileBytes, int maxFileCount, string message, string paramName, Exception innerException) :
            base(message, paramName, innerException)
        {
            MaxFilesAllowed = maxFileCount;
            MaxFileBytes = maxFileBytes;
            FileBytes = inputFile.Size;
            Files = new List<FileUploadContent> { inputFile };
            if(FilesCount > MaxFileBytes && FileBytes > MaxFileBytes) ExceptionType = ExceptionType.MaxCount | ExceptionType.MaxSize;
            else if(FilesCount > MaxFileBytes) ExceptionType = ExceptionType.MaxCount;
            else if(FileBytes > MaxFileBytes) ExceptionType = ExceptionType.MaxSize;
            else ExceptionType = ExceptionType.Generic;
        }
        #endregion

        #region properties
        /// <summary>
        /// Files affected in the exception
        /// </summary>
        public IEnumerable<FileUploadContent> Files { get; }

        /// <summary>
        /// Files count
        /// </summary>
        public int FilesCount => Files.Count();

        /// <summary>
        /// File bytes uploaded
        /// </summary>
        public long FileBytes { get; }

        /// <summary>
        /// Calculate file bytes in Mb
        /// </summary>
        public decimal FileMbBytes => FileBytes / 1048576.0M;

        /// <summary>
        /// Maximum allowed file size
        /// </summary>
        public long MaxFileBytes { get; }

        /// <summary>
        /// Calculate maximum allowed file size in Mb
        /// </summary>
        public decimal MaxFileMbBytes => MaxFileBytes / 1048576.0M;

        /// <summary>
        /// Maximum files allowed
        /// </summary>
        public int MaxFilesAllowed { get; }

        /// <summary>
        /// What kind of exception
        /// </summary>
        public ExceptionType ExceptionType { get; }
        #endregion

    }
}
