using ChicCarrental_Models.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core;

namespace ChicCarrental_Controller.Service
{
    public class CarService
    {
        public tb_car_price GetCarPrice(int carid, DateTime stdate, DateTime endate, int numdate)
        {
            var db = ApplicationContext.Current.DatabaseContext.Database;

            var sql = string.Format(@"select top(1) * 
                                      from tb_car_price 
                                      where car_branch_id = {0} 
                                            and ( '{1}' between isnull(start_date,'{1}') and isnull(end_date,'{2}') )
                                            and numdate <= {3} and status = 'A'
                                      order by abs(isnull(datediff(day, '{1}', start_date),99)),numdate desc;"
                                    , carid
                                    , stdate
                                    , endate
                                    , numdate);

            return db.Fetch<tb_car_price>(sql).FirstOrDefault() ?? new tb_car_price();
        }

        public tb_car_price GetCarPriceByid(int price_id)
        {
            var db = ApplicationContext.Current.DatabaseContext.Database;
            var sql = string.Format(@"select top(1) * 
                                      from tb_car_price 
                                      where price_id = {0}"
                                   , price_id );

            return db.Fetch<tb_car_price>(sql).FirstOrDefault() ?? new tb_car_price();
        }

        public int GetAviable(int carid,DateTime startdate)
        {
            var db = ApplicationContext.Current.DatabaseContext.Database;
            var sql = string.Format(@"select count(*) from tb_transection 
                                      where Car_Branch_ID = {0} 
                                        and ('{1}'
                                             between Convert(date, PickUpDatetime) 
                                             and Convert(date, DropOffDatetime));"
                                    ,carid
                                    ,startdate);
            return db.ExecuteScalar<int>(sql);
        }
    }
}
