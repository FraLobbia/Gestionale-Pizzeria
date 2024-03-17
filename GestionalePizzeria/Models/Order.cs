using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionalePizzeria.Models
{

    public class Order
    {
        //  ___    ___   ___    ___   ___ 
        // / _ \  | _ \ |   \  | __| | _ \
        //| (_) | |   / | |) | | _|  |   /
        // \___/  |_|_\ |___/  |___| |_|_\
        //=======================================================================================================
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdOrder { get; set; }
        //=======================================================================================================
        [DataType(DataType.Date)]
        public DateTime OrarioOrdine { get; set; }
        //=======================================================================================================
        [Required]
        public string Address { get; set; }
        //=======================================================================================================
        [DataType(DataType.MultilineText)]
        public string Note { get; set; }
        //=======================================================================================================
        [Display(Name = "Spedito?")]
        public bool isDelivered { get; set; }
        //=======================================================================================================
        [DataType(DataType.Currency)]
        public decimal Total
        {
            get
            {
                decimal total = 0;
                if (Products == null) return total;

                foreach (var item in Products)
                {
                    total += item.Product.Prezzo;
                }
                return total;
            }
        }
        //=======================================================================================================
        [DataType(DataType.Time)]
        public DateTime OrarioConsegna
        {
            get
            {
                // inizializzo l'orario di consegna con l'orario dell'ordine per
                // poi aggiungere i tempi di preparazione dei prodotti
                DateTime orarioConsegna = OrarioOrdine;

                if (Products == null) return orarioConsegna;

                foreach (var item in Products)
                {
                    // aggiungo il tempo di preparazione di ogni prodotto
                    orarioConsegna = orarioConsegna.Add(item.Product.TempoPreparazione);
                }
                return orarioConsegna;
            }
        }
        //=======================================================================================================
        public virtual List<OrderDetail> Products { get; set; }
        [Required]
        [ForeignKey("User")]
        public int IdUser { get; set; }
        public virtual User User { get; set; }
    }
}