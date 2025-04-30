using Amal9909.App_Code;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Amal9909.demo3
{
    public partial class Repeator : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadTasks();
        }
        protected void LoadTasks()
        {
            CRUD myCrud = new CRUD();
            string mySql = @"SELECT * FROM v_TaskInfo";
            SqlDataReader dr = myCrud.getDrPassSql(mySql);

            rptTasks.DataSource = dr;
            rptTasks.DataBind();
        }

        protected void rptTasks_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

        }
    }
}