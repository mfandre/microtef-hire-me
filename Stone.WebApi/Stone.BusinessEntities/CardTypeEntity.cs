using Stone.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stone.BusinessEntities
{
    public class CardTypeEntity : Entity
    {
        public enum Types : int
        {
            [StringValue("Chip")]
            Chip = 1,
            [StringValue("Tarja Magnética")]
            Tarja = 2
        } 

        /// <summary>
        /// Chip ou tarja magnética
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
