using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChicCarrental_Models.BaseModel
{
    public class tb_optional
    {
        public int A_ID { get; set; }

        public string GroupName { get; set; }

        public string Name { get; set; }

        public string Teaser { get; set; }

        public decimal Price { get; set; }

        public string Status { get; set; }

        //
        public string Item_Type { get; set; }

        public System.Nullable<decimal> Item_Price { get; set; }

        public decimal Sub_Total { get; set; }

        public decimal Vat_Total { get; set; }

        public decimal Total { get; set; }

    }
}
