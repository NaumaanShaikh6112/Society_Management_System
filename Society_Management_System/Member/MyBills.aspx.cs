using System;
using System.Data;
using System.Data.SqlClient;

namespace Society_Management_System.Member
{
    public partial class MyBills : System.Web.UI.Page
    {
        private static readonly string ConnStr = System.Configuration.ConfigurationManager
            .ConnectionStrings["SocietyDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // ✅ Check if user is logged in
                if (Session["user_id"] == null)
                {
                    Response.Redirect("~/Login.aspx");
                    return;
                }

                LoadMyBills();
                LoadMyPayments();
            }
        }

        private void LoadMyBills()
        {
            using (SqlConnection con = new SqlConnection(ConnStr))
            {
                string query = @"
                    SELECT
                        MB.bill_id,
                        S.name AS society_name,
                        U.unit_no,
                        MB.total_amount,
                        MB.due_date,
                        MB.status,
                        (
                            SELECT MAX(paid_on)
                            FROM payments
                            WHERE bill_id = MB.bill_id
                        ) AS last_payment_date
                    FROM maintenance_bills MB
                    INNER JOIN units U ON MB.unit_id = U.unit_id
                    INNER JOIN societies S ON MB.society_id = S.society_id
                    WHERE U.user_id = @user_id
                    ORDER BY MB.due_date DESC";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@user_id", Convert.ToInt32(Session["user_id"]));

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                GvMyBills.DataSource = dt;
                GvMyBills.DataBind();
            }
        }

        private void LoadMyPayments()
        {
            using (SqlConnection con = new SqlConnection(ConnStr))
            {
                string query = @"
                    SELECT
                        P.payment_id,
                        P.bill_id,
                        P.amount,
                        P.mode,
                        P.paid_on,
                        P.reference_no
                    FROM payments P
                    INNER JOIN maintenance_bills MB ON P.bill_id = MB.bill_id
                    INNER JOIN units U ON MB.unit_id = U.unit_id
                    WHERE U.user_id = @user_id
                    ORDER BY P.paid_on DESC";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@user_id", Convert.ToInt32(Session["user_id"]));

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                GvMyPayments.DataSource = dt;
                GvMyPayments.DataBind();
            }
        }
    }
}