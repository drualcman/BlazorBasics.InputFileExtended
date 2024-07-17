namespace BlazorBasics.InputFileExtended.ValueObjects;
/// <summary>
/// How to configure the input file
/// </summary>
public class InputFileParameters
{
    /// <summary>
    /// Enable or dissable show file list. Default enabled
    /// </summary>
    public bool ShowFileList { get; set; } = true;
    /// <summary>
    /// Enable or dissable copy and pase. Default desabled
    /// </summary>
    public bool AllowPasteFiles { get; set; }

    /// <summary>
    /// Set if we will accept multiple files uploaded or not
    /// </summary>
    public bool MultiFile { get; set; } = true;

    /// <summary>
    /// Number maximum of files can be uploaded
    /// </summary>
    public int MaxUploatedFiles { get; set; } = 5;

    /// <summary>
    /// Maximum file size per each file
    /// </summary>
    public long MaxFileSize { get; set; } = 1536000;

    /// <summary>
    /// File types accepted. Example: image/*
    /// </summary>
    public string InputFileTypes { get; set; } = string.Empty; 
    
    /// <summary>
    /// CSS for the input file
    /// </summary>
    public string InputFileCss { get; set; } = string.Empty;   

    /// <summary>
    /// Setup button for upload like a container class
    /// </summary>
    public ButtonOptions ButtonOptions { get; set; } = new();

    /// <summary>
    /// Setup images options
    /// </summary>
    public PreviewOptions PreviewOptions { get; set; } = new();

    /// <summary>
    /// Setup drag and drop options
    /// </summary>
    public DragAndDropOptions DragAndDropOptions { get; set; } = new();

    /// <summary>
    /// Delegate to execute some code before execute the click. Return true if want to cancel the click event
    /// </summary>
    public Func<Task<bool>> OnShouldCancelClick;


}
