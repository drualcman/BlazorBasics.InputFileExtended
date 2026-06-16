const OpenDialog = (elementId) => {
    document.getElementById(elementId).click();
}

const PreventDefault = (elementId, result) => {
    const element = document.getElementById(elementId);
    if (element && result) {
        element.addEventListener('click', function (event) {
            event.preventDefault();
            event.stopPropagation();
        }, { once: true });
    }
}

const GetFileDetails = (elementId) => {
    return new Promise((resolve, reject) => {
        console.log('GetFileDetails', 1, elementId);
        const element = document.getElementById(elementId);

        console.log('GetFileDetails', 2, element);
        if (!element) {
            reject("Element not found");
            return;
        }

        if (element.files && element.files.length > 0) {

            const fileDetails = [];

            for (let i = 0; i < element.files.length; i++) {
                const file = element.files[i];

                fileDetails.push({
                    name: file.name,
                    size: file.size,
                    type: file.type,
                    lastModified: file.lastModified
                });
            }

            resolve(fileDetails);
            return;
        }

        element.addEventListener(
            'change',
            function (e) {

                const files = e.target.files;
                const fileDetails = [];

                for (let i = 0; i < files.length; i++) {
                    const file = files[i];

                    fileDetails.push({
                        name: file.name,
                        size: file.size,
                        type: file.type,
                        lastModified: file.lastModified
                    });
                }

                resolve(fileDetails);
            },
            { once: true }
        );
    });
};

// Notifies .NET the instant the native `change` event fires — reading only file metadata
// (name/size/type) straight from the DOM, BEFORE Blazor's own change pipeline or any byte
// read. This is the earliest possible signal so consumers can show immediate UI feedback
// for heavy files (e.g. large videos). Attached to a stable element (the wrapper label);
// `change` bubbles up from the inner input, and capture phase makes it fire first.
const RegisterSelectionNotifier = (element, dotNetRef) => {
    if (!element || element.__ifeSelectionNotifier)
        return;

    const handler = (e) => {
        const target = e.target;
        if (target && target.matches && target.matches('input[type=file]') &&
            target.files && target.files.length > 0) {
            const details = [];
            for (let i = 0; i < target.files.length; i++) {
                const file = target.files[i];
                details.push({
                    name: file.name,
                    size: file.size,
                    type: file.type,
                    lastModified: file.lastModified
                });
            }
            dotNetRef.invokeMethodAsync('NotifySelected', details);
        }
    };

    element.addEventListener('change', handler, true);
    element.__ifeSelectionNotifier = handler;
};

const UnregisterSelectionNotifier = (element) => {
    if (!element || !element.__ifeSelectionNotifier)
        return;

    element.removeEventListener('change', element.__ifeSelectionNotifier, true);
    element.__ifeSelectionNotifier = null;
};

export { OpenDialog, PreventDefault, GetFileDetails, RegisterSelectionNotifier, UnregisterSelectionNotifier }