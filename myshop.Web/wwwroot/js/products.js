$(document).ready(function () {

    $("#mytable").DataTable({
        ajax: {
            url: "/Product/GetData",
            type: "GET",
            dataSrc: "data"
        },
        columns: [
            { data: "name" },
            { data: "description" },
            { data: "price" },
            { data: "categoryName" },
            {
                data: "id",
                render: function (id) {
                    return `
                        <a href="/Product/Edit/${id}" class="btn btn-success btn-sm">
                            <i class="fa-solid fa-pen"></i>
                        </a>

                        <button onclick="deleteProduct(${id})" class="btn btn-danger btn-sm">
                            <i class="fa-solid fa-trash"></i>
                        </button>
                    `;
                }
            }
        ],
        autoWidth: false,
        scrollX: true
    });
});

function deleteProduct(id) {
   if (!confirm('Are you sure you want to delete this product?')) return;

   $.ajax({
      url: `/Product/Delete/${id}`,
      type: 'DELETE',
      success: function (response) {
         if (response.success) {
            $('#mytable').DataTable().ajax.reload();
            console.log(response.message);
         } else {
            console.log(response.message);
         }
      },
      error: function () { // Fires if status_codes are errors (e.g. 400, 401,.. )
         console.error('faild to delete');
      }
   });
}