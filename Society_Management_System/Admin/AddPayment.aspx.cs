using System;
using System.Data.SqlClient;
using System.Web.UI;

namespace Society_Management_System.Admin
{
    public partial class AddPayment : Page
    {
        // Retrieve connection string from web.config
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["societyDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        // Save the Payment to the database
        protected void btnSavePayment_Click(object sender, EventArgs e)
        {
            // Your SQL query to insert payment
            string query = "INSERT INTO payments (bill_id, paid_on, amount, mode) VALUES (@BillId, @PaidOn, @Amount, @Mode)";

            // Using statement for SQL connection
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);

                // Adding parameters for SQL query
                cmd.Parameters.AddWithValue("@BillId", txtBillId.Text); // Make sure txtBillId is valid
                cmd.Parameters.AddWithValue("@PaidOn", DateTime.Now); // The current date and time
                cmd.Parameters.AddWithValue("@Amount", Convert.ToDecimal(txtAmount.Text)); // Make sure to convert the amount to decimal
                cmd.Parameters.AddWithValue("@Mode", ddlPaymentMode.SelectedValue); // Payment mode selected in dropdown

                // Open the connection and execute the command
                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    lblMessage.Text = "Payment successfully added!";
                    Response.Redirect("ManagePayments.aspx");
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "Error: " + ex.Message;
                }
            }
        }
    }
}