using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Razorpay.Api;

namespace SecondProject.User
{
    public partial class course_Membership : System.Web.UI.Page
    {
        SqlConnection conn;
        protected void Page_Load(object sender, EventArgs e)
        {
            string cs = ConfigurationManager.ConnectionStrings["dbconn"].ConnectionString;
            conn = new SqlConnection(cs);
            conn.Open();
            if (!IsPostBack)
            {
                fetchPlan();
            }
        }

        public void fetchPlan()
        {
            string q = $"exec fetchbundle2";
            SqlCommand cmd = new SqlCommand(q, conn);
            SqlDataReader rdr = cmd.ExecuteReader();
            DataList1.DataSource = rdr;
            DataList1.DataBind();

        }
        protected void DataList1_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            int PlanId = int.Parse(DataBinder.Eval(e.Item.DataItem, "PlanId").ToString());

            string q = $"exec el_GetPlanSubCourse '{PlanId}'";
            SqlCommand cmd = new SqlCommand(q, conn);
            SqlDataReader rdr = cmd.ExecuteReader();

            DataList innerList = (DataList)e.Item.FindControl("DataList2");
            innerList.DataSource = rdr;
            innerList.DataBind();

        }
        protected void DataList1_ItemCommand(object source, DataListCommandEventArgs e)
        {
            int userid = 1;
            string sourceName = "Membership";
            if (e.CommandName == "buynow")
            {
                int planid = int.Parse(e.CommandArgument.ToString());
                string q = $"SELECT * FROM MembershipPlan_Subcourse AS mps JOIN MembershipPlan AS mp ON mp.PlanId = mps.PlanId WHERE mp.PlanId ={planid}";
                SqlCommand cmd = new SqlCommand(q, conn);
                SqlDataReader rdr = cmd.ExecuteReader();
                double total = 0;
                List<int> subcourseidlist = new List<int>();
                while (rdr.Read())
                {
                    int subcourseid = int.Parse(rdr["SubCourseId"].ToString());
                    subcourseidlist.Add(subcourseid);
                    total = double.Parse(rdr["Price"].ToString());
                }
                Session["total"] = total;
                foreach (int subcourseid in subcourseidlist)
                {
                    DateTime dt = DateTime.Now.Date;

                    string monthh = dt.ToString("MMMM");   // "05"
                    string year = dt.ToString("yyyy");    // "25"

                    string q2 = $"insert into User_SubCourseAccess(UserID,SubCourseId,Source,month,year)values({userid},{subcourseid},'{sourceName}','{monthh}','{year}')";
                    SqlCommand cmd2 = new SqlCommand(q2, conn);
                    cmd2.ExecuteNonQuery();

                }

                string keyId = "rzp_test_Kl7588Yie2yJTV";
                string keySecret = "6dN9Nqs7M6HPFMlL45AhaTgp";

                RazorpayClient razorpayClient = new RazorpayClient(keyId, keySecret);

                double amount = double.Parse(Session["total"].ToString());


                // Create an order
                Dictionary<string, object> options = new Dictionary<string, object>();
                options.Add("amount", amount * 100); // Amount should be in paisa (multiply by 100 for rupees)
                options.Add("currency", "INR");
                options.Add("receipt", "order_receipt_123");
                options.Add("payment_capture", 1); // Auto capture payment

                Razorpay.Api.Order order = razorpayClient.Order.Create(options);

                string orderId = order["id"].ToString();

                // Generate checkout form and redirect user to Razorpay payment page
                string razorpayScript = $@"
            var options = {{
                'key': '{keyId}',
                'amount': {amount * 100},
                'currency': 'INR',
                'name': 'Masstech Business Solutions Pvt.Ltd',
                'description': 'Checkout Payment',
                'order_id': '{orderId}',
                'handler': function(response) {{
                    // Handle successful payment response
                    alert('Payment successful. Payment ID: ' + response.razorpay_payment_id);
                }},
                  'modal': {{
                'ondismiss': function () {{
                    // On closing Razorpay (❌ or cancel)
                    window.location.href = 'generatepdf.aspx?status=cancel';
                }}
            }},
                'prefill': {{
                    'name': 'Krish Kheloji',
                    'email': 'khelojikrish@gmail.com',
                    'contact': '7208921898'
                }},
                'theme': {{
                    'color': '#F37254'
                }}
            }};
            var rzp1 = new Razorpay(options);
            rzp1.open();";

                // Register the script on the page

                ClientScript.RegisterStartupScript(this.GetType(), "razorpayScript", razorpayScript, true);

            }



        }
        protected void DataList2_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HtmlGenericControl liItem = (HtmlGenericControl)e.Item.FindControl("liItem");
                if (liItem != null)
                {
                    string title = DataBinder.Eval(e.Item.DataItem, "Title").ToString();
                    int index = e.Item.ItemIndex + 1;
                    liItem.InnerText = $"{index}. {title}";
                }
            }
        }
        public void pdfgenrate()
        {
            int userid = 1;
            string q = $"select * from Cart join SubCourse on SubCourse.SubCourseId=Cart.SubCourseId where UserId={userid}";
            SqlCommand cmd = new SqlCommand(q, conn);
            SqlDataReader rdr = cmd.ExecuteReader();

            List<string> Title = new List<string>();
            List<double> Price = new List<double>();
            if (rdr.HasRows)
            {
                while (rdr.Read())
                {
                    string title = rdr["Title"].ToString();
                    Title.Add(title);
                    double price = double.Parse(rdr["Price"].ToString());
                    Price.Add(price);

                }
            }
            Session["Title"] = Title;
            Session["Price"] = Price;

        }

    }


}