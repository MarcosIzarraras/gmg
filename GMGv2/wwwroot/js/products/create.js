const form = document.getElementById('frm-create-product');
const dropImages = new DragDrop('#drop-files');

form.addEventListener('submit', async (event) => {
    requestManager.submitForm(event.target, 'create-product', '#btn-create-product');
});