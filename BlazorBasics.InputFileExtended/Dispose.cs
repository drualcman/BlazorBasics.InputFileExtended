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
        await UnLoadPaseteScript();
        await UnLoadDropScriptsAsync();
        GlobalEvents.ItemDeleted -= RemoveFile;
    }
}
