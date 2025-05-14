using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static SecondProject.User.MCQs;
using static SecondProject.User.MyCourses;

namespace SecondProject.User
{
    public partial class MCQs : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["ELearning_Project"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int subCourseID = Convert.ToInt32(Request.QueryString["scid"]);
                List<McqItem> mcqs = GetMcqsBySubCourse(subCourseID);
                rptMcqs.DataSource = mcqs;
                rptMcqs.DataBind();
                TitleLabel.Text = GetTitle(subCourseID);
                ViewState["McqsList"] = mcqs;
            }
        }


        private string GetTitle(int subCourseId)
        {
            string title = "";
            string query = "SELECT Title FROM Topic WHERE SubCourseId = @SubCourseId";

            SqlConnection conn = new SqlConnection(connStr);

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@SubCourseId", subCourseId);
            conn.Open();

            object result = cmd.ExecuteScalar();
            if (result != null)
            {
                title = result.ToString();
            }
            return title;
        }


        private List<McqItem> GetMcqsBySubCourse(int subCourseId)
        {
            List<McqItem> list = new List<McqItem>();

            SqlConnection conn = new SqlConnection(connStr);
            {
                string query = @"
            SELECT M.Question, M.OptionA, M.OptionB, M.OptionC, M.OptionD, M.Answer
            FROM MCQ M
            INNER JOIN Topic T ON M.TopicId = T.TopicId
            WHERE T.SubCourseId = @SubCourseId";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SubCourseId", subCourseId);

                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    list.Add(new McqItem
                    {
                        Question = rdr["Question"].ToString(),
                        OptionA = rdr["OptionA"].ToString(),
                        OptionB = rdr["OptionB"].ToString(),
                        OptionC = rdr["OptionC"].ToString(),
                        OptionD = rdr["OptionD"].ToString(),
                        CorrectAnswer = rdr["Answer"].ToString()
                    });
                }
            }

            return list;
        }

        protected void btnSubmitMCQs_Click(object sender, EventArgs e)
        {
            int correctAnswers = 0;
            int attempted = 0;

            List<McqItem> mcqs = ViewState["McqsList"] as List<McqItem>;
            if (mcqs == null)
            {
                int SubCourseId = Convert.ToInt32(Request.QueryString["scid"]);
                mcqs = GetMcqsBySubCourse(SubCourseId);
            }

            for (int i = 0; i < rptMcqs.Items.Count; i++)
            {
                RepeaterItem item = rptMcqs.Items[i];
                RadioButton optA = (RadioButton)item.FindControl("optA");
                RadioButton optB = (RadioButton)item.FindControl("optB");
                RadioButton optC = (RadioButton)item.FindControl("optC");
                RadioButton optD = (RadioButton)item.FindControl("optD");

                string selected = null;
                if (optA.Checked) selected = "A";
                else if (optB.Checked) selected = "B";
                else if (optC.Checked) selected = "C";
                else if (optD.Checked) selected = "D";

                if (selected != null)
                {
                    attempted++;
                    if (selected?.Trim().ToLower() == mcqs[i].CorrectAnswer?.Trim().ToLower())
                    {
                        correctAnswers++;
                    }
                }
                optA.Enabled = false;
                optB.Enabled = false;
                optC.Enabled = false;
                optD.Enabled = false;
            }

            int userId = Convert.ToInt32(Session["UserId"]);
            int subCourseId = Convert.ToInt32(Request.QueryString["scid"]);

            SqlConnection conn = new SqlConnection(connStr);
            {
                string checkQuery = @"SELECT COUNT(*) FROM User_SubCourseProgress 
                              WHERE UserId = @UserId AND SubCourseId = @SubCourseId";

                SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("@UserId", userId);
                checkCmd.Parameters.AddWithValue("@SubCourseId", subCourseId);

                conn.Open();
                int exists = (int)checkCmd.ExecuteScalar();
                conn.Close();

                string query = exists > 0
                    ? @"UPDATE User_SubCourseProgress 
               SET McqsAttempted = @McqsAttempted 
               WHERE UserId = @UserId AND SubCourseId = @SubCourseId"
                    : @"INSERT INTO User_SubCourseProgress (McqsAttempted, UserId, SubCourseId)
               VALUES (@McqsAttempted, @UserId, @SubCourseId)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@McqsAttempted", attempted);
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@SubCourseId", subCourseId);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            string msg = $"MCQs Attempted: {attempted}\\nCorrect Answers: {correctAnswers}";
            ScriptManager.RegisterStartupScript(this, GetType(), "showalert", $"alert('{msg}');", true);
        }


        [Serializable]
        public class McqItem
        {
            public string Question { get; set; }
            public string OptionA { get; set; }
            public string OptionB { get; set; }
            public string OptionC { get; set; }
            public string OptionD { get; set; }
            public string CorrectAnswer { get; set; }
        }
    }
}