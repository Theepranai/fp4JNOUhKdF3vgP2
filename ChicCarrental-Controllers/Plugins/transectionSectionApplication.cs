using ChicCarrental_Models.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using umbraco.businesslogic;
using umbraco.BusinessLogic.Actions;
using umbraco.interfaces;
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
            var item = this.CreateTreeNode("dashboard", id, queryStrings, "My item", "icon-truck", true);
            nodes.Add(item);
            var query = @"Select * From tb_branch";
            var x = DatabaseContext.Database.Query<tb_branch>(query, new
            {
                id
            });
            var IconNode = "icon-store";
            foreach (var thing in x)
            {
                var node = CreateTreeNode(thing.A_ID.ToString(), id, queryStrings, thing.Branch_Name, IconNode, true);
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
}
