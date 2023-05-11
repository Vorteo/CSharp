using System.ComponentModel.DataAnnotations;

namespace projekt.Models
{
    [Table(Name = "Address")]
    public class Address
    {
        [PrimaryKey(Skip = true)]
        public int id { get; set; }

        [Display(Name = "Ulice")]
        [Required(ErrorMessage = "Ulice musí být zadáná")]
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
