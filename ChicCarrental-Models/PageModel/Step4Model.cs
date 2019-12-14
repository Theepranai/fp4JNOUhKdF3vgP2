using ChicCarrental_Models.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models;

namespace ChicCarrental_Models.PageModel
{
    public class Step4Model : Step3Model
    {
        public Step4Model(IPublishedContent content)
            : base(content)
        {

        }

        public int Transection_ID { get; set; }
        public tb_transection Transection { get; set; }

    }
}
