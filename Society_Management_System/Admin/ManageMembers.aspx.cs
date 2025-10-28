using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace Society_Management_System.Admin
{
    public partial class ManageMembers : System.Web.UI.Page
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
                BindMemberGrid();
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

        private void BindMemberGrid()
        {
            if (ddlSocieties.SelectedValue == "0" || string.IsNullOrEmpty(ddlSocieties.SelectedValue))
            {
                gvMembers.DataSource = null;
                litSocietyName.Text = "No Society Selected";
            }
            else
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("sp_Members_GetBySocietyID", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@SocietyID", Convert.ToInt64(ddlSocieties.SelectedValue));
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            gvMembers.DataSource = dt;
                        }
                    }
                    litSocietyName.Text = ddlSocieties.SelectedItem.Text;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    gvMembers.DataSource = null;
                }
            }
            gvMembers.DataBind();
        }

        private void ClearForm()
        {
            hfMemberID.Value = "0";
            txtFullName.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtPhone.Text = string.Empty;
            ddlStatus.SelectedValue = "Active";
            lblFormTitle.Text = "Add New Member";
            btnSave.Text = "Save Member";
            // Keep ddlSocieties selected
        }

        protected void ddlSocieties_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindMemberGrid();
            ClearForm();
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

                if (hfMemberID.Value == "0")
                {
                    // INSERT
                    spName = "sp_Members_Insert";
                    using (SqlCommand cmd = new SqlCommand(spName, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@SocietyID", Convert.ToInt64(ddlSocieties.SelectedValue));
                        cmd.Parameters.AddWithValue("@FullName", txtFullName.Text.Trim());
                        cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                        cmd.Parameters.AddWithValue("@Phone", txtPhone.Text.Trim());
                        cmd.Parameters.AddWithValue("@Status", ddlStatus.SelectedValue);
                        cmd.ExecuteNonQuery();
                    }
                }
                else
                {
                    // UPDATE
                    spName = "sp_Members_Update";
                    using (SqlCommand cmd = new SqlCommand(spName, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@MemberID", Convert.ToInt64(hfMemberID.Value));
                        cmd.Parameters.AddWithValue("@FullName", txtFullName.Text.Trim());
                        cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                        cmd.Parameters.AddWithValue("@Phone", txtPhone.Text.Trim());
                        cmd.Parameters.AddWithValue("@Status", ddlStatus.SelectedValue);
                        cmd.ExecuteNonQuery();
                    }
                }

                BindMemberGrid();
                ClearForm();
            }
            catch (SqlException sqlex)
            {
                System.Diagnostics.Debug.WriteLine(sqlex.Message);
                if (sqlex.Number == 2601 || sqlex.Number == 2627) // Unique constraint violation
                {
                    // Show error to user about duplicate email
                    // e.g., Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('A member with this email already exists.');", true);
                }
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

        protected void gvMembers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditRow")
            {
                try
                {
                    long memberID = Convert.ToInt64(e.CommandArgument);
                    using (SqlCommand cmd = new SqlCommand("sp_Members_GetByID", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@MemberID", memberID);
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);

                            if (dt.Rows.Count > 0)
                            {
                                DataRow row = dt.Rows[0];
                                hfMemberID.Value = row["member_id"].ToString();
                                txtFullName.Text = row["full_name"].ToString();
                                txtEmail.Text = row["email"].ToString();
                                txtPhone.Text = row["phone"].ToString();

                                // Safely set dropdown values
                                if (ddlStatus.Items.FindByValue(row["status"].ToString()) != null)
                                {
                                    ddlStatus.SelectedValue = row["status"].ToString();
                                }
                                if (ddlSocieties.Items.FindByValue(row["society_id"].ToString()) != null)
                                {
                                    ddlSocieties.SelectedValue = row["society_id"].ToString();
                                }

                                lblFormTitle.Text = "Edit Member";
                                btnSave.Text = "Update Member";
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
        }

        protected void gvMembers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                long memberID = Convert.ToInt64(e.Keys[0]);
                using (SqlCommand cmd = new SqlCommand("sp_Members_Delete", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MemberID", memberID);
                    cmd.ExecuteNonQuery();
                }
                BindMemberGrid();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                // Handle error (e.g., if member is tied to a user or unit)
            }
        }
    }
}