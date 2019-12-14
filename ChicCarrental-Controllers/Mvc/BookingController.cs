using ChicCarrental_Controller.Service;
using ChicCarrental_Models.BaseModel;
using ChicCarrental_Models.PageModel;
using ChicCarrental_Models.ParamModel;
using ChicCarrental_Models.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Models;


namespace ChicCarrental_Controller.Mvc
{
    public class BookingController : Umbraco.Web.Mvc.RenderMvcController
    {
        private Functional _functional;
        private CarService _carService;

        public BookingController()
        {
            _functional = new Functional();
            _carService = new CarService();
        }

        [HttpGet]
        public ActionResult Step1(RenderModel model)
        {
            return Content(RenderRazorViewToString("sds", model));
            //return Redirect("/");  
        }

        [HttpPost]
        public ActionResult Step1(RenderModel model, BookPO param)
        {
            var rto = new Step1Model(CurrentPage);
            rto.Branch = DatabaseContext.Database.Fetch<tb_branch>("select * from tb_branch");

            DateTime pudate = _functional.ConvertDate(param.pickupdate);
            DateTime dfdate = _functional.ConvertDate(param.dropoffdate);
            int ndate = (dfdate - pudate).Days;
            if (ndate <= 0)
            {
                dfdate = dfdate.AddDays(1);
            }

            var sql = string.Format(@"Select A_ID,[Branch_ID],[Car_ID],t2.*
                                            From tb_branch_car t1
                                            Inner join tb_car t2 on t1.Car_ID = t2.id
                                            Where Branch_ID = {0}", param.pickuploc);

            rto.ListCar = DatabaseContext.Database.Fetch<dto_carprice>(sql);
            rto.ListCar.ForEach(x=>
            {
                x.price = _carService.GetCarPrice(x.A_ID,pudate,dfdate,ndate);
            });

            return CurrentTemplate(rto);
        }

        [HttpGet]
        public ActionResult Step2(RenderModel model)
        {
            return Redirect("/");
        }

        [HttpPost]
        public ActionResult Step2(RenderModel model, Step1 param)
        {
            var rto = new Step2Model(CurrentPage);
            DateTime pudate = _functional.ConvertDate(param.pickupdate);
            DateTime dfdate = _functional.ConvertDate(param.dropoffdate);
            rto.NumDate = (dfdate - pudate).Days + 1;
            rto.PickupBranch = DatabaseContext.Database.Fetch<tb_branch>(string.Format("select * from tb_branch where A_ID = {0}", param.pickuploc)).FirstOrDefault();
            rto.DropoffBranch = DatabaseContext.Database.Fetch<tb_branch>(string.Format("select * from tb_branch where A_ID = {0}", param.dropoffloc)).FirstOrDefault();

            var sql = string.Format(@"Select t1.A_ID,Branch_ID,Car_ID,t2.*
                                    From tb_branch_car t1
                                    Inner join tb_car t2 on t1.Car_ID = t2.id
                                    Where t1.A_ID = {0}", param.car_id);

            rto.Car = DatabaseContext.Database.Fetch<dto_carprice>(sql).FirstOrDefault();

            rto.Car.price = _carService.GetCarPriceByid(param.price_id);

            rto.TotalPrice = (rto.Car?.price.price ?? 0) * rto.NumDate;

            rto.ExtraOption = DatabaseContext.Database.Fetch<tb_optional>(string.Format("select * from tb_optional"));

            rto.Insurance = DatabaseContext.Database.Fetch<tb_insurance>(string.Format("select * from tb_insurance"));

            return CurrentTemplate(rto);
        }
    
        [HttpGet]
        public ActionResult Step3(RenderModel model)
        {
            return Redirect("/");
        }

        [HttpPost] //select optional
        public ActionResult Step3(RenderModel model, Step2 param)
        {
            var rto = new Step3Model(CurrentPage);
            DateTime pudate = _functional.ConvertDate(param.pickupdate);
            DateTime dfdate = _functional.ConvertDate(param.dropoffdate);
            rto.NumDate = (dfdate - pudate).Days + 1;
            rto.PickupBranch = DatabaseContext.Database.Fetch<tb_branch>(string.Format("select * from tb_branch where A_ID = {0}", param.pickuploc)).FirstOrDefault();
            rto.DropoffBranch = DatabaseContext.Database.Fetch<tb_branch>(string.Format("select * from tb_branch where A_ID = {0}", param.dropoffloc)).FirstOrDefault();

            var sql = string.Format(@"Select t1.A_ID,Branch_ID,Car_ID,t2.*
                                    From tb_branch_car t1
                                    Inner join tb_car t2 on t1.Car_ID = t2.id
                                    Where t1.A_ID = {0}", param.car_id);

            rto.Car = DatabaseContext.Database.Fetch<dto_carprice>(sql).FirstOrDefault();

            rto.Car.price = _carService.GetCarPriceByid(param.price_id);

            rto.TotalCarPrice = (rto.Car?.price.price ?? 0) * rto.NumDate;

            var listextra = string.Join(",", param.extra ?? new int[] { 0 });
            rto.ExtraOption = DatabaseContext.Database.Fetch<tb_optional>(string.Format("select * from tb_optional where A_ID in ({0})", listextra));
            
            rto.TotalExtra = rto.ExtraOption.Sum(x => x.Price) * rto.NumDate;

            rto.TotalPrice = rto.TotalCarPrice + rto.TotalExtra;

            return CurrentTemplate(rto);
        }

        [HttpGet] //payment status
        public ActionResult Step4(RenderModel model, string id)
        {
            var rto = new Step4Model(CurrentPage);
            rto.Transection_ID = Convert.ToInt32(id);
            rto.Transection = DatabaseContext.Database.Fetch<tb_transection>(string.Format("select top(1) * from tb_transection where ID = {0}", rto.Transection_ID)).FirstOrDefault();

            return CurrentTemplate(rto);
        }

        [HttpPost] //payment
        public ActionResult Step4(RenderModel model, Step3 param)
        {
            var rto = new Step4Model(CurrentPage);

            DateTime pudate = _functional.ConvertDate(param.pickupdate);
            DateTime dfdate = _functional.ConvertDate(param.dropoffdate);
            rto.NumDate = (dfdate - pudate).Days + 1;
            rto.PickupBranch = DatabaseContext.Database.Fetch<tb_branch>(string.Format("select * from tb_branch where A_ID = {0}", param.pickuploc)).FirstOrDefault();
            rto.DropoffBranch = DatabaseContext.Database.Fetch<tb_branch>(string.Format("select * from tb_branch where A_ID = {0}", param.dropoffloc)).FirstOrDefault();

            var sql = string.Format(@"Select t1.A_ID,Branch_ID,Car_ID,t2.*
                                    From tb_branch_car t1
                                    Inner join tb_car t2 on t1.Car_ID = t2.id
                                    Where t1.A_ID = {0}", param.car_id);

            rto.Car = DatabaseContext.Database.Fetch<dto_carprice>(sql).FirstOrDefault();

            rto.Car.price = _carService.GetCarPriceByid(param.price_id);

            rto.TotalCarPrice = (rto.Car?.price.price ?? 0) * rto.NumDate;

            var listextra = string.Join(",", param.extra ?? new int[] { 0 });
            rto.ExtraOption = DatabaseContext.Database.Fetch<tb_optional>(string.Format("select * from tb_optional where A_ID in ({0})", listextra));

            rto.TotalExtra = rto.ExtraOption.Sum(x => x.Price) * rto.NumDate;

            rto.TotalPrice = rto.TotalCarPrice + rto.TotalExtra;

            var transection = new tb_transection();
          
            transection.Customer_ID = GetMemberID(param.email[0]
                                                  ,string.Format("{0} {1}", param.name[0], param.lname[0])
                                                  ,param.title[0]
                                                  ,param.name[0]
                                                  ,param.lname[0]
                                                  ,param.phone[0]
                                                  ,param.license[0]
                                                  );

            if(param.diffpick == "1")
            {
                transection.PickupCus_ID = GetMemberID(param.pemail, string.Format("{0} {1}", param.pname, param.plname));
            }

            DateTime putime = _functional.ConvertTime(param.pickuptime);
            DateTime dftime = _functional.ConvertTime(param.dropofftime);

            transection.PickUpDatetime = _functional.MergeDatetime(pudate, putime);
            transection.DropOffDatetime = _functional.MergeDatetime(dfdate, dftime);
           
            transection.Numdate = rto.NumDate;
            transection.Status = "0";
            if(param.pay == "now")
            {
                transection.Payment_Type = "0";
            }
           
            rto.PickupBranch = DatabaseContext.Database.Fetch<tb_branch>(string.Format("select * from tb_branch where A_ID = {0}", param.pickuploc)).FirstOrDefault();
            rto.DropoffBranch = DatabaseContext.Database.Fetch<tb_branch>(string.Format("select * from tb_branch where A_ID = {0}", param.dropoffloc)).FirstOrDefault();

            transection.PickUpLoc = rto.PickupBranch.A_ID;
            transection.DropOffLoc = rto.DropoffBranch.A_ID;

            transection.Car_Branch_ID = rto.Car.Car_Branch_ID;

            transection.Sum_Price = rto.TotalPrice;
            transection.Total_Price = rto.TotalPrice;

            DatabaseContext.Database.Save(transection);

            rto.Transection_ID = transection.ID;

            var transection_cus = new List<tb_transection_cus>();

            for(var i=0;i<param.email.Length;i++)
            {
                var cus = new tb_transection_cus();
                cus.Transection = rto.Transection_ID;
                cus.User_ID = GetMemberID(param.email[0]
                                            , string.Format("{0} {1}", param.name[i], param.lname[i])
                                            , param.title[i]
                                            , param.name[i]
                                            , param.lname[i]
                                            , param.phone[i]
                                            , param.license[i]
                                            );
                cus.Customer_Type = string.Format("Driver #{0}", i+1);
                DatabaseContext.Database.Save(cus);
                
            }

            if (param.diffpick == "1")
            {
                var cus = new tb_transection_cus();
                cus.Transection = rto.Transection_ID;
                cus.User_ID = GetMemberID(param.pemail, string.Format("{0} {1}", param.pname, param.plname));
                cus.Customer_Type = string.Format("Pick car");
                DatabaseContext.Database.Save(cus);
            }

            foreach (var item in rto.ExtraOption)
            {
                var detail = new tb_transection_detail();
                detail.Transection = rto.Transection_ID;
                detail.Item_ID = item.A_ID;
                detail.Item_Name = item.Name;
                detail.Item_Price = item.Price;
                detail.Item_Type = "E";
                DatabaseContext.Database.Save(detail);
            }

            return Redirect("/booking/step4/" + rto.Transection_ID);
        }

        [HttpPost] //payment status
        public ActionResult Result(RenderModel model,string result,string payment)
        {
            var rto = new Step4Model(CurrentPage);
            rto.Transection_ID = Convert.ToInt32(result);
            rto.Transection = DatabaseContext.Database.Fetch<tb_transection>(string.Format("select top(1) * from tb_transection where ID = {0}", rto.Transection_ID)).FirstOrDefault();

            var version = Request["version"];
            var request_timestamp = Request["request_timestamp"];
            var merchant_id = Request["merchant_id"];
            var currency = Request["currency"];
            var order_id = Request["order_id"];
            var amount = Request["amount"];
            var invoice_no = Request["invoice_no"];
            var transaction_ref = Request["transaction_ref"];
            var approval_code = Request["approval_code"];
            var eci = Request["eci"];
            var transaction_datetime = Request["transaction_datetime"];
            var payment_channel = Request["payment_channel"];
            var payment_status = Request["payment_status"];
            var channel_response_code = Request["channel_response_code"];
            var channel_response_desc = Request["channel_response_desc"];
            var masked_pan = Request["masked_pan"];
            var stored_card_unique_id = Request["stored_card_unique_id"];
            var backend_invoice = Request["backend_invoice"];
            var paid_channel = Request["paid_channel"];
            var paid_agent = Request["paid_agent"];
            var payment_scheme = Request["payment_scheme"];
            var user_defined_1 = Request["user_defined_1"];
            var user_defined_2 = Request["user_defined_2"];
            var user_defined_3 = Request["user_defined_3"];
            var user_defined_4 = Request["user_defined_4"];
            var user_defined_5 = Request["user_defined_5"];
            var browser_info = Request["browser_info"];
            var hash_value = Request["hash_value"];

            var checkHashStr = version +""+ request_timestamp+""+merchant_id+""+order_id+""+invoice_no+""+currency+""+amount+""+transaction_ref+""+approval_code+""+eci+""+transaction_datetime+""+payment_channel+""+payment_status+""+channel_response_code+""+channel_response_desc+""+masked_pan+""+stored_card_unique_id+""+backend_invoice+""+paid_channel+""+paid_agent+""+user_defined_1+""+user_defined_2+""+user_defined_3+""+user_defined_4+""+user_defined_5+""+browser_info+""+payment_scheme; 
	  
	        var SECRETKEY = "7jYcp4FxFdf0";
	        var checkHash = hash_hmac_sha1(checkHashStr,SECRETKEY);    //Compute hash value  
                                                                                //Validate response hash_value
            if (hash_value.ToLower() == checkHash.ToLower())
                {
                    rto.Transection.Status = "1";
                    rto.Transection.Transection_ID = Convert.ToInt32(string.Format("10{0}",rto.Transection.ID.ToString("D8")));
                }
                else
                {
                    rto.Transection.Status = "0";
                }

            rto.Transection.Payment_Type = "1";

            DatabaseContext.Database.Update(rto.Transection);

            return Redirect("/booking/result/" + rto.Transection_ID);
        }

        [HttpGet] //payment status
        public ActionResult Result(RenderModel model, string id)
        {
            var rto = new Step4Model(CurrentPage);
            rto.Transection_ID = Convert.ToInt32(id);
            return Redirect("/booking/step4/" + rto.Transection_ID);
        }

        public int GetMemberID(string email,string name,string title = "",string fname ="", string lname="", string phone = "", string license ="")
        {
            var m = Services.MemberService.GetByEmail(email);
            if (m != null)
            {
                return m.Id;
            }
            else
            {
                IMember newMember = Services.MemberService.CreateMemberWithIdentity(email, email, name, "Customer");

                var cus = new tb_customer();
                cus.Title = title;
                cus.Name = fname;
                cus.LName = lname;
                cus.Phone = phone;
                cus.License = license;
                DatabaseContext.Database.Save(cus);

                return newMember.Id;
            }

        }

        //2c2p
        public static string hash_hmac_sha1(string message, string key)
        {
            byte[] keyByte = UTF8Encoding.UTF8.GetBytes(key);

            HMACSHA1 hmacsha1 = new HMACSHA1(keyByte);

            byte[] messageBytes = UTF8Encoding.UTF8.GetBytes(message);

            byte[] hashmessage = hmacsha1.ComputeHash(messageBytes);

            string calcHashString = ByteToString(hashmessage);
            return calcHashString.ToLower();
        }

        /*converts byte to encrypted string*/
        public static string ByteToString(byte[] buff)
        {
            string sbinary = "";

            for (int i = 0; i < buff.Length; i++)
            {
                sbinary += buff[i].ToString("X2"); // hex format
            }
            return (sbinary);
        }

        public string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext,
                                                                         viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View,
                                             ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

    }
}
