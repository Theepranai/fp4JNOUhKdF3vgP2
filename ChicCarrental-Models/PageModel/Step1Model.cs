using ChicCarrental_Models.BaseModel;
using System.Collections.Generic;
using Umbraco.Core.Models;
using Umbraco.Web.Models;

namespace ChicCarrental_Models.PageModel
{
    public class Step1Model : RenderModel
    {
        public Step1Model(IPublishedContent content)
            : base(content)
        {

        }

        public List<tb_branch> Branch { get; set; }

        public List<dto_carprice> ListCar { get; set; }


    }
}
