namespace BlazorBasics.InputFileExtended;

public partial class InputFileComponent
{
    IJSObjectReference PasteScriptsReference;

    #region Management
    /// <summary>
    /// Add the scripts for can pase files
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
    /// Remove paste options
    /// </summary>
    /// <returns></returns>
    async Task UnLoadPasteScript()
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
