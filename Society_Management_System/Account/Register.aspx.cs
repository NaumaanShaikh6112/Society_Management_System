using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Society_Management_System.Account
{
    public partial class Register : System.Web.UI.Page
    {
        protected void btnRegister_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            // 1️⃣ Collect values from form
            string fullName = txtFullName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string phone = txtMobile.Text.Trim();
            string flatNo = txtFlatNo.Text.Trim();
            string username = email; // using email as username
            string password = txtPassword.Text.Trim();
            string occupancyType = ddlOccupancyType.SelectedValue;
            long societyId = Convert.ToInt64(ddlSociety.SelectedValue);
            int roleId = Convert.ToInt32(hdnRoleId.Value);

            // 2️⃣ Map flatNo to unit_id (assuming unit_no is unique)
            long unitId = 0;
            string conStr = ConfigurationManager.ConnectionStrings["societyDB"].ConnectionString;

            try
            {
                using (SqlConnection conn = new SqlConnection(conStr))
                {
                    conn.Open();

                    SqlCommand cmdUnit = new SqlCommand("SELECT unit_id FROM units WHERE unit_no = @unit_no AND building_id = @building_id", conn);
                    cmdUnit.Parameters.AddWithValue("@unit_no", flatNo);
                    cmdUnit.Parameters.AddWithValue("@building_id", buildingId); // You need to capture this from UI
                    object result = cmdUnit.ExecuteScalar();

                    if (result != null)
                        unitId = Convert.ToInt64(result);
                    else
                        throw new Exception("Invalid flat/unit number.");

                    // 3️⃣ Call stored procedure
                    SqlCommand cmd = new SqlCommand("RegisterUserProc", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@full_name", fullName);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@phone", phone);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password_hash", password); // hash later
                    cmd.Parameters.AddWithValue("@society_id", societyId);
                    cmd.Parameters.AddWithValue("@role_id", roleId);
                    cmd.Parameters.AddWithValue("@unit_id", unitId);
                    cmd.Parameters.AddWithValue("@occupancy_type", occupancyType);

                    object status = cmd.ExecuteScalar();

                    if (status != null && status.ToString() == "success")
                    {
                        pnlSuccess.Visible = true;
                        lblSuccess.Text = "Account created successfully. You can now log in.";
                        pnlError.Visible = false;
                    }
                    else
                    {
                        pnlError.Visible = true;
                        lblError.Text = "Registration failed. Please try again.";
                        pnlSuccess.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                pnlError.Visible = true;
                lblError.Text = "Error: " + ex.Message;
                pnlSuccess.Visible = false;
            }
        }
    }
}
