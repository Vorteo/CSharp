using System.ComponentModel.DataAnnotations;

namespace projekt.Models.Forms
{
    public class CartItemForm
    {
        [Required]
        public int productId { get; set; }

        [Required]
        [Display(Name ="Název")]
        public string productName { get; set; }

        [Required(ErrorMessage = "Musíš zadat množství")]
        [Display(Name = "Počet kusů")]
        public int count { get; set; }
    }
}
