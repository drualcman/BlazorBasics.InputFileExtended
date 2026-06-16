[![Nuget](https://img.shields.io/nuget/v/BlazorBasics.InputFileExtended?style=for-the-badge)](https://www.nuget.org/packages/BlazorBasics.InputFileExtended)
[![Nuget](https://img.shields.io/nuget/dt/BlazorBasics.InputFileExtended?style=for-the-badge)](https://www.nuget.org/packages/BlazorBasics.InputFileExtended)

# BlazorBasics.InputFileExtended

Extend the traditional `InputFile` component with more options like **drag and drop**, **copy and paste**, **preview**, **upload button**, **streaming for big files** and a rich set of **events** — with less coding for everyone.

Official [web documentation and live examples](https://blazorinputfileextended.community-mall.com/).

Supported target frameworks: **.NET 8**, **.NET 9** and **.NET 10**.

## Table of contents

- [Features](#features)
- [Installation](#installation)
- [Quick start](#quick-start)
- [Drag and drop](#drag-and-drop)
- [Copy and paste](#copy-and-paste)
- [Image preview](#image-preview)
- [Upload button](#upload-button)
- [Auto upload](#auto-upload)
- [Streaming big files](#streaming-big-files)
- [Events](#events)
  - [OnSelected (new)](#onselected)
  - [OnChange](#onchange)
  - [OnAddFile](#onaddfile)
  - [OnError](#onerror)
- [Configuration reference](#configuration-reference)
  - [InputFileParameters](#inputfileparameters)
  - [ButtonOptions](#buttonoptions)
  - [PreviewOptions](#previewoptions)
  - [DragAndDropOptions](#draganddropoptions)
- [Models](#models)
- [Public members of the component](#public-members-of-the-component)
- [Customizing the UI with RenderFragments](#customizing-the-ui-with-renderfragments)

## Features

- Standard file selection like the native `InputFile`.
- **Drag and drop** files into a drop zone.
- **Copy and paste** files / images from the clipboard.
- **Image preview** of the selected files.
- Optional **upload button** with before / after submit hooks.
- **Auto upload** after selection (one by one as an event callback).
- **Streaming mode** to upload very big files by chunks.
- Maximum number of files and maximum file size validation with detailed exceptions.
- Rich **event model**: `OnSelected`, `OnChange`, `OnAddFile`, `OnError`.
- Full UI customization through `RenderFragment` parameters.
- Works inside an `EditForm` (validated through the cascading `EditContext`).

## Installation

Install from NuGet:

```
dotnet add package BlazorBasics.InputFileExtended
```

Import the namespace adding this line to `_Imports.razor`:

```razor
@using BlazorBasics.InputFileExtended
```

> Tip: you can also import the value objects and models namespaces to avoid the long fully-qualified names used in some examples below:
>
> ```razor
> @using BlazorBasics.InputFileExtended.ValueObjects
> @using BlazorBasics.InputFileExtended.Models
> ```

## Quick start

```razor
@using BlazorBasics.InputFileExtended

<InputFileComponent OnChange="LoadFiles" />

@code {
    private void LoadFiles(FilesUploadEventArgs e)
    {
        // e.Files  -> List<FileUploadContent>
        // e.Count  -> number of files
        // e.Size   -> total size in bytes
        // e.Action -> EventAction (Added, Removed, ...)
    }
}
```

## Drag and drop

```razor
@using BlazorBasics.InputFileExtended
@using BlazorBasics.InputFileExtended.ValueObjects

<InputFileComponent Parameters="Parameters" OnChange="LoadFiles" />

@code {
    InputFileParameters Parameters = new()
    {
        DragAndDropOptions = new DragAndDropOptions
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

## Copy and paste

Paste works independently of drag and drop — you can enable either or both.

```razor
@using BlazorBasics.InputFileExtended
@using BlazorBasics.InputFileExtended.ValueObjects

<InputFileComponent Parameters="Parameters" OnChange="LoadFiles" />

@code {
    InputFileParameters Parameters = new()
    {
        AllowPasteFiles = true
    };

    private void LoadFiles(FilesUploadEventArgs e)
    {
        // ...
    }
}
```

## Image preview

```razor
@using BlazorBasics.InputFileExtended
@using BlazorBasics.InputFileExtended.ValueObjects

<InputFileComponent Parameters="Parameters" OnChange="LoadFiles" />

@code {
    InputFileParameters Parameters = new()
    {
        PreviewOptions = new PreviewOptions
        {
            IsImage = true,
            ShowPreview = true,
            // Show a delete button even when there is no callback wired
            CanDeleteIfNotCallBack = true
        }
    };

    private void LoadFiles(FilesUploadEventArgs e) { }
}
```

When `PreviewOptions.IsImage` is `true` and no `InputFileTypes` is set, the input automatically restricts the selection to `image/*`.

Each selected file exposes a ready-to-use `ImageDataUrl` you can bind directly to an `img` element:

```razor
@foreach (var file in files)
{
    <img src="@file.ImageDataUrl" alt="@file.Name" />
}
```

## Upload button

When you show the upload button, `ButtonOptions.OnSubmit` is **required**.

```razor
@using BlazorBasics.InputFileExtended
@using BlazorBasics.InputFileExtended.ValueObjects
@using BlazorBasics.InputFileExtended.Models

<InputFileComponent Parameters="Parameters" />

@code {
    InputFileParameters Parameters;

    protected override void OnInitialized()
    {
        Parameters = new InputFileParameters
        {
            ButtonOptions = new ButtonOptions
            {
                ButtonShow = true,
                ButtonTitle = "Upload",
                CleanOnSuccessUpload = true,
                OnBeforeSubmit = BeforeSubmit,
                OnSubmit = UploadFiles,
                OnAfterSubmit = AfterSubmit
            }
        };
    }

    Task BeforeSubmit(IReadOnlyList<FileUploadContent> files)
    {
        // e.g. show a spinner
        return Task.CompletedTask;
    }

    async Task UploadFiles(IReadOnlyList<FileUploadContent> files)
    {
        // process your upload, you have access to each file bytes
        foreach (var file in files)
        {
            byte[] bytes = file.FileBytes;
            // send to your API / storage ...
        }
        await Task.Delay(1);
    }

    Task AfterSubmit(bool isValid)
    {
        // e.g. hide the spinner, show a toast
        return Task.CompletedTask;
    }
}
```

> Inside an `EditForm`, the button submit is validated against the cascading `EditContext` automatically. You can also trigger the submit programmatically by calling the public `FormSave()` method (see [Public members](#public-members-of-the-component)).

## Auto upload

With `AutoUpload = true` each file is uploaded right after it is selected (handled one by one through the submit callback), without showing a button.

```razor
@using BlazorBasics.InputFileExtended
@using BlazorBasics.InputFileExtended.ValueObjects
@using BlazorBasics.InputFileExtended.Models

<InputFileComponent Parameters="Parameters" />

@code {
    InputFileParameters Parameters;

    protected override void OnInitialized()
    {
        Parameters = new InputFileParameters
        {
            ButtonOptions = new ButtonOptions
            {
                AutoUpload = true,
                OnSubmit = UploadFiles
            }
        };
    }

    async Task UploadFiles(IReadOnlyList<FileUploadContent> files)
    {
        // ...
        await Task.Delay(1);
    }
}
```

## Streaming big files

Enable `EnableStreaming` to handle very big files by chunks. In this mode the component reads only the file metadata on the .NET side and relies on the JavaScript layer to stream the content.

```razor
@using BlazorBasics.InputFileExtended
@using BlazorBasics.InputFileExtended.ValueObjects

<InputFileComponent Parameters="Parameters" OnChange="LoadFiles" />

@code {
    InputFileParameters Parameters = new()
    {
        EnableStreaming = true,
        MaxFileSize = 1024L * 1024L * 500L // 500 MB
    };

    private void LoadFiles(FilesUploadEventArgs e) { }
}
```

## Events

All events are exposed as `EventCallback` parameters on `InputFileComponent`.

| Event | Payload | When it is raised |
| --- | --- | --- |
| `OnSelected` | `FilesUploadEventArgs` | The instant files are picked, **before any byte is read**. Metadata only. |
| `OnChange` | `FilesUploadEventArgs` | After files are processed / added or removed from the container. |
| `OnAddFile` | `FileUploadEventArgs` | Per each file added (single file payload). |
| `OnError` | `InputFileException` | When a validation or upload error occurs. |

### OnSelected

> **New.** Raised the instant a selection is known (file name, size and content type), **before any byte is read from disk**. Use it to give immediate UI feedback (for example a "processing…" indicator) for heavy files such as videos, whose read can take several seconds.

The `FileUploadContent` items in the payload carry **metadata only** — `FileBytes` and `ImageDataUrl` are **not** populated yet. The reported `Action` is `EventAction.Change`.

Internally, when JavaScript is available, `OnSelected` fires from a native `change` listener the moment files are picked (even before Blazor's own change pipeline runs). If JS is unavailable it gracefully falls back to firing during the in-`Change` notification.

```razor
@using BlazorBasics.InputFileExtended
@using BlazorBasics.InputFileExtended.Models

<InputFileComponent OnSelected="HandleSelected" OnChange="HandleChange" />

@if (isProcessing)
{
    <p>Processing @pendingCount file(s)…</p>
}

@code {
    bool isProcessing;
    int pendingCount;

    // Fires immediately when the user picks files (metadata only, no bytes yet)
    private void HandleSelected(FilesUploadEventArgs e)
    {
        isProcessing = true;
        pendingCount = e.Count;

        foreach (var file in e.Files)
        {
            // file.Name, file.Size, file.ContentType, file.LastModified available
            // file.FileBytes is NOT available here yet
        }
    }

    // Fires later, once bytes are read and the files are added
    private void HandleChange(FilesUploadEventArgs e)
    {
        isProcessing = false;
        // Now e.Files contain the bytes (file.FileBytes / file.ImageDataUrl)
    }
}
```

### OnChange

Raised once the selected files have been processed and added to the container, and also when a file is removed from the collection. The `FileUploadContent` items now contain the bytes.

```razor
<InputFileComponent OnChange="HandleChange" />

@code {
    private void HandleChange(FilesUploadEventArgs e)
    {
        switch (e.Action)
        {
            case EventAction.Added:   /* files added */    break;
            case EventAction.Removed: /* a file removed */ break;
        }
    }
}
```

### OnAddFile

Raised per each individual file added, with a single-file payload. Useful together with auto upload to react file by file.

```razor
<InputFileComponent OnAddFile="HandleAddFile" />

@code {
    private void HandleAddFile(FileUploadEventArgs e)
    {
        FileUploadContent file = e.File;
        Guid id = e.FileId;
        EventAction action = e.Action;
    }
}
```

### OnError

Raised when a validation rule (max file count, max size) or an upload fails. The `InputFileException` exposes detailed information about what went wrong.

```razor
<InputFileComponent Parameters="Parameters" OnError="HandleError" />

@code {
    InputFileParameters Parameters = new()
    {
        MaxUploatedFiles = 3,
        MaxFileSize = 1_048_576 // 1 MB
    };

    private void HandleError(InputFileException ex)
    {
        // ex.ExceptionType -> Generic | MaxCount | MaxSize (Flags)
        // ex.FilesCount, ex.FileBytes, ex.FileMbBytes
        // ex.MaxFilesAllowed, ex.MaxFileBytes, ex.MaxFileMbBytes
        // ex.Files -> files affected
        if (ex.ExceptionType.HasFlag(ExceptionType.MaxSize))
        {
            // show a friendly "file too big" message
        }
    }
}
```

> If you do **not** wire `OnError`, the component shows the error message inline by itself.

## Configuration reference

### InputFileParameters

| Property | Type | Default | Description |
| --- | --- | --- | --- |
| `ShowFileList` | `bool` | `true` | Show / hide the list of selected files. |
| `AllowPasteFiles` | `bool` | `false` | Enable copy and paste of files / images. |
| `MultiFile` | `bool` | `true` | Allow selecting multiple files. |
| `MaxUploatedFiles` | `int` | `5` | Maximum number of files allowed. |
| `MaxFileSize` | `long` | `1536000` | Maximum size per file, in bytes. |
| `InputFileTypes` | `string` | `""` | Accepted file types, e.g. `image/*`. |
| `InputFileCss` | `string` | `""` | CSS class for the input file element. |
| `ButtonOptions` | `ButtonOptions` | `new()` | Upload button configuration. |
| `PreviewOptions` | `PreviewOptions` | `new()` | Image / preview configuration. |
| `DragAndDropOptions` | `DragAndDropOptions` | `new()` | Drag and drop configuration. |
| `OnShouldCancelClick` | `Func<Task<bool>>` | `null` | Run code before the input click; return `true` to cancel it. |
| `EnableStreaming` | `bool` | `false` | Enable streaming mode for big files. |

### ButtonOptions

| Property | Type | Default | Description |
| --- | --- | --- | --- |
| `OnBeforeSubmit` | `Func<IReadOnlyList<FileUploadContent>, Task>` | `null` | Run code before submit. |
| `OnSubmit` | `Func<IReadOnlyList<FileUploadContent>, Task>` | `null` | Submit handler. **Required** when `ButtonShow` is `true`. |
| `OnAfterSubmit` | `Func<bool, Task>` | `null` | Run code after submit (receives validity). |
| `ButtonShow` | `bool` | `false` | Show the upload button. |
| `ButtonCss` | `string` | `"input-file button-upload"` | CSS class for the button. |
| `ButtonTitle` | `string` | `""` | Button text. |
| `ButtonType` | `ButtonType` | `Button` | `Button` or `Submit`. |
| `AutoUpload` | `bool` | `false` | Upload automatically after selection. |
| `CleanOnSuccessUpload` | `bool` | `true` | Clear files after a successful upload. |

### PreviewOptions

| Property | Type | Default | Description |
| --- | --- | --- | --- |
| `IsImage` | `bool` | `true` | Indicates the files are images. |
| `ShowPreview` | `bool` | `false` | Render an image preview of each file. |
| `PreviewWrapperCss` | `string` | `"image-container"` | CSS class for the preview wrapper. |
| `ImagePreviewCss` | `string` | `"image"` | CSS class for the preview image. |
| `CanDeleteIfNotCallBack` | `bool` | `false` | Show a delete button even when no callback is set. |

### DragAndDropOptions

| Property | Type | Default | Description |
| --- | --- | --- | --- |
| `CanDropFiles` | `bool` | `false` | Enable dropping files. |
| `DropZoneCss` | `string` | `"drop-zone"` | CSS class for the drop zone. |
| `DroppingCss` | `string` | `"drop-zone-drag"` | CSS class while dragging over the drop zone. |

## Models

### FileUploadContent

The full representation of an uploaded file.

| Member | Type | Description |
| --- | --- | --- |
| `Name` | `string` | File name. |
| `LastModified` | `DateTimeOffset` | Last modified date. |
| `Size` | `long` | Size in bytes. |
| `ContentType` | `string` | MIME type. |
| `FileId` | `Guid` | Unique identifier for the file. |
| `ImageDataUrl` | `string` | Data URL ready to be used as an `img` `src`. |
| `FileBytes` | `byte[]` | Raw file bytes. |
| `Dispose()` | `void` | Clears `FileBytes` to release memory. |

### FileBasicInfo

> **Now public.** Lightweight metadata model (no bytes), used by the streaming / early-selection pipeline and exposed publicly so you can consume it from your own code.

| Member | Type | Description |
| --- | --- | --- |
| `Name` | `string` | File name. |
| `Size` | `long` | Size in bytes. |
| `Type` | `string` | MIME type. |
| `LastModified` | `long` | Last modified as Unix time (ms). |
| `LastModifiedDateTime` | `DateTimeOffset` | `LastModified` converted to `DateTimeOffset`. |

### FilesUploadEventArgs

Payload for `OnSelected` and `OnChange`: `Files` (`List<FileUploadContent>`), `Size` (`long`), `Count` (`int`), `Action` (`EventAction`).

### FileUploadEventArgs

Payload for `OnAddFile`: `File` (`FileUploadContent`), `FileId` (`Guid`), `Action` (`EventAction`).

### EventAction

`Change`, `Added`, `Updated`, `Removed`, `Clean`, `Upload`.

## Public members of the component

Capture a reference with `@ref` to access these:

```razor
<InputFileComponent @ref="inputFile" Parameters="Parameters" />

@code {
    InputFileComponent inputFile;

    void ClearAll() => inputFile.Clean();
    Task Save() => inputFile.FormSave();
    Task PickFiles() => inputFile.OpenFileDialog();
}
```

| Member | Description |
| --- | --- |
| `Files` | `InputFileHandler` exposing the managed files (e.g. to show all images). |
| `InputFileId` | The generated `id` of the input element (useful for external CSS / JS). |
| `Clean()` | Remove all selected files. |
| `FormSave()` | Trigger the save / submit (validated when inside an `EditForm`). |
| `OpenFileDialog()` | Open the file picker programmatically, without a user click. |

## Customizing the UI with RenderFragments

`InputFileComponent` exposes several `RenderFragment` parameters to fully customize the UI:

| Parameter | Type | Description |
| --- | --- | --- |
| `InputContent` | `RenderFragment` | Content shown for the file selection area. |
| `SelectContent` | `RenderFragment<IReadOnlyList<FileUploadContent>>` | Content rendered once with the whole selection. |
| `ButtonContent` | `RenderFragment` | Custom content / text for the upload button. |
| `ChildContent` | `RenderFragment<FileUploadContent>` | Content rendered per each file (used when there is no preview). |
| `Attributes` | `Dictionary<string, object>` | Additional HTML attributes (captured unmatched values). |

```razor
<InputFileComponent Parameters="Parameters" OnChange="LoadFiles">
    <InputContent>
        <span>Drag your files here or click to browse</span>
    </InputContent>
    <ChildContent Context="file">
        <div class="file-row">
            <strong>@file.Name</strong> — @file.Size bytes
        </div>
    </ChildContent>
</InputFileComponent>
```

---

Made with ❤️ by [DrUalcman](https://github.com/drualcman). Issues and contributions are welcome at the [GitHub repository](https://github.com/drualcman/BlazorBasics.InputFileExtended).
