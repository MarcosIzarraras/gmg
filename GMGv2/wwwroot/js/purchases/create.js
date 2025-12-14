let details = [];
let selectedProductId = null;
let selectedProductName = null;
let searchTimeout = null;

const productQuantity = document.getElementById('product-quantity');
const productPrice = document.getElementById('product-price');
const addProductBtn = document.getElementById('add-product-btn');
const purchaseForm = document.getElementById('purchase-form');
const detailsTableBody = document.getElementById('details-table-body');
const emptyRow = document.getElementById('empty-row');
const totalAmount = document.getElementById('total-amount');
const productSearch = new SelectSearch({
    selector: '#product-search',
    placeholder: "Search product...",
    label: 'Product',
    url: 'products/search',
    propText: 'name',
    propValue: 'id',
    valueCallback: setProduct
});

productQuantity.addEventListener('keydown', HandleEnterKey);
productPrice.addEventListener('keydown', HandleEnterKey);
addProductBtn.addEventListener('click', AddProduct);
purchaseForm.addEventListener('submit', SubmitPurchase);

function HandleEnterKey(e) {
    if (e.key === 'Enter') {
        e.preventDefault();
        AddProduct();
    }
}

function AddProduct() {
    if (!selectedProductId) {
        notyf.error('Please select a product');
        return;
    }

    const quantity = parseFloat(productQuantity.value);
    const price = parseFloat(productPrice.value);

    if (!quantity || quantity <= 0) {
        notyf.error('Please enter a valid quantity');
        return;
    }

    if (!price || price <= 0) {
        notyf.error('Please enter a valid price');
        return;
    }

    const existingIndex = details.findIndex(d => d.productId === selectedProductId);
    if (existingIndex >= 0) {
        notyf.error('This product is already in the list');
        return;
    }

    details.push({
        productId: selectedProductId,
        productName: selectedProductName,
        quantity: quantity,
        unitPrice: price
    });

    productSearch.value = '';
    productQuantity.value = '';
    productPrice.value = '';
    selectedProductId = null;
    selectedProductName = null;

    UpdateTable();
    productSearch.focus();
}

function RemoveDetail(index) {
    details.splice(index, 1);
    UpdateTable();
}

function UpdateTable() {
    if (details.length === 0) {
        emptyRow.style.display = '';
        detailsTableBody.querySelectorAll('tr:not(#empty-row)').forEach(row => row.remove());
        UpdateTotal();
        return;
    }

    emptyRow.style.display = 'none';
    detailsTableBody.innerHTML = '';

    details.forEach((detail, index) => {
        const subtotal = detail.quantity * detail.unitPrice;
        const row = document.createElement('tr');
        row.innerHTML = `
            <td>${detail.productName}</td>
            <td>${detail.quantity}</td>
            <td>$${detail.unitPrice.toFixed(2)}</td>
            <td>$${subtotal.toFixed(2)}</td>
            <td>
                <button type="button" class="btn btn-danger btn-sm" onclick="RemoveDetail(${index})">Remove</button>
            </td>
        `;
        detailsTableBody.appendChild(row);
    });

    UpdateTotal();
}

function UpdateTotal() {
    const total = details.reduce((sum, detail) => sum + (detail.quantity * detail.unitPrice), 0);
    totalAmount.textContent = `$${total.toFixed(2)}`;
}

async function SubmitPurchase(e) {
    e.preventDefault();

    if (details.length === 0) {
        notyf.error('Please add at least one product');
        return;
    }

    const formData = new FormData();
    details.forEach((detail, index) => {
        formData.append(`Details[${index}].ProductId`, detail.productId);
        formData.append(`Details[${index}].Quantity`, detail.quantity);
        formData.append(`Details[${index}].UnitPrice`, detail.unitPrice);
    });

    try {
        const response = await fetch('/Purchases/Create', {
            method: 'POST',
            body: formData
        });

        if (response.ok) {
            notyf.success('Purchase created successfully');
            setTimeout(() => {
                window.location.href = '/Purchases/Index';
            }, 1000);
        } else {
            notyf.error('Error creating purchase');
        }
    } catch (error) {
        console.error('Error:', error);
        notyf.error('Error creating purchase');
    }
}

function setProduct(data) {
    selectedProductId = data.id;
    selectedProductName = data.name;
    productPrice.value = data.price;
    productQuantity.focus();
}

window.RemoveDetail = RemoveDetail;