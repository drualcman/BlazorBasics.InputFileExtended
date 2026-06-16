using System.Text.Json;

namespace BlazorBasics.InputFileExtended;
/// <summary>
/// Parameters
/// </summary>
public partial class InputFileComponent
{
    internal JsonSerializerOptions SERIALIZE_OPTIONS = new JsonSerializerOptions()
    {
        PropertyNameCaseInsensitive = true
    };

    /// <summary>
    /// Configure the input file parameters
    /// </summary>
    [Parameter] public InputFileParameters Parameters { get; set; }

    #region html formating
    /// <summary>
    /// Aditional HTML attributes
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object> Attributes { get; set; }

    /// <summary>
    /// Text to show for the file selection
    /// </summary>
    [Parameter] public RenderFragment InputContent { get; set; }

    /// <summary>
    /// Content per each file selected
    /// </summary>
    [Parameter] public RenderFragment<IReadOnlyList<FileUploadContent>> SelectContent { get; set; }

    /// <summary>
    /// Button text
    /// </summary>
    [Parameter] public RenderFragment ButtonContent { get; set; }

    /// <summary>
    /// Button text
    /// </summary>
    [Parameter] public RenderFragment<FileUploadContent> ChildContent { get; set; }

    #endregion

    #region form    
    /// <summary>
    /// Get the context from the form
    /// </summary>
    [CascadingParameter] public EditContext Model { get; set; }
    #endregion

    #region events
    /// <summary>
    /// Raised the instant a file selection is known (file name, size and content type),
    /// BEFORE any byte is read from disk. Use it to give immediate UI feedback (e.g. a
    /// "processing" indicator) for heavy files such as videos, whose read can take seconds.
    /// The <see cref="FileUploadContent"/> items carry metadata only — <c>FileBytes</c> is
    /// not populated yet. <see cref="EventAction.Change"/> is reported as the action.
    /// </summary>
    [Parameter] public EventCallback<FilesUploadEventArgs> OnSelected { get; set; }

    /// <summary>
    /// When each file is uploaded
    /// </summary>
    [Parameter] public EventCallback<FileUploadEventArgs> OnAddFile { get; set; }

    /// <summary>
    /// When all files is uploaded
    /// </summary>
    [Parameter] public EventCallback<FilesUploadEventArgs> OnChange { get; set; }

    /// <summary>
    /// When some error occurs
    /// </summary>
    [Parameter] public EventCallback<InputFileException> OnError { get; set; }
    #endregion

    /// <summary>
    /// Format the component with the properties
    /// </summary>
    protected override void OnParametersSet()
    {
        if (Parameters is null)
            Parameters = new();

        if (Parameters.DragAndDropOptions.CanDropFiles)
        {
            DropZoneCss = Parameters.DragAndDropOptions.DropZoneCss;
            Dropping = Parameters.DragAndDropOptions.DroppingCss;
        }

        Files.SetMaxFiles(Parameters.MaxUploatedFiles);
        Files.SetMaxFileSize(Parameters.MaxFileSize);

        InputFileTypes = Parameters.InputFileTypes;
        if (Parameters.PreviewOptions.IsImage && string.IsNullOrEmpty(Parameters.InputFileTypes))
            InputFileTypes = "image/*";

        if (Parameters.ButtonOptions.ButtonShow &&
            Parameters.ButtonOptions.OnSubmit is null)
        {
            APIErrorMessages = $"If <i>{nameof(Parameters.ButtonOptions.ButtonShow)}</i> is enabled then <i>{nameof(Parameters.ButtonOptions.OnSubmit)}</i> is <strong>required</strong>.";
            InvokeAsync(StateHasChanged);
            throw new ArgumentException($"If {nameof(Parameters.ButtonOptions.ButtonShow)} is true. {nameof(Parameters.ButtonOptions.OnSubmit)} is required.", nameof(Parameters.ButtonOptions.OnSubmit));
        }

        if (Attributes is not null && Attributes.TryGetValue("class", out object value))
            LabelWrapperCss = value.ToString();
    }
}
