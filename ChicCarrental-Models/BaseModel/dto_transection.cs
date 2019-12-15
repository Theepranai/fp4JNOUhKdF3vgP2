using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChicCarrental_Models.BaseModel
{
    public class dto_transection : tb_transection
    {
        public tb_car car { get; set; }
        public List<tb_optional> optional { get; set; }

    }
}
