var dataTable;


$(function () {
    LoadDataTable();
});

function LoadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/admin/product/getall' },
        "columns": [
            { data: 'title', width: "20%" },
            { data: 'price', width: "15%" },
            { data: 'duration', width: "15%" },
            { data: 'level', width: "10%" },
            { data: 'isPublished', width: "10%" },

            {
                data: 'id',
                render: function (data) {
                    return `<div class="w-75 btn-group" role="group">
                                <a href="/admin/product/upsert?id=${data}" class="btn btn-info mx-2">
                                    <i class="bi bi-pencil-square"> Edit</i>
                                </a>
                                <a onClick="Delete('/admin/product/delete/${data}')" class="btn btn-danger mx-2">
                                    <i class="bi bi-trash3-fill"> Delete</i>
                                </a>
                            </div>`;
                },
                width: "30%"
            }
        ]
    });
}