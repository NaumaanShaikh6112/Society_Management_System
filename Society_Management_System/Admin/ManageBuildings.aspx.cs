using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace Society_Manage.Admin
{
    public partial class ManageBuildings : System.Web.UI.Page
    {
        private SqlConnection con;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Initialize and open connection
            string cnf = ConfigurationManager.ConnectionStrings["societyDB"].ConnectionString;
            con = new SqlConnection(cnf);
            con.Open();

            if (!IsPostBack)
            {
                PopulateSocieties();
                BindBuildingGrid();
            }
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            if (con != null && con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }

        private void PopulateSocieties()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("sp_Societies_GetAll", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        ddlSocieties.DataSource = dt;
                        ddlSocieties.DataTextField = "name";
                        ddlSocieties.DataValueField = "society_id";
                        ddlSocieties.DataBind();
                    }
                }
                ddlSocieties.Items.Insert(0, new ListItem("-- Select Society --", "0"));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void BindBuildingGrid()
        {
            if (ddlSocieties.SelectedValue == "0" || string.IsNullOrEmpty(ddlSocieties.SelectedValue))
            {
                gvBuildings.DataSource = null; // Clear grid if no society is selected
                litSocietyName.Text = "No Society Selected";
            }
            else
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("sp_Buildings_GetBySocietyID", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@SocietyID", Convert.ToInt64(ddlSocieties.SelectedValue));
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            gvBuildings.DataSource = dt;
                        }
                    }
                    litSocietyName.Text = ddlSocieties.SelectedItem.Text;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    gvBuildings.DataSource = null;
                }
            }
            gvBuildings.DataBind();
        }

        private void ClearForm()
        {
            hfBuildingID.Value = "0";
            txtName.Text = string.Empty;
            txtFloors.Text = string.Empty;
            lblFormTitle.Text = "Add New Building";
            btnSave.Text = "Save Building";
            // Keep ddlSocieties selected
        }

        protected void ddlSocieties_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindBuildingGrid();
            ClearForm(); // Clear form when society changes
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (ddlSocieties.SelectedValue == "0")
            {
                // Validator will catch
                return;
            }

            try
            {
                string spName;

                if (hfBuildingID.Value == "0")
                {
                    // INSERT
                    spName = "sp_Buildings_Insert";
                    using (SqlCommand cmd = new SqlCommand(spName, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@SocietyID", Convert.ToInt64(ddlSocieties.SelectedValue));
                        cmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
                        cmd.Parameters.AddWithValue("@Floors", Convert.ToInt32(txtFloors.Text));
                        cmd.ExecuteNonQuery();
                    }
                }
                else
                {
                    // UPDATE
                    spName = "sp_Buildings_Update";
                    using (SqlCommand cmd = new SqlCommand(spName, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@BuildingID", Convert.ToInt64(hfBuildingID.Value));
                        cmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
                        cmd.Parameters.AddWithValue("@Floors", Convert.ToInt32(txtFloors.Text));
                        cmd.ExecuteNonQuery();
                    }
                }

                BindBuildingGrid();
                ClearForm();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        protected void gvBuildings_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditRow")
            {
                try
                {
                    GridViewRow row = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
                    hfBuildingID.Value = e.CommandArgument.ToString();

                    // Use HttpUtility.HtmlDecode to handle encoded characters
                    txtName.Text = System.Web.HttpUtility.HtmlDecode(row.Cells[0].Text);
                    txtFloors.Text = System.Web.HttpUtility.HtmlDecode(row.Cells[1].Text);

                    lblFormTitle.Text = "Edit Building";
                    btnSave.Text = "Update Building";
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
        }

        protected void gvBuildings_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                long buildingID = Convert.ToInt64(e.Keys[0]);
                using (SqlCommand cmd = new SqlCommand("sp_Buildings_Delete", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BuildingID", buildingID);
                    cmd.ExecuteNonQuery();
                }
                BindBuildingGrid();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                // Handle error (e.g., if units exist in this building)
            }
        }
    }
}