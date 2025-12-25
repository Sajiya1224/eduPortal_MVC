var courseDataTable;   
var courseDataTable1;  
var courseDataTable2;  

$(function () {
    LoadCourseDataTable();
});

function LoadCourseDataTable() {
    if ($('#tblCourseData').length && !$.fn.DataTable.isDataTable('#tblCourseData')) {
        courseDataTable = $('#tblCourseData').DataTable({
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
                                    <a  onclick="Delete('/instructor/course/delete/${data}')" class="btn btn-danger mx-2">
                                        <i class="bi bi-trash3-fill"> Delete</i>
                                    </a>
                                </div>`;
                    },
                    width: "40%"
                }
            ]
        });
    }


    if ($('#tblCourseData1').length && !$.fn.DataTable.isDataTable('#tblCourseData1')) {
        courseDataTable1 = $('#tblCourseData1').DataTable({
            "ajax": { url: '/instructor/course/getall' },
            "columns": [
                { data: 'title', width: "25%" },
                { data: 'duration', width: "15%" },
                { data: 'level', width: "15%" },
                { data: 'isPublished', width: "15%" },
                { data: 'applicationUserId', width: "20%" },
                {
                    data: 'id',
                    render: function (data) {
                        return `<div class="w-75 btn-group" role="group">
                                    <a href="/instructor/course/upsertVideo/${data}" class="btn btn-secondary mx-2">
                                        <i class="bi bi-eye-fill"> View</i>
                                    </a>
                                </div>`;
                    },
                    width: "100%"
                }
            ]
        });
    }


    if ($('#tblCourseData2').length && !$.fn.DataTable.isDataTable('#tblCourseData2')) {
        courseDataTable2 = $('#tblCourseData2').DataTable({
            "ajax": { url: '/instructor/course/getall?take=4' },
            "columns": [
                { data: 'title', width: "15%" },
                { data: 'duration', width: "15%" },
                { data: 'level', width: "10%" },
                { data: 'isPublished', width: "10%" },
                { data: 'applicationUserId', width: "10%" },
              
            ]
        });
    }
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
                    // Reload both tables if they exist
                    if (courseDataTable) {
                        courseDataTable.ajax.reload();
                    }
                    if (courseDataTable1) {
                        courseDataTable1.ajax.reload();
                    }
                    if (courseDataTable2) {
                        courseDataTable2.ajax.reload();
                    }
                    toastr.success(data.message);
                }
            })
        }
    });
}
