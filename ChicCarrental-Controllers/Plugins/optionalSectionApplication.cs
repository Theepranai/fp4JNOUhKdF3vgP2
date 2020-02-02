using ChicCarrental_Models.BaseModel;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using umbraco.businesslogic;
using umbraco.BusinessLogic.Actions;
using umbraco.interfaces;
using Umbraco.Web.Editors;
using Umbraco.Web.Models.Trees;
using Umbraco.Web.Mvc;
using Umbraco.Web.Trees;

namespace ChicCarrental_Controller.Plugins
{
    [Application("optional", "optional", "icon-checkbox")]
    public class optionalSectionApplication : IApplication { }

    [PluginController("optional")]
    [Umbraco.Web.Trees.Tree("optional", "optional", "optional")]
    public class optionalSectionTreeController : TreeController
    {
        protected override TreeNodeCollection GetTreeNodes(string id, FormDataCollection queryStrings)
        {
            var nodes = new TreeNodeCollection();
            var query = @"Select * From tb_optional";
            var x = DatabaseContext.Database.Query<tb_optional>(query);

            var IconNode = "icon-check";

            foreach (var thing in x)
            {
                if (thing.Status == "U"){
                    IconNode = "icon-wrong";
                } else {
                    IconNode = "icon-check";
                };

                var node = CreateTreeNode(thing.A_ID.ToString(), id, queryStrings, thing.Name, IconNode, false);

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
                menuItem.NavigateToRoute("/optional/optional/edit/-1");
                menu.Items.Add(menuItem);
                menu.Items.Add<ActionRefresh>("Refresh");
            }
            return menu;
        }
    }

    [PluginController("optional")]
    public class optionalApiController : UmbracoAuthorizedJsonController
    {
        public tb_optional get(int id)
        {
            var query = string.Format(@"select top(1) * from tb_optional where A_ID = {0}", id);
            return DatabaseContext.Database.Query<tb_optional>(query).FirstOrDefault();
        }

        [HttpPost]
        public int edit([FromBody]tb_optional form)
        {
            var query = $@" 
                        UPDATE [dbo].[tb_optional]
                           SET [GroupName] = @GroupName
                              ,[Name] = @Name
                              ,[Teaser] = @Teaser
                              ,[Price] = @Price
                              ,[Status] = @Status
                              ,[Add_Date] = Getdate()
                              ,[Update_Date] = Getdate()
                             WHERE A_ID = @A_ID
                        ";

            return DatabaseContext.Database.Execute(query, form);
        }

        [HttpPut]
        public int create([FromBody]tb_optional form)
        {
            var query = $@" 
                      INSERT INTO [dbo].[tb_optional]
                           ([GroupName]
                           ,[Name]
                           ,[Teaser]
                           ,[Price]
                           ,[Status]
                           ,[Add_Date]
                           ,[Update_Date])
                     VALUES
                           (@GroupName
                           ,@Name
                           ,@Teaser
                           ,@Price
                           ,@Status
                           ,Getdate()
                           ,Getdate())
                        ";

            return DatabaseContext.Database.Execute(query, form);
        }

    }

}