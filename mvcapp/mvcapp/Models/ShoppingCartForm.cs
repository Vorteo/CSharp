using System.ComponentModel.DataAnnotations;

namespace mvcapp.Models
{
    public class ShoppingCartForm
    {
        [Display(Name = "Jméno")]
        [Required(ErrorMessage = "musi byt zadano")]
        public string Name { get; set; }
        [Display(Name = "Email")]
        [Required]
        [EmailAddress(ErrorMessage = "Email má špatný formát")]
        public string Email { get; set; }
        [Display(Name = "Adresa")]
        [Required]
        public string Address { get; set; }
        [Display(Name ="Věk")]
        [Range(1, 100, ErrorMessage ="Věk musí být v rozmezi 1 - 100")]
        
        public string Age { get; set; }
    }
}
