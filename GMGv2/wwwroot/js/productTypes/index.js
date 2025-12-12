const mdlProductTypeBody = document.getElementById('mdl-product-type-body');
const tableContainer = document.getElementById('table-container');
const btnNewCategory = document.getElementById('btn-new-category');
const modalProductType = new Modal('#mdl-product-type');
const table = new Table({
    selector: '#tbl-container',
    url: 'productTypes/pagination',
    columns: [
        { title: 'Id', data: 'id' },
        { title: 'Name', data: 'name' },
        { title: 'Create Date', data: 'createdAt' },
        { title: 'Update Date', data: 'updatedAt' },
        { title: 'Actions', data: 'id', render: (data) => `<div class="actions-cell"><button data-id="edit" class="btn btn-icon btn-ghost"><i data-lucide="pencil" style="width:16px;height:16px"></i></button></div>` }
    ],
    afterRender: () => {
        if (lucide) lucide.createIcons();
    },
    onRowClick: (data, e) => {
        if (e.target.closest('button').dataset.id === "edit")
            EditProductType(data.id);
    }
});

mdlProductTypeBody.addEventListener("submit", SubmitProductType);
btnNewCategory.addEventListener('click', NewProductType)

function NewProductType() {
    modalProductType.open();
    EditProductType(null);
}


async function SubmitProductType(e) {
    e.preventDefault();

    const form = e.target;
    const formData = new FormData(form);

    try {
        const response = await fetch(form.action, {
            method: "POST",
            body: formData
        });

        if (!response.ok) {
            notyf.error("Error in save product type");
        }
        else {
            notyf.success("Product type has been saved.");
            modalProductType.close();
            table.getData();
        }

    } catch (err) {
        console.error("Error en fetch:", err);
    }
}

async function EditProductType(productTypeId) {
    console.log(productTypeId);
    const response = await fetch(`ProductTypes/PartialForm?id=${productTypeId}`);
    if (response.ok) {
        mdlProductTypeBody.innerHTML = await response.text();
        modalProductType.open();
    }
}