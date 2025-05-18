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
    public partial class adminchat : System.Web.UI.Page
    {
        SqlConnection conn;
        protected void Page_Load(object sender, EventArgs e)
        {


            string cn = ConfigurationManager.ConnectionStrings["ELearning_Project"].ConnectionString;
            conn = new SqlConnection(cn);
            conn.Open();

        }

        protected void rptUsers_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "SelectUser")
            {
                string selectedUser = e.CommandArgument.ToString();
                lblChatHeader.Text = selectedUser;
                Session["us"] = lblChatHeader.Text;
                LoadChatMessages();
            }
        }

        protected void LoadChatMessages()
        {
            //  string user= Session["us"].ToString();
            string query = $"SELECT Send,Reply,SendTime, ReplyTime FROM Chats Where FullName='{Session["us"].ToString()}'";

            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);


            rptChatMessagess.DataSource = dt;
            rptChatMessagess.DataBind();

        }

        //protected void Loadsend()
        //{
        //    string user= Session["us"].ToString();
        //    string query = $"SELECT Reply,ReplyTime FROM Chats Where UserName='{user}'";

        //    SqlCommand cmd = new SqlCommand(query, conn);
        //    SqlDataAdapter da = new SqlDataAdapter(cmd);
        //    DataTable dt = new DataTable();
        //    da.Fill(dt);


        //    Repeater1.DataSource = dt;
        //    Repeater1.DataBind();

        //}



        protected void Button1_Click(object sender, EventArgs e)
        {
            string name = Session["us"].ToString();
            string send = TextBox1.Text;
            DateTime now = DateTime.Now;

            string query = $"exec InsertChatadmin '{name}','{send}','{now:yyyy-MM-dd HH:mm:ss}'";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.ExecuteNonQuery();


            TextBox1.Text = "";
            LoadChatMessages();
            //Loadsend();
        }
    }
}