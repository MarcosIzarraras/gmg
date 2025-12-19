const form = document.getElementById('frm-update-product');
const dropImages = new DragDrop('#drop-files');

form.addEventListener('submit', async (event) => {
    event.preventDefault();
    var formData = new FormData(event.target);
    dropImages.imageIdsToDelete.forEach((imgId, index) => {
        formData.append(`ImageIdsToDelete[${index}]`, imgId);
    });

    requestManager.submitFormData(formData, event.target.method, event.target.action, 'update-product', '#btn-update-product');
});