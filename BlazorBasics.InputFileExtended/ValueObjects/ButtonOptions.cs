namespace BlazorBasics.InputFileExtended.ValueObjects;
/// <summary>
/// configuration for teh upload button
/// </summary>
public class ButtonOptions
{
    /// <summary>
    /// Delegate to execute some code before submit
    /// </summary>
    public Func<IReadOnlyList<FileUploadContent>, Task> OnBeforeSubmit;
    /// <summary>
    /// Delegate to execute submit form
    /// </summary>
    public Func<IReadOnlyList<FileUploadContent>, Task> OnSubmit;
    /// <summary>
    /// Delegate to execute some code after submit
    /// </summary>
    public Func<bool, Task> OnAfterSubmit;
    /// <summary>
    /// Show the save button
    /// </summary>
    public bool ButtonShow { get; set; } = false;
    /// <summary>
    /// CSS button save
    /// </summary>
    public string ButtonCss { get; set; } = "input-file button-upload";
    /// <summary>
    /// Button title
    /// </summary>
    public string ButtonTitle { get; set; } = string.Empty;
    /// <summary>
    /// Set the button type. Default is button
    /// </summary>
    public ButtonType ButtonType { get; set; } = ButtonType.Button;

    /// <summary>
    /// Determinate the file must be upload after selection
    /// </summary>
    public bool AutoUpload { get; set; }
    /// <summary>
    /// Clean all files after success upload
    /// </summary>
    public bool CleanOnSuccessUpload { get; set; } = true;
}
