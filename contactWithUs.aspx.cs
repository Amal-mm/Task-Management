using Amal9909.App_Code;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Amal9909.demo3
{
    public partial class ContactWithUs : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnSendMailViaMailMgr_Click(object sender, EventArgs e)
        {
            sendEmail2();
        }
        private void InsertContactUs()
        {
            CRUD myCrud = new CRUD();
            string mySql = @"Insert contactus(senderEmail,subject, message) 
                        values (@senderEmail,@subject,@message)";
            Dictionary<string, object> myPara = new Dictionary<string, object>();
            myPara.Add("@senderEmail", txtSenderEmail.Text);
            myPara.Add("@subject", txtSubject.Text);
            myPara.Add("@message", txtBody.Text);
            int rtn = myCrud.InsertUpdateDelete(mySql, myPara);
            if (rtn >= 1)
            {
                lblMsg.Text = "Email log Successed";
                lblMsg.ForeColor = Color.Green;
            }
            else
            {
                lblMsg.Text = "Email log failed";
                lblMsg.ForeColor = Color.Red;
            }
        }

        /// <summary>
        /// updated on Oct 30,2020
        /// 
        /// to do :create a class to insert attached files to db and another one to inert attached files to folder
        /// </summary>
        protected void sendEmail2()
        {
            int intSenderId = 111;
            string senderName = Page.User.Identity.Name;
            senderName = (string.IsNullOrWhiteSpace(senderName)) ? "Admin" : Page.User.Identity.Name;
            string rtn = "";
            if (string.IsNullOrEmpty(txtSubject.Text) || string.IsNullOrEmpty(txtBody.Text))
            {
                lblMsg.Text = "Please fill Subject & email body";
                lblMsg.ForeColor = Color.Red;
                return;
            }

            //   InsertContactUs(); // to save email in database 
            string senderEmail = txtSenderEmail.Text;
            using (mailMgr myMailMgr = new mailMgr())
            {
                myMailMgr.mySubject = txtSubject.Text + " " + senderEmail + ": " + senderName;
                myMailMgr.myBody = txtBody.Text;
                if (fuAttachment.HasFile)
                {
                    foreach (HttpPostedFile file in fuAttachment.PostedFiles)
                    {
                        //1. get fileName
                        string fileName = Path.GetFileName(file.FileName);
                        //2. eihter save it to folder or 
                        //. file.SaveAs(Server.MapPath("~/docEmailed/") + fileName);// to save attached files to a folder docEmailed
                        //3. send it to email or 
                        //.  myMailMgr.Attachments.Add(new Attachment(file.InputStream, fileName)); // to attached files to email 

                        // 4. save it to db   
                        //  InsertDocuments(intSenderId);  // works but double entry
                    }
                    rtn = myMailMgr.sendEmailViaGmail(fuAttachment);
                }
                else
                {
                    rtn = myMailMgr.sendEmailViaGmail();
                }
                lblMsg.Text = rtn;
                lblMsg.ForeColor = Color.Green; // using System.Drawing above 2/2018
                lblMsg.BackColor = Color.Yellow;
            }
        }

        protected void InsertDocuments(int myClientId)
        {
            // this works 
            foreach (HttpPostedFile postedFile in fuAttachment.PostedFiles)
            {
                string filename = Path.GetFileName(postedFile.FileName);
                string contentType = postedFile.ContentType;
                using (Stream fs = postedFile.InputStream)
                {
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        byte[] bytes = br.ReadBytes((Int32)fs.Length);
                        CRUD DocInsert = new CRUD();
                        string mySql = @"insert into trainerDoc(trainerId,DocName,contentType,DocData) 
                                          values (@trainerId,@DocName,@contentType,@DocData)";
                        Dictionary<string, object> myPara = new Dictionary<string, object>();
                        myPara.Add("@trainerId", myClientId);
                        myPara.Add("@DocName", filename);
                        myPara.Add("@contentType", contentType);
                        myPara.Add("@DocData", bytes);
                        int rtn = DocInsert.InsertUpdateDelete(mySql, myPara);
                        lblMsg.Text = rtn.ToString();
                        // common.PostMsg(lblOutput, "thanks for submitting your data ");
                        //common.PostMsg(lblOutput,rtn,"This is custom msg based on rtn passed");
                    }
                }
            }


            //////    // this works too for a single posted file
            //////    string filename = Path.GetFileName(fuAttachment.PostedFile.FileName);
            //////    string contentType = fuAttachment.PostedFile.ContentType;
            //////using (Stream fs = fuAttachment.PostedFile.InputStream)
            //////{
            //////    using (BinaryReader br = new BinaryReader(fs))
            //////    {
            //////        byte[] bytes = br.ReadBytes((Int32)fs.Length);
            //////        CRUD DocInsert = new CRUD();
            //////        string mySql = @"insert into trainerDoc(trainerId,DocName,contentType,DocData) 
            //////                              values (@trainerId,@DocName,@contentType,@DocData)";
            //////        Dictionary<string, object> myPara = new Dictionary<string, object>();
            //////        myPara.Add("@trainerId", myClientId);
            //////        myPara.Add("@DocName", filename);
            //////        myPara.Add("@contentType", contentType);
            //////        myPara.Add("@DocData", bytes);
            //////        int rtn = DocInsert.InsertUpdateDelete(mySql, myPara);
            //////        common.PostMsg(lblMsg, rtn);
            //////        // common.PostMsg(lblOutput, "thanks for submitting your data ");
            //////        //common.PostMsg(lblOutput,rtn,"This is custom msg based on rtn passed");
            //////    }
            //////}

        }
        protected void lblOutputClear_txtSubject(object sender, EventArgs e)
        {
            lblMsg.Text = "";
        }

        protected void btnSendViaCode_Click(object sender, EventArgs e)
        {

        }

        //protected void Button1_Click(object sender, EventArgs e)
        //{
        //        List<string> AcceptanceSent = new List<string>();
        //        string myTab = "\t";
        //        string myNextLine = "<br/>";// "\r\n";
        //        using (System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage())
        //        {
        //            msg.From = new MailAddress("appdev4y@gmail.com");
        //            msg.Subject = "Internship Training Acceptance letter";
        //            //  msg.Body = "<h1>Hello user</h1> Email Body content";
        //            msg.IsBodyHtml = true;
        //            foreach (string file in files)
        //            {
        //                int index = file.IndexOf(".pdf");
        //                //int intRefNo = int.Parse(file.Substring(0, index));
        //                string myEmail = (file.Substring(0, index));
        //                //  string strInternEmail = getInternEmail(intRefNo);\

        //                msg.Subject = "Internship Training Acceptance letter for : " + file;  // intRefNo;
        //                msg.To.Add(new MailAddress(myEmail));   //strInternEmail   myEmail  file
        //                msg.Attachments.Add(new Attachment(Server.MapPath("~/myPdf/" + file))); // Attach Each file=
        //                string strBody = "";

        //                strBody += @"Dear Intern, " + myNextLine;
        //                strBody += @" " + myNextLine;
        //                //strBody += @" Attached your Training Certificate" + myNextLine;
        //                strBody += @" Attached your acceptance letter. Training will start on March 12, 2023." + myNextLine;
        //                strBody += @"Make sure to have a laptop with the following specifications:" + myNextLine;
        //                strBody += @"<pre>  
        //                    Operating System : windows
        //                    Processor : i5 or more 
        //                    Ram : 8 GB or more 
        //                    Disk space : 500 GB

        //                    Note: Please read attached file, if you received online training acceptance, make sure you get approval from your university supervisor. 
        //                                    </pre> " + myNextLine;
        //                strBody += @" " + myNextLine;
        //                strBody += @" " + myNextLine;
        //                strBody += @" <pre>
        //                Regards,

        //                Ali Mohamed Hamidaddin
        //                IT Consultant, Chairperson of Application Training Department
        //                Executive Administration of Information Technology
        //                King Fahad Medical City
        //                P.O. Box 59046 ,Riyadh 11525
        //                Kingdom of Saudi Arabia
        //                (+966) 11 288 9999 Ext: 19110
        //                (+966) 538692448 
        //                ahameed@kfmc.med.sa
        //                </pre>


        //                " + myNextLine;
        //                msg.Body = strBody;
        //                SmtpClient smtp = new SmtpClient();
        //                smtp.Host = "smtp.gmail.com";
        //                smtp.EnableSsl = true;
        //                System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
        //                NetworkCred.UserName = msg.From.Address;
        //                NetworkCred.Password = "qenikcidyavnzspy";
        //                smtp.UseDefaultCredentials = true;
        //                smtp.Credentials = NetworkCred;
        //                smtp.Port = 587;
        //                smtp.Send(msg);
        //                msg.To.Clear();
        //                msg.Attachments.Clear();
        //                AcceptanceSent.Add(myEmail);
        //            }
        //            if (msg.Attachments != null)//added
        //                msg.Attachments.Dispose();// added
        //            msg.Dispose();// to clear the object 
        //        }
        //        //     UpdateAcceptanceSent(AcceptanceSent);  // continue 
        //    }


        //protected void Button1_Click(object sender, EventArgs e)
        //{
        //    mailMgr myMailmgr = new mailMgr();
        //    string rtn=  myMailmgr.sendEmailViaGmail2("aalhussein@yahoo.com", "aalhussein63@gmail.com", "test");  // to notify user via eamil done mar 2020
        //    lblMsg.Text = rtn;
        //}
    }
}