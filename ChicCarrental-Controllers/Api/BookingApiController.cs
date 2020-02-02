using Umbraco.Web.WebApi;

namespace ChicCarrental_Controller.Api
{
    public class BookingApiController : UmbracoApiController
    {
        /// <summary>
        /// /admin/api/BookingApi/GetTotal
        /// </summary>
        /// <returns></returns>
        public decimal GetTotal()
        {
            return (decimal)500;
        }
    }

}
