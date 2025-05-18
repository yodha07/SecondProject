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
    public partial class AssignmentList : System.Web.UI.Page
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
            string q = "exec el_GetAllAssi";
            SqlDataAdapter ada = new SqlDataAdapter(q, conn);
            DataTable dt = new DataTable();
            ada.Fill(dt);
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int AsId = int.Parse(e.CommandArgument.ToString());

            if (e.CommandName == "ViewAs")
            {
                string q = $"exec el_GetAssiById '{AsId}'";
                SqlCommand cmd = new SqlCommand(q, conn);
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    string filePath = rdr["Assignment"].ToString();
                    if (!string.IsNullOrEmpty(filePath))
                    {
                        string fullPath = Server.MapPath("~/" + filePath);

                        if (File.Exists(fullPath))
                        {
                            Response.ContentType = GetContentType(fullPath);
                            Response.WriteFile(fullPath);
                            Response.End();
                        }
                        else
                        {
                            Response.Write("<script>alert('File not found.');</script>");
                        }
                    }
                }
                else
                {
                    Response.Write("<script>alert('No file path found for the selected document.');</script>");
                }
            }
            else if (e.CommandName == "EditAs")
            {
                UpdateAs(AsId);
            }
            else if (e.CommandName == "DeleteAs")
            {
                DeleteAs(AsId);
                fetchData(); // refresh grid
            }

        }

        private void DeleteAs(int id)
        {
            string q2 = $"exec el_DeleteAssi '{id}'";
            SqlCommand cmd2 = new SqlCommand(q2, conn);
            cmd2.ExecuteNonQuery();
        }

        private void UpdateAs(int id)
        {
            string q = $"exec el_GetAssiById '{id}'";
            SqlCommand cmd = new SqlCommand(q, conn);
            SqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                txtTopic.Text = rdr["Title"].ToString();
                HiddenField2.Value = rdr["Assignment"].ToString();
                HiddenField1.Value = id.ToString();
                txtMasterCourse.Text = rdr["MasterCourse"].ToString();
                txtSubCourse.Text = rdr["SubCourse"].ToString();
            }
            rdr.Close();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#exampleModal').modal('show');", true);
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            int id = int.Parse(HiddenField1.Value);
            string newTitle = txtTopic.Text;
            string filePath = "";
            if (FileUpload1.HasFile)
            {
                string[] allowedExts = { ".pdf" };
                string[] allowedMime = { "application/pdf" };

                string ext = Path.GetExtension(FileUpload1.FileName).ToLower().Trim();
                string mime = FileUpload1.PostedFile.ContentType.ToLower();

                if (!allowedExts.Contains(ext) || !allowedMime.Contains(mime))
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "UploadFailed", "alert('Only pdf files are allowed.');", true);
                    return;
                }
                string fileName = newTitle + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ext;
                filePath = "Assignments/" + fileName;

                FileUpload1.SaveAs(Server.MapPath(filePath));
            }
            else
            {
                filePath = HiddenField2.Value;
            }
            string q2 = $"exec el_UpdateAssi '{id}','{filePath}'";
            SqlCommand cmd2 = new SqlCommand(q2, conn);
            int r = cmd2.ExecuteNonQuery();
            if (r > 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "UpdateSuccess", "alert('Assignment updated successfully');", true);
                fetchData();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "UpdateFailed", "alert('Assignment not updated');", true);
            }
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("AssignmentList.aspx");
        }

        private string GetContentType(string path)
        {
            string ext = Path.GetExtension(path).ToLower();
            switch (ext)
            {
                case ".pdf": return "application/pdf";
                default: return "application/octet-stream";
            }
        }
    }

}