using projekt.Services;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace projekt.Models
{
    [Table(Name = "User")]
    public class User
    {
        [PrimaryKey(Skip = true)]
        public int id { get; set; }

        [Display(Name = "Jméno")]
       
        public string fName { get; set; }

        [Display(Name = "Příjmení")]
        public string lName { get; set; }

        [Display(Name = "Telefoní číslo")]
        public string phone { get; set; }

        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Email musí být ve správném formátu")]
        [Required(ErrorMessage = "Email musí být vyplněn")]
        public string email { get; set; }

        [Display(Name = "Typ účtu")]
        public int type { get; set; }

        [Display(Name = "Heslo")]
        [Required(ErrorMessage = "Heslo musí být vyplněno")]
        public string password { get; set; }

        [Display(Name = "Datum vytvoření")]
        public string dateOfCreation { get; set; }

        [ForeignKey]
        public int addressId { get; set; }

        public override string ToString()
        {
            return fName + " " + lName;
        }

        public async Task<bool> RegisterCheck()
        {
            if(lName == "" || fName == "" || email == "")
            {
                return false;
            }
            else
            {
                string regex = @"^[a-z0-9\.\-]+@[a-z0-9\.\-]+\.[a-z]{2,3}$";
                Regex reg = new Regex(regex, RegexOptions.IgnoreCase | RegexOptions.Compiled);
                if(!reg.IsMatch(email))
                {
                    return false;
                }
            }

            if(password == "" || password.Length < 7 )
            {
                return false;
            }
            else
            {
                int digit = 0, upper = 0;
                for(int c = 0; c < password.Length; c++)
                {
                    if(Char.IsDigit(password[c]))
                    {
                        digit++;
                    }
                    else if(Char.IsUpper(password[c]))
                    {
                        upper++;
                    }
                }

                if(digit < 2 || upper < 1)
                {
                    return false;
                }
            }
            if(phone.Length !=15 || phone == "")
            {
                return false;
            }

            int space = 0;
            for(int i=0; i<phone.Length; i++)
            {
                if(phone[i] == ' ')
                {
                    space++;
                }
            }

            if(space != 3)
            {
                return false;
            }

            var sameEmailUser = await ORM.Select<User>(Database.GetInstance().connection, "SELECT * FROM User WHERE email = @0", email);
            if(sameEmailUser != null)
            {
                return false;
            }

            return true;
        }
    }
}
