using Stone.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stone.BusinessEntities
{
    public class TransactionTypeEntity : Entity
    {
        public enum Types : int
        {
            [StringValue("Crédito")]
            Credito = 1,
            [StringValue("Débito")]
            Debito = 2
        }

        /// <summary>
        /// Crédito ou Debito
        /// </summary>
        [Required]
        public string Name
        {
            get
            {
                Types type = (Types)Id;
                return type.GetStringValue();
            }
        }
    }
}
