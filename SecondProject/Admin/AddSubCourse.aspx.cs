using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SecondProject.Admin
{
    public partial class AddSubCourse : System.Web.UI.Page
    {
        SqlConnection conn;
        protected void Page_Load(object sender, EventArgs e)
        {
            string cs = ConfigurationManager.ConnectionStrings["ELearning_Project"].ConnectionString;
            conn = new SqlConnection(cs);
            conn.Open();
            if (!IsPostBack)
            {
                loadMcName();
            }
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            string scName = TextBox1.Text;
            double price = double.Parse(TextBox2.Text);
            int id = int.Parse(DropDownList1.SelectedValue);

            if (FileUpload1.HasFile)
            {
                string ext = Path.GetExtension(FileUpload1.FileName).ToLower();
                string[] allowedExts = { ".jpg", ".jpeg", ".png", ".gif" };

                if (!allowedExts.Contains(ext))
                {
                    Response.Write("<script>alert('Only image files are allowed.')</script>");
                    return;
                }
                string fileName = scName + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ext;
                string relativePath = "Thumbnails/" + fileName;
                string folderPath = Server.MapPath("~/Thumbnails");

                // Create folder if it doesn't exist
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string fullPath = Server.MapPath("~/" + relativePath);
                FileUpload1.SaveAs(fullPath);

                string query = $"exec el_AddSubCourse '{id}','{scName}','{relativePath}','{price}'";
                SqlCommand cmd = new SqlCommand(query, conn);
                int r = cmd.ExecuteNonQuery();
                if (r > 0)
                {
                    string query2 = $"exec el_GetUser";
                    SqlCommand cmd2 = new SqlCommand(query2, conn);
                    SqlDataReader rdr2 = cmd2.ExecuteReader();
                    while (rdr2.Read())
                    {
                        string name = rdr2["FullName"].ToString();
                        string email = rdr2["Email"].ToString();
                        MailMessage mail = new MailMessage();
                        mail.From = new MailAddress("kstesterjune@gmail.com");
                        mail.To.Add(email);
                        mail.Subject = "New Course";
                        mail.Body = $"Dear {name},\n\nWe have brought a new {scName} course, starting at a price of just ₹{price}.\n\nRegards,\nASSSK Edu";

                        SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                        smtp.Credentials = new NetworkCredential("kstesterjune@gmail.com", "dfmjyqbqfmuqqqdm");
                        smtp.Port = 587;
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                    Response.Write("<script>alert('Sub Course Added')</script>");
                }
                else
                {
                    Response.Write("<script>alert('Sub Course Not Added')</script>");
                }
            }
            else
            {
                Response.Write("<script>alert('Please make sure a file is selected.')</script>");
                return;
            }
        }

        public void loadMcName()
        {
            DropDownList1.Items.Add(new ListItem("--Select Master Course--", "0"));
            string query = $"exec el_GetMasterCourse";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                DropDownList1.Items.Add(new ListItem(rdr["Title"].ToString(), rdr["MasterCourseId"].ToString()));
            }
            rdr.Close();
        }
    }

}