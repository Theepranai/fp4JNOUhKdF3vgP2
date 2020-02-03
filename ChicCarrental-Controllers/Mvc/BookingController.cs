using ChicCarrental_Controller.Service;
using ChicCarrental_Models.BaseModel;
using ChicCarrental_Models.PageModel;
using ChicCarrental_Models.ParamModel;
using ChicCarrental_Models.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
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
            rto.Branch = DatabaseContext.Database.Fetch<tb_branch>("select * from tb_branch;");

            DateTime pudate = _functional.ConvertDate(param.pickupdate);
            DateTime dfdate = _functional.ConvertDate(param.dropoffdate);
            DateTime putime = _functional.ConvertTime(param.pickuptime);
            DateTime dftime = _functional.ConvertTime(param.dropofftime);

            pudate = _functional.MergeDatetime(pudate, putime);
            dfdate = _functional.MergeDatetime(dfdate, dftime);

            int ndate = (dfdate - pudate).Days;
            int nhour = (dfdate - pudate).Hours;
            int nmin = (dfdate - pudate).Minutes;

            if (nhour >= 4 && nmin > 1)
            {
                ndate += 1;
            }

            if (ndate <= 0)
            {
                dfdate = dfdate.AddDays(1);
                param.dropoffdate = dfdate.ToString("yyyy-MM-dd");
            }

            var sql = string.Format(@"Select A_ID,[Branch_ID],[Car_ID],t2.*,t1.Quantity
                                            From tb_branch_car t1
                                            Inner join tb_car t2 on t1.Car_ID = t2.id
                                            Where Branch_ID = {0} and t1.Status ='A';", param.pickuploc);

            rto.ListCar = DatabaseContext.Database.Fetch<dto_carprice>(sql);
            rto.ListCar.ForEach(x=>
            {
                x.price = _carService.GetCarPrice(x.A_ID,pudate,dfdate,ndate);
            });

            rto.ListCar.ForEach(x =>
            {
                x.Aviable = x.Quantity - (_carService.GetAviable(x.A_ID, pudate));
            });

            rto.ListCar = rto.ListCar.Where(x => x.Aviable > 0).ToList();

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
            DateTime putime = _functional.ConvertTime(param.pickuptime);
            DateTime dftime = _functional.ConvertTime(param.dropofftime);

            pudate = _functional.MergeDatetime(pudate, putime);
            dfdate = _functional.MergeDatetime(dfdate, dftime);

            int ndate = (dfdate - pudate).Days;
            int nhour = (dfdate - pudate).Hours;
            int nmin = (dfdate - pudate).Minutes;

            if (nhour >= 4 && nmin > 1)
            {
                ndate += 1;
            }

            rto.NumDate = ndate;

            rto.PickupBranch = DatabaseContext.Database.Fetch<tb_branch>(string.Format("select * from tb_branch where A_ID = {0}", param.pickuploc)).FirstOrDefault();
            rto.DropoffBranch = DatabaseContext.Database.Fetch<tb_branch>(string.Format("select * from tb_branch where A_ID = {0}", param.dropoffloc)).FirstOrDefault();

            var sql = string.Format(@"Select t1.A_ID,Branch_ID,Car_ID,t2.*
                                    From tb_branch_car t1
                                    Inner join tb_car t2 on t1.Car_ID = t2.id
                                    Where t1.A_ID = {0} and t1.Status ='A'", param.car_id);

            rto.Car = DatabaseContext.Database.Fetch<dto_carprice>(sql).FirstOrDefault();

            rto.Car.price = _carService.GetCarPriceByid(param.price_id);

            rto.TotalPrice = (rto.Car?.price.price ?? 0) * rto.NumDate;

            rto.ExtraOption = DatabaseContext.Database.Fetch<tb_optional>(string.Format("select * from tb_optional where Status = 'A' "));

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
            DateTime putime = _functional.ConvertTime(param.pickuptime);
            DateTime dftime = _functional.ConvertTime(param.dropofftime);

            pudate = _functional.MergeDatetime(pudate, putime);
            dfdate = _functional.MergeDatetime(dfdate, dftime);

            int ndate = (dfdate - pudate).Days;
            int nhour = (dfdate - pudate).Hours;
            int nmin = (dfdate - pudate).Minutes;

            if (nhour >= 4 && nmin > 1)
            {
                ndate += 1;
            }

            rto.NumDate = ndate;
            rto.PickupBranch = DatabaseContext.Database.Fetch<tb_branch>(string.Format("select * from tb_branch where A_ID = {0}", param.pickuploc)).FirstOrDefault();
            rto.DropoffBranch = DatabaseContext.Database.Fetch<tb_branch>(string.Format("select * from tb_branch where A_ID = {0}", param.dropoffloc)).FirstOrDefault();

            var sql = string.Format(@"Select t1.A_ID,Branch_ID,Car_ID,t2.*
                                    From tb_branch_car t1
                                    Inner join tb_car t2 on t1.Car_ID = t2.id
                                    Where t1.A_ID = {0} and t1.Status ='A'", param.car_id);

            rto.Car = DatabaseContext.Database.Fetch<dto_carprice>(sql).FirstOrDefault();

            rto.Car.price = _carService.GetCarPriceByid(param.price_id);

            rto.TotalCarPrice = (rto.Car?.price.price ?? 0) * rto.NumDate;

            var listextra = string.Join(", ", param.extra ?? new int[] { 0 });

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

            var sqlcar = string.Format(@"Select t1.A_ID,Branch_ID,Car_ID,t2.*
                                    From tb_branch_car t1
                                    Inner join tb_car t2 on t1.Car_ID = t2.id
                                    Where t1.A_ID = {0}",rto.Transection.Car_Branch_ID);
            rto.Car = new dto_carprice();
            rto.Car = DatabaseContext.Database.Fetch<dto_carprice>(sqlcar).FirstOrDefault();

            var sqloption = string.Format(@"SELECT t2.*
                                            FROM tb_transection_detail t1
                                            INNER JOIN tb_optional t2 on t1.Item_ID = t2.A_ID
                                            Where t1.Transection = {0}", rto.Transection.ID);

            rto.ExtraOption = DatabaseContext.Database.Fetch<tb_optional>(sqloption).ToList();

            return CurrentTemplate(rto);
        }

        [HttpPost] //payment
        public ActionResult Step4(RenderModel model, Step3 param)
        {
            var rto = new Step4Model(CurrentPage);

            DateTime pudate = _functional.ConvertDate(param.pickupdate);
            DateTime dfdate = _functional.ConvertDate(param.dropoffdate);
            DateTime putime = _functional.ConvertTime(param.pickuptime);
            DateTime dftime = _functional.ConvertTime(param.dropofftime);

            pudate = _functional.MergeDatetime(pudate, putime);
            dfdate = _functional.MergeDatetime(dfdate, dftime);

            int ndate = (dfdate - pudate).Days;
            int nhour = (dfdate - pudate).Hours;
            int nmin = (dfdate - pudate).Minutes;

            if (nhour >= 4 && nmin > 1)
            {
                ndate += 1;
            }

            rto.NumDate = ndate;
            rto.PickupBranch = DatabaseContext.Database.Fetch<tb_branch>(string.Format("select * from tb_branch where A_ID = {0};", param.pickuploc)).FirstOrDefault();
            rto.DropoffBranch = DatabaseContext.Database.Fetch<tb_branch>(string.Format("select * from tb_branch where A_ID = {0};", param.dropoffloc)).FirstOrDefault();

            var sql = string.Format(@"Select t1.A_ID,Branch_ID,Car_ID,t2.*
                                    From tb_branch_car t1
                                    Inner join tb_car t2 on t1.Car_ID = t2.id
                                    Where t1.A_ID = {0};", param.car_id);

            rto.Car = new dto_carprice();

            rto.Car = DatabaseContext.Database.Fetch<dto_carprice>(sql).FirstOrDefault();

            rto.Car.price = new tb_car_price();

            rto.Car.price = _carService.GetCarPriceByid(param.price_id);

            rto.TotalCarPrice = (rto.Car?.price.price ?? 0) * rto.NumDate;

            var listextra = string.Join(",", param.extra ?? new int[] { 0 });

            rto.ExtraOption = DatabaseContext.Database.Fetch<tb_optional>(string.Format("select * from tb_optional where A_ID in ({0});", listextra));

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
               // transection.PickupCus_ID = GetMemberID(param.pemail, string.Format("{0} {1}", param.pname, param.plname));
            }

            transection.PickUpDatetime = _functional.MergeDatetime(pudate, putime);
            transection.DropOffDatetime = _functional.MergeDatetime(dfdate, dftime);

            transection.Car_Branch_ID = rto.Car.A_ID;
            transection.Numdate = rto.NumDate;
            transection.Status = "P";
            transection.Price_ID = rto.Car.price.price_id;

            //transectionid 
            int lastid = DatabaseContext.Database.ExecuteScalar<int>("select count(*) + 1 from tb_transection where  Convert(date, Add_Date) = Convert(date, getdate());");
            DateTime dt = DateTime.Now;
            transection.Transection_ID = Convert.ToInt32(dt.ToString("yyMMdd") + lastid.ToString("0000"));

            rto.PickupBranch = DatabaseContext.Database.Fetch<tb_branch>(string.Format("select * from tb_branch where A_ID = {0};", param.pickuploc)).FirstOrDefault();
            rto.DropoffBranch = DatabaseContext.Database.Fetch<tb_branch>(string.Format("select * from tb_branch where A_ID = {0};", param.dropoffloc)).FirstOrDefault();

            transection.PickUpLoc = rto.PickupBranch.A_ID;
            transection.DropOffLoc = rto.DropoffBranch.A_ID;

            transection.Total_Price = rto.TotalPrice;

            var discounttype = rto.Car.price.paynow_type;
            var discount = rto.Car.price.paynow ?? 0;
            var totaldiscount = 0M;

            var sendemail = 0;

            if (param.pay == "now")
            {
                transection.Payment_Type = "N";
                if (discounttype == "percent")
                {
                    totaldiscount = rto.TotalCarPrice * (discount / 100);
                    transection.Total_Price = rto.TotalPrice - totaldiscount;
                }
                else
                {
                    totaldiscount = discount;
                    transection.Total_Price = rto.TotalPrice - discount;
                }
            }
            else
            {
                transection.Payment_Type = "L";
                sendemail = 1;
            }

            transection.Discount_Total = totaldiscount;

            transection.Sub_Total = transection.Total_Price / 1.07M;

            transection.Vat_Total = transection.Total_Price - transection.Sub_Total;

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
                detail.Item_Price = item.Price;
                detail.Total = item.Price * rto.NumDate;
                detail.Vat_Total = Math.Round(detail.Total * 0.07M,2);
                detail.Sub_Total = detail.Total - detail.Vat_Total;
                detail.Item_Type = "E";
                DatabaseContext.Database.Save(detail);
            }

            return Redirect("/booking/Result/" + rto.Transection_ID + "?sendmail="+ sendemail);
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
                rto.Transection.Status = "A";
                rto.Transection.Payment_Ref = string.Format("10{0}",rto.Transection.ID.ToString("D8"));
            }
            else
            {
                rto.Transection.Status = "P";
            }

            //rto.Transection.Payment_Type = "L";

            DatabaseContext.Database.Update(rto.Transection);

            return Redirect("/booking/result/" + rto.Transection_ID);
        }

        [HttpGet] //payment status
        public ActionResult Result(RenderModel model, string id)
        {
            var sendmail = Request["sendmail"];
            var testmail = Request["testmail"];

            var rto = new Step4Model(CurrentPage);

            rto.Transection_ID = Convert.ToInt32(id);

            var transection = new dto_transection();

            transection = DatabaseContext.Database.Query<dto_transection>(string.Format("select top(1) * from tb_transection where ID = {0};", rto.Transection_ID)).FirstOrDefault();

            var customer = new tb_customer();
                customer = DatabaseContext.Database.Query<tb_customer>(string.Format("select top(1) * from tb_customer where User_ID = {0};", transection.Customer_ID)).FirstOrDefault();

            var car = new tb_car();
            car = DatabaseContext.Database.Query<tb_car>(
                              string.Format(@"SELECT top(1) t1.A_ID,Branch_ID,Car_ID,t2.*
                                    From tb_branch_car t1
                                    INNER join tb_car t2 on t1.Car_ID = t2.id
                                    WHERE t1.A_ID = {0};", transection.Car_Branch_ID)).FirstOrDefault();

            var optional = new List<tb_optional>();
                optional = DatabaseContext.Database.Fetch<tb_optional>(
                              string.Format(@"select [Item_Type]
                                                      ,[Item_Price]
                                                      ,[Sub_Total]
                                                      ,[Vat_Total]
                                                      ,[Total]
                                                     ,t2.*
                                              FROM tb_transection_detail t1
                                              INNER JOIN tb_optional t2 on t1.Item_ID = t2.A_ID
                                              WHERE t1.Transection = {0};", transection.ID)).ToList();

            var pickuploc = new tb_branch();
                pickuploc = DatabaseContext.Database.Fetch<tb_branch>(
                              string.Format(@"select top(1) * 
                                              FROM tb_branch
                                              WHERE A_ID = {0};", transection.PickUpLoc)).FirstOrDefault();
            var dropoffloc = new tb_branch();
                dropoffloc = DatabaseContext.Database.Fetch<tb_branch>(
                             string.Format(@"select top(1) * 
                                              FROM tb_branch
                                              WHERE A_ID = {0};", transection.DropOffLoc)).FirstOrDefault();
            var price = new tb_car_price();
                price = DatabaseContext.Database.Fetch<tb_car_price>(
                             string.Format(@"select top(1) * 
                                              FROM tb_car_price
                                              WHERE price_id = {0};", transection.Price_ID)).FirstOrDefault();

            transection.car = car;
            transection.customer = customer;
            transection.optional = optional;
            transection.pickuplocation = pickuploc;
            transection.dropofflocation = dropoffloc;
            transection.car_price = price;

            var textDetail = model.Content.GetProperty("textContent").Value.ToString();

            textDetail = textDetail.Replace("{name}",customer.Name + " " + customer.LName);
            textDetail = textDetail.Replace("{email}", customer.Email);
            textDetail = textDetail.Replace("{phone}", customer.Phone);
            
            textDetail = textDetail.Replace("{pricecode}", price.price_code);
            textDetail = textDetail.Replace("{adddate}", transection.Add_Date.ToString("yyyy-MM-dd hh:mm tt"));
            textDetail = textDetail.Replace("{pickuplocation}", pickuploc.Branch_Name);

            var status = transection.Status == "A" ? "Comfirm" : "Pending";
            textDetail = textDetail.Replace("{status}", status);

            var paytype = transection.Payment_Type == "N" ? "Online" : "At counter";
            textDetail = textDetail.Replace("{paytype}", paytype);

            textDetail = textDetail.Replace("{transectionno}", "#"+ transection.Payment_Type+""+ transection.Transection_ID);

            textDetail = textDetail.Replace("{pickupdate}", transection.PickUpDatetime?.ToString("yyyy-MM-dd hh:mm tt"));
            textDetail = textDetail.Replace("{dropofftime}", transection.DropOffDatetime?.ToString("yyyy-MM-dd hh:mm tt"));
            textDetail = textDetail.Replace("{pickuplocation}",pickuploc.Branch_Name );
            textDetail = textDetail.Replace("{droplocation}", dropoffloc.Branch_Name );

            textDetail = textDetail.Replace("{discount}", transection.Discount_Total?.ToString("N2"));
            textDetail = textDetail.Replace("{subtotal}", transection.Sub_Total?.ToString("N2"));
            textDetail = textDetail.Replace("{vat}", transection.Vat_Total?.ToString("N2"));
            textDetail = textDetail.Replace("{total}", transection.Total_Price?.ToString("N2"));

            var totaltext = "";
          
            var carprice = price.price;

            var itemcar = string.Format(@"<tr>
                                  <td>{0}</td>
                                  <td>{1}</td>
                                  <td>{2}</td>
                                  <td>{3}</td>
                                </tr>"
                                , car.name
                                , carprice?.ToString("N2")
                                , transection.Numdate
                                , (carprice * transection.Numdate)?.ToString("N2"));

            var itemoption = "";
            foreach (var op in optional)
            {
                var opprice = op.Price;
                itemoption += string.Format(@"<tr>
                                  <td>{0}</td>
                                  <td>{1}</td>
                                  <td>{2}</td>
                                  <td>{3}</td>
                                </tr>"
                                , op.Name
                                , opprice.ToString("N2")
                                , transection.Numdate
                                , (opprice * transection.Numdate)?.ToString("N2"));
            }

            var itemdetail = @"<table style='width:100%' border='0'>
                                <tr>
                                  <td width='50%'><b>Description</b></td>
                                  <td><b>Rate</b></td>
                                  <td><b>Day(s)</b></td>
                                  <td><b>Total</b></td>
                                </tr>
                                 "+ itemcar + @"
                                 "+ itemoption +@"
                                 "+ totaltext +@"
                            </table>";

            textDetail = textDetail.Replace("{detail}", itemdetail);

            ViewBag.text = textDetail;

            if(sendmail == "1") SendEmail(customer.Email, "Your reservation details.", textDetail);

            if(testmail == "1") return CurrentTemplate(rto);

            return Redirect("/booking/step4/" + rto.Transection_ID);

        }

        public int GetMemberID(string email,string name,string title = "",string fname ="", string lname="", string phone = "", string license ="")
        {
            var m = Services.MemberService.GetByEmail(email);

            var userid = 0;

            if (m != null)
            {
                userid =  m.Id;
            }
            else
            {
                IMember newMember = Services.MemberService.CreateMemberWithIdentity(email, email, name, "Customer");
                userid = newMember.Id;
            }

            int trancus = DatabaseContext.Database.ExecuteScalar<int>("select count(*) from tb_customer where Email = @0", email);

            var cus = DatabaseContext.Database.Query<tb_customer>("select top(1) * from tb_customer where Email = @0", email).FirstOrDefault() ?? new tb_customer();

            if (trancus > 0)
            {
                cus.User_ID = userid;
                cus.Title = title;
                cus.Name = fname;
                cus.LName = lname;
                cus.Phone = phone;
                cus.Email = email;
                cus.License = license;
                DatabaseContext.Database.Update(cus);
            }
            else
            {
                cus.User_ID = userid;
                cus.Title = title;
                cus.Name = fname;
                cus.LName = lname;
                cus.Phone = phone;
                cus.Email = email;
                cus.License = license;
                DatabaseContext.Database.Save(cus);
            }

            return userid;

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

        public int SendEmail(string to, string subject, string text, string fileurl = "", string filename = "", string from = "")
        {
            try
            {
                MailMessage mail = new MailMessage();
                string[] Mutimail = to.Split(',');

                foreach (string mailto in Mutimail)
                {
                    if (string.IsNullOrEmpty(mailto))
                    {

                    }
                    else
                    {
                        if (!mail.To.Contains(new MailAddress(mailto)))
                        {
                            mail.To.Add(new MailAddress(mailto));
                        }
                    }
                }

                if (from != "")
                {
                    mail.From = new MailAddress(from);
                }

                mail.Subject = subject;
                string body = text;
                mail.Body = body;
                mail.IsBodyHtml = true;

                //if (fileurl != "" && filename != "")
                //{
                //    var clientfile = new WebClient();
                //    try
                //    {
                //        if (File.Exists(filename))
                //        {
                //            File.Delete(filename);
                //        }

                //        clientfile.DownloadFile(fileurl, filename);

                //        var attachment = new Attachment(filename, MediaTypeNames.Application.Pdf);
                //        mail.Attachments.Add(attachment);


                //    }
                //    catch
                //    {

                //    }
                //    finally
                //    {
                //        clientfile.Dispose();
                //    }
                //}

                SmtpClient client = new SmtpClient
                {
                    EnableSsl = true
                };

                client.Send(mail);
                return 1;
            }
            catch (Exception ex)
            {
                throw new Exception("Sendmail", ex);
            }
        }


    }
}
