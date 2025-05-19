using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.DataVisualization.Charting;
using System.Net.Mail;
using System.Net;


namespace SecondProject.Admin
{
    public partial class admindashboard : System.Web.UI.Page
    {
        SqlConnection conn;
        protected void Page_Load(object sender, EventArgs e)
        {

            string role = Session["Role"].ToString() ;
            if(role != "Admin")
            {
                Response.Redirect("~/Login/Login.aspx");
            }

            string cs = ConfigurationManager.ConnectionStrings["ELearning_Project"].ConnectionString;
            conn = new SqlConnection(cs);
            conn.Open();
            if (!IsPostBack)
            {
                status_active();
                status_inactive();
                master_course_count();
                sub_course_count();
                //chart();
                piechart();
                dropdown();
                sales_dropdown();
                grivience();

            }


        }
        protected void status_active()
        {
            string query = "exec count_is_Active";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            //reader.Read();
            DataList1.DataSource = reader;
            DataList1.DataBind();
            reader.Read();

        }
        protected void status_inactive()
        {
            string query = "exec count_is_inActive";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            //reader.Read();
            DataList2.DataSource = reader;
            DataList2.DataBind();
            reader.Read();
        }
        protected void master_course_count()
        {
            string query = "exec master_course_count";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            //reader.Read();
            DataList3.DataSource = reader;
            DataList3.DataBind();
            reader.Read();
        }
        protected void sub_course_count()
        {
            string query = "exec sub_course_count";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            //reader.Read();
            DataList4.DataSource = reader;
            DataList4.DataBind();
            reader.Read();
        }

        protected void piechart()
        {
            string query = "exec piechart";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            //reader.Read();
            Chart2.DataSource = reader;
            Chart2.DataBind();
            reader.Read();
        }
        //protected void chart()
        //{
        //    int year = 2020;
        //    string query = $"exec sale_data '{year}'";
        //    SqlCommand cmd = new SqlCommand(query, conn);
        //    SqlDataReader reader = cmd.ExecuteReader();
        //    //reader.Read();
        //    Chart1.DataSource = reader;
        //    Chart1.DataBind();  
        //    reader.Read();
        //}
        protected void dropdown()
        {
            string query = "exec dropdown";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader rdr = cmd.ExecuteReader();
            //reader.Read();
            //DropDownList1.DataSource = rdr;
            //DropDownList1.DataBind();
            while (rdr.Read())
            {
                DropDownList1.Items.Add(new ListItem(rdr["Title"].ToString(), rdr["MasterCourseId"].ToString()));

            }

        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedId = int.Parse(DropDownList1.SelectedValue);

            string query = $"exec dropdown_grid'{selectedId}'";

            SqlCommand cmd = new SqlCommand(query, conn);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            GridView1.DataSource = dt;
            GridView1.DataBind();
            //chart();
            piechart();

        }

        protected void sales_dropdown()
        {
            string query = "exec sales_dropdown";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader rdr = cmd.ExecuteReader();
            //reader.Read();
            DropDownList2.DataSource = rdr;
            DropDownList2.DataBind();
            //while (rdr.Read())
            //{
            //    DropDownList1.Items.Add(new ListItem(rdr["Title"].ToString(), rdr["MasterCourseId"].ToString()));

            //}

        }

        protected void DropDownList2_SelectedIndexChanged(object sender, EventArgs e)
        {
            int year = int.Parse(DropDownList2.SelectedValue);
            string query = $"exec sale_data '{year}'";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            //reader.Read();
            Chart1.DataSource = reader;
            Chart1.DataBind();
            reader.Read();
            piechart();
        }
        protected void grivience()
        {
            string query = $"exec reason_grid";

            SqlCommand cmd = new SqlCommand(query, conn);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            GridView2.DataSource = dt;
            GridView2.DataBind();
        }

        protected void GridView2_OnRowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "yes")
            {
                int id = int.Parse(e.CommandArgument.ToString());
                DateTime lastlogin = DateTime.Now;
                string query = $"exec reasons_yes '{lastlogin:yyyy-MM-dd HH:mm:ss}', '{id}'";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.ExecuteNonQuery();
                string query2 = $"select email from [user] where userid = '{id}'";
                SqlCommand cmd2 = new SqlCommand(query2, conn);
                SqlDataReader rdr2 = cmd2.ExecuteReader();
                string email = rdr2["email"].ToString();
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("suyashchavan.sit.comp@gmail.com");
                mail.To.Add(email);
                mail.Subject = "You can now login";
                mail.Body = " You can can now free to login ASSSK.Edu Team";

                SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                smtp.Credentials = new NetworkCredential("suyashchavan.sit.comp@gmail.com", "mdvriiwsxfoeihyz");
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.Send(mail);
                Response.Write("<script>alert('mail sent')</script>");
                Response.Redirect("admindashboard.aspx");


            }
            else if (e.CommandName == "no")
            {
                int id = int.Parse(e.CommandArgument.ToString());
                string query = $"exec reasons_no '{id}'";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.ExecuteNonQuery();
                string query2 = $"select email from [user] where userid = '{id}'";
                SqlCommand cmd2 = new SqlCommand(query2, conn);
                SqlDataReader rdr2 = cmd2.ExecuteReader();
                string email = rdr2["email"].ToString();
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("suyashchavan.sit.comp@gmail.com");
                mail.To.Add(email);
                mail.Subject = "You can now login";
                mail.Body = " You can can now free to login ASSSK.Edu Team";

                SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                smtp.Credentials = new NetworkCredential("suyashchavan.sit.comp@gmail.com", "mdvriiwsxfoeihyz");
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.Send(mail);
                Response.Redirect("admindashboard.aspx");

            }
        }

    }
}