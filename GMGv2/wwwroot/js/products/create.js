const form = document.getElementById('frm-create-product');
form.addEventListener('submit', async (event) => {
    requestManager.submitForm(event.target, 'create-product', '#btn-create-product');
});