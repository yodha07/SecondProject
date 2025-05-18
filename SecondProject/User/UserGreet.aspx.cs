using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

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
                //lblGreeting.Text = "Hello, " + " (" + email + ")";
            }
            
       
            string cn = ConfigurationManager.ConnectionStrings["ELearning_Project"].ConnectionString;
            conn = new SqlConnection(cn);
            conn.Open();
            UserName();
            LoadChatMessages();

        }

        protected void rptChatMessagess_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

        }

        protected void LoadChatMessages()
        {
            string query2 = $"select FullName from [User] where Userid = {Session["UserId"].ToString()}";
            SqlCommand cmd2 = new SqlCommand(query2, conn);
            SqlDataReader rdr2 = cmd2.ExecuteReader();
            rdr2.Read();

            string user = rdr2["FullName"].ToString();
            string query = $"SELECT Send,Reply,SendTime, ReplyTime FROM Chats Where FullName='{user}'";

            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            rptChatMessagess.DataSource = dt;
            rptChatMessagess.DataBind();

        }
        protected void UserName() 
        {
            string query2 = $"select FullName from [User] where Userid = {Session["UserId"].ToString()}";
            SqlCommand cmd2 = new SqlCommand(query2, conn);
            SqlDataReader rdr2 = cmd2.ExecuteReader();
            rdr2.Read();

            Label1.Text = rdr2["FullName"].ToString();
        }



        protected void Button1_Click(object sender, EventArgs e)
        {
            string query2 = $"select FullName from [User] where Userid = {Session["UserId"].ToString()}";
            SqlCommand cmd2 = new SqlCommand(query2, conn);
            SqlDataReader rdr2 = cmd2.ExecuteReader();
            rdr2.Read();
              
            string name = rdr2["FullName"].ToString();
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
