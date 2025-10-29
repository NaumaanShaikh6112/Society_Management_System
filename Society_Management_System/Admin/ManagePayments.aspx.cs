using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

namespace Society_Management_System.Admin
{
    public partial class ManagePayments : Page
    {
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["societyDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadPayments();
            }
        }

        // ✅ Load Payments from database into GridView
        private void LoadPayments()
        {
            string query = "SELECT payment_id, bill_id, paid_on, amount, mode, reference_no FROM payments";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                gvPayments.DataSource = reader;
                gvPayments.DataBind();
            }
        }

        // ✅ Add Payment - Redirect to AddPayment.aspx
        protected void btnAddPayment_Click(object sender, EventArgs e)
        {
            Response.Redirect("AddPayment.aspx");
        }

        // ✅ Handle Edit and Delete Commands
        protected void gvPayments_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int paymentId = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditPayment")
            {
                Response.Redirect($"EditPayment.aspx?payment_id={paymentId}");
            }
            else if (e.CommandName == "DeletePayment")
            {
                DeletePayment(paymentId);
            }
        }

        // ✅ Delete Payment from database
        private void DeletePayment(int paymentId)
        {
            string query = "DELETE FROM payments WHERE payment_id = @payment_id";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@payment_id", paymentId);
                conn.Open();
                cmd.ExecuteNonQuery();
            }

            LoadPayments();
        }

        // ✅ Optional: Insert Payment (if needed for AddPayment.aspx)
        protected void InsertPayment(int billId, DateTime paidOn, decimal amount, string mode)
        {
            string query = @"
                INSERT INTO payments (bill_id, paid_on, amount, mode, reference_no)
                VALUES (@bill_id, @paid_on, @amount, @mode, @reference_no)";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@bill_id", billId);
                cmd.Parameters.AddWithValue("@paid_on", paidOn);
                cmd.Parameters.AddWithValue("@amount", amount);
                cmd.Parameters.AddWithValue("@mode", mode);
                cmd.Parameters.AddWithValue("@reference_no", Guid.NewGuid().ToString()); // ✅ Ensures uniqueness

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            LoadPayments();
        }
    }
}