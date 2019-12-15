using ChicCarrental_Models.BaseModel;
using System.Net.Http.Formatting;
using umbraco.businesslogic;
using umbraco.BusinessLogic.Actions;
using umbraco.interfaces;
using Umbraco.Web.Models.Trees;
using Umbraco.Web.Mvc;
using Umbraco.Web.Trees;
using Umbraco.Web.WebApi;
using System.Linq;
using Umbraco.Web.Editors;
using System.Web.Http;
using System.Collections.Generic;

namespace ChicCarrental_Controller.Plugins
{
    [Application("branches", "branches", "icon-store")]
    public class branchesSectionApplication : IApplication { }

    [Umbraco.Web.Trees.Tree("branches", "branches", "Branch")]
    [PluginController("branches")]
    public class branchesSectionTreeController : TreeController
    {
        protected override TreeNodeCollection GetTreeNodes(string id, FormDataCollection queryStrings)
        {
            var nodes = new TreeNodeCollection();
            var query = @"Select * From tb_branch";
            var x = DatabaseContext.Database.Query<tb_branch>(query, new
            {
                id
            });
            var IconNode = "icon-store";
            foreach (var thing in x)
            {
                var node = CreateTreeNode(thing.A_ID.ToString(), id, queryStrings, thing.Branch_Name, IconNode, false);
                nodes.Add(node);
            }
            return nodes;
        }

        protected override MenuItemCollection GetMenuForNode(string id, FormDataCollection queryStrings)
        {
            var menu = new MenuItemCollection();
            if (id.Equals("-1"))
            {
                MenuItem menuItem = new MenuItem("create", "Create");
                menuItem.Icon = "icon icon-add";
                menuItem.NavigateToRoute("/branches/branches/edit/-1");
                menu.Items.Add(menuItem);
                menu.Items.Add<ActionRefresh>("Refresh");
            }
            else
            {
                MenuItem menuItem = new MenuItem("car", "Cars");
                menuItem.Icon = "icon icon-car";
                menuItem.NavigateToRoute("/branches/branches/car/" + id);
                menu.Items.Add(menuItem);
            }
            return menu;
        }
    }

    [PluginController("branches")]
    public class branchesApiController : UmbracoAuthorizedJsonController
    {
        public tb_branch get(int id)
        {
            var query = string.Format(@"select top(1) * from tb_branch where A_ID = {0}",id);
            return DatabaseContext.Database.Query<tb_branch>(query).FirstOrDefault();
        }

        [HttpPost]
        public int edit([FromBody]tb_branch form)
        {
            var query = $@" 
                        UPDATE [dbo].[tb_branch]
                               SET [Branch_Name] = @Branch_Name
                                  ,[Branch_Image] = @Branch_Image
                                  ,[Branch_Teaser] = @Branch_Teaser
                                  ,[Lat] = @Lat
                                  ,[Lon] = @Lon
                                  ,[Update_Date] = Getdate()
                                  ,[Status] = @Status
                             WHERE A_ID = @A_ID
                        ";
            
            return DatabaseContext.Database.Execute(query, form);
        }

        [HttpPut]
        public int create([FromBody]tb_branch form)
        {
            var query = $@" 
                       INSERT INTO [dbo].[tb_branch]
                                   ([Branch_Name]
                                   ,[Branch_Image]
                                   ,[Branch_Teaser]
                                   ,[Lat]
                                   ,[Lon]
                                   ,[Add_Date]
                                   ,[Update_Date]
                                   ,[Status])
                             VALUES
                                   (@Branch_Name
                                   ,@Branch_Image
                                   ,@Branch_Teaser
                                   ,@Lat
                                   ,@Lon
                                   ,Getdate()
                                   ,Getdate()
                                   ,@Status)
                        ";

            return DatabaseContext.Database.Execute(query,form);
        }

        [HttpGet]
        public object car(int id)
        {
            var query = string.Format(@"select t1.*,t2.name,t3.Branch_Name
                                        from tb_branch_car t1 
                                        inner join tb_car t2 on t1.Car_ID = t2.id
                                        inner join tb_branch t3 on t1.Branch_ID = t3.A_ID
                                        where t1.Branch_ID = {0} and t1.Status <>'D'", id);

            return DatabaseContext.Database.Query<dto_carprice>(query).ToList();
        }

        [HttpPost]
        public int caredit([FromBody]tb_branch_car form)
        {
            var query = $@" 
                        UPDATE [dbo].[tb_branch_car]
                               SET [Quantity] = @Quantity
                                  ,[Teaser] = @Teaser
                                  ,[Update_Date] = Getdate()
                                  ,[Status] = @Status
                             WHERE A_ID = @A_ID
                        ";

            return DatabaseContext.Database.Execute(query, form);
        }

        [HttpPut]
        public int carcreate([FromBody]tb_branch_car form)
        {
            var query = $@" 
                    INSERT INTO [dbo].[tb_branch_car]
                           ([Branch_ID]
                           ,[Car_ID]
                           ,[Teaser]
                           ,[Quantity]
                           ,[Status])
                     VALUES
                           (@Branch_ID
                           ,@Car_ID
                           ,@Teaser
                           ,@Quantity
                           ,@Status) ";

            return DatabaseContext.Database.Execute(query, form);
        }

        [HttpGet]
        public object price(int id)
        {
            var query = string.Format(@"select t1.A_ID,t4.*,t2.name,t3.Branch_Name,t1.Branch_ID
                                        from tb_branch_car t1
                                        inner join tb_car t2 on t1.Car_ID = t2.id
                                        inner join tb_branch t3 on t1.Branch_ID = t3.A_ID
                                        left join tb_car_price t4 on t1.A_ID = t4.car_branch_id
                                        where t1.A_ID = {0} and t4.status <> 'D'", id);

            return DatabaseContext.Database.Query<tb_car_price>(query).ToList();
        }

        [HttpPost]
        public int pricedit([FromBody]tb_car_price form)
        {
            var query = $@" 
                        UPDATE [dbo].[tb_car_price]
                               SET [car_branch_id] = @car_branch_id
                                  ,[numdate] = @numdate
                                  ,[start_date] = @start_date
                                  ,[end_date] = @end_date
                                  ,[price_cost] = @price_cost
                                  ,[price] = @price
                                  ,[price_code] = @price_code
                                  ,[paynow] = @paynow
                                  ,[status  ] = @status
                                  ,[paynow_type] = @paynow_type
                                  ,[update_date] = Getdate()
                             WHERE price_id = @price_id
                        ";

            return DatabaseContext.Database.Execute(query, form);
        }

        [HttpPut]
        public int pricecreate([FromBody]tb_car_price form)
        {
            var query = $@" 
                    INSERT INTO [dbo].[tb_car_price]
                           ([car_branch_id]
                           ,[numdate]
                           ,[start_date]
                           ,[end_date]
                           ,[price_cost]
                           ,[price]
                           ,[price_code]
                           ,[paynow]
                           ,[paynow_type]
                           ,[status])
                     VALUES
                           (@car_branch_id
                           ,@numdate
                           ,@start_date
                           ,@end_date
                           ,@price_cost
                           ,@price
                           ,@price_code
                           ,@paynow
                           ,@paynow_type
                           ,@status)";

            return DatabaseContext.Database.Execute(query, form);
        }

    }

}
