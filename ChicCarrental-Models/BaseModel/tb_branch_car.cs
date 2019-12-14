using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChicCarrental_Models.BaseModel
{
    public class tb_branch_car
    {
        public int A_ID { get; set; }

        public int Branch_ID { get; set; }

        public int Car_ID { get; set; }

        public string Teaser { get; set; }

        public int Quantity { get; set; }

        public string Status { get; set; }

        public DateTime Add_Date { get; set; }

        public DateTime Update_Date { get; set; }
    }
}
