using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Umbraco.Web.WebApi;

namespace ChicCarrental_Controller.Api
{
    class BookingApiController : UmbracoApiController
    {
        [HttpGet]
        [Route("/api/booking")]
        public decimal GetTotal()
        {
            return (decimal)500;
        }
    }

}
