using Stone.BusinessEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Stone.WebApi.Models
{
    public class CardEntityVM : CardEntity
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Required")]
        [DisplayName("Bandeira")]
        public int CardBrandId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Required")]
        [DisplayName("Tipo")]
        public int CardTypeId { get; set; }
    }
}