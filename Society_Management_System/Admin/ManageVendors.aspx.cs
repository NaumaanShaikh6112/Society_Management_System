using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Society_Management_System.Admin
{
    public partial class ManageVendors : System.Web.UI.Page
    {
        private readonly string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["societyDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindVendors();
            }
        }

        private void BindVendors()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT vendor_id, name, phone, email FROM vendors ORDER BY vendor_id DESC";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvVendors.DataSource = dt;
                gvVendors.DataBind();
            }
        }

        protected void BtnAddVendor_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtVendorName.Text))
            {
                lblVendorMessage.Text = "Vendor Name is required.";
                return;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO vendors (name, phone, email) VALUES (@name, @phone, @email)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@name", txtVendorName.Text.Trim());
                cmd.Parameters.AddWithValue("@phone", (object)txtVendorPhone.Text.Trim() ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@email", (object)txtVendorEmail.Text.Trim() ?? DBNull.Value);

                con.Open();
                int result = cmd.ExecuteNonQuery();
                if (result > 0)
                {
                    lblVendorMessage.ForeColor = System.Drawing.Color.LawnGreen;
                    lblVendorMessage.Text = "Vendor added successfully!";
                    ClearVendorInputs();
                    BindVendors();
                }
                else
                {
                    lblVendorMessage.ForeColor = System.Drawing.Color.Red;
                    lblVendorMessage.Text = "Error adding vendor.";
                }
            }
        }

        private void ClearVendorInputs()
        {
            txtVendorName.Text = "";
            txtVendorPhone.Text = "";
            txtVendorEmail.Text = "";
        }

        protected void GvVendors_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvVendors.EditIndex = e.NewEditIndex;
            BindVendors();
        }

        protected void GvVendors_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvVendors.EditIndex = -1;
            BindVendors();
        }

        protected void GvVendors_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = gvVendors.Rows[e.RowIndex];
            int vendorId = Convert.ToInt32(gvVendors.DataKeys[e.RowIndex].Value);

            string name = ((TextBox)row.Cells[1].Controls[0]).Text.Trim();
            string phone = ((TextBox)row.Cells[2].Controls[0]).Text.Trim();
            string email = ((TextBox)row.Cells[3].Controls[0]).Text.Trim();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "UPDATE vendors SET name=@name, phone=@phone, email=@email WHERE vendor_id=@vendor_id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@phone", phone);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@vendor_id", vendorId);

                con.Open();
                cmd.ExecuteNonQuery();
            }
            gvVendors.EditIndex = -1;
            BindVendors();
        }

        protected void GvVendors_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int vendorId = Convert.ToInt32(gvVendors.DataKeys[e.RowIndex].Value);

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM vendors WHERE vendor_id=@vendor_id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@vendor_id", vendorId);

                con.Open();
                cmd.ExecuteNonQuery();
            }
            BindVendors();
        }

        protected void GvVendors_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if ((e.Row.RowState & DataControlRowState.Edit) > 0)
            {
                e.Row.CssClass = "edit-row";

                foreach (TableCell cell in e.Row.Cells)
                {
                    foreach (Control control in cell.Controls)
                    {
                        if (control is TextBox tb)
                        {
                            tb.CssClass = "glass-input";
                        }
                    }
                }
            }
        }
    }
}