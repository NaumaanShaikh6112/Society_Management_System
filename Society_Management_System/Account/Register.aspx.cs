using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace Society_Management_System.Account
{
    public partial class Register : System.Web.UI.Page
    {
        private string conStr = ConfigurationManager.ConnectionStrings["societyDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindSocieties();
                BindOccupancyTypes();
                ddlSociety.Items.Insert(0, new ListItem("Select Society", ""));
                ddlBuilding.Items.Insert(0, new ListItem("Select Building", ""));
            }
        }

        private void BindSocieties()
        {
            using (SqlConnection conn = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand("SELECT society_id, name FROM societies", conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                ddlSociety.DataSource = reader;
                ddlSociety.DataTextField = "name";
                ddlSociety.DataValueField = "society_id";
                ddlSociety.DataBind();
            }
        }

        private void BindBuildings(long societyId)
        {
            string conStr = ConfigurationManager.ConnectionStrings["societyDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand("SELECT building_id, name FROM buildings WHERE society_id = @society_id", conn);
                cmd.Parameters.AddWithValue("@society_id", societyId);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                ddlBuilding.DataSource = reader;
                ddlBuilding.DataTextField = "name";
                ddlBuilding.DataValueField = "building_id";
                ddlBuilding.DataBind();
            }
        }

        private void BindOccupancyTypes()
        {
            ddlOccupancyType.DataSource = new string[] { "Owner", "Tenant", "Vacant" };
            ddlOccupancyType.DataBind();
        }

        protected void ddlSociety_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlSociety.SelectedValue))
            {
                long societyId = Convert.ToInt64(ddlSociety.SelectedValue);
                BindBuildings(societyId);
                ddlBuilding.Items.Insert(0, new ListItem("Select Building", ""));
            }
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            string fullName = txtFullName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string phone = txtMobile.Text.Trim();
            string flatNo = txtFlatNo.Text.Trim();
            string username = email;
            string password = txtPassword.Text.Trim();
            long societyId = Convert.ToInt64(ddlSociety.SelectedValue);
            long buildingId = Convert.ToInt64(ddlBuilding.SelectedValue);
            string occupancyType = ddlOccupancyType.SelectedValue;
            int roleId = Convert.ToInt32(hdnRoleId.Value);

            long unitId = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(conStr))
                {
                    conn.Open();

                    SqlCommand cmdUnit = new SqlCommand("SELECT unit_id FROM units WHERE unit_no = @unit_no AND building_id = @building_id", conn);
                    cmdUnit.Parameters.AddWithValue("@unit_no", flatNo);
                    cmdUnit.Parameters.AddWithValue("@building_id", buildingId);
                    object result = cmdUnit.ExecuteScalar();

                    if (result != null)
                        unitId = Convert.ToInt64(result);
                    else
                        throw new Exception("Invalid flat/unit number.");

                    SqlCommand cmd = new SqlCommand("RegisterUserProc", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@full_name", fullName);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@phone", phone);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password_hash", password);
                    cmd.Parameters.AddWithValue("@society_id", societyId);
                    cmd.Parameters.AddWithValue("@role_id", roleId);
                    cmd.Parameters.AddWithValue("@unit_id", unitId);
                    cmd.Parameters.AddWithValue("@occupancy_type", occupancyType);

                    object status = cmd.ExecuteScalar();

                    if (status != null && status.ToString().ToLower() == "success")
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