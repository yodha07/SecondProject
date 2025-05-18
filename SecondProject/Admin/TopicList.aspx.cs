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
    public partial class TopicList : System.Web.UI.Page
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
            string q = "exec el_GetAllTopic";
            SqlDataAdapter ada = new SqlDataAdapter(q, conn);
            DataTable dt = new DataTable();
            ada.Fill(dt);
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int TpId = int.Parse(e.CommandArgument.ToString());

            if (e.CommandName == "EditTp")
            {
                UpdateTp(TpId);
            }
            else if (e.CommandName == "DeleteTp")
            {
                DeleteTp(TpId);
                fetchData(); // refresh grid
            }

        }

        private void DeleteTp(int id)
        {
            try
            {
                string q2 = $"exec el_DeleteTopic '{id}'";
                SqlCommand cmd2 = new SqlCommand(q2, conn);
                cmd2.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Response.Write("<script>alert('First delete the Assignments and MCQs associated with this Topic')</script>");
            }
        }

        private void UpdateTp(int id)
        {
            string q = $"exec el_GetTopicById '{id}'";
            SqlCommand cmd = new SqlCommand(q, conn);
            SqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                txtTitle.Text = rdr["Title"].ToString();
                HiddenField1.Value = id.ToString();
                txtEmbedCode.Text = rdr["VideoEmbedCode"].ToString();
                txtDuration.Text = rdr["Duration"].ToString();
                txtMasterCourse.Text = rdr["MasterCourse"].ToString();
                txtSubCourse.Text = rdr["SubCourse"].ToString();
            }
            rdr.Close();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#exampleModal').modal('show');", true);
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            int id = int.Parse(HiddenField1.Value);
            string newTitle = txtTitle.Text;
            string newEmbedCode = txtEmbedCode.Text;
            string newDuration = txtDuration.Text;
            string q2 = $"exec el_UpdateTopic '{id}','{newTitle}','{newEmbedCode}','{newDuration}'";
            SqlCommand cmd2 = new SqlCommand(q2, conn);
            int r = cmd2.ExecuteNonQuery();
            if (r > 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "UpdateSuccess", "alert('Topic updated successfully');", true);
                fetchData();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "UpdateFailed", "alert('Topic not updated');", true);
            }

        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("TopicList.aspx");
        }
    }
}