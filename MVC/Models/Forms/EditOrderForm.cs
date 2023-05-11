using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace projekt.Models.Forms
{
    public class EditOrderForm
    {
        [Required]
        [Display(Name = "Jméno")]
        public string fName { get; set; }
        [Required]
        [Display(Name = "Příjmení")]
        public string lName { get; set; }
        [Required]
        [Display(Name = "Stav objednávky")]
        public string state { get; set; }
        [Required]
        [Display(Name = "Celková cena")]
        public double totalPrice { get; set; }
        [Required]
        public List<CartItemForm> items { get; set; }

    }
}
