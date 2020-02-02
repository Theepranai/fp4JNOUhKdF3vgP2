using ChicCarrental_Models.BaseModel;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using umbraco.businesslogic;
using umbraco.BusinessLogic.Actions;
using umbraco.interfaces;
using Umbraco.Core;
using Umbraco.Web.Editors;
using Umbraco.Web.Models.Trees;
using Umbraco.Web.Mvc;
using Umbraco.Web.Trees;

namespace ChicCarrental_Controller.Plugins
{
    [Application("Transections", "Transections", "icon-categories")]
    public class transectionSectionApplication : IApplication { }

    [PluginController("Transections")]
    [Umbraco.Web.Trees.Tree("Transections", "Transections", "Transections")]
    public class transectionSectionTreeController : TreeController
    {
        protected override TreeNodeCollection GetTreeNodes(string id, FormDataCollection queryStrings)
        {
            var nodes = new TreeNodeCollection();

            var query = @"select left(convert(varchar, PickUpDatetime, 23),7)
                                from tb_transection 
                                group by left(convert(varchar, PickUpDatetime, 23),7)
                                order by left(convert(varchar, PickUpDatetime, 23),7)";

            var x = DatabaseContext.Database.Query<string>(query).ToList();

            var IconNode = "icon-calendar-alt";
            var xurl = queryStrings;

            foreach (var thing in x)
            {
                var node = CreateTreeNode(thing, id, queryStrings, thing, IconNode, false);
                node.RoutePath = "Transections/Transections/list/" + thing;
                nodes.Add(node);
            }
            return nodes;
        }

        protected override MenuItemCollection GetMenuForNode(string id, FormDataCollection queryStrings)
        {
            var menu = new MenuItemCollection();
            menu.DefaultMenuAlias = ActionNew.Instance.Alias;
            menu.Items.Add<ActionNew>("Create");
            return menu;
        }
    }

    [PluginController("Transections")]
    public class TransectionsApiController : UmbracoAuthorizedJsonController
    {
        public object get(string year,string month)
        {
            var query = string.Format(@"select t1.*,t3.name as Car_name ,t4.Branch_Name as Branch_name 
                                        from tb_transection t1 
                                        inner join tb_branch_car t2 on t1.Car_Branch_ID = t2.A_ID
                                        inner join tb_car t3 on t2.Car_ID = t3.id
                                        inner join tb_branch t4 on t2.Branch_ID = t4.A_ID
                                        where year(t1.Add_Date) = {0} and month(t1.Add_Date) = {1}"
                                        , year
                                        , month);

            return DatabaseContext.Database.Query<dto_transection>(query).ToList();
        }

        public object getdetail(int id)
        {
            var transection = new dto_transection();

            transection = DatabaseContext.Database.Query<dto_transection>(string.Format("select top(1) * from tb_transection where ID = {0};", id)).FirstOrDefault();

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

            return transection;
        }
    }

}
