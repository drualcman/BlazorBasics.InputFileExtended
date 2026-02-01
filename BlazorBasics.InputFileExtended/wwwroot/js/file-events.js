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

export { OpenDialog, PreventDefault, GetFileDetails }