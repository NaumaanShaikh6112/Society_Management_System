using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Society_Management_System.Admin
{
    public partial class ManageBills : System.Web.UI.Page
    {
        string connectionString = ConfigurationManager.ConnectionStrings["societyDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindUnits();
                BindBills();
            }
        }

        // ✅ Load Units into DropDown
        private void BindUnits()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT unit_id, unit_no FROM units ORDER BY unit_no";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                ddlUnits.DataSource = dt;
                ddlUnits.DataTextField = "unit_no";
                ddlUnits.DataValueField = "unit_id";
                ddlUnits.DataBind();
                ddlUnits.Items.Insert(0, new ListItem("-- Select Unit --", ""));
            }
        }

        // ✅ Bind All Bills to GridView
        private void BindBills()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT mb.bill_id, u.unit_no, mb.bill_month, mb.due_date, mb.total_amount, mb.status
                    FROM maintenance_bills mb
                    JOIN units u ON mb.unit_id = u.unit_id
                    ORDER BY mb.bill_id DESC";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvBills.DataSource = dt;
                gvBills.DataBind();
            }
        }

        // ✅ Show Add Bill Modal
        protected void btnAddBill_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "showModal", "showAddModal();", true);
        }

        // ✅ Save New Bill
        protected void btnSaveBill_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ddlUnits.SelectedValue))
                return;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    INSERT INTO maintenance_bills (society_id, unit_id, bill_month, due_date, total_amount, status)
                    VALUES (@society_id, @unit_id, @bill_month, @due_date, @total_amount, 'Unpaid')";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@society_id", 1); // Replace with session society_id if available
                    cmd.Parameters.AddWithValue("@unit_id", ddlUnits.SelectedValue);
                    cmd.Parameters.AddWithValue("@bill_month", DateTime.Parse(txtBillMonth.Text + "-01"));
                    cmd.Parameters.AddWithValue("@due_date", DateTime.Parse(txtDueDate.Text));
                    cmd.Parameters.AddWithValue("@total_amount", Convert.ToDecimal(txtAmount.Text));

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            BindBills();
        }

        // ✅ Edit Row
        protected void gvBills_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvBills.EditIndex = e.NewEditIndex;
            BindBills();
        }

        // ✅ Cancel Edit
        protected void gvBills_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvBills.EditIndex = -1;
            BindBills();
        }

        // ✅ Update Bill
        protected void gvBills_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = gvBills.Rows[e.RowIndex];
            int billId = Convert.ToInt32(gvBills.DataKeys[e.RowIndex].Value);
            string status = (row.Cells[5].Controls[0] as TextBox).Text;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "UPDATE maintenance_bills SET status = @status WHERE bill_id = @bill_id";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@status", status);
                    cmd.Parameters.AddWithValue("@bill_id", billId);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            gvBills.EditIndex = -1;
            BindBills();
        }

        // ✅ Delete Bill
        protected void gvBills_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int billId = Convert.ToInt32(gvBills.DataKeys[e.RowIndex].Value);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    DELETE FROM bill_items WHERE bill_id = @bill_id;
                    DELETE FROM maintenance_bills WHERE bill_id = @bill_id;";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@bill_id", billId);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            BindBills();
        }
    }
}