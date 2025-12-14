const table = new Table({
    selector: '#tbl-container',
    url: 'sales/pagination',
    columns: [
        { data: 'id', title: 'Id' },
        { data: 'createdAt', title: 'Create Date' },
        { data: 'client', title: 'Client' },
        { data: 'total', title: 'Total' },
    ]
});