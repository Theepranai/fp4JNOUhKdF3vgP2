using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChicCarrental_Models.BaseModel
{
    public class tb_transection_detail
    {
        public int ID { get; set; }

        public int Transection { get; set; }

        public int Item_ID { get; set; }

        public string Item_Type { get; set; }

        public System.Nullable<decimal> Item_Price { get; set; }

        public decimal Sub_Total { get; set; }

        public decimal Vat_Total { get; set; }

        public decimal Total { get; set; }

        public string Status { get; set; }

        public System.DateTime Add_Date { get; set; } = DateTime.Now;

        public System.DateTime Update_Date { get; set; } = DateTime.Now;

    }
}
