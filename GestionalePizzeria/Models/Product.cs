
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace GestionalePizzeria.Models
{
    public class Product
    {

        // ___   ___    ___    ___    _   _    ___   _____ 
        //| _ \ | _ \  / _ \  |   \  | | | |  / __| |_   _|
        //|  _/ |   / | (_) | | |) | | |_| | | (__    | |  
        //|_|   |_|_\  \___/  |___/   \___/   \___|   |_|  
        //=======================================================================================================
        [Key]
        public int IdProduct { get; set; }
        //=======================================================================================================
        [Required]
        public string Nome { get; set; }
        //=======================================================================================================

        [Required]
        public string Immagine { get; set; }
        //=======================================================================================================

        [Required]
        public decimal Prezzo { get; set; }
        //=======================================================================================================
        [Required]

        public TimeSpan TempoPreparazione { get; set; }
        //======================================================================================================= 
        // necessaria per il form di creazione
        [NotMapped]
        public int[] SelectedIngredientIDs { get; set; }
        //=======================================================================================================
        [Display(Name = "Ingredienti")]
        public virtual List<ProductDetail> ProductDetails { get; set; }
        public virtual List<OrderDetail> OrderDetails { get; set; }

    }
}