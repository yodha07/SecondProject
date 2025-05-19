using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SecondProject.User
{
    public partial class UserDashBoard : System.Web.UI.Page
    {
        SqlConnection conn;

        protected void Page_Load(object sender, EventArgs e)
        {
            string cs = ConfigurationManager.ConnectionStrings["ELearning_Project"].ConnectionString;
            conn = new SqlConnection(cs);
            conn.Open();
            if (!IsPostBack)
            {
                Lastlogin();
                                                                            
            }


        }
        protected void course()
        {
            int user = int.Parse(Session["UserId"].ToString());
            string query = $"exec user_subcourse '{user}'";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            //reader.Read();
            DataList1.DataSource = reader;
            DataList1.DataBind();
            reader.Read();

        }
        protected void complete_course()
        {
            int user = int.Parse(Session["UserId"].ToString()); 
            string query = $"exec compelete_subcourse_user '{user}'";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            //reader.Read();
            DataList2.DataSource = reader;
            DataList2.DataBind();
            reader.Read();

        }
        protected void incomplete_course()
        {
            int user = int.Parse(Session["UserId"].ToString()); 

            string query = $"exec incomplete_subcourse_user '{user}'";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            //reader.Read();
            DataList3.DataSource = reader;
            DataList3.DataBind();
            reader.Read();

        }
        protected void piechart()
        {
            int user = int.Parse(Session["UserId"].ToString()); ;

            string query = $"exec user_piechart '{user}'";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            //reader.Read();
            Chart1.DataSource = reader;
            Chart1.DataBind();
            reader.Read();
        }
        protected void progress_bar()
        {
            int user = int.Parse(Session["UserId"].ToString()); ;

            string query = $"exec user_subcourse '{user}'";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            int total = int.Parse(reader["count"].ToString());
            string query2 = $"exec compelete_subcourse_user '{user}'";
            SqlCommand cmd2 = new SqlCommand(query2, conn);
            SqlDataReader reader2 = cmd2.ExecuteReader();
            //reader.Read();
            reader2.Read();
            int complete = int.Parse(reader2["count"].ToString());

            double percentage;
            if (total == 0)
            {
                percentage = 0;
            }
            else
            {
                //string a =  (complete / total * 100).ToString();
                //percentage = double.Parse(a);
                percentage = (double)complete / total * 100;
            }
            progressBar.Style["width"] = percentage + "%";
            label4.Text = $"{percentage:0.##}% Complete";

        }
        protected void month_duration()
        {
            int user = int.Parse(Session["UserId"].ToString()); 
            int subcourseid = 2;
            string query = $"exec month_limit '{user}' , '{subcourseid}'";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.HasRows)
            {
                rdr.Read();
                DateTime purchase_date = Convert.ToDateTime(rdr["PurchaseDate"]);
                TimeSpan month = DateTime.Now - purchase_date;
                if (month.TotalDays <= 30)
                {
                    int current_days = (int)month.TotalDays;
                    int daysRemaining = 30 - current_days;
                    Label1.Text = $"You Have {daysRemaining} Days Remaining";
                }
                else
                {
                    Label1.Text = "Your access to this subcourse has expired.";
                }
            }
        }
        protected void Lastlogin()
        {

            int user = int.Parse(Session["UserId"].ToString());
            string query = $"exec last_login '{user}'";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.HasRows)
            {
                rdr.Read();
                DateTime Lastlogin = Convert.ToDateTime(rdr["Lastlogin"]);
                TimeSpan Times = DateTime.Now - Lastlogin;
                if (Times.TotalDays <= 7)
                {
                    int current_times = (int)Times.TotalDays;
                    int pastlogin = 7 - current_times;
                    Label2.Text = $"You Have login {current_times} Days ago";
                    DateTime latestlogin = DateTime.Now;
                    string query2 = $"exec latest_login '{user}' , '{latestlogin:yyyy-MM-dd HH:mm:ss}'";
                    SqlCommand cmd2 = new SqlCommand(query2, conn);
                    int r=cmd2.ExecuteNonQuery();
                    if(r>0)
                    {
                        course();
                        complete_course();
                        incomplete_course();
                        piechart();
                        progress_bar();
                        month_duration();

                        user_subcourse_fetch();
                    }

                }
                else
                {
                    //Response.Write("<script>alert('You have not Login from past 1 Week\nPlease contact Admin');</script>");
                    
                   
                    
                    string script = "alert('You have not Login from past 1 Week\\nPlease contact Admin'); window.location='~/Login/Login.aspx';";
                    ClientScript.RegisterStartupScript(this.GetType(), "alertAndRedirect", script, true);
                    Session.Clear();
                    Session.Abandon();
                    //Response.Redirect("~/Login/Login.aspx");
                    //Label2.Text = "you have not login from past 1 week";

                }
            }
        }
        protected void user_subcourse_fetch()
        {
            int user = int.Parse(Session["UserId"].ToString());
            string query = $"exec get_subcourse_user '{user}'";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader rdr = cmd.ExecuteReader();
            DataList4.DataSource = rdr;
            DataList4.DataBind();
            rdr.Read();

            //if (rdr.HasRows)
            //{
            //    rdr.Read();
            //    int subcourse = int.Parse( rdr["SubCourseId"].ToString());
            //    string query2 = $"exec subcourse_progress_bar '{user}' ,'{subcourse}'";
            //    SqlCommand cmd2 = new SqlCommand( query2, conn);
            //    SqlDataReader rdr2 = cmd2.ExecuteReader();
            //    rdr2.Read();
            //    int iswatch = int.Parse(rdr2["VideosWatched"].ToString());
            //    int total = int.Parse(rdr2["total_videos"].ToString());
            //    double percentage;
            //    if (total == 0) 
            //    {
            //        percentage = 0;
            //    }
            //    else
            //    {
            //        percentage = (double)iswatch / total * 100;
            //    }
            //Label lblId = (Label)item.FindControl("LblID");
            //}
        }

        protected void DataList4_ItemCommand(object source, DataListCommandEventArgs e)
        {


            if (e.CommandName == "Select")
            {
                // Optional: Get the user ID if needed
                //int userId = Convert.ToInt32(DataList4.DataKeys[e.Item.ItemIndex]);

                // 🔹 Access the LinkButton first

                int user = int.Parse(Session["UserId"].ToString());
                int subcourse = int.Parse(e.CommandArgument.ToString());
                double percentage = 0;
                //string query = $"exec get_subcourse_user '{user}'";
                //SqlCommand cmd = new SqlCommand(query, conn);
                //SqlDataReader rdr = cmd.ExecuteReader();
                ////DataList4.DataSource = rdr;
                ////DataList4.DataBind();
                //if (rdr.HasRows)
                //{
                //    rdr.Read();
                //int subcourse = int.Parse(rdr["SubCourseId"].ToString());
                string query2 = $"exec subcourse_progress_bar '{user}' ,'{subcourse}'";
                SqlCommand cmd2 = new SqlCommand(query2, conn);
                SqlDataReader rdr2 = cmd2.ExecuteReader();


                if (rdr2.Read())
                {

                    int iswatch = int.Parse(rdr2["VideosWatched"].ToString());
                    int total = int.Parse(rdr2["total_videos"].ToString());

                    if (total == 0)
                    {
                        percentage = 0;
                    }
                    else
                    {
                        percentage = (double)iswatch / total * 100;
                    }

                }
                HtmlGenericControl progressBar = (HtmlGenericControl)e.Item.FindControl("progressBar1");
                Label lbl = (Label)e.Item.FindControl("label3");
                //double percentage1 = percentage
                if (progressBar != null && lbl != null)
                {
                    progressBar.Attributes["style"] = $"width: {percentage}%";
                    lbl.Text = $"{percentage:0.##}% Complete";
                }
            }

        }

    }
}