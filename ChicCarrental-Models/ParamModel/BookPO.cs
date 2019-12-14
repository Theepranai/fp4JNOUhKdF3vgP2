using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChicCarrental_Models.ParamModel
{
    public class BookPO
    {
        public int pickuploc { get; set; }
        public int dropoffloc { get; set; }
        public string pickupdate { get; set; }
        public string pickuptime { get; set; }
        public string dropoffdate { get; set; }
        public string dropofftime { get; set; }
        
        public int typeselect { get; set; }
        
    }
    public class Step1: BookPO
    {
        public int car_id { get; set; }
        public int price_id { get; set; }

    }

    public class Step2 : Step1
    {
        public int[] extra { get; set; }
        public int[] insurence { get; set; }
        public int driver { get; set; }

    }

    public class Step3 : Step2
    {
        //customer
        public string[] title { get; set; }
        public string[] name { get; set; }
        public string[] lname { get; set; }
        public string[] email { get; set; }
        public string[] phone { get; set; }
        public string[] license { get; set; }

        public string diffpick { get; set; }

        public string pname { get; set; }
        public string plname { get; set; }
        public string pemail { get; set; }
        public string pphone { get; set; }
        public string plicense { get; set; }

        public string flightno { get; set; }

        public string pay { get; set; }

    }

}
