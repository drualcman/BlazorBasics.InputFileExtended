namespace BlazorBasics.InputFileExtended;

public partial class InputFileComponent
{
    IJSObjectReference PasteScriptsReference;

    #region Management
    /// <summary>
    /// Add the scripts fro can drop files
    /// </summary>
    /// <returns></returns>
    async Task LoadPasteScript()
    {
        if(Parameters.AllowPasteFiles)
        {

            try
            {
                PasteScriptsReference = await GetJSObjectReference("paste-files");
                await PasteScriptsReference.InvokeVoidAsync("PasteFiles", InputFileReference);
            }
            catch(Exception ex)
            {
                PasteScriptsReference = null;
                Console.WriteLine(ex.Message);
            }
        }
    }


    /// <summary>
    /// Remove drag and drop options
    /// </summary>
    /// <returns></returns>
    async Task UnLoadPaseteScript()
    {            
        // disabled paste files
        if(Parameters.AllowPasteFiles && PasteScriptsReference is not null)
        {         
            try
            {
                //await PasteScriptsReference.InvokeVoidAsync("Dispose");
                await PasteScriptsReference.DisposeAsync();
            }
            catch { }
        }
    }
    #endregion

}
