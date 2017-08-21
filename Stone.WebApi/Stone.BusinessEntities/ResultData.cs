using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stone.BusinessEntities
{
    public class ResultData<T>
    {
        public int Count
        {
            get
            {
                if (Data != null)
                    return Data.Count();
                return 0;
            }
        }

        public string Msg { get; set; }

        public bool Success { get; set; }
        public IEnumerable<T> Data { get; set; }
    }
}
