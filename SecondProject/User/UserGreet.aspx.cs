using System;

namespace SecondProject.User
{
    public partial class UserGreet : System.Web.UI.Page
    {
        SqlConnection conn;
        protected void Page_Load(object sender, EventArgs e)
        {
            // Change 'UserEmail' to 'Email' to match the key used in the login page
            string email = Session["Email"]?.ToString();

            if (string.IsNullOrEmpty(email))
            {
                Response.Redirect("~/Login/Login.aspx");
            }
            else
            {
                lblGreeting.Text = "Hello, " + " (" + email + ")";
            }
            
       
            string cn = ConfigurationManager.ConnectionStrings["aprilbatchConnectionString"].ConnectionString;
            conn = new SqlConnection(cn);
            conn.Open();
            Session["us"] = TextBox2.Text;
            LoadChatMessages();

        }

        protected void rptChatMessagess_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

        }

        protected void LoadChatMessages()
        {
            string user = Session["us"].ToString();
            string query = $"SELECT Send,Reply,SendTime, ReplyTime FROM Chats Where UserName='{user}'";

            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            rptChatMessagess.DataSource = dt;
            rptChatMessagess.DataBind();

        }



        protected void Button1_Click(object sender, EventArgs e)
        {
            string name = Session["us"].ToString();
            string send = TextBox1.Text;
            DateTime now = DateTime.Now;

            string query = $"exec InsertChatMessage '{name}','{send}','{now:yyyy-MM-dd HH:mm:ss}'";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.ExecuteNonQuery();


            TextBox1.Text = "";
            LoadChatMessages();

        }


    }
}
