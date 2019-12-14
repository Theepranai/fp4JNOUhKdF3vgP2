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
   public class Step2Model : RenderModel
    {
        public Step2Model(IPublishedContent content)
            : base(content)
        {

        }

        public int NumDate { get; set; }

        public decimal TotalCarPrice { get; set; }

        public decimal TotalExtra { get; set; }

        public decimal TotalPrice { get; set; }

        public tb_branch PickupBranch { get; set; }

        public tb_branch DropoffBranch { get; set; }

        public dto_carprice Car { get; set; }

        public List<tb_optional> ExtraOption { get; set; }

        public List<tb_insurance> Insurance { get; set; }
    }
}
