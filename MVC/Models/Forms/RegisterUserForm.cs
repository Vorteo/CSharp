using System.ComponentModel.DataAnnotations;

namespace projekt.Models.Forms
{
    public class RegisterUserForm
    {
        [Display(Name = "Jméno")]
        [Required(ErrorMessage = "Jméno musí být zadáno")]
        public string fName { get; set; }

        [Display(Name = "Příjmení")]
        [Required(ErrorMessage = "Přijmení musí být zadáno")]
        public string lName { get; set; }

        [Display(Name = "Telefon")]
        [Required(ErrorMessage = "Telefonní číslo musí být zadáno")]
        public string phone { get; set; }

        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Email musí být ve správném formátu")]
        [Required(ErrorMessage = "Email musí být zadáno")]
        public string email { get; set; }

        [Display(Name = "Heslo")]
        [Required(ErrorMessage = "Heslo musí být zadáno")]
        public string password { get; set; }

        [Display(Name = "Ulice")]
        [Required(ErrorMessage = "Ulice musí být zadáno")]
        public string street { get; set; }

        [Display(Name = "Město")]
        [Required(ErrorMessage = "Město musí být zadáno")]
        public string city { get; set; }

        [Display(Name = "PSČ")]
        [Required(ErrorMessage = "PSČ musí být zadáno")]
        public string postalCode { get; set; }

        [Display(Name = "Stát")]
        [Required(ErrorMessage = "Stát musí být zadán")]
        public string country { get; set; }
    }
}
