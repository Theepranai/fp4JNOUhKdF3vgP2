using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChicCarrental_Models.BaseModel
{
    public class tb_car
    { 
		public int id { get; set; }

        public int image { get; set; }

        public string brand { get; set; }

        public string name { get; set; }

        public string vgroup { get; set; }

        public string sipp { get; set; }

        public string feature { get; set; }

        public string tranmission { get; set; }

        public string year { get; set; }

        public string enging { get; set; }

        public string door { get; set; }

        public string passenger { get; set; }

        public string luggage { get; set; }

        public string music_system { get; set; }

        public string status { get; set; }

        public System.DateTime add_Date { get; set; }

        public System.DateTime update_Date { get; set; }
    }
    public class tb_car_delete
    {
        public int A_ID { get; set; }

        public string Brand { get; set; }

        public string Name { get; set; }

        public string Code_Name { get; set; }

        public string Size { get; set; }

        public string Gear { get; set; }

        public string Enging_Power { get; set; }

        public string Engine_Type { get; set; }

        public string Engine_Name { get; set; }

        public string Fuel_Type { get; set; }

        public string Door { get; set; }

        public bool Air { get; set; }

        public bool ABS { get; set; }

        public int Airbag { get; set; }

        public int Seat  { get; set; }

        public int Bag  { get; set; }

        public string Car_Audio { get; set; }

        public System.DateTime Add_Date { get; set; }

        public System.DateTime Update_Date { get; set; }
    }
}
