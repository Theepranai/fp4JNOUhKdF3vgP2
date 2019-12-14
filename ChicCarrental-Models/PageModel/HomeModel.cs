using ChicCarrental_Models.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models;
using Umbraco.Web.Models;

namespace ChicCarrental_Models.PageModel
{
    public class HomeModel : RenderModel
    {
        public HomeModel(IPublishedContent content)
            : base(content)
        {

        }

        public DateTime CurrentDate
        {
            get
            {
                return DateTime.Now;
            }
        }

        public List<tb_branch> Branch { get; set; }
    }
}
