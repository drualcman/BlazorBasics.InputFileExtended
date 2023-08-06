namespace BlazorBasics.InputFileExtended;

public partial class InputFileComponent
{
    /// <summary>
    /// Inject JavaScript interoperability
    /// </summary>
    [Inject] IJSRuntime JavaScript { get; set; }
}
