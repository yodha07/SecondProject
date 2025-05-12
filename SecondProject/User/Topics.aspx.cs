using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SecondProject.User
{
    public partial class Topics : System.Web.UI.Page
    {
        [Serializable]
        public class TopicItem
        {
            public int TopicId { get; set; }
            public string Title { get; set; }
            public string VideoEmbedCode { get; set; }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            LoadTopics();
            BindTopics();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Email"] == null || Session["UserId"] == null)
            {
                Response.Redirect("~/Login/Login.aspx");
            }

            if (!IsPostBack)
            {
                BindPlaylist();
                UpdateNavigationButtons();
            }
        }

        private List<TopicItem> GetTopics(int subCourseId)
        {
            List<TopicItem> list = new List<TopicItem>();
            string connStr = ConfigurationManager.ConnectionStrings["ELearning_Project"].ConnectionString;

            SqlConnection conn = new SqlConnection(connStr);
            {
                string query = "SELECT TopicId, Title, VideoEmbedCode FROM Topic WHERE SubCourseId = @SubCourseId ORDER BY TopicId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SubCourseId", subCourseId);
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    list.Add(new TopicItem
                    {
                        TopicId = Convert.ToInt32(rdr["TopicId"]),
                        Title = rdr["Title"].ToString(),
                        VideoEmbedCode = rdr["VideoEmbedCode"].ToString()
                    });
                }
            }
            return list;
        }

        private void LoadTopics()
        {
            int subCourseId = Convert.ToInt32(Request.QueryString["scid"]);

            if (ViewState["Topics"] == null)
            {
                var topics = GetTopics(subCourseId);
                ViewState["Topics"] = topics;
            }
        }

        private void BindTopics()
        {
            var topics = (List<TopicItem>)ViewState["Topics"];
            int currentIndex = mvTopics.ActiveViewIndex; 

            mvTopics.Views.Clear();
            foreach (var topic in topics)
            {
                View view = new View();
                view.Controls.Add(new LiteralControl(topic.VideoEmbedCode));
                mvTopics.Views.Add(view);
            }

            
            mvTopics.ActiveViewIndex = (currentIndex >= 0 && currentIndex < mvTopics.Views.Count)
                ? currentIndex
                : 0;
        }

        private void BindPlaylist()
        {
            rptPlaylist.DataSource = (List<TopicItem>)ViewState["Topics"];
            rptPlaylist.DataBind();
        }

        private void UpdateNavigationButtons()
        {
            btnPrev.Enabled = mvTopics.ActiveViewIndex > 0;
            btnNext.Enabled = mvTopics.ActiveViewIndex < mvTopics.Views.Count - 1;
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            if (mvTopics.ActiveViewIndex < mvTopics.Views.Count - 1)
                mvTopics.ActiveViewIndex++;
            BindPlaylist(); 
            UpdateNavigationButtons();
        }

        protected void btnPrev_Click(object sender, EventArgs e)
        {
            if (mvTopics.ActiveViewIndex > 0)
                mvTopics.ActiveViewIndex--;
            BindPlaylist();
            UpdateNavigationButtons();
        }

        protected void PlayVideo_Command(object sender, CommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            if (index >= 0 && index < mvTopics.Views.Count)
            {
                mvTopics.ActiveViewIndex = index;
            }
            BindPlaylist();
            UpdateNavigationButtons();
        }
    }
}
