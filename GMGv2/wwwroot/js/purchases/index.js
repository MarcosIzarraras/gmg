const table = new Table({
    selector: '#tbl-container',
    url: 'purchases/pagination',
    columns: [
        { data: 'id', title: 'Id' },
        { data: 'total', title: 'Total' },
        { data: 'createdAt', title: 'Create Date' },
    ]
});