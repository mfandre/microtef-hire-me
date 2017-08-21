using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stone.BusinessEntities
{
    public class CardBrandEntity : Entity
    {
        /// <summary>
        /// Nome da Bandeira do cartão
        /// </summary>
        [Required]
        public string Name { get; set; }
    }
}
