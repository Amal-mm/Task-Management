using Amal9909.App_Code;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace demo3.Amal9909
{
    public partial class TaskManegeer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                populateDdlPriority();
                populateRbTaskType();
                


            }
           
        }


        public override void VerifyRenderingInServerForm(Control control)
        {

        }


        protected void populateDdlPriority()
        {
            CRUD myCrud = new CRUD();
            string mySql = @"select PriorityId, PriorityName from Priority";
            SqlDataReader dr = myCrud.getDrPassSql(mySql);
            ddlPriority.DataValueField = "PriorityId";
            ddlPriority.DataTextField = "PriorityName";
            ddlPriority.DataSource = dr;
            ddlPriority.DataBind();
        }

        protected void populateRbTaskType()
        {
            

            
            CRUD myCrud = new CRUD();
            string mySql = @"select taskTypeId , taskType from taskType";
            SqlDataReader dr = myCrud.getDrPassSql(mySql);
            rblTaskType.DataValueField = "taskTypeId";
            rblTaskType.DataTextField = "taskType";
            rblTaskType.DataSource = dr;
            rblTaskType.DataBind();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            if (String.IsNullOrEmpty(rblTaskType.Text))
            {
                lblOoutput.Text = "Please fill task type field!";
                lblOoutput.ForeColor = System.Drawing.Color.Red;
                txtTaskTitle.Focus();
                return;
            }

            if (String.IsNullOrEmpty(txtTaskTitle.Text))
            {
                lblOoutput.Text = "Please fill task title field!";
                lblOoutput.ForeColor = System.Drawing.Color.Red;
                txtTaskTitle.Focus();
                return;
            }

            if (String.IsNullOrEmpty(txtTaskDescription.Text))
            {
                lblOoutput.Text = "Please fill task description desc field!";
                lblOoutput.ForeColor = System.Drawing.Color.Red;
                txtTaskDescription.Focus();
                return;
            }

            CRUD myCrud = new CRUD();
            string mySql = @"insert Tasks(taskTitle,taskDescription,PriorityId,DueDate,Complete,taskTypeId)
                           values(@taskTitle,@taskDescription,@PriorityId,@DueDate,@Complete,@taskTypeId)";
            Dictionary<string, object> myPara = new Dictionary<string, object>();
            myPara.Add("@taskTitle", txtTaskTitle.Text);
            myPara.Add("@taskDescription",txtTaskDescription.Text);
            myPara.Add("@PriorityId",ddlPriority.SelectedValue);
            myPara.Add("@Complete", cblComplete.Checked);
            myPara.Add("@taskTypeId", rblTaskType.SelectedValue);
            myPara.Add("@DueDate", txtDate.Text);
            int rtn =  myCrud.InsertUpdateDelete(mySql, myPara);
              if (rtn >= 1)
              {
                lblOoutput.Text = "The task was submited successfully! ";
              }
              else
              { lblOoutput.Text = "Failed to submit the task! "; }
            populateGvTasks();
        }





        protected void ddlPriority_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void Button6_Click(object sender, EventArgs e)
        {
            ExportGridToExcel(gvGetTaskInfo);
        }

        protected void cblComplete_CheckedChanged(object sender, EventArgs e)
        {

        }

        protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnGetData_Click(object sender, EventArgs e)
        {
            populateGvTasks();
        }

        protected void populateGvTasks()
        {
            CRUD myCrud = new CRUD();
            string mySql = @"select * from v_TaskInfo";
            SqlDataReader dr = myCrud.getDrPassSql(mySql);
            gvGetTaskInfo.DataSource = dr;
            gvGetTaskInfo.DataBind();

        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtTaskId.Text))
            {
                lblOoutput.Text = "Please fill task ID field!";
                lblOoutput.ForeColor = System.Drawing.Color.Red;
                txtTaskTitle.Focus();
                return;
            }
            CRUD myCrud = new CRUD();
            string mySql = @"delete Tasks
                            where taskID = @taskID";
            Dictionary<string, object> myPara = new Dictionary<string, object>();
            myPara.Add("@taskID",txtTaskId.Text);
           
            int rtn = myCrud.InsertUpdateDelete(mySql, myPara);
            if (rtn >= 1)
            {
                lblOoutput.Text = "The task was deleted successfully! ";
            }
            else
            { lblOoutput.Text = "Failed to delete the task! "; }
            populateGvTasks();
        
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {

            if (String.IsNullOrEmpty(txtTaskId.Text))
            {
                lblOoutput.Text = "Please fill task ID field!";
                lblOoutput.ForeColor = System.Drawing.Color.Red;
                txtTaskTitle.Focus();
                return;
            }
            CRUD myCrud = new CRUD();
            string mySql = @"update Tasks
                             set taskTitle = @taskTitle, taskDescription = @taskDescription, PriorityId = @PriorityId, Complete = @Complete , taskTypeId = @taskTypeId ,DueDate =@DueDate
                                    where taskID = @taskID";
            Dictionary<string, object> myPara = new Dictionary<string, object>();
            myPara.Add("@taskID",int.Parse(txtTaskId.Text));
            myPara.Add("@taskTitle", txtTaskTitle.Text);
            myPara.Add("@taskDescription", txtTaskDescription.Text);
            myPara.Add("@PriorityId", ddlPriority.SelectedValue);
            myPara.Add("@Complete", cblComplete.Checked);
            myPara.Add("@taskTypeId", rblTaskType.SelectedValue);
            myPara.Add("@DueDate", txtDate.Text);
            int rtn = myCrud.InsertUpdateDelete(mySql, myPara);
            if (rtn >= 1)
            {
                lblOoutput.Text = "The task was updateed successfully! ";
            }
            else
            { lblOoutput.Text = "Failed to update the task!"; }
            populateGvTasks();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtTaskId.Text = "";
            txtTaskTitle.Text = "";
            txtTaskDescription.Text = "";
            cblComplete.Checked = false;
            
        }


        protected void populateForm_Click(object sender, EventArgs e)// hyperLink
        {
            int PK = int.Parse((sender as LinkButton).CommandArgument);
            //lblOuput.Text = PK.ToString();

            string mySql = @"  select TaskID,taskTitle,taskDescription,PriorityId,DueDate,Complete,taskTypeId
                                         from Tasks 
                                               where TaskID =@TaskID";
            Dictionary<string, object> myPara = new Dictionary<string, object>();
            myPara.Add("@TaskID", PK);
            CRUD myCrud = new CRUD();
            using (SqlDataReader dr = myCrud.getDrPassSql(mySql, myPara))
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        String taskId = dr["TaskID"].ToString();
                        String taskTitle = dr["taskTitle"].ToString();
                        String taskDescription = dr["taskDescription"].ToString();
                        String PriorityId = dr["PriorityId"].ToString();
                        String DueDate = dr["DueDate"].ToString();
                        bool Complete = bool.Parse(dr["Complete"].ToString());
                        String taskTypeId = dr["taskTypeId"].ToString();


                        //lblOuput.Text = empId + employee+ depId;
                        txtTaskId.Text = taskId;
                        txtTaskTitle.Text = taskTitle;
                        txtTaskDescription.Text = taskDescription;
                        ddlPriority.SelectedValue = PriorityId;
                        txtDate.Text = DueDate;
                        cblComplete.Checked = Complete;
                        rblTaskType.SelectedValue = taskTypeId;



                    }
                }
            }
        }

        public static void ExportGridToExcel(GridView myGv) // working 1
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.Charset = "";
            string FileName = "ExportedReport_" + DateTime.Now + ".xls";
            StringWriter strwritter = new StringWriter();
            HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
            myGv.GridLines = GridLines.Both;
            myGv.HeaderStyle.Font.Bold = true;
            myGv.RenderControl(htmltextwrtter);
            HttpContext.Current.Response.Write(strwritter.ToString());
            HttpContext.Current.Response.End();
        }

    


    }

        
 }