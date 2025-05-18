using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace SecondProject.Admin
{
    public partial class AddMaterial : System.Web.UI.Page
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
            if (IsPostBack && !string.IsNullOrEmpty(TextBox1.Text))
            {
                TextBox1_TextChanged(null, null);
            }
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            string topicName = DropDownList3.SelectedItem.Text;
            int id = int.Parse(DropDownList3.SelectedValue);

            if (FileUpload1.HasFile)
            {
                string[] allowedExts = { ".pdf" };
                string[] allowedMime = { "application/pdf" };

                string ext = Path.GetExtension(FileUpload1.FileName).ToLower().Trim();
                string mime = FileUpload1.PostedFile.ContentType.ToLower();

                if (!allowedExts.Contains(ext) || !allowedMime.Contains(mime))
                {
                    Response.Write("<script>alert('Only PDF files are allowed.')</script>");
                    return;
                }

                string fileName = topicName + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ext;
                string filePath = "Assignments/" + fileName;
                FileUpload1.SaveAs(Server.MapPath(filePath));
                string query = $"exec el_AddAssignment '{id}','{filePath}'";
                SqlCommand cmd = new SqlCommand(query, conn);
                int r = cmd.ExecuteNonQuery();
            }

            int count = int.Parse(TextBox1.Text);
            string question = "", optionA = "", optionB = "", optionC = "", optionD = "", ans = "";

            for (int q = 1; q <= count; q++)
            {
                TextBox tbQuestion = (TextBox)PlaceHolder1.FindControl("Question_" + q);
                TextBox tbA = (TextBox)PlaceHolder1.FindControl($"Q{q}_OptionA");
                TextBox tbB = (TextBox)PlaceHolder1.FindControl($"Q{q}_OptionB");
                TextBox tbC = (TextBox)PlaceHolder1.FindControl($"Q{q}_OptionC");
                TextBox tbD = (TextBox)PlaceHolder1.FindControl($"Q{q}_OptionD");
                TextBox tbAnswer = (TextBox)PlaceHolder1.FindControl("Answer_" + q);

                if (tbQuestion != null && tbA != null && tbB != null && tbC != null && tbD != null && tbAnswer != null)
                {
                    question = tbQuestion.Text;
                    optionA = tbA.Text;
                    optionB = tbB.Text;
                    optionC = tbC.Text;
                    optionD = tbD.Text;
                    ans = tbAnswer.Text;
                    string query2 = $"exec el_AddMCQ '{id}','{question}','{optionA}','{optionB}','{optionC}','{optionD}','{ans}'";
                    SqlCommand cmd2 = new SqlCommand(query2, conn);
                    int r2 = cmd2.ExecuteNonQuery();
                    optionA = optionB = optionC = optionD = ans = "";
                }
            }
            PlaceHolder1.Controls.Clear();
            Response.Write("<script>alert('Assignments and MCQ added successfully.')</script>");
        }

        public void loadMcName()
        {
            string query = $"exec el_GetMasterCourse";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader rdr = cmd.ExecuteReader();
            DropDownList1.Items.Add(new ListItem("--Select Master Course--", "0"));
            while (rdr.Read())
            {
                DropDownList1.Items.Add(new ListItem(rdr["Title"].ToString(), rdr["MasterCourseId"].ToString()));
            }
            rdr.Close();
            DropDownList2.Items.Add(new ListItem("--Select Master Course First--", "0"));
            DropDownList3.Items.Add(new ListItem("--Select Master/Sub Course First--", "0"));
        }
        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList2.Items.Clear();
            DropDownList3.Items.Clear();
            if (int.Parse(DropDownList1.SelectedValue) == 0)
            {
                DropDownList2.Items.Add(new ListItem("--Select Master Course First--", "0"));
                DropDownList3.Items.Add(new ListItem("--Select Master/Sub Course First--", "0"));
            }
            else
            {
                DropDownList2.Items.Add(new ListItem("--Select Sub Course--", "0"));
                DropDownList3.Items.Add(new ListItem("--Select Master/Sub Course First--", "0"));
                int mcid = int.Parse(DropDownList1.SelectedValue);
                string query2 = $"exec el_GetSubCourses '{mcid}'";
                SqlCommand cmd2 = new SqlCommand(query2, conn);
                SqlDataReader rdr2 = cmd2.ExecuteReader();
                while (rdr2.Read())
                {
                    DropDownList2.Items.Add(new ListItem(rdr2["Title"].ToString(), rdr2["SubCourseId"].ToString()));
                }
                rdr2.Close();
            }

        }

        protected void DropDownList2_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList3.Items.Clear();
            if (int.Parse(DropDownList1.SelectedValue) == 0 || int.Parse(DropDownList2.SelectedValue) == 0)
            {
                DropDownList3.Items.Add(new ListItem("--Select Master/Sub Course First--", "0"));
            }
            else
            {
                DropDownList3.Items.Add(new ListItem("--Select Topic--", "0"));
                int scid = int.Parse(DropDownList2.SelectedValue);
                string query3 = $"exec el_GetTopic '{scid}'";
                SqlCommand cmd3 = new SqlCommand(query3, conn);
                SqlDataReader rdr3 = cmd3.ExecuteReader();
                while (rdr3.Read())
                {
                    DropDownList3.Items.Add(new ListItem(rdr3["Title"].ToString(), rdr3["TopicId"].ToString()));
                }
                rdr3.Close();
            }

        }

        protected void TextBox1_TextChanged(object sender, EventArgs e)
        {
            PlaceHolder1.Controls.Clear();

            int count = int.Parse(TextBox1.Text);
            int totalControls = count * 6;

            int q = 1;
            char opt = 'A';

            for (int i = 1; i <= totalControls; i++)
            {
                if ((i % 6) == 1)
                {
                    Panel questionPanel = new Panel();
                    questionPanel.ID = "Panel_Q" + q;
                    questionPanel.CssClass = "mb-3 p-3 border rounded";
                    PlaceHolder1.Controls.Add(questionPanel);

                    TextBox txt = new TextBox();
                    txt.ID = "Question_" + q;
                    txt.CssClass = "form-control mb-2";
                    txt.Attributes["placeholder"] = "Question No. " + q;
                    questionPanel.Controls.Add(txt);

                    opt = 'A';
                }
                else if ((i % 6) != 0)
                {
                    Panel currentPanel = (Panel)PlaceHolder1.FindControl("Panel_Q" + q);
                    TextBox txt = new TextBox();
                    txt.ID = $"Q{q}_Option{opt}";
                    txt.CssClass = "form-control mb-2";
                    txt.Attributes["placeholder"] = "Option " + opt;
                    currentPanel.Controls.Add(txt);
                    opt++;
                }
                else
                {
                    Panel currentPanel = (Panel)PlaceHolder1.FindControl("Panel_Q" + q);
                    TextBox txt = new TextBox();
                    txt.ID = "Answer_" + q;
                    txt.CssClass = "form-control mb-2";
                    txt.Attributes["placeholder"] = "Answer for Qno. " + q;
                    currentPanel.Controls.Add(txt);
                    q++;
                }
            }
        }

    }

}