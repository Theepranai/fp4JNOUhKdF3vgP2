using ChicCarrental_Models.BaseModel;
using ChicCarrental_Models.PageModel;
using System.Web.Mvc;
using Umbraco.Core.Persistence;
using Umbraco.Web.Models;

namespace ChicCarrental_Controllers.Controllers.Mvc
{
    public class HomeController : Umbraco.Web.Mvc.RenderMvcController
    {
        public ActionResult Home(RenderModel model)
        {
           
            var homeModel = new HomeModel(CurrentPage);

            homeModel.Branch = DatabaseContext.Database.Fetch<tb_branch>("select * from tb_branch");

            return CurrentTemplate(homeModel);
        }
    }
}
