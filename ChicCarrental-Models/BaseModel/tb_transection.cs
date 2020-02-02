using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChicCarrental_Models.BaseModel
{
    public class tb_transection
    {
        public int ID { get; set; }

        public System.Nullable<int> Transection_ID { get; set; }

        public string Payment_Ref { get; set; }

        public string Payment_Type { get; set; }

        public string Payment_status { get; set; }

        public System.Nullable<int> Cancel_transection { get; set; }

        public int Car_Branch_ID { get; set; }

        public System.Nullable<int> PickUpLoc { get; set; }

        public System.Nullable<int> DropOffLoc { get; set; }

        public System.Nullable<System.DateTime> PickUpDatetime { get; set; }

        public System.Nullable<System.DateTime> DropOffDatetime { get; set; }

        public System.Nullable<int> Price_ID { get; set; }

        public System.Nullable<int> Numdate { get; set; }

        public System.Nullable<decimal> Sub_Total { get; set; }

        public System.Nullable<decimal> Vat_Total { get; set; }

        public System.Nullable<decimal> Discount_Total { get; set; }

        public System.Nullable<decimal> Total_Price { get; set; }

        public System.Nullable<int> Customer_ID { get; set; }

        public string Comment { get; set; }

        public string Status { get; set; }

        public int Sendmail { get; set; }

        public System.DateTime Add_Date { get; set; } = DateTime.Now;

        public System.DateTime Update_Date { get; set; } = DateTime.Now;
    }
}
