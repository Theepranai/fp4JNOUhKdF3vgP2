using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChicCarrental_Models.BaseModel
{
    public class tb_car_price
    {
        public int price_id { get; set; }

        public int car_branch_id { get; set; }

        public int numdate { get; set; }

        public System.Nullable<System.DateTime> start_date { get; set; }

        public System.Nullable<System.DateTime> end_date { get; set; }

        public System.Nullable<decimal> price_cost { get; set; }

        public System.Nullable<decimal> price { get; set; }

        public string price_code { get; set; }

        public System.Nullable<decimal> paynow { get; set; }

        public string paynow_type { get; set; }

        public System.Nullable<System.DateTime> add_date { get; set; }

        public System.Nullable<System.DateTime> update_date { get; set; }

        public string name { get; set; }

        public string Branch_Name { get; set; }

    }
}
