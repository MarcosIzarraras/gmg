var table = new Table({
    selector: '#tbl-container',
    url: 'products/pagination',
    columns: [
        { data: 'id', title: 'Id'},
        { data: 'name', title: 'Name' },
        { data: 'description', title: 'Description' },
        { data: 'productType.name', title: 'Product Type' },
        { data: 'price', title: 'Price' },
        { data: 'stock', title: 'Stock' },
    ],
    actions: {
        edit: (data) => { window.location.href = `products/detail?id=${ data.id }`; }
    },
    afterRender: () => {
        if (lucide) lucide.createIcons();
    }
});