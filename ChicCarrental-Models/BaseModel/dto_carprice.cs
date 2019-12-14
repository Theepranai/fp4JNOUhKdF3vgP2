using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChicCarrental_Models.BaseModel
{
    public class dto_carprice
    {
        public int A_ID { get; set; }

        public int Branch_ID { get; set; }

        public int Car_ID { get; set; }

        public string Teaser { get; set; }

        public decimal Display_Price { get; set; }

        public decimal Default_Price { get; set; }

        public string Status { get; set; }

        public int Quantity { get; set; }

        public System.DateTime Add_Date { get; set; }

        public System.DateTime Update_Date { get; set; }

        public string Branch_Name { get; set; }

        public int image { get; set; }

        public string name { get; set; }

        public string feature { get; set; }

        public string tranmission { get; set; }

        public int Car_Branch_ID { get; set; }

        public tb_car_price price { get; set; }
    }
}
