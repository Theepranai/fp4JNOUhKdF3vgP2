using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChicCarrental_Models.BaseModel
{
    public class tb_customer
    {
        public int ID { get; set; }

        public int User_ID { get; set; }

        public string Title { get; set; }

        public string Name { get; set; }

        public string LName { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string License { get; set; }

        public string Status { get; set; } = "A";

        public System.DateTime Add_Date { get; set; } = DateTime.Now;

        public System.DateTime Update_Date { get; set; } = DateTime.Now;
    }
}
