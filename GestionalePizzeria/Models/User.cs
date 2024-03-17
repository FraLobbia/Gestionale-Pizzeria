using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionalePizzeria.Models
{
    public class User
    {
        //=======================================================================================================
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdUser { get; set; }
        //=======================================================================================================
        [Required]
        public string Name { get; set; }
        //=======================================================================================================
        [Required]
        public string Surname { get; set; }
        //=======================================================================================================
        public string Address { get; set; }
        //=======================================================================================================
        [DataType(DataType.EmailAddress)]
        [Required]
        public string Email { get; set; }
        //=======================================================================================================
        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }
        //=======================================================================================================
        public string Role { get; set; }
        //=======================================================================================================
        public string CompleteName
        {
            get
            {
                return Name + " " + Surname;
            }
        }
        //=======================================================================================================
        public virtual List<Order> Orders { get; set; }
    }
}