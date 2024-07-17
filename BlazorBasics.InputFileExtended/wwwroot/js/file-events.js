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
export { OpenDialog, PreventDefault }