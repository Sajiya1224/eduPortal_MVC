var instructorDataTable;  
var instructorDataTable1; 
var tblUserData; 
var tblUserData1; 

$(function () {
    LoadInstructorDataTable();
});

function LoadInstructorDataTable() {
    if ($('#tblInstructorData').length && !$.fn.DataTable.isDataTable('#tblInstructorData')) {
        instructorDataTable = $('#tblInstructorData').DataTable({
            "ajax": { url: '/admin/admin/getInstructor' },
            "columns": [
                { data: 'name', width: "25%" },
                { data: 'email', width: "25%" },
                { data: 'gender', width: "10%" },
                { data: 'cv', width: "15%" },
                { data: 'demoVid', width: "15%" },
                {
                    data: 'id',
                    render: function (data) {
                        return `<div class="w-75 btn-group" role="group">
                                    <a href="/admin/admin/InstructorDetail/${data}" class="btn btn-secondary mx-2">
                                        <i class="bi bi-eye-fill"> View</i>
                                    </a>
                                </div>`;
                    },
                    width: "10%"
                }
            ]
        });
    }


    if ($('#tblUserData').length && !$.fn.DataTable.isDataTable('#tblUserData')) {
        userDataTable = $('#tblUserData').DataTable({
            ajax: { url: '/admin/admin/getUser' },
            columns: [
                { data: 'name', width: "25%" },
                { data: 'email', width: "15%" },
                { data: 'gender', width: "15%" },
                { data: 'role', width: "15%" },
                {
                    data: 'id',
                    render: function (data) {
                        return `
                            <div class="w-75 btn-group" role="group">
                                <a href="/admin/admin/InstructorDetail/${data}" 
                                   class="btn btn-secondary mx-2">
                                    <i class="bi bi-eye-fill"></i> View
                                </a>
                            </div>`;
                    },
                    width: "10%"
                }
            ]
        });
    }


    if ($('#tblInstructorData1').length && !$.fn.DataTable.isDataTable('#tblInstructorData1')) {
        instructorDataTable1 = $('#tblInstructorData1').DataTable({
            "ajax": {url: '/admin/admin/getInstructor?take=4'
            },
            "columns": [
                { data: 'name', width: "25%" },
                { data: 'email', width: "15%" },
                { data: 'gender', width: "10%" },
                { data: 'cv', width: "10%" },
                { data: 'demoVid', width: "10%" }
            ]
        });
    }

    if ($('#tblUserData1').length && !$.fn.DataTable.isDataTable('#tblUserData1')) {
        userDataTable1 = $('#tblUserData1').DataTable({
            ajax: {
                url: '/admin/admin/getUser?take=4',
                error: function (xhr, error, thrown) {
                    console.error('Error loading user data:', error);
                    console.error('Response:', xhr.responseText);
                }
            },
            columns: [
                { data: 'name', width: "25%" },
                { data: 'email', width: "15%" },
                { data: 'gender', width: "10%" },
                { data: 'role', width: "10%" }
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
        confirmButtonText: "Yes, reject it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (response) {
                    // Reload both DataTables if they exist
                    if (instructorDataTable) {
                        instructorDataTable.ajax.reload();
                    }
                    if (instructorDataTable1) {
                        instructorDataTable1.ajax.reload();
                    }
                    if (userDataTable) {
                        userDataTable.ajax.reload();
                    }
                    if (userDataTable1) {
                        userDataTable1.ajax.reload();
                    }
                    if (response.success) {
                        toastr.success(response.message);
                    } else {
                        toastr.error(response.message);
                    }
                },
                error: function () {
                    toastr.error('An error occurred while rejecting the instructor');
                }
            })
        }
    });
}