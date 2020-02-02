using ChicCarrental_Models.BaseModel;
using System.Linq;
using Umbraco.Web.WebApi;

namespace ChicCarrental_Controller.Api
{
    public class CustomerApiController : UmbracoApiController
    {
        /// <summary>
        /// /admin/api/BookingApi/GetTotal
        /// </summary>
        /// <returns></returns>
        public object GetByEmail(string id)
        {
           var sql = "select * from tb_customer where Email like @0";
           var customer = DatabaseContext.Database.Query<tb_customer>(sql,id).FirstOrDefault();

            if (!string.IsNullOrEmpty(id) && customer != null)
            {
                return customer;
            }

            return null;
        }


    }

}
