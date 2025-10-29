using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace Society_Management_System.Admin
{
    public partial class ManagePayments : System.Web.UI.Page
    {
        private static readonly string ConnStr = System.Configuration.ConfigurationManager.ConnectionStrings["SocietyDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadUnpaidBills();
                LoadRecentPayments();
            }
        }

        private void LoadUnpaidBills()
        {
            using (SqlConnection con = new SqlConnection(ConnStr))
            {
                string query = @"
                    SELECT bill_id, CONCAT('BillID: ', bill_id, ', Society: ', s.name, ', Unit: ', u.unit_no, ', Due: ', FORMAT(due_date, 'yyyy-MM-dd')) AS BillDescription
                    FROM maintenance_bills MB
                    INNER JOIN societies s ON MB.society_id = s.society_id
                    INNER JOIN units u ON MB.unit_id = u.unit_id
                    WHERE status IN ('Unpaid', 'Partially Paid')
                    ORDER BY due_date";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                DdlBills.DataSource = dt;
                DdlBills.DataTextField = "BillDescription";
                DdlBills.DataValueField = "bill_id";
                DdlBills.DataBind();
                DdlBills.Items.Insert(0, new ListItem("-- Select Bill --", "0"));
            }
        }

        private void LoadRecentPayments()
        {
            using (SqlConnection con = new SqlConnection(ConnStr))
            {
                string query = @"
                    SELECT payment_id, bill_id, paid_on, amount, mode, reference_no
                    FROM payments
                    ORDER BY paid_on DESC";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                GvPayments.DataSource = dt;
                GvPayments.DataBind();
            }
        }

        protected void BtnRecordPayment_Click(object sender, EventArgs e)
        {
            if (DdlBills.SelectedIndex == 0)
            {
                LblMessage.Text = "Please select a bill to record payment.";
                return;
            }

            if (!decimal.TryParse(TxtAmount.Text, out decimal amount) || amount <= 0)
            {
                LblMessage.Text = "Please enter a valid payment amount.";
                return;
            }

            long billId = Convert.ToInt64(DdlBills.SelectedValue);
            string mode = DdlPaymentMode.SelectedValue;
            string reference = TxtReference.Text;
            DateTime paidOn = DateTime.Now;

            using (SqlConnection con = new SqlConnection(ConnStr))
            {
                con.Open();
                SqlTransaction trans = con.BeginTransaction();

                try
                {
                    // Insert payment record
                    string insertPaymentSql = @"
                        INSERT INTO payments (bill_id, paid_on, amount, mode, reference_no)
                        VALUES (@bill_id, @paid_on, @amount, @mode, @reference)";
                    using (SqlCommand cmd = new SqlCommand(insertPaymentSql, con, trans))
                    {
                        cmd.Parameters.AddWithValue("@bill_id", billId);
                        cmd.Parameters.AddWithValue("@paid_on", paidOn);
                        cmd.Parameters.AddWithValue("@amount", amount);
                        cmd.Parameters.AddWithValue("@mode", mode);
                        cmd.Parameters.AddWithValue("@reference", string.IsNullOrEmpty(reference) ? DBNull.Value : (object)reference);
                        cmd.ExecuteNonQuery();
                    }

                    // Update maintenance_bills status based on payments sum versus total_amount

                    string updateStatusSql = @"
                        UPDATE maintenance_bills
                        SET status =
                            CASE
                                WHEN (SELECT ISNULL(SUM(amount), 0) FROM payments WHERE bill_id = @bill_id) >= total_amount THEN 'Paid'
                                WHEN (SELECT ISNULL(SUM(amount), 0) FROM payments WHERE bill_id = @bill_id) > 0 THEN 'Partially Paid'
                                ELSE 'Unpaid'
                            END
                        WHERE bill_id = @bill_id";

                    using (SqlCommand cmd = new SqlCommand(updateStatusSql, con, trans))
                    {
                        cmd.Parameters.AddWithValue("@bill_id", billId);
                        cmd.ExecuteNonQuery();
                    }

                    trans.Commit();

                    LoadUnpaidBills();
                    LoadRecentPayments();

                    LblMessage.Text = "Payment recorded successfully.";

                    DdlBills.SelectedIndex = 0;
                    TxtAmount.Text = "";
                    TxtReference.Text = "";
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    LblMessage.Text = "Payment recording failed: " + ex.Message;
                }
            }
        }
    }
}