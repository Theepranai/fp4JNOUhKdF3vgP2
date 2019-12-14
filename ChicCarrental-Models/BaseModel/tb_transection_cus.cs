using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChicCarrental_Models.BaseModel
{
    public class tb_transection_cus
    {
        public int ID { get; set; }

        public int Transection { get; set; }

        public int User_ID { get; set; }

        public string Customer_Type { get; set; }

        public string Status { get; set; }

        public System.DateTime Add_Date { get; set; } = DateTime.Now;

        public System.DateTime Update_Date { get; set; } = DateTime.Now;
    }
}
