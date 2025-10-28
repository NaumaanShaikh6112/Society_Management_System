using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Society_Management_System.Admin
{
    public partial class ManageUnits : System.Web.UI.Page
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
                // Initialize dropdowns
                ddlBuildings.Items.Insert(0, new ListItem("-- Select Building --", "0"));
                BindUnitGrid(); // Bind empty grid initially
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

        protected void ddlSocieties_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateBuildings();
            BindUnitGrid(); // Clear unit grid
            ClearForm();
        }

        private void PopulateBuildings()
        {
            ddlBuildings.Items.Clear();
            if (ddlSocieties.SelectedValue != "0")
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
                            ddlBuildings.DataSource = dt;
                            ddlBuildings.DataTextField = "name";
                            ddlBuildings.DataValueField = "building_id";
                            ddlBuildings.DataBind();
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
            ddlBuildings.Items.Insert(0, new ListItem("-- Select Building --", "0"));
        }

        protected void ddlBuildings_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindUnitGrid();
            ClearForm();
        }

        private void BindUnitGrid()
        {
            if (ddlBuildings.SelectedValue == "0" || string.IsNullOrEmpty(ddlBuildings.SelectedValue))
            {
                gvUnits.DataSource = null;
                litBuildingName.Text = "No Building Selected";
            }
            else
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("sp_Units_GetByBuildingID", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@BuildingID", Convert.ToInt64(ddlBuildings.SelectedValue));
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            gvUnits.DataSource = dt;
                        }
                    }
                    litBuildingName.Text = $"{ddlBuildings.SelectedItem.Text} ({ddlSocieties.SelectedItem.Text})";
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    gvUnits.DataSource = null;
                }
            }
            gvUnits.DataBind();
        }

        private void ClearForm()
        {
            hfUnitID.Value = "0";
            txtUnitNo.Text = string.Empty;
            txtFloorNo.Text = string.Empty;
            txtCarpetArea.Text = string.Empty;
            chkIsParkingAllocated.Checked = false;
            lblFormTitle.Text = "Add New Unit";
            btnSave.Text = "Save Unit";
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (ddlBuildings.SelectedValue == "0" || string.IsNullOrEmpty(ddlBuildings.SelectedValue))
            {
                // Validator will catch
                return;

            }

            try
            {
                string spName;
                decimal carpetArea;
                Decimal.TryParse(txtCarpetArea.Text.Trim(), out carpetArea);

                if (hfUnitID.Value == "0")
                {
                    // INSERT
                    spName = "sp_Units_Insert";
                    using (SqlCommand cmd = new SqlCommand(spName, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@BuildingID", Convert.ToInt64(ddlBuildings.SelectedValue));
                        cmd.Parameters.AddWithValue("@UnitNo", txtUnitNo.Text.Trim());
                        cmd.Parameters.AddWithValue("@FloorNo", Convert.ToInt32(txtFloorNo.Text));
                        cmd.Parameters.AddWithValue("@CarpetArea", carpetArea);
                        cmd.Parameters.AddWithValue("@IsParking", chkIsParkingAllocated.Checked);
                        cmd.ExecuteNonQuery();
                    }
                }
                else
                {
                    // UPDATE
                    spName = "sp_Units_Update";
                    using (SqlCommand cmd = new SqlCommand(spName, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UnitID", Convert.ToInt64(hfUnitID.Value));
                        cmd.Parameters.AddWithValue("@UnitNo", txtUnitNo.Text.Trim());
                        cmd.Parameters.AddWithValue("@FloorNo", Convert.ToInt32(txtFloorNo.Text));
                        cmd.Parameters.AddWithValue("@CarpetArea", carpetArea);
                        cmd.Parameters.AddWithValue("@IsParking", chkIsParkingAllocated.Checked);
                        cmd.ExecuteNonQuery();
                    }
                }

                BindUnitGrid();
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

        protected void gvUnits_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditRow")
            {
                try
                {
                    GridViewRow row = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
                    hfUnitID.Value = e.CommandArgument.ToString();

                    // Populate form
                    txtUnitNo.Text = System.Web.HttpUtility.HtmlDecode(row.Cells[0].Text);
                    txtFloorNo.Text = System.Web.HttpUtility.HtmlDecode(row.Cells[1].Text);
                    txtCarpetArea.Text = System.Web.HttpUtility.HtmlDecode(row.Cells[2].Text);

                    // Find the checkbox control in the cell
                    Control chk = row.Cells[3].Controls[0];
                    if (chk is CheckBox)
                    {
                        chkIsParkingAllocated.Checked = ((CheckBox)chk).Checked;
                    }

                    lblFormTitle.Text = "Edit Unit";
                    btnSave.Text = "Update Unit";
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
        }

        protected void gvUnits_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                long unitID = Convert.ToInt64(e.Keys[0]);
                using (SqlCommand cmd = new SqlCommand("sp_Units_Delete", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UnitID", unitID);
                    cmd.ExecuteNonQuery();
                }
                BindUnitGrid();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                // Handle error (e.g., if member is assigned to this unit)
            }
        }
    }
}