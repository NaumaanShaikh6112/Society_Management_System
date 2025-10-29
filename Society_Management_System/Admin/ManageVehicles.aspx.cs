using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Society_Management_System.Admin
{
    public partial class ManageVehicles : Page
    {
        private readonly string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["societyDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindVehicles();
                BindMembersDropdown();
            }
        }

        private void BindVehicles()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                // Joining members and units for display purposes
                string query = @"
                    SELECT v.vehicle_id, v.registration_no, v.type, m.full_name AS member_full_name, u.unit_no
                    FROM vehicles v
                    LEFT JOIN members m ON v.member_id = m.member_id
                    LEFT JOIN units u ON v.unit_id = u.unit_id
                    ORDER BY v.vehicle_id DESC";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvVehicles.DataSource = dt;
                gvVehicles.DataBind();
            }
        }

        private void BindMembersDropdown()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT member_id, full_name FROM members WHERE status = 'Active' ORDER BY full_name";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                ddlMember.DataSource = reader;
                ddlMember.DataTextField = "full_name";
                ddlMember.DataValueField = "member_id";
                ddlMember.DataBind();
                ddlMember.Items.Insert(0, new ListItem("-- Select Owner --", ""));
            }
        }

        protected void BtnAddVehicle_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRegistrationNo.Text) || string.IsNullOrWhiteSpace(txtType.Text) || string.IsNullOrEmpty(ddlMember.SelectedValue))
            {
                lblVehicleMessage.Text = "All fields are required, including owner.";
                return;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"
                    INSERT INTO vehicles (member_id, unit_id, registration_no, type) 
                    VALUES (@member_id, NULL, @registration_no, @type)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@member_id", ddlMember.SelectedValue);
                cmd.Parameters.AddWithValue("@registration_no", txtRegistrationNo.Text.Trim());
                cmd.Parameters.AddWithValue("@type", txtType.Text.Trim());

                con.Open();
                int result = cmd.ExecuteNonQuery();
                if (result > 0)
                {
                    lblVehicleMessage.ForeColor = System.Drawing.Color.LawnGreen;
                    lblVehicleMessage.Text = "Vehicle added successfully!";
                    ClearVehicleInputs();
                    BindVehicles();
                }
                else
                {
                    lblVehicleMessage.ForeColor = System.Drawing.Color.Red;
                    lblVehicleMessage.Text = "Error adding vehicle.";
                }
            }
        }

        private void ClearVehicleInputs()
        {
            txtRegistrationNo.Text = "";
            txtType.Text = "";
            ddlMember.SelectedIndex = 0;
        }

        protected void GvVehicles_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvVehicles.EditIndex = e.NewEditIndex;
            BindVehicles();
        }

        protected void GvVehicles_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvVehicles.EditIndex = -1;
            BindVehicles();
        }

        protected void GvVehicles_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = gvVehicles.Rows[e.RowIndex];
            int vehicleId = Convert.ToInt32(gvVehicles.DataKeys[e.RowIndex].Value);

            string regNo = ((TextBox)row.Cells[1].Controls[0]).Text.Trim();
            string type = ((TextBox)row.Cells[2].Controls[0]).Text.Trim();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "UPDATE vehicles SET registration_no=@regNo, type=@type WHERE vehicle_id=@vehicleId";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@regNo", regNo);
                cmd.Parameters.AddWithValue("@type", type);
                cmd.Parameters.AddWithValue("@vehicleId", vehicleId);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            gvVehicles.EditIndex = -1;
            BindVehicles();
        }

        protected void GvVehicles_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int vehicleId = Convert.ToInt32(gvVehicles.DataKeys[e.RowIndex].Value);

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM vehicles WHERE vehicle_id=@vehicleId";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@vehicleId", vehicleId);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            BindVehicles();
        }

        protected void GvVehicles_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if ((e.Row.RowState & DataControlRowState.Edit) > 0)
            {
                e.Row.CssClass = "edit-row";
                foreach (TableCell cell in e.Row.Cells)
                {
                    foreach (Control control in cell.Controls)
                    {
                        if (control is TextBox tb)
                            tb.CssClass = "glass-input";
                    }
                }
            }
        }
    }
}