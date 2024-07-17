namespace BlazorBasics.InputFileExtended;

public partial class InputFileComponent
{
    #region variables 
    IJSObjectReference DropScriptsReference;
    string Dropping;
    string DropZoneCss;
    bool IsDropping;
    #endregion

    #region Management
    /// <summary>
    /// Add the scripts for can drop files
    /// </summary>
    /// <returns></returns>
    async Task LoadDragAdnDropScripts()
    {
        if(Parameters.DragAndDropOptions.CanDropFiles)
        {
            try
            {
                DropScriptsReference = await GetJSObjectReference("drag-and-drop");
                await DropScriptsReference.InvokeVoidAsync("DragAndDrop", InputFileReference);
            }
            catch(Exception ex)
            {
                DropScriptsReference = null;
                Console.WriteLine(ex.Message);
            }
        }
    }

    /// <summary>
    /// Remove drag and drop options
    /// </summary>
    /// <returns></returns>
    async Task UnLoadDropScriptsAsync()
    {
        // unload the JavaScript for drag and drop
        if(Parameters.DragAndDropOptions.CanDropFiles && DropScriptsReference is not null)
        {
            try
            {
                //await DropScriptsReference.InvokeVoidAsync("Dispose");
                await DropScriptsReference.DisposeAsync();
            }
            catch { }
        }
    }

    /// <summary>
    /// Change class to know we are in the drag area
    /// </summary>
    void DragEnter()
    {
        IsDropping = true;
        if(Parameters.DragAndDropOptions.CanDropFiles && IsDropping) Dropping = Parameters.DragAndDropOptions.DroppingCss;
        else Dropping = string.Empty;
    }
    /// <summary>
    /// Remove the class because we are not in the drag area
    /// </summary>
    void DragLeave()
    {
        Dropping = string.Empty;
        IsDropping = false;
    }
    #endregion

}
