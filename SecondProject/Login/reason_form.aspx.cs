using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SecondProject.Login
{
    public partial class reason_form : System.Web.UI.Page
    {
        SqlConnection conn;
        protected void Page_Load(object sender, EventArgs e)
        {
            string cs = ConfigurationManager.ConnectionStrings["ELearning_Project"].ConnectionString;
            conn = new SqlConnection(cs);
            conn.Open();
            if (!IsPostBack)
            {

            }


        }


        protected void Button1_Click(object sender, EventArgs e)
        {
            string name = TextBox3.Text;
            string email = TextBox1.Text;
            string reason = TextBox2.Text;
            string query2 = $"exec user_email_reason '{email}'";
            SqlCommand cmd2 = new SqlCommand(query2, conn);
            SqlDataReader reader = cmd2.ExecuteReader();
            reader.Read();
            int userid = int.Parse(reader["userid"].ToString());
            string query = $"exec reasons '{reason}', '{userid}'";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.ExecuteNonQuery();
            //Response.Write("<script>alert('add')</script>");
            mail();
        }
        protected void mail()
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("suyashchavan.sit.comp@gmail.com");
            mail.To.Add(TextBox1.Text);
            mail.Subject = "\"We Have Received Your Grievance Form\"";
            mail.Body = "Dear User,\n\nThank you for submitting your grievance form. We want to assure you that your concerns are important to us.\n\nOur team is currently reviewing the details you've provided, and we will get back to you with a response or resolution as soon as possible. If any further information is needed, we will reach out to you.\n\nWe appreciate your patience and understanding.\n\nBest regards,\nSupport Team\n[ASSSK.Edu]";

            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
            smtp.Credentials = new NetworkCredential("suyashchavan.sit.comp@gmail.com", "mdvriiwsxfoeihyz");
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.Send(mail);
            Response.Write("<script>alert('mail sent')</script>");


        }
    }
}