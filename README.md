[![Nuget](https://img.shields.io/nuget/v/BlazorBasics.InputFileExtended?style=for-the-badge)](https://www.nuget.org/packages/BlazorBasics.InputFileExtended)
[![Nuget](https://img.shields.io/nuget/dt/BlazorBasics.InputFileExtended?style=for-the-badge)](https://www.nuget.org/packages/BlazorBasics.InputFileExtended)

# Description
Extend the traditional component InputFile with more options like drag and drop, copy and paste. Less coding for all. Oficial [web](https://blazorinputfileextended.community-mall.com/) documentation and examples.
# How to use simple way
Import the name space adding to _Imports.razor this line:
```
@using BlazorBasics.InputFileExtended
```
Add into your component:
``` razor
<InputFileComponent OnChange=LoadFiles />

@code {
    private void LoadFiles(FilesUploadEventArgs e)
    {
        ...
    }
}
```
## How to use with drag and drop
``` razor
@using BlazorBasics.InputFileExtended

<InputFileComponent Parameters="Parameters" OnChange=LoadFiles />

@code{
    BlazorBasics.InputFileExtended.ValueObjects.InputFileParameters Parameters = new BlazorBasics.InputFileExtended.ValueObjects.InputFileParameters()
    {
        DragAndDropOptions = new BlazorBasics.InputFileExtended.ValueObjects.DragAndDropOptions
        {
            CanDropFiles = true
        }
    };

    private void LoadFiles(FilesUploadEventArgs e)
    {
        // ...
    }
}
```
## How to use with copy and paste
``` razor
@using BlazorBasics.InputFileExtended

<InputFileComponent Parameters="Parameters" OnChange=LoadFiles />

@code{
    BlazorBasics.InputFileExtended.ValueObjects.InputFileParameters Parameters = new BlazorBasics.InputFileExtended.ValueObjects.InputFileParameters()
    {
        AllowPasteFiles = true
    };

    private void LoadFiles(FilesUploadEventArgs e)
    {
        // ...
    }
}
```
## How to use with upload button
``` razor
@using BlazorBasics.InputFileExtended

<InputFileComponent Parameters="Parameters" />

@code {
    BlazorBasics.InputFileExtended.ValueObjects.InputFileParameters Parameters;

    Task<bool> UploadFles(IReadOnlyList<BlazorBasics.InputFileExtended.Models.FileUploadContent> files)
    {
        // process your upload
        // ...
        await Task.Delay(1);
        return true;
    }

    protected override void OnInitialized()
    {
        Parameters = new BlazorBasics.InputFileExtended.ValueObjects.InputFileParameters()
        {
            ButtonOptions = new BlazorBasics.InputFileExtended.ValueObjects.ButtonOptions
            {
                ButtonShow = true,
                CleanOnSuccessUpload = true            
            }
        };
        Parameters.ButtonOptions.OnSubmit = UploadFles;
    }
}
```
