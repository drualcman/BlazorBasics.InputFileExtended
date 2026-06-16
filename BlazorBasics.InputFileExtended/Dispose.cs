namespace BlazorBasics.InputFileExtended;

public partial class InputFileComponent : IDisposable, IAsyncDisposable
{

    /// <summary>
    /// Dispose action
    /// </summary>
    public void Dispose()
    {     
        Console.WriteLine("Dispose");
        Files.Dispose();
    }

    /// <summary>
    /// Dispose async objects
    /// </summary>
    /// <returns></returns>
    public async ValueTask DisposeAsync()
    {
        await UnregisterSelectionNotifierAsync();
        await UnLoadPasteScript();
        await UnLoadDropScriptsAsync();
        await UnLoadFileEventsScript();
        SelfReference?.Dispose();
        GlobalEvents.ItemDeleted -= RemoveFile;
    }

    async Task UnregisterSelectionNotifierAsync()
    {
        if (!SelectionNotifierRegistered || FileEventScriptsReference is null)
            return;

        try
        {
            await FileEventScriptsReference.InvokeVoidAsync("UnregisterSelectionNotifier", InputFileReference);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            SelectionNotifierRegistered = false;
        }
    }
}
