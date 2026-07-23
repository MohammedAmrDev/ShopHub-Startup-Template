$(document).ready(function () {

   $("#mytable").DataTable({
      ajax: {
         url: "/UserManagement/GetData",
         type: "POST",
         dataSrc: "data",
      },
      columns: [
         { data: "username", name: "Username", autowidth: true },
         { data: "email", name: "Email", autowidth: true },
         { data: "role", name: "Role", autowidth: true, orderable: false },
         {
            data: "isLocked",
            name: "IsLocked",
            render: function (data) {
               return `<span class="badge status-badge ${data ? 'bg-danger' : 'bg-success'}">${data ? 'Locked' : 'Active'}</span>`;
            },
            autowidth: true,
            orderable: false
         },
         {
            data: "id",
            render: function (data, type, row) {
               return `
                        <button onclick="toggleRole('${data}')" class="action-btn btn-primary">
                           <i class="fa-solid ${row.role == "Admin" ? "fa-user" : "fa-user-tie"} me-1"></i>
                        </button>
                        <button onclick="toggleLockUser('${data}')" class="action-btn ${row.isLocked ? "btn-success" : "btn-danger"}">
                           <i class="fa-solid ${row.isLocked ? "fa-unlock" : "fa-lock"} me-1"></i>
                        </button>
                        <button onclick="deleteUser('${data}')" class="action-btn btn-danger">
                           <i class="fa-solid fa-trash"></i>
                        </button>
                     `;
            },
            orderable: false
         }
      ],
      serverSide: true,
      lengthChange: false,
      pageLength: 5
   });
});

function deleteUser(id) {
   Swal.fire({
      title: 'Are you sure?',
      text: "You won't be able to revert this!",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Yes, delete it!'
   }).then((result) => {
      if (result.isConfirmed) {
         $.ajax({
            type: "DELETE",
            url: `/UserManagement/Delete/${id}`,
            success: function (data) {
               if (data.success) {
                  toastr.success(data.message);
                  $('#mytable').DataTable().ajax.reload();
               }
               else {
                  toastr.error(data.message);
               }
            },
            error: function () { // Fires if status_codes are errors (e.g. 400, 401,.. )
               console.error('faild to delete');
            }
         })
      }
   });
}

function toggleLockUser(id) {
   Swal.fire({
      title: 'Are you sure?',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Yes'
   }).then((result) => {
      if (result.isConfirmed) {
         $.ajax({
            type: "POST",
            url: `/UserManagement/ToggleLock/${id}`,
            success: function (data) {
               if (data.success) {
                  toastr.success(data.message);
                  $('#mytable').DataTable().ajax.reload();
               }
               else {
                  toastr.error(data.message);
               }
            },
            error: function () { // Fires if status_codes are errors (e.g. 400, 401,.. )
               console.error('faild to delete');
            }
         })
      }
   });
}

function toggleRole(id) {
   Swal.fire({
      title: 'Are you sure?',
      text: "You want to change the role of the user!",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Confirm'
   }).then((result) => {
      if (result.isConfirmed) {
         $.ajax({
            type: "POST",
            url: `/UserManagement/ToggleRole/${id}`,
            success: function (data) {
               if (data.success) {
                  toastr.success(data.message);
                  $('#mytable').DataTable().ajax.reload();
               }
               else {
                  toastr.error(data.message);
               }
            },
            error: function () { // Fires if status_codes are errors (e.g. 400, 401,.. )
               console.error('faild to delete');
            }
         })
      }
   });
}