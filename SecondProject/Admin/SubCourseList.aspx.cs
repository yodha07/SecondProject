using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SecondProject.Admin
{
    public partial class SubCourseList : System.Web.UI.Page
    {
        SqlConnection conn;
        protected void Page_Load(object sender, EventArgs e)
        {
            string cs = ConfigurationManager.ConnectionStrings["ELearning_Project"].ConnectionString;
            conn = new SqlConnection(cs);
            conn.Open();
            if (!IsPostBack)
            {
                fetchData();
            }
        }

        public void fetchData()
        {
            string q = "exec el_GetAllSubCourse";
            SqlDataAdapter ada = new SqlDataAdapter(q, conn);
            DataTable dt = new DataTable();
            ada.Fill(dt);
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int ScId = int.Parse(e.CommandArgument.ToString());

            if (e.CommandName == "EditSc")
            {
                UpdateSc(ScId);
            }
            else if (e.CommandName == "DeleteSc")
            {
                DeleteSc(ScId);
                fetchData(); // refresh grid
            }

        }

        private void DeleteSc(int id)
        {
            try
            {
                string q2 = $"exec el_DeleteSubCourse '{id}'";
                SqlCommand cmd2 = new SqlCommand(q2, conn);
                cmd2.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                Response.Write("<script>alert('First delete the Topics/Memberships associated with this SubCourse')</script>");
            }
        }

        private void UpdateSc(int id)
        {
            string q = $"exec el_GetSubCourseById '{id}'";
            SqlCommand cmd = new SqlCommand(q, conn);
            SqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                txtTitle.Text = rdr["Title"].ToString();
                HiddenField2.Value = rdr["Thumbnail"].ToString();
                HiddenField1.Value = id.ToString();
                txtPrice.Text = rdr["Price"].ToString();
                txtMasterCourse.Text = rdr["MasterCourse"].ToString();
                txtThumbnail.Text = rdr["Thumbnail"].ToString();
            }
            rdr.Close();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#exampleModal').modal('show');", true);
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            int id = int.Parse(HiddenField1.Value);
            string newTitle = txtTitle.Text;
            double price = double.Parse(txtPrice.Text);
            string filePath = "";
            if (FileUpload1.HasFile)
            {
                string ext = Path.GetExtension(FileUpload1.FileName).ToLower();
                string[] allowedExts = { ".jpg", ".jpeg", ".png", ".gif" };

                if (!allowedExts.Contains(ext))
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "UploadFailed", "alert('Only image files are allowed.');", true);
                    return;
                }
                string fileName = newTitle + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ext;
                filePath = "Thumbnails/" + fileName;
                string folderPath = Server.MapPath("~/Thumbnails");

                // Create folder if it doesn't exist
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string fullPath = Server.MapPath("~/" + filePath);
                FileUpload1.SaveAs(fullPath);
            }
            else
            {
                filePath = HiddenField2.Value;
            }
            string q2 = $"exec el_UpdateSubCourse '{id}','{newTitle}','{filePath}','{price}'";
            SqlCommand cmd2 = new SqlCommand(q2, conn);
            int r = cmd2.ExecuteNonQuery();
            if (r > 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "UpdateSuccess", "alert('SubCourse updated successfully');", true);
                fetchData();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "UpdateFailed", "alert('SubCourse not updated');", true);
            }
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("SubCourseList.aspx");
        }
    }

}