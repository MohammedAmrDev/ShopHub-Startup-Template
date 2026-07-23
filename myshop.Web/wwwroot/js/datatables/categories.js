$(document).ready(function () {

   $("#mytable").DataTable({
      ajax: {
         url: "/Category/GetData",
         type: "POST",
         dataSrc: "data",
      },
      columns: [
         { data: "name", name: "Name", autowidth: true },
         { data: "description", name: "Description", autowidth: true },
         {
            data: "createdTime",
            name: "CreatedTime",
            render: function (data, type) {
               const date = new Date(data);
               if (type === "sort")
                  return date.getTime();
               if (type === "display")
                  return date.toLocaleString();
               return date;
            },
            autowidth: true
         },
         {
            data: "id",
            render: function (data) {
               return `
                        <a href="/Category/Edit/${data}" class="btn btn-success btn-sm">
                              <i class="fa-solid fa-pen"></i>
                        </a>

                        <button onclick="deleteCategory(${data})" class="btn btn-danger btn-sm">
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

function deleteCategory(id) {
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
            url: `/Category/Delete/${id}`,
            success: function (data) {
               if (data.success) {
                  toastr.success(data.message);
                  $('#mytable').DataTable().ajax.reload();
               }
               else {
                  toastr.error(data.message);
               }
            },
            error: function (status, error) { // Fires if status_codes are errors (e.g. 400, 401,.. )
               console.error('faild to delete');
            }
         })
      }
   });
}