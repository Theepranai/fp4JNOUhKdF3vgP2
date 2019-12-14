using ChicCarrental_Models.BaseModel;
using System;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace ChicCarrental_Models.Utility
{
    public class Functional
    {
        public DateTime ConvertDate(string x)
        {
            var locale = System.Threading.Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;

            string[] splt = x.Split('-');

            return new DateTime(int.Parse(splt[0]), int.Parse(splt[1]), int.Parse(splt[2]));
        }

        public int Calculatedate(string p, string d)
        {
            try
            {
                var x = ConvertDate(p);
                var y = ConvertDate(d);

                return (y - x).Days;
            }
            catch
            {
                return 0;
            }
           
        }

        public DateTime ConvertTime(string x)
        {
            var locale = System.Threading.Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;

            string[] splt = x.Split('-');

            return Convert.ToDateTime(x);

        }

        public DateTime MergeDatetime(DateTime date,DateTime time)
        {
            var locale = System.Threading.Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;

            return date.Date.Add(time.TimeOfDay);

        }


       

    }
}
