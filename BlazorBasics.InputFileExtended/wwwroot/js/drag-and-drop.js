const DragAndDrop = (dropZone) => {
    const inputFile = dropZone.querySelector('input[type="file"]');

    function onDropHandler(ev) {
        ev.preventDefault();
        inputFile.files = ev.dataTransfer.files;
        try {
            let event = new Event('change', { bubbles: true });
            inputFile.dispatchEvent(event);
        } catch (e) {
            console.warn(e);
        }
        RemoveDragData(ev)
    }

    /*** Setup the events */
    RemoveEvents();
    dropZone.addEventListener('dragover', onDragOverHandler, false);
    dropZone.addEventListener('drop', onDropHandler, false);
    console.info('DragAndDrop: Events registered.');

    function onDragOverHandler(ev) {
        ev.preventDefault();
        ev.stopPropagation();
    }

    function RemoveDragData(ev) {
        if (ev.dataTransfer.items) {
            ev.dataTransfer.items.clear();
        } else {
            ev.dataTransfer.clearData();
        }
        console.info('DragAndDrop: RemoveDragData cleanup.');
    }

    function RemoveEvents() {
        try {
            dropZone.removeEventListener('dragover', onDragOverHandler, false);
            dropZone.removeEventListener('drop', onDropHandler, false);
            console.info('DragAndDrop: Events cleanup.');
        } catch (e) {
            console.warn('DragAndDrop: RemoveEvents.', e);
        }
    }

    console.info(`DragAndDrop: Enebled.`);

    return {
        Dispose: () => {
            console.info(`DragAndDrop: Disposed.`);
            RemoveEvents();
        }
    }
}

export { DragAndDrop }