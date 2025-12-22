var enrollDataTable;   // For tblenrollData
var enrollDataTable1;  // For tblenrollData1

$(function () {
    LoadDataTable();
});

function LoadDataTable() {
    if ($('#tblenrollData').length && !$.fn.DataTable.isDataTable('#tblenrollData')) {
        enrollDataTable = $('#tblenrollData').DataTable({
            "ajax": { url: '/admin/enrollement/enrollementDetail' },
            "columns": [
                { data: 'orderHeader.id', width: "10%" },
                { data: 'orderHeader.applicationUsers.name', width: "15%" },
                { data: 'orderHeader.applicationUsers.email', width: "20%" },
                { data: 'vCourse.title', width: "25%" },
                { data: 'vCourse.applicationUser.name', width: "10%" },
                { data: 'vCourse.price', width: "10%" },
                {
                    data: 'id',
                    render: function (data) {
                        return `<div class="w-75 btn-group" role="group">
                                    <a href="/Instructor/course/upsertVideo/${data}" class="btn btn-secondary mx-2">
                                        <i class="bi bi-pencil-square"> Edit</i>
                                    </a>
                                </div>`;
                    },
                    width: "10%"
                }
            ]
        });
    }

    if ($('#tblenrollData1').length && !$.fn.DataTable.isDataTable('#tblenrollData1')) {
        enrollDataTable1 = $('#tblenrollData1').DataTable({
            "ajax": {
                url: '/admin/enrollement/enrollementDetail?take=4'
            },
            "columns": [
                { data: 'orderHeader.id', width: "10%" },
                { data: 'orderHeader.applicationUsers.name', width: "15%" },
                { data: 'orderHeader.applicationUsers.email', width: "20%" },
                { data: 'vCourse.title', width: "25%" },
                { data: 'vCourse.applicationUser.name', width: "10%" },
                { data: 'vCourse.price', width: "10%" }
            ]
        });
    }
}
