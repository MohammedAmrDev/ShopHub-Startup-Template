$(document).ready(function () {

   $("#mytable").DataTable({
      ajax: {
         url: "/Product/GetData",
         type: "POST",
         dataSrc: "data",
      },
      columns: [
         { data: "name", name: "Name", autowidth: true },
         { data: "description", name: "Description", autowidth: true },
         { data: "price", name: "Price", autowidth: true },
         { data: "categoryName", name: "CategoryName", autowidth: true },
         {
            data: "id",
            render: function (data) {
               return `
                        <a href="/Product/Edit/${data}" class="btn btn-success btn-sm">
                              <i class="fa-solid fa-pen"></i>
                        </a>

                        <button onclick="deleteProduct(${data})" class="btn btn-danger btn-sm">
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

function deleteProduct(id) {
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
            url: `/Product/Delete/${id}`,
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