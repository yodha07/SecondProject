using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace SecondProject.Admin
{
    public partial class AddMasterCourse : System.Web.UI.Page
    {
        SqlConnection conn;
        protected void Page_Load(object sender, EventArgs e)
        {
            string cs = ConfigurationManager.ConnectionStrings["ELearning_Project"].ConnectionString;
            conn = new SqlConnection(cs);
            conn.Open();
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            string mcName = TextBox1.Text;
            string createdBy = "Admin";

            if (FileUpload1.HasFile)
            {
                string ext = Path.GetExtension(FileUpload1.FileName).ToLower();
                string[] allowedExts = { ".jpg", ".jpeg", ".png", ".gif" };

                if (!allowedExts.Contains(ext))
                {
                    Response.Write("<script>alert('Only image files are allowed.')</script>");
                    return;
                }
                string fileName = mcName + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ext;
                string relativePath = "Thumbnails/" + fileName;
                string folderPath = Server.MapPath("~/Thumbnails");

                // Create folder if it doesn't exist
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string fullPath = Server.MapPath("~/" + relativePath);
                FileUpload1.SaveAs(fullPath);

                string query = $"exec el_AddMasterCourse '{mcName}','{relativePath}','{createdBy}'";
                SqlCommand cmd = new SqlCommand(query, conn);
                int r = cmd.ExecuteNonQuery();
                if (r > 0)
                {
                    Response.Write("<script>alert('Master Course Added')</script>");
                }
                else
                {
                    Response.Write("<script>alert('Master Course Not Added')</script>");
                }
            }
            else
            {
                Response.Write("<script>alert('Please make sure a file is selected.')</script>");
                return;
            }
        }
    }
}