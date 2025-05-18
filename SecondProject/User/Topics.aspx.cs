using iTextSharp.text.pdf;
using iTextSharp.text;
using iTextSharp.tool.xml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
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
            public int SubCourseId { get; set; }
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

                var topics = ViewState["Topics"] as List<TopicItem>;
                if (topics != null && topics.Count > 0)
                {
                    int subCourseId = topics[0].SubCourseId;

                    btnCertificate.CommandArgument = subCourseId.ToString();
                    btnAssignments.CommandArgument = subCourseId.ToString();
                    btnMCQ.CommandArgument = subCourseId.ToString();
                    //Button1.CommandArgument = subCourseId.ToString(); // For second copy
                    //Button2.CommandArgument = subCourseId.ToString();
                    MarkVideoWatchedIfNeeded();
                }
            }
        }


        protected void Assignments_Click(object sender, CommandEventArgs e)
        {
            int subCourseId = Convert.ToInt32(e.CommandArgument);
            Response.Redirect($"Assignments.aspx?scid={subCourseId}");
        }

        protected void Mcq_Click(object sender, CommandEventArgs e)
        {
            if (int.TryParse(e.CommandArgument?.ToString(), out int subCourseId))
            {
                Response.Redirect($"MCQs.aspx?scid={subCourseId}");
            }
            else
            { 
                Response.Write("Invalid or missing SubCourseId");
            }
        }


        private List<TopicItem> GetTopics(int subCourseId)
        {
            string connStr = ConfigurationManager.ConnectionStrings["ELearning_Project"].ConnectionString;

            SqlConnection conn = new SqlConnection(connStr);
            List<TopicItem> list = new List<TopicItem>();
            {
                string query = "SELECT TopicId, Title, VideoEmbedCode, SubCourseId FROM Topic WHERE SubCourseId = @SubCourseId ORDER BY TopicId";
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
                        VideoEmbedCode = rdr["VideoEmbedCode"].ToString(),
                        SubCourseId = Convert.ToInt32(rdr["SubCourseId"])
                    });
                }
            }
            return list;
        }

        private void LoadTopics()
        {
            if (ViewState["Topics"] == null && Request.QueryString["scid"] != null)
            {
                int subCourseId = Convert.ToInt32(Request.QueryString["scid"]);
                var topics = GetTopics(subCourseId);
                ViewState["Topics"] = topics;
            }
        }

        private int GetLoggedInUserId()
        {
            string connStr = ConfigurationManager.ConnectionStrings["ELearning_Project"].ConnectionString;

            SqlConnection conn = new SqlConnection(connStr);
            int userID = -1;
            string email = Session["Email"]?.ToString();
            if (string.IsNullOrEmpty(email))
            {
                Response.Redirect("~/Login/Login.aspx");
            }
            else
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT UserId FROM [User] WHERE Email = @Email", conn);
                cmd.Parameters.AddWithValue("@Email", email);

                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    userID = Convert.ToInt32(rdr["UserId"]);
                    Session["UserId"] = userID;
                    Response.Write("UserId: " + userID);
                }
                conn.Close();
            }
            return userID;
        }

        protected void Certificate_Click(object sender, CommandEventArgs e)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ELearning_Project"].ConnectionString);
            LinkButton btn = (LinkButton)sender;
            int subCourseId = Convert.ToInt32(e.CommandArgument);
            int userId = GetLoggedInUserId();
            string userName = "";
            string courseTitle = "";

            conn.Open();

            SqlCommand cmdUser = new SqlCommand("SELECT Fullname FROM [User] WHERE UserId = @UserId", conn);
            {
                cmdUser.Parameters.AddWithValue("@UserId", userId);
                object userResult = cmdUser.ExecuteScalar();
                if (userResult != null)
                    userName = userResult.ToString();
            }

            SqlCommand cmdCourse = new SqlCommand(@"SELECT sc.Title AS Title
    FROM SubCourse sc
    JOIN MasterCourse mc ON sc.MasterCourseId = mc.MasterCourseId
    WHERE sc.SubCourseId = @SubCourseId", conn);
            {
                cmdCourse.Parameters.AddWithValue("@SubCourseId", subCourseId);
                object titleResult = cmdCourse.ExecuteScalar();
                if (titleResult != null)
                    courseTitle = titleResult.ToString();
            }

            conn.Close();


            string fileName = $"Certificate_{userName}_{DateTime.Now:dd-MM-yyyy_HH-mm-ss}.pdf";
            string filePath = "Certificates/" + fileName;
            string fullPath = Server.MapPath("~/" + filePath);

            string certificateDir = Server.MapPath("~/Certificates");
            if (!Directory.Exists(certificateDir))
            {
                Directory.CreateDirectory(certificateDir);
            }

            string logoPath = Server.MapPath("~/Images/Logo.png");
            string base64Logo = "";

            if (File.Exists(logoPath))
            {
                byte[] imageBytes = File.ReadAllBytes(logoPath);
                base64Logo = Convert.ToBase64String(imageBytes);
            }

            string htmlContent = $@"
            <html>
                <head>
                    <style>
                        body {{
                            font-family: Arial, sans-serif;
                            text-align: center;
                        }}
                        h1 {{
                            color: #4CAF50;
                        }}
                        p {{
                            font-size: 20px;
                        }}
                    </style>
                </head>
                <body>
                    <h1>Certificate of Completion</h1>
                    <p>This is to certify that</p>
                    <h2>{userName}</h2>
                    <p>has successfully completed the course</p>
                    <h2>{courseTitle}</h2>
                    <p>on {DateTime.Now:dd/MM/yyyy}</p>
                </body>
            </html>";




            byte[] pdfBytes = GeneratePdfFromHtml(htmlContent);

            File.WriteAllBytes(fullPath, pdfBytes);

            string IssuedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            conn.Open();  // REOPEN after reading
            SqlCommand insertCmd = new SqlCommand(@"
    INSERT INTO Certificate (UserId, SubCourseId, IssuedDate, CertificatePath) 
    VALUES (@UserId, @SubCourseId, @IssuedDate, @CertificatePath)", conn);
            insertCmd.Parameters.AddWithValue("@UserId", userId);
            insertCmd.Parameters.AddWithValue("@SubCourseId", subCourseId);
            insertCmd.Parameters.AddWithValue("@IssuedDate", IssuedDate);
            insertCmd.Parameters.AddWithValue("@CertificatePath", "/" + filePath);
            insertCmd.ExecuteNonQuery();
            conn.Close();


            if (System.IO.File.Exists(fullPath))
            {
                Response.ContentType = "application/pdf";
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(fullPath));
                Response.TransmitFile(fullPath);
                Response.End();
            }
            else
            {
                Response.Write("Not found");
            }
        }

        private byte[] GeneratePdfFromHtml(string htmlContent)
        {
            string logoPath = Server.MapPath("~/Images/Logo.png");

            using (MemoryStream memoryStream = new MemoryStream())
            {
                Document document = new Document(PageSize.A4, 10f, 10f, 10f, 10f);
                PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                document.Open();


                if (File.Exists(logoPath))
                {
                    iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(logoPath);
                    logo.ScaleToFit(150f, 150f);
                    logo.Alignment = Element.ALIGN_CENTER;
                    document.Add(logo);
                }

                using (StringReader stringReader = new StringReader(htmlContent))
                {
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, stringReader);
                }

                document.Close();
                return memoryStream.ToArray();
            }
        }

        private void BindTopics()
        {
            var topics = ViewState["Topics"] as List<TopicItem>;
            if (topics == null) return;

            mvTopics.Views.Clear();
            foreach (var topic in topics)
            {
                View view = new View();
                view.Controls.Add(new LiteralControl(topic.VideoEmbedCode));
                mvTopics.Views.Add(view);
            }

            mvTopics.ActiveViewIndex = (ActiveViewIndex >= 0 && ActiveViewIndex < mvTopics.Views.Count)
                ? ActiveViewIndex
                : 0;
        }


        private void BindPlaylist()
        {
            rptPlaylist.DataSource = ViewState["Topics"] as List<TopicItem>;
            rptPlaylist.DataBind();
        }

        private void UpdateNavigationButtons()
        {
            var topics = ViewState["Topics"] as List<TopicItem>;
            if (topics != null && topics.Count > 0)
            {
                int currentIndex = mvTopics.ActiveViewIndex;

                btnPrev.Enabled = currentIndex > 0;
                btnNext.Enabled = currentIndex < topics.Count - 1;

               
                if (currentIndex == topics.Count - 1)
                {
                    btnCertificate.Enabled = true;
                    btnCertificate.CommandArgument = topics[0].SubCourseId.ToString(); 
                }
                else
                {
                    btnCertificate.Enabled = false;
                }
            }
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            if (ActiveViewIndex < mvTopics.Views.Count - 1)
                ActiveViewIndex++;

            BindTopics();
            BindPlaylist();
            UpdateNavigationButtons();
        }

        private void UpdateCertificateButton()
        {
            btnCertificate.Enabled = (mvTopics.ActiveViewIndex == mvTopics.Views.Count - 1);
        }
        protected void btnPrev_Click(object sender, EventArgs e)
        {
            if (ActiveViewIndex > 0)
                ActiveViewIndex--;

            BindTopics();
            BindPlaylist();
            UpdateNavigationButtons();
        }

        private void MarkVideoWatchedIfNeeded()
        {
            int userId = GetLoggedInUserId();
            int subCourseId = Convert.ToInt32(Request.QueryString["scid"]);
            string connStr = ConfigurationManager.ConnectionStrings["ELearning_Project"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand($"UPDATE User_SubCourseProgress SET VideosWatched = 1 WHERE UserId = '{userId}' AND SubCourseId = '{subCourseId}' AND VideosWatched <> 1", conn);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }


        protected void PlayVideo_Command(object sender, CommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            if (index >= 0 && index < mvTopics.Views.Count)
            {
                ActiveViewIndex = index;
            }

            BindTopics();
            BindPlaylist();
            UpdateNavigationButtons();
            MarkVideoWatchedIfNeeded();
        }


        private int ActiveViewIndex
        {
            get => ViewState["ActiveViewIndex"] != null ? (int)ViewState["ActiveViewIndex"] : 0;
            set => ViewState["ActiveViewIndex"] = value;
        }


    }
}
