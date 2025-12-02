var dataTable;


$(function () {
    LoadDataTable();
});

function LoadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/instructor/course/getall' },
        "columns": [
            { data: 'title', width: "15%" },
            { data: 'duration', width: "15%" },
            { data: 'level', width: "10%" },
            { data: 'isPublished', width: "10%" },
            { data: 'applicationUserId', width: "10%" },
            {
                data: 'id',
                render: function (data) {
                    return `<div class="w-75 btn-group" role="group">
                                <a href="/instructor/course/upsertVideo/${data}" class="btn btn-secondary mx-2">
                                    <i class="bi bi-pencil-square"> Edit</i>
                                </a>
                                <a  OnClick="Delete('/instructor/course/delete/${data}')" class="btn btn-danger mx-2">
                                    <i class="bi bi-trash3-fill"> Delete</i>
                                </a>
                            </div>`;
                },
                width: "40%"
            }
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
