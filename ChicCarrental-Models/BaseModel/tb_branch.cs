using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChicCarrental_Models.BaseModel
{
    public class tb_branch
    {
        public int A_ID { get; set; }

        public string Branch_Name { get; set; }

        public int Branch_Image { get; set; }

        public string Branch_Teaser { get; set; }

        public string Lat { get; set; }

        public string Lon { get; set; }

        public System.Nullable<System.DateTime> Add_Date { get; set; }

        public System.Nullable<System.DateTime> Update_Date { get; set; }

        public string Status { get; set; }
    }
}
