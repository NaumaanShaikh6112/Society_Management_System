using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace Society_Management_System.Account
{
    public partial class Register : System.Web.UI.Page
    {
        private readonly string conStr = ConfigurationManager.ConnectionStrings["societyDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindSocieties();
                BindOccupancyTypes();

                ddlSociety.Items.Insert(0, new ListItem("Select Society", ""));
                ddlBuilding.Items.Insert(0, new ListItem("Select Building", ""));
                ddlUnit.Items.Insert(0, new ListItem("Select Unit", ""));
            }
        }

        // ✅ 1. Bind all societies
        private void BindSocieties()
        {
            using (SqlConnection conn = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand("SELECT society_id, name FROM societies", conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                ddlSociety.DataSource = reader;
                ddlSociety.DataTextField = "name";       // ✅ column name: name
                ddlSociety.DataValueField = "society_id";
                ddlSociety.DataBind();
            }
        }

        // ✅ 2. Bind buildings based on selected society
        private void BindBuildings(long societyId)
        {
            using (SqlConnection conn = new SqlConnection(conStr))
            {
                // 🏢 column name in DB is "name", not "building_name"
                SqlCommand cmd = new SqlCommand("SELECT building_id, name FROM buildings WHERE society_id = @society_id", conn);
                cmd.Parameters.AddWithValue("@society_id", societyId);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                ddlBuilding.DataSource = reader;
                ddlBuilding.DataTextField = "name";       // ✅ correct column
                ddlBuilding.DataValueField = "building_id";
                ddlBuilding.DataBind();
            }
        }

        // ✅ 3. Bind units based on selected building
        private void BindUnits(long buildingId)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = @"SELECT unit_id, unit_no FROM units WHERE building_id = @building_id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@building_id", buildingId);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                ddlUnit.DataSource = rdr;
                ddlUnit.DataTextField = "unit_no";
                ddlUnit.DataValueField = "unit_id";
                ddlUnit.DataBind();
            }

            ddlUnit.Items.Insert(0, new ListItem("Select Unit", ""));
        }

        // ✅ 4. Bind Occupancy types manually
        private void BindOccupancyTypes()
        {
            ddlOccupancyType.Items.Clear();
            ddlOccupancyType.Items.Add(new ListItem("Select Occupancy Type", ""));
            ddlOccupancyType.Items.Add(new ListItem("Owner", "Owner"));
            ddlOccupancyType.Items.Add(new ListItem("Tenant", "Tenant"));
            ddlOccupancyType.Items.Add(new ListItem("Vacant", "Vacant"));
        }

        // ✅ 5. When Society changes → Bind buildings
        protected void ddlSociety_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlBuilding.Items.Clear();
            ddlUnit.Items.Clear();

            ddlBuilding.Items.Insert(0, new ListItem("Select Building", ""));
            ddlUnit.Items.Insert(0, new ListItem("Select Unit", ""));

            if (!string.IsNullOrEmpty(ddlSociety.SelectedValue))
            {
                long societyId = Convert.ToInt64(ddlSociety.SelectedValue);
                BindBuildings(societyId);
            }
        }

        // ✅ 6. When Building changes → Bind units
        protected void ddlBuilding_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlUnit.Items.Clear();
            ddlUnit.Items.Insert(0, new ListItem("Select Unit", ""));

            if (!string.IsNullOrEmpty(ddlBuilding.SelectedValue))
            {
                long buildingId = Convert.ToInt64(ddlBuilding.SelectedValue);
                BindUnits(buildingId);
            }
        }

        // ✅ 7. Register button click
        protected void btnRegister_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            string fullName = txtFullName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string phone = txtMobile.Text.Trim();
            string username = email;
            string password = txtPassword.Text.Trim();
            string occupancyType = ddlOccupancyType.SelectedValue;

            if (string.IsNullOrEmpty(ddlSociety.SelectedValue) ||
                string.IsNullOrEmpty(ddlBuilding.SelectedValue) ||
                string.IsNullOrEmpty(ddlUnit.SelectedValue))
            {
                pnlError.Visible = true;
                lblError.Text = "⚠️ Please select Society, Building, and Unit properly.";
                return;
            }

            long societyId = Convert.ToInt64(ddlSociety.SelectedValue);
            long buildingId = Convert.ToInt64(ddlBuilding.SelectedValue);
            string unitNo = ddlUnit.SelectedItem.Text; // ✅ fetch actual unit number (text)
            int roleId = 2; // Default: Member role

            try
            {
                using (SqlConnection conn = new SqlConnection(conStr))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_RegisterUser", conn)) // ✅ correct proc name
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // 🔹 Match exactly with stored procedure parameters
                        cmd.Parameters.AddWithValue("@full_name", fullName);
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@phone", phone);
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password_hash", password);
                        cmd.Parameters.AddWithValue("@society_id", societyId);
                        cmd.Parameters.AddWithValue("@role_id", roleId);
                        cmd.Parameters.AddWithValue("@building_id", buildingId);
                        cmd.Parameters.AddWithValue("@unit_no", unitNo);
                        cmd.Parameters.AddWithValue("@occupancy_type", occupancyType);

                        cmd.ExecuteNonQuery();

                        pnlSuccess.Visible = true;
                        lblSuccess.Text = "🎉 Registration successful! You can now login.";
                        pnlError.Visible = false;
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                pnlError.Visible = true;
                lblError.Text = "⚠️ Database error: " + sqlEx.Message;
                pnlSuccess.Visible = false;
            }
            catch (Exception ex)
            {
                pnlError.Visible = true;
                lblError.Text = "❌ Unexpected error: " + ex.Message;
                pnlSuccess.Visible = false;
            }
        }
    }
}