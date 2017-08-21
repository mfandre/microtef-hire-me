using Stone.BusinessEntities.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stone.BusinessEntities
{
    public class CardEntity : Entity
    {
        /// <summary>
        /// Nome do portador do cartão
        /// </summary>
        [Required]
        [DisplayName("Nome")]
        public string CardholderName { get; set; }

        /// <summary>
        /// Os números que são impressos no cartão, podendo variar entre 12 à 19 dígitos
        /// </summary>
        [Required]
        [StringLength(19, MinimumLength=12, ErrorMessage = "CardNumber length between 12-19 characters")]
        [DisplayName("Número")]
        public string Number { get; set; }

        /// <summary>
        /// Data de expiração do cartão
        /// </summary>
        [Required]
        [DisplayName("Data Validade")]
        public DateTime ExpirationDate { get; set; }

        /// <summary>
        /// Bandeira do cartão
        /// </summary>
        [Required]
        [DisplayName("Bandeira")]
        public CardBrandEntity CardBrand { get; set; }

        //public int IdCardBrand { get; set; }

        /// <summary>
        /// Senha do cartão
        /// </summary>
        [Required]
        [SensitiveData]
        [DisplayName("Senha")]
        [StringLength(6, MinimumLength = 4, ErrorMessage = "Password length between 4-6 characters")]
        public string Password { get; set; }

        /// <summary>
        /// Chip ou tarja magnética
        /// </summary>
        [Required]
        [DisplayName("Tipo")]
        public CardTypeEntity CardType { get; set; }

        //public int IdCardType { get; set; }

        /// <summary>
        /// Chip ou tarja magnética
        /// </summary>
        [Required]
        [DisplayName("Tem Senha?")]
        public bool HasPassword { get; set; }

        /// <summary>
        /// Limite do carão
        /// </summary>
        [Required]
        [DisplayName("Limite")]
        public Decimal Limit { get; set; }

        /// <summary>
        /// Limite do carão
        /// </summary>
        [Required]
        [DisplayName("Limite Aprisionado")]
        public Decimal LimitUsed { get; set; }

        /// <summary>
        /// Saldo atual
        /// </summary>
        [Required]
        [DisplayName("Saldo")]
        public Decimal Balance { get; set; }

        /// <summary>
        /// Cartão bloqueado?
        /// </summary>
        [Required]
        [DisplayName("Bloqueado?")]
        public bool Blocked { get; set; }

        /// <summary>
        /// Controle de Tentativas para bloquear
        /// </summary>
        [Required]
        [DisplayName("Tentativas")]
        public int Attempts { get; set; }
    }
}
