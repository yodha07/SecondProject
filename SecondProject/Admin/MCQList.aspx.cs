using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SecondProject.Admin
{
    public partial class MCQList : System.Web.UI.Page
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
            string q = "exec el_GetAllMCQ";
            SqlDataAdapter ada = new SqlDataAdapter(q, conn);
            DataTable dt = new DataTable();
            ada.Fill(dt);
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int McqId = int.Parse(e.CommandArgument.ToString());

            if (e.CommandName == "EditMcq")
            {
                UpdateMcq(McqId);
            }
            else if (e.CommandName == "DeleteMcq")
            {
                DeleteMcq(McqId);
                fetchData(); // refresh grid
            }

        }

        private void DeleteMcq(int id)
        {
            string q2 = $"exec el_DeleteMCQ '{id}'";
            SqlCommand cmd2 = new SqlCommand(q2, conn);
            cmd2.ExecuteNonQuery();
        }

        private void UpdateMcq(int id)
        {
            string q = $"exec el_GetMCQById '{id}'";
            SqlCommand cmd = new SqlCommand(q, conn);
            SqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                HiddenField1.Value = id.ToString();
                txtQuestion.Text = rdr["Question"].ToString();
                txtOptionA.Text = rdr["OptionA"].ToString();
                txtOptionB.Text = rdr["OptionB"].ToString();
                txtOptionC.Text = rdr["OptionC"].ToString();
                txtOptionD.Text = rdr["OptionD"].ToString();
                txtAns.Text = rdr["Answer"].ToString();
                txtMasterCourse.Text = rdr["MasterCourse"].ToString();
                txtSubCourse.Text = rdr["SubCourse"].ToString();
                txtTopic.Text = rdr["Topic"].ToString();
            }
            rdr.Close();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "$('#exampleModal').modal('show');", true);
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            int id = int.Parse(HiddenField1.Value);
            string newQuestion = txtQuestion.Text;
            string newOptA = txtOptionA.Text;
            string newOptB = txtOptionB.Text;
            string newOptC = txtOptionC.Text;
            string newOptD = txtOptionD.Text;
            string newAns = txtAns.Text;
            string q2 = $"exec el_UpdateMCQ '{id}','{newQuestion}','{newOptA}','{newOptB}','{newOptC}','{newOptD}','{newAns}'";
            SqlCommand cmd2 = new SqlCommand(q2, conn);
            int r = cmd2.ExecuteNonQuery();
            if (r > 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "UpdateSuccess", "alert('MCQ updated successfully');", true);
                fetchData();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "UpdateFailed", "alert('MCQ not updated');", true);
            }

        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("MCQList.aspx");
        }
    }
}