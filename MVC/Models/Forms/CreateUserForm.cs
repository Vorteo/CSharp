using System.ComponentModel.DataAnnotations;

namespace projekt.Models.Forms
{
    public class CreateUserForm
    {
       public User user { get; set; }
       public Address address { get; set; }
    }
}
