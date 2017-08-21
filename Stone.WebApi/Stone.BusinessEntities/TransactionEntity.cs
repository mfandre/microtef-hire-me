using Stone.BusinessEntities.Attributes;
using Stone.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stone.BusinessEntities
{
    public class TransactionEntity : Entity
    {
        public enum TransactionReturnEnum
        {
            [StringValue("Aprovada")]
            Aprovado = 1,
            [StringValue("Transação Negada")]
            Transação_negada = 2,
            [StringValue("Saldo Insuficiente")]
            Saldo_insuficiente = 3,
            [StringValue("Valor Inválido")]
            Valor_invalido = 4,
            [StringValue("Cartão Bloqueado")]
            Cartao_bloqueado = 5,
            [StringValue("Cartão Vencido")]
            Cartao_vencido = 6,
            [StringValue("Limite Excedido")]
            Limite_excedido = 7,
            [StringValue("Número de caracteres na senha está incorreto")]
            Erro_no_tamanho_da_senha = 8,
            [StringValue("Senha Incorreta")]
            Senha_invalida = 9,
            [StringValue("Cartão Inválido")]
            Cartao_invalido = 10,
        }

        /// <summary>
        /// Chip ou tarja magnética
        /// </summary>
        [Required]
        public Decimal Amount { get; set; }

        [Required]
        public TransactionTypeEntity TransactionType { get; set; }

        //public int IdTransactionType { get; set; }

        [Required]
        [SensitiveData]
        public CardEntity Card { get; set; }

        //public int IdCard { get; set; }

        [Required]
        public int Number { get; set; }

        /// <summary>
        /// Data hora da Transacação
        /// </summary>
        [Required]
        public DateTime TransactionDate { get; set; }

        /// <summary>
        /// Código que identifica unicamente de qual "maquina" (clienteApp) enviou a transação
        /// </summary>
        [Required]
        public Guid ClientCode { get; set; }

        public TransactionReturnEnum TransactionReturn { get; set; }

    }
}
