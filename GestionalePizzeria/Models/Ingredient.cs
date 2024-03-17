using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace GestionalePizzeria.Models
{
    public class Ingredient
    {

        // ___   _  _    ___   ___   ___   ___    ___   ___   _  _   _____   ___ 
        //|_ _| | \| |  / __| | _ \ | __| |   \  |_ _| | __| | \| | |_   _| | __|
        // | |  | .` | | (_ | |   / | _|  | |) |  | |  | _|  | .` |   | |   | _| 
        //|___| |_|\_|  \___| |_|_\ |___| |___/  |___| |___| |_|\_|   |_|   |___|

        //=======================================================================================================
        [Key]
        public int IdIngredient { get; set; }
        //=======================================================================================================
        [Required]
        public string Name { get; set; }

        public virtual List<ProductDetail> ProductDetails { get; set; }
    }
}