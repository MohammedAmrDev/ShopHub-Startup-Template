using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using myshop.Models.Entities;
using myshop.Models.IdentityEntities;
using System.ComponentModel.DataAnnotations.Schema;

namespace myshop.Entities.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        [ValidateNever]
        public Product Product { get; set; }

        public int Count { get; set; }

        public string ApplicationUserId { get; set; }

        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }
    }
}
