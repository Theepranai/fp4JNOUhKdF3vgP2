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

namespace ChicCarrental_Controller.Plugins
{
    [Application("cars", "cars", "icon-car")]
    public class carsSectionApplication : IApplication { }

    [Umbraco.Web.Trees.Tree("cars", "cars", "cars")]
    [PluginController("cars")]
    public class carsSectionTreeController : TreeController
    {
        protected override TreeNodeCollection GetTreeNodes(string id, FormDataCollection queryStrings)
        {
            var nodes = new TreeNodeCollection();
            var query = @"Select * From tb_car";
            var x = DatabaseContext.Database.Query<tb_car>(query, new
            {
                id
            });
            var IconNode = "icon-car";
            foreach (var thing in x)
            {
                var node = CreateTreeNode(thing.id.ToString(), id, queryStrings, thing.name, IconNode, false);
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
                menuItem.NavigateToRoute("/cars/cars/edit/-1");
                menu.Items.Add(menuItem);
                menu.Items.Add<ActionRefresh>("Refresh");
            }
            
            return menu;
        }
    }

    [PluginController("cars")]
    public class carsApiController : UmbracoAuthorizedJsonController
    {
        public tb_car get(int id)
        {
            var query = string.Format(@"select top(1) * from tb_car where id = {0}", id);
            return DatabaseContext.Database.Query<tb_car>(query).FirstOrDefault();
        }

        [HttpPost]
        public int edit([FromBody]tb_car form)
        {
            var query = $@" 
                        UPDATE [dbo].[tb_car]
                           SET [image] = @image
                              ,[brand] = @brand
                              ,[name] = @name
                              ,[vgroup] = @vgroup
                              ,[sipp] = @sipp
                              ,[feature] = @feature
                              ,[tranmission] = @tranmission
                              ,[year] = @year
                              ,[enging] = @enging
                              ,[door] = @door
                              ,[passenger] = @passenger
                              ,[luggage] = @luggage
                              ,[music_system] = @music_system
                              ,[status] = @status
                              ,[update_Date] = Getdate()
                         WHERE id = @id
                        ";
            
            return DatabaseContext.Database.Execute(query, form);
        }

        [HttpPut]
        public int create([FromBody]tb_car form)
        {
            var query = $@" 
                       INSERT INTO [dbo].[tb_car]
                                        ([image]
                                        ,[brand]
                                        ,[name]
                                        ,[vgroup]
                                        ,[sipp]
                                        ,[feature]
                                        ,[tranmission]
                                        ,[year]
                                        ,[enging]
                                        ,[door]
                                        ,[passenger]
                                        ,[luggage]
                                        ,[music_system]
                                        ,[status])
                                    VALUES
                                        (@image
                                        ,@brand
                                        ,@name
                                        ,@vgroup
                                        ,@sipp
                                        ,@feature
                                        ,@tranmission
                                        ,@year
                                        ,@enging
                                        ,@door
                                        ,@passenger
                                        ,@luggage
                                        ,@music_system
                                        ,@status)";

            return DatabaseContext.Database.Execute(query,form);
        }

        [HttpGet]
        public object getlist(int id)
        {
            var query = string.Format(@"SELECT * FROM tb_car", id);
            return DatabaseContext.Database.Query<tb_car>(query);
        }

    }

}
