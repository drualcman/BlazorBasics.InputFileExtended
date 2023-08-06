const PasteFiles = (dropZone) => {
    const inputFile = dropZone.querySelector('input[type="file"]');

    function onInputFilePaste(ev) {
        inputFile.files = ev.clipboardData.files;
        let event = new Event('change', { bubbles: true });
        inputFile.dispatchEvent(event);
    }

    /*** Setup the events */
    RemoveEvents();
    document.body.addEventListener('paste', onInputFilePaste, false);
    console.info('PasteFiles: Events registered.');


    function RemoveEvents() {
        try {
            document.body.removeEventListener('paste', onInputFilePaste, false);
            console.info('PasteFiles: Events cleanup.');
        } catch (e) {
            console.warn('PasteFiles: RemoveEvents.', e);
        }
    }

    console.info(`PasteFiles: Enebled.`);

    return {
        Dispose: () => {
            console.info(`PasteFiles: Disposed.`);
            RemoveEvents();
        }
    }
}
export { PasteFiles }