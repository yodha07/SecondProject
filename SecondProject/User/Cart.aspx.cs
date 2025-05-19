using iTextSharp.text;
using iTextSharp.text.pdf;
using Razorpay.Api;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace SecondProject.User
{
    
        public partial class Cart : System.Web.UI.Page
        {

            SqlConnection conn;
            int cartcount = 0;

       
        protected void Page_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ELearning_Project"].ConnectionString);
            conn.Open();
            //filldatalist();
            if (!IsPostBack)

            protected void Page_Load(object sender, EventArgs e)
            {
                string cs = ConfigurationManager.ConnectionStrings["ELearning_Project"].ConnectionString;
                conn = new SqlConnection(cs);
                conn.Open();
                //filldatalist();
                if (!IsPostBack)

                {
                    filldatalist();
                    checkCart();
                    Label2.Text = cartcount.ToString() + " Item";
                    lblTotalItems.Text = cartcount.ToString();
                }
            }
            public void filldatalist()
            {
            int userid = int.Parse(Session["UserId"].ToString());
            //string q = "select * from SubCourse join Cart on SubCourse.SubCourseId=Cart.SubCourseId where UserId='" + userid + "'";
            string q = $"exec fetchcart {userid}";
                SqlCommand cmd = new SqlCommand(q, conn);
                SqlDataReader rdr = cmd.ExecuteReader();
                DataList1.DataSource = rdr;
                DataList1.DataBind();

                string q2 = "select sum(Price) as CartTotal from SubCourse join Cart on SubCourse.SubCourseId=Cart.SubCourseId where UserId='" + userid + "'";
                SqlCommand cmd2 = new SqlCommand(q2, conn);
                SqlDataReader rdr2 = cmd2.ExecuteReader();
                double total = 0;
                if (rdr.HasRows)
                {
                    while (rdr2.Read())
                    {
                        total = double.Parse(rdr2["CartTotal"].ToString());
                    }
                }
                lblTotalPrice.Text = $"{total}";
                Session["total"] = total;
            }
            public string GetStarsHtml(object ratingObj)
            {
                double rating = double.Parse((ratingObj).ToString());
                string starsHtml = "";

                for (int i = 1; i <= 5; i++)
                {
                    if (i <= Math.Floor(rating))
                    {
                        starsHtml += "<i class='fa fa-star text-warning'></i>"; // full
                    }
                    else if (i - rating <= 0.5)
                    {
                        starsHtml += "<i class='fa fa-star-half-o text-warning'></i>"; // half
                    }
                    else
                    {
                        starsHtml += "<i class='fa fa-star-o text-warning'></i>"; // empty
                    }
                }

                starsHtml += $" <span class='ml-1'>({rating:F1})</span>"; // show 1 decimal point
                return starsHtml;
            }
            protected void DataList1_ItemCommand(object source, DataListCommandEventArgs e)
            {
            int userid = int.Parse(Session["UserId"].ToString());
            if (e.CommandName == "dltcart")
                {
                    int cartid = int.Parse(e.CommandArgument.ToString());

                    string q = "delete Cart where CartId='" + cartid + "'";
                    SqlCommand cmd = new SqlCommand(q, conn);
                    cmd.ExecuteNonQuery();
                    filldatalist();
                    checkCart();
                    Label2.Text = cartcount.ToString() + " Item";
                    lblTotalItems.Text = cartcount.ToString();






                }
            }
            public void checkCart()
            {
            int userid = int.Parse(Session["UserId"].ToString());
            string q = $"select * from Cart where UserId={userid}";
                SqlCommand cmd = new SqlCommand(q, conn);
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        cartcount++;
                    }
                }
                else
                {
                    Response.Redirect("emptyCart.aspx");
                    //Response.Redirect("WebForm1.aspx");
                }
            }
            protected void btnPayNow_Click(object sender, EventArgs e)
            {
            int userid = int.Parse(Session["UserId"].ToString());


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


                updateSubCourseAccess();
                pdfgenrate();
                deleteCart();

                //string q = $"delete from Cart where UserID={userid}";
                //SqlCommand cmd= new SqlCommand(q, conn);
                //cmd.ExecuteNonQuery();
            }
            public void deleteCart()
            {
            int userid = int.Parse(Session["UserId"].ToString());
            string q = $"delete from Cart where UserID={userid}";
                SqlCommand cmd = new SqlCommand(q, conn);
                cmd.ExecuteNonQuery();
            }
            public void updateSubCourseAccess()
            {
            int userid = int.Parse(Session["UserId"].ToString());
            string q = $"select * from Cart where UserId={userid}";
                SqlCommand cmd = new SqlCommand(q, conn);
                SqlDataReader rdr = cmd.ExecuteReader();
                List<int> subcourseids = new List<int>();
                while (rdr.Read())
                {
                    int subcourseid = int.Parse(rdr["SubCourseId"].ToString());
                    subcourseids.Add(subcourseid);

                }
                foreach (int subcourseid in subcourseids)
                {
                    DateTime dt = DateTime.Now.Date;

                    string monthh = dt.ToString("MMMM");   // "05"
                    string year = dt.ToString("yyyy");    // "25"

                    string q2 = $"insert into User_SubCourseAccess(UserID,SubCourseId,month,year)values({userid},{subcourseid},'{monthh}','{year}')";
                    SqlCommand cmd2 = new SqlCommand(q2, conn);
                    cmd2.ExecuteNonQuery();
                }
            }
            public void pdfgenrate()
            {
            int userid = int.Parse(Session["UserId"].ToString());
            //string q = $"select * from Cart join SubCourse on SubCourse.SubCourseId=Cart.SubCourseId where UserId={userid}";
            string q = $"exec Billgenerate {userid}";
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