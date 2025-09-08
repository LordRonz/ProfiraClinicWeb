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

class DrawingWidget {
    _idWidget = "";

    static Create(guid) {
        return new DrawingWidget(guid);
    }

    constructor(guid) {
        console.log("Object constructor: " + guid);

        this._idWidget = "dw-" + guid;


        this.canvas = document.getElementById("dw-drw-" + guid);
        this.ctx = this.canvas.getContext("2d");
        this.isDrawing = false;
        this.currentMode = "free";
        this.startX = 0;
        this.startY = 0;
        this.currentColor = "#000000";
        this.currentWidth = 3;
        this.imageLoaded = false;
        this.backgroundImage = null;
        this.savedImageData = null;

        this.initializeEventListeners();

    }

    initializeEventListeners() {
        

        // Canvas events - using pointer events for better touch support
        this.canvas.addEventListener("pointerdown", (e) =>
            this.startDrawing(e)
        );
        this.canvas.addEventListener("pointermove", (e) =>
            this.draw(e)
        );
        this.canvas.addEventListener("pointerup", () =>
            this.stopDrawing()
        );
        this.canvas.addEventListener("pointerout", () =>
            this.stopDrawing()
        );

        // Prevent default touch behaviors
        this.canvas.addEventListener("touchstart", (e) =>
            e.preventDefault()
        );
        this.canvas.addEventListener("touchmove", (e) =>
            e.preventDefault()
        );
        this.canvas.addEventListener("touchend", (e) =>
            e.preventDefault()
        );
    }

    drawImageFromId(IdElement) {
        const srcImage = document.getElementById(IdElement);
        this.#drawCanvas(srcImage);
    }

    /**
     * draw to canvas from image object (or img Element)
     * @param {Image} img
     */
    #drawCanvas(img) {
        const canvasAspect =
            this.canvas.width / this.canvas.height;
        const imageAspect = img.width / img.height;

        let drawWidth,
            drawHeight,
            offsetX = 0,
            offsetY = 0;

        if (imageAspect > canvasAspect) {
            // Image is wider than canvas aspect ratio
            drawWidth = this.canvas.width;
            drawHeight = drawWidth / imageAspect;
            offsetY = (this.canvas.height - drawHeight) / 2;
        } else {
            // Image is taller than canvas aspect ratio
            drawHeight = this.canvas.height;
            drawWidth = drawHeight * imageAspect;
            offsetX = (this.canvas.width - drawWidth) / 2;
        }

        // Clear canvas and draw image
        this.ctx.clearRect(
            0,
            0,
            this.canvas.width,
            this.canvas.height
        );
        this.ctx.drawImage(
            img,
            offsetX,
            offsetY,
            drawWidth,
            drawHeight
        );

        this.backgroundImage = img;
        this.imageLoaded = true;
        this.#updateStatus(
            "Image loaded! You can now start drawing."
        );
    }

    /**
     * load image to canvas
     * @param {blob} file
     * @returns
     */
    loadImage(file) {
        if (!file) return;

        const reader = new FileReader();
        reader.onload = (e) => {
            const img = new Image();
            img.onload = () => this.#drawCanvas(img);
            img.src = e.target.result;
        };
        reader.readAsDataURL(file);
    }

    setMode(mode) {
        if (!this.imageLoaded) {
            this.#updateStatus("Please select an image first!");
            return;
        }

        this.currentMode = mode;

        // Update button states
        document.querySelectorAll(".mode-btn").forEach((btn) => {
            btn.classList.remove("active");
        });
        document
            .querySelector(`[data-mode="${mode}"]`)
            .classList.add("active");

        this.#updateStatus(
            `Drawing mode: ${mode.charAt(0).toUpperCase() + mode.slice(1)
            }`
        );
    }

    getCanvasCoordinates(e) {
        const rect = this.canvas.getBoundingClientRect();
        return {
            x: e.clientX - rect.left,
            y: e.clientY - rect.top,
        };
    }

    startDrawing(e) {
        if (!this.imageLoaded) {
            this.#updateStatus("Please select an image first!");
            return;
        }

        this.isDrawing = true;
        const coords = this.getCanvasCoordinates(e);
        this.startX = coords.x;
        this.startY = coords.y;

        this.ctx.strokeStyle = this.currentColor;
        this.ctx.lineWidth = this.currentWidth;
        this.ctx.lineCap = "round";
        this.ctx.lineJoin = "round";

        if (this.currentMode === "free") {
            this.ctx.beginPath();
            this.ctx.moveTo(this.startX, this.startY);
        } else {
            // Save the current canvas state for rectangle/ellipse drawing
            this.savedImageData = this.ctx.getImageData(
                0,
                0,
                this.canvas.width,
                this.canvas.height
            );
        }
        if (e.pointerType === "pen") {
            // This event was triggered by a stylus
            this.#updateStatus("stylus used");
            if (e.button === 2) {
                this.#updateStatus(
                    "Stylus barrel button (right-click equivalent) was pressed."
                );
            } else if (e.button === 3) {
                this.#updateStatus(
                    "Stylus eraser or another button was pressed."
                );
            } else {
                this.#updateStatus("Stylus tip was pressed.");
            }
        } else {
            this.#updateStatus("not a stylus");
        }
    }

    draw(e) {
        if (!this.isDrawing || !this.imageLoaded) return;

        const coords = this.getCanvasCoordinates(e);

        if (this.currentMode === "free") {
            this.ctx.lineTo(coords.x, coords.y);
            this.ctx.stroke();
        } else if (this.currentMode === "rectangle") {
            // Restore the saved image data and draw rectangle
            this.ctx.putImageData(this.savedImageData, 0, 0);
            this.ctx.strokeRect(
                this.startX,
                this.startY,
                coords.x - this.startX,
                coords.y - this.startY
            );
        } else if (this.currentMode === "ellipse") {
            // Restore the saved image data and draw ellipse
            this.ctx.putImageData(this.savedImageData, 0, 0);
            this.drawEllipse(
                this.startX,
                this.startY,
                coords.x,
                coords.y
            );
        }
    }

    stopDrawing() {
        if (this.isDrawing) {
            this.isDrawing = false;
            this.savedImageData = null;
        }
    }

    drawEllipse(x1, y1, x2, y2) {
        const centerX = (x1 + x2) / 2;
        const centerY = (y1 + y2) / 2;
        const radiusX = Math.abs(x2 - x1) / 2;
        const radiusY = Math.abs(y2 - y1) / 2;

        this.ctx.beginPath();
        this.ctx.ellipse(
            centerX,
            centerY,
            radiusX,
            radiusY,
            0,
            0,
            2 * Math.PI
        );
        this.ctx.stroke();
    }

    resetCanvas() {
        if (!this.imageLoaded) {
            this.#updateStatus("No image to reset!");
            return;
        }

        // Redraw the original image
        this.ctx.clearRect(
            0,
            0,
            this.canvas.width,
            this.canvas.height
        );
        if (this.backgroundImage) {
            const canvasAspect =
                this.canvas.width / this.canvas.height;
            const imageAspect =
                this.backgroundImage.width /
                this.backgroundImage.height;

            let drawWidth,
                drawHeight,
                offsetX = 0,
                offsetY = 0;

            if (imageAspect > canvasAspect) {
                drawWidth = this.canvas.width;
                drawHeight = drawWidth / imageAspect;
                offsetY = (this.canvas.height - drawHeight) / 2;
            } else {
                drawHeight = this.canvas.height;
                drawWidth = drawHeight * imageAspect;
                offsetX = (this.canvas.width - drawWidth) / 2;
            }

            this.ctx.drawImage(
                this.backgroundImage,
                offsetX,
                offsetY,
                drawWidth,
                drawHeight
            );
        }

        this.#updateStatus("Canvas reset to original image.");
    }

    #updateStatus(message) {
        //document.getElementById("status").textContent = message;
        console.log(message);
    }
}
this.DrawingWidget = DrawingWidget;

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
    genBlob: async function (idCanvas, blobName) {
        this._idWidget = idCanvas;
        this._blobName = blobName;
        console.log(`genBlob start: store blob from ${idCanvas} to ${blobName}`);
        const canvas = document.getElementById(idCanvas);
        const opStatus = new OpStatus();
        opStatus.status = 1;
        if (!canvas) {
            opStatus.message = `Canvas with id ${idCanvas} not found`;
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