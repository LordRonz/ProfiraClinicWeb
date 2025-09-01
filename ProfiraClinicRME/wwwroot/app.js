var hello = async function (name) {
    console.log(`hello ${name}`);
    return { status: 0, message: 'hello ' };
}

var ImageService = {

    /**
     * upload blob in global variable to server
     * @param {string} url upload url
     * @param {string} paramName
     * @param {string} blobName globally scoped blob name
     * @param {string} fileName
     * @returns {Promise<OpStatus>} Operation status object
     */
    uploadBlob: async function (url, paramName, blobName, fileName) {
        console.log(`start upload {paramName} from {blobName}`);
        const blob = window[blobName];
        const opStatus = new OpStatus();
        opStatus.status = 1;
        try {
            if (!blob) {
                opStatus.message = `Blob variable '${blobName}' not found in global scope.`;
                console.error(opStatus);
                return opStatus;
            }

            const formData = new FormData();
            formData.append(paramName, blob, fileName);

            const response = await fetch(url, {
                method: 'POST',
                body: formData,
            });

            const jsonResponse = await response.json();
            if (!response.ok) {
                opStatus.message = `Server error: ${response.status} ${response.statusText}`;
                opStatus.data = jsonResponse;
                console.error(opStatus);
                return opStatus;
            }

            opStatus.status = 0;
            opStatus.message = 'Upload successful';
            opStatus.data = jsonResponse;
            console.log(opStatus);
            return opStatus;
        } catch (error) {
            opStatus.message = `Error uploading blob data: ${error.message}`;
            console.error(`error: opstatus status: ${opStatus.status} message: ${opStatus.message}`);
            return opStatus;
        }
    }
};

/**
 * Operation status object
 * @class
 * @property {number} status - 0 = success, 1 = fail
 * @property {string} message - status message
 * @property {any} data - additional data
 */
class OpStatus {
    status = 1; 
    message = '';
    data = null
}

var DrawingService = {
    _blobStore: {},
    _idWidget: '',
    _blobName: '',

    getId: function () {
        return this._idWidget;
    },
    /**
     * not used: get blob from blob store
     * @param {string} blobName
     * @returns {Blob|null} Blob object or null if not found
     */
    getBlob: function (blobName) {
        console.log('Get blob:', blobName); 
        if (this.blobStore.hasOwnProperty(blobName)) {
            return this.blobStore[blobName];
        } else {
            console.warn(`Blob not found: ${blobName}`);
            return null;
        }
    },

    /**
     * Generate blob from canvas in widget and store it in global
     * @param {string} idWidget - ID of the widget containing the canvas
     * @param {string} blobName - Name to store the blob under
     * @returns {Promise<OpStatus>} Operation status object
     */
    genBlob: async function (idWidget, blobName) {
        this._idWidget = idWidget;
        this._blobName = blobName;
        console.log(`Store blob from ${idWidget} to ${blobName}`);
        const canvas = document.querySelector(`#${idWidget} canvas`);
        const opStatus = new OpStatus();
        opStatus.status = 1;
        if (!canvas) {
            opStatus.message = `Canvas not found for widget ${idWidget}`;
            console.error(opStatus);
            return opStatus;
        }

        try {
            const blob = await new Promise((resolve) => canvas.toBlob(resolve, 'image/jpeg'));
            window[blobName] = blob;
            opStatus.status = 0;
            opStatus.message = 'Blob generated successfully';
            opStatus.data = blobName;
            console.log(opStatus);
            return opStatus;
        } catch (err) {
            opStatus.message = `Error generating blob: ${err.message}`;
            console.error(opStatus);
            return opStatus;
        }
    },

    /**
     * copy canvas content to image element
     * @param {any} idWidget
     * @param {any} idImage
     * @returns
     */
    copyToImage: async function (idWidget, idImage) {
        console.log(`Copy canvas from ${idWidget} to image ${idImage}`);
        const canvas = document.querySelector(`#${idWidget} canvas`);
        const img = document.getElementById(idImage);
        const opStatus = new OpStatus();
        opStatus.status = 1;
        if (!canvas) {
            opStatus.message = `Canvas not found for widget ${idWidget}`;
            console.error(opStatus);
            return opStatus;
        }
        if (!img) {
            opStatus.message = `Image element not found: ${idImage}`;
            console.error(opStatus);
            return opStatus;
        }

        try {
            const urlData = await new Promise((resolve) => canvas.toBlob(resolve, 'image/jpeg'));
            img.src = urlData;
            opStatus.status = 0;
            opStatus.message = 'Image updated successfully';
            console.log(opStatus);
            return opStatus;
        } catch (err) {
            opStatus.message = `Error updating image: ${err.message}`;
            console.error(opStatus);
            return opStatus;
        }

    }
};

window.DrawingService = DrawingService;


const test = 'supah';