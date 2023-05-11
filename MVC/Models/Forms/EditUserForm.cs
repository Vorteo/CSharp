using System.ComponentModel.DataAnnotations;

namespace projekt.Models.Forms
{
    public class EditUserForm
    {
        [Display(Name = "Jméno")]
        [Required(ErrorMessage = "Jméno musí být zadáno")]
        public string fName { get; set; }

        [Display(Name = "Příjmení")]
        [Required(ErrorMessage = "Příjmení musí být zadáno")]
        public string lName { get; set; }

        [Display(Name = "Telefon")]
        [Required(ErrorMessage = "Telefon musí být zadáno")]
        [RegularExpression(@"^([0-9]{3})\s([0-9]{3})\s([0-9]{3})\s([0-9]{3})$",ErrorMessage = "Nespravný formát")]
        public string phone { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email musí být zadáno")]
        public string email { get; set; }

        [Display(Name = "Typ")]
        public int type { get; set; }

        [Display(Name = "Ulice")]
        [Required(ErrorMessage = "Ulice musí být zadána")]
        public string street { get; set; }

        [Display(Name = "Město")]
        [Required(ErrorMessage = "Město musí být zadáno")]
        public string city { get; set; }

        [Display(Name = "PSČ")]
        [Required(ErrorMessage = "PSČ musí být zadáno")]
        public string postalCode { get; set; }

        [Display(Name = "Stát")]
        [Required(ErrorMessage = "Země musí být zadána")]
        public string country { get; set; }
    }
}
