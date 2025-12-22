var dataTable;


$(function () {
    LoadCatDataTable();
});

function LoadCatDataTable() {
    dataTable = $('#tblCatData').DataTable({
        "ajax": { url: '/admin/category/getall' },
        "columns": [
            { data: 'name', width: "33%" },
            { data: 'createdAt', width: "33%" },
            {
                data: 'id',
                render: function (data) {
                    return `<div class="w-75 btn-group" role="group">
                                <a href="/admin/category/edit/${data}" class="btn btn-secondary mx-2">
                                    <i class="bi bi-pencil-square"> Edit</i>
                                </a>
                                <a  OnClick="Delete('/admin/category/delete/${data}')" class="btn btn-danger mx-2">
                                    <i class="bi bi-trash3-fill"> Delete</i>
                                </a>
                            </div>`;
                },
                width: "33%"
            }
        ]
    });

    dataTable = $('#tblCatData1').DataTable({
        "ajax": { url: '/admin/category/getall?take=4' },
        "columns": [
            { data: 'name', width: "33%" },
            { data: 'createdAt', width: "33%" },

        ]
    });
}



function Delete(url) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    dataTable.ajax.reload();
                    toastr.success(data.message);
                }
            })
        }
    });
}
