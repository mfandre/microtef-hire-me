using Stone.BusinessEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stone.ClientApp.Model
{
    public class TransactionVM
    {
        [Display(Name = "Data Hora")]
        /// <summary>
        /// Data hora da transacao
        /// </summary>
        public DateTime TransactionDate { get; set; }

        [Display(Name = "Tipo")]
        /// <summary>
        /// debito ou credito
        /// </summary>
        public string Type { get; set; }

        [Display(Name = "Número Cartão")]
        /// <summary>
        /// Numero cartao
        /// </summary>
        public string CardNumber { get; set; }

        [Display(Name = "Valor")]
        /// <summary>
        /// Valor
        /// </summary>
        public decimal Amount { get; set; }

        [Display(Name = "Parcelas")]
        [DisplayName("Parcelas")]
        /// <summary>
        /// numero de parcelas
        /// </summary>
        public int Number { get; set; }

        [Display(Name = "Retorno")]
        public string TransactionReturn { get; set; }
    }
}
