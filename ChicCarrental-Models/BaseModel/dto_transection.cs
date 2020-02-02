using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChicCarrental_Models.BaseModel
{
    public class dto_transection : tb_transection
    {
        public string Branch_name { get; set; }

        public string Car_name { get; set; }

        public tb_car car { get; set; }

        public List<tb_optional> optional { get; set; }

        public tb_customer customer { get; set; }

        public tb_branch pickuplocation { get; set; }

        public tb_branch dropofflocation { get; set; }

        public tb_car_price car_price { get; set; }
    }
}
